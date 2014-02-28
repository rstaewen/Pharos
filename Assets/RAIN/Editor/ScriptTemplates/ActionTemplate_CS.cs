using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Action;

[RAINAction]
public class ActionTemplate_CS : RAINAction
{
    public ActionTemplate_CS()
    {
        actionName = "ActionTemplate_CS";
    }

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

	public override ActionResult Execute(RAIN.Core.AI ai)
    {
        return ActionResult.SUCCESS;
    }

	public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}