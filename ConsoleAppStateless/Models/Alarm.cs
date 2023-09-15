using ConsoleAppStateless.Enumerators;
using Stateless;
using System.Diagnostics;

namespace ConsoleAppStateless.Models
{
    /// <summary>
    /// Também mostra uma maneira pela qual os estados temporários podem ser implementados com o uso de
    /// Temporizadores. PreArmed, PreTriggered, Triggered e ArmPaused são estados "temporários" com
    /// um atraso configurável (ou seja, para permitir um "atraso de armar"... um atraso entre Desarmado
    /// e Armado). O estado Disparado também é considerado temporário, pois se um alarme
    /// soa por um certo período de tempo e ninguém reconhece isso, a máquina de estado
    /// retorna ao estado Armado.
    /// 
    /// Timers são acionados via OnEntry() e OnExit() metodos. As transições são escritas para
    /// o Trace para mostrar o que acontece.
    /// 
    /// O arquivo PNG incluído mostra a aparência do fluxo de estado.
    /// 
    /// </summary>
    /// 
    public class Alarm
    {
        private StateMachine<AlarmStateEnum, AlarmCommandEnum> _machine;
        private System.Timers.Timer? preArmTimer;
        private System.Timers.Timer? pauseTimer;
        private System.Timers.Timer? triggerDelayTimer;
        private System.Timers.Timer? triggerTimeOutTimer;

        public bool IsConfigured { get; private set; }

        /// <summary>
        /// Move o alarme para o local fornecido <see cref="AlarmState" /> através do definido <see cref="AlarmCommand" />.
        /// </summary>
        /// <param name="command">The <see cref="AlarmCommand" /> para executar no atual <see cref="AlarmState" />.</param>
        /// <returns>Um novo <see cref="AlarmState" />.</returns>
        public AlarmStateEnum ExecuteTransition(AlarmCommandEnum command)
        {
            if (_machine.CanFire(command))
                _machine.Fire(command);
            else
                throw new InvalidOperationException($"Cannot transition from {CurrentState} via {command}");


            return CurrentState();
        }

        public AlarmStateEnum CurrentState()
        {
            if (_machine != null)
                return _machine.State;
            else
                throw new InvalidOperationException("Alarm hasn't been configured yet.");
        }

        /// <summary>
        /// Retorna se o comando fornecido é uma transição válida do Estado Atual.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool CanFireCommand(AlarmCommandEnum command)
        {
            return _machine.CanFire(command);
        }
        private void TimeoutTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            _machine.Fire(AlarmCommandEnum.TimeOut);
        }

        private void OnTransition(StateMachine<AlarmStateEnum, AlarmCommandEnum>.Transition transition)
        {
            Trace.WriteLine($"Transitioned from {transition.Source} to " +
                $"{transition.Destination} via {transition.Trigger}.");
        }

        private void ConfigureTimer(bool active, System.Timers.Timer timer, string timerName)
        {
            if (timer != null)
                if (active)
                {
                    timer.Start();
                    Trace.WriteLine($"{timerName} started.");
                }
                else
                {
                    timer.Stop();
                    Trace.WriteLine($"{timerName} cancelled.");
                }
        }

