using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XTC.FSM;

class ActionPrint : Action
{
    public string owner = "";
    protected override void onEnter()
    {
        Debug.LogFormat("Enter {0}", owner);
        doFinish();
    }

    protected override void onExit()
    {
        Debug.LogFormat("Exit {0}", owner);
    }

    protected override void onUpdate()
    {

    }
}

class ActionAddRound : Action
{
    protected override void onEnter()
    {
        doFinish();
    }

    protected override void onExit()
    {
        Parameter parameter = findParameter("round");
        parameter.AsInt += 1;
        Debug.LogFormat("--- Round {0} Finish ------------", parameter.AsInt);
    }

    protected override void onUpdate()
    {

    }
}

class ActionWait : Action
{
    public string owner = "";
    public float seconds = 1;
    private float timer { get; set; }

    protected override void onEnter()
    {
        Debug.LogFormat("{0}\t| Enter {1}.ActionWait ", Time.realtimeSinceStartup, owner);
        timer = 0;
    }

    protected override void onExit()
    {
        Debug.LogFormat("{0}\t| Exit {1}.ActionWait", Time.realtimeSinceStartup, owner);
    }

    protected override void onUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= seconds)
        {
            Debug.LogFormat("{0}\t| Finish {1}.ActionWait", Time.realtimeSinceStartup, owner);
            doFinish();
        }
    }
}

class ActionRand : Action
{
    public string owner = "";
    public Command cmd1 { get; set; }
    public Command cmd2 { get; set; }

    protected override void onEnter()
    {
        Debug.LogFormat("{0}\t| Enter {1}.ActionRand ", Time.realtimeSinceStartup, owner);
        //使用自定义Command时，不需要调用doFinish();
        //doFinish();
        int count = Random.Range(0, 4);
        if (count == 0)
            invoker.InvokeInternal(cmd1);
        else if (count == 1)
            invoker.InvokeInternal(cmd2);
        else
            invoker.InvokeInternal(finishCommand);
    }

    protected override void onExit()
    {
        Debug.LogFormat("{0}\t| Exit {1}.ActionRand", Time.realtimeSinceStartup, owner);
    }

    protected override void onUpdate()
    {

    }
}

public class Sample : MonoBehaviour
{
    public Transform target;
    private Machine machine { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        Sample1();
    }

    // Update is called once per frame
    void Update()
    {
        machine.Update();
    }

    void Sample1()
    {
        machine = new Machine();

        State stateMone = machine.NewState();

        State stateHappy = machine.NewState();
        ActionPrint actionPrintHappy = stateHappy.NewAction<ActionPrint>();
        actionPrintHappy.owner = "Happy";
        
        State stateCry = machine.NewState();
        ActionPrint actionPrintCry = stateCry.NewAction<ActionPrint>();
        actionPrintCry.owner = "Cry";

        State stateAngry = machine.NewState();
        ActionPrint actionPrintAngry = stateAngry.NewAction<ActionPrint>();
        actionPrintAngry.owner = "Angry";

        Command cmdHappy = machine.NewExternalCommand("happy");
        cmdHappy.state = stateHappy;
        Command cmdCry = machine.NewExternalCommand("cry");
        cmdCry.state = stateCry;
        Command cmdAngry = machine.NewExternalCommand("angry");
        cmdAngry.state = stateAngry;

        machine.startupCommand.state = stateMone;
        machine.Run();

        machine.InvokeExternalCommand("happy");
        machine.InvokeExternalCommand("angry");
        machine.InvokeExternalCommand("cry");
    }

    void Sample2()
    {
        machine = new Machine();

        Parameter round = new Parameter(0);
        machine.AddParameter("round", round);

        // 基本规则
        // 吃饱了运动，运动累了睡，睡醒了吃

        // 1. 定义状态

        // 睡觉的状态
        // ActionWait会调用doFinish，所以当行为完成时，调用状态的默认的Finish行为
        State stateSleep = machine.NewState();
        ActionWait actionWaitSleep = stateSleep.NewAction<ActionWait>();
        actionWaitSleep.seconds = 6;
        actionWaitSleep.owner = "Sleep";

        // 吃的状态
        // ActionWait会调用doFinish，所以当行为完成时，调用状态的默认的Finish行为
        State stateEat = machine.NewState();
        ActionWait actionWaitEatRice = stateEat.NewAction<ActionWait>();
        actionWaitEatRice.seconds = 2;
        actionWaitEatRice.owner = "Eate.Rice";
        ActionWait actionWaitEatMeat = stateEat.NewAction<ActionWait>();
        actionWaitEatMeat.seconds = 6;
        actionWaitEatMeat.owner = "Eate.Meat";


        // 走的状态
        // ActionWait会调用doFinish，所以当行为完成时，调用状态的默认的Finish行为
        State stateWalk = machine.NewState();
        ActionWait actionWaitWalk = stateWalk.NewAction<ActionWait>();
        actionWaitWalk.seconds = 3;
        actionWaitWalk.owner = "Walk";
        stateWalk.NewAction<ActionAddRound>();

        // 跑的状态
        // ActionWait会调用doFinish，所以当行为完成时，调用状态的默认的Finish行为
        State stateRun = machine.NewState();
        ActionWait actionWaitRun = stateRun.NewAction<ActionWait>();
        actionWaitRun.seconds = 1.5f;
        actionWaitRun.owner = "Run";
        stateRun.NewAction<ActionAddRound>();

        // 跳的状态
        // ActionWait会调用doFinish，所以当行为完成时，调用状态的默认的Finish行为
        State stateJump = machine.NewState();
        ActionWait actionWaitJump = stateJump.NewAction<ActionWait>();
        actionWaitJump.seconds = 3;
        actionWaitJump.owner = "Jump";
        stateJump.NewAction<ActionAddRound>();

        // 思考的状态
        // ActionRand没有调用doFinish，而是执行了指定的命令，所以不会调用状态的默认的Finish行为
        State stateThink = machine.NewState();
        ActionRand actionRand = stateThink.NewAction<ActionRand>();
        actionRand.cmd1 = machine.NewInternalCommand("Run");
        actionRand.cmd2 = machine.NewInternalCommand("Walk");
        actionRand.owner = "Think";


        // 2. 关联状态

        // stateThink中的行为执行了指定的命令，所以不需要指定默认Finish行为的状态
        stateEat.finishAction.state = stateThink;
        actionRand.cmd1.state = stateRun;
        actionRand.cmd2.state = stateJump;
        actionRand.finishCommand.state = stateWalk;
        stateRun.finishAction.state = stateSleep;
        stateWalk.finishAction.state = stateSleep;
        stateJump.finishAction.state = stateSleep;
        stateSleep.finishAction.state = stateEat;

        // 3. 设置初始状态

        machine.startupCommand.state = stateSleep;

        // 4. 运行状态机
        machine.Run();
    }
}