        /// <summary>
        /// Construtor padrão.
        /// </summary>
        /// <param name="armDelay">O tempo (em segundos) que o alarme permanecerá no
        /// Status pré-armado antes de continuar para o status Armado (se não tiver passado para
        /// Desarmado via Desarmar).</param>
        /// <param name="pauseDelay">O tempo (em segundos) que o alarme permanecerá no
        /// Status ArmPaused antes de retornar para Armado (se não tiver transitado para Triggered
        /// via Trigger).</param>
        /// <param name="triggerDelay">O tempo (em segundos) que o alarme passará no
        /// Status PreTriggered antes de continuar para o status Triggered (se não
        /// transição para Desarmado via Desarmar).</param>
        /// <param name="triggerTimeOut">O tempo (em segundos) que o alarme passará no
        /// Status acionado antes de retornar ao status Armado (se não tiver transitado para
        /// Desarmado via Desarmar).</param>
        public Alarm(int armDelay, int pauseDelay, int triggerDelay, int triggerTimeOut)
        {

            _machine = new StateMachine<AlarmStateEnum, AlarmCommandEnum>(AlarmStateEnum.Undefined);

            preArmTimer = new System.Timers.Timer(armDelay * 1000) { AutoReset = false, Enabled = false };
            preArmTimer.Elapsed += TimeoutTimerElapsed;
            pauseTimer = new System.Timers.Timer(pauseDelay * 1000) { AutoReset = false, Enabled = false };
            pauseTimer.Elapsed += TimeoutTimerElapsed;
            triggerDelayTimer = new System.Timers.Timer(triggerDelay * 1000) { AutoReset = false, Enabled = false };
            triggerDelayTimer.Elapsed += TimeoutTimerElapsed;
            triggerTimeOutTimer = new System.Timers.Timer(triggerTimeOut * 1000) { AutoReset = false, Enabled = false };
            triggerTimeOutTimer.Elapsed += TimeoutTimerElapsed;


            _machine.OnTransitioned(OnTransition);

            _machine.Configure(AlarmStateEnum.Undefined)
                .Permit(AlarmCommandEnum.Startup, AlarmStateEnum.Disarmed)
                .OnExit(() => IsConfigured = true);

            _machine.Configure(AlarmStateEnum.Disarmed)
              .Permit(AlarmCommandEnum.Arm, AlarmStateEnum.Prearmed);

            _machine.Configure(AlarmStateEnum.Armed)
                .Permit(AlarmCommandEnum.Disarm, AlarmStateEnum.Disarmed)
                .Permit(AlarmCommandEnum.Trigger, AlarmStateEnum.PreTriggered)
                .Permit(AlarmCommandEnum.Pause, AlarmStateEnum.ArmPaused);

            _machine.Configure(AlarmStateEnum.Prearmed)
                .OnEntry(() => ConfigureTimer(true, preArmTimer, "Pre-arm"))
                .OnExit(() => ConfigureTimer(false, preArmTimer, "Pre-arm"))
                .Permit(AlarmCommandEnum.TimeOut, AlarmStateEnum.Armed)
                .Permit(AlarmCommandEnum.Disarm, AlarmStateEnum.Disarmed);

            _machine.Configure(AlarmStateEnum.ArmPaused)
                .OnEntry(() => ConfigureTimer(true, pauseTimer, "Pause delay"))
                .OnExit(() => ConfigureTimer(false, pauseTimer, "Pause delay"))
                .Permit(AlarmCommandEnum.TimeOut, AlarmStateEnum.Armed)
                .Permit(AlarmCommandEnum.Trigger, AlarmStateEnum.PreTriggered);

            _machine.Configure(AlarmStateEnum.Triggered)
                .OnEntry(() => ConfigureTimer(true, triggerTimeOutTimer, "Trigger timeout"))
                .OnExit(() => ConfigureTimer(false, triggerTimeOutTimer, "Trigger timeout"))
                .Permit(AlarmCommandEnum.TimeOut, AlarmStateEnum.Armed)
                .Permit(AlarmCommandEnum.Acknowledge, AlarmStateEnum.Acknowledged);

            _machine.Configure(AlarmStateEnum.PreTriggered)
                .OnEntry(() => ConfigureTimer(true, triggerDelayTimer, "Trigger delay"))
                .OnExit(() => ConfigureTimer(false, triggerDelayTimer, "Trigger delay"))
                .Permit(AlarmCommandEnum.TimeOut, AlarmStateEnum.Triggered)
                .Permit(AlarmCommandEnum.Disarm, AlarmStateEnum.Disarmed);

            _machine.Configure(AlarmStateEnum.Acknowledged)
                .Permit(AlarmCommandEnum.Disarm, AlarmStateEnum.Disarmed);

            _machine.Fire(AlarmCommandEnum.Startup);

        }
    }
}
