using System;
using System.Collections.Generic;
using UnityEngine;

public class SwapController //: MonoBehaviour
{
    private UnitController _unit1;
    private UnitController _unit2;

    private Vector3 _unit1Pos;
    private Vector3 _unit2Pos;

    public void Set1(UnitController unit1)
    {
        _unit1 = unit1;
    }

    public void Set2(UnitController unit2)
    {
        _unit2 = unit2;
    }

    public bool IsBusy;


    public void Swap()
    {
        if (!IsValid)
        {
            return;
        }

        if (!IsBusy)
        {
            if (IsValidUnits(_unit1.gameObject, _unit2.gameObject))
            {
                _unit1Pos = _unit2.transform.position;
                _unit2Pos = _unit1.transform.position;
                IsBusy = true;
                //SetSwapPosition(destination.transform.position);
                //destination.SetSwapPosition(transform.position);
            }
            //else
            //{
            //    return false;
            //}
        }

        IsBusy = _unit1.MoveTo(_unit1Pos);
        IsBusy |= _unit2.MoveTo(_unit2Pos);
    }

    public bool IsValid
    {
        get { return _unit1 != null && _unit2 != null && _unit1.Name != _unit2.Name; }
    }

    //public void SetSwapPosition(Vector3 swapPos)
    //{
    //    if (IsBusy)
    //    {
    //        return;
    //    }

    //    swapPosition = swapPos;
    //    IsBusy = true;
    //}


    private bool IsValidUnits(GameObject unit1, GameObject unit2)
    {
        var fromPosition = unit1.transform.position;
        var toPosition = unit2.transform.position;
        var direction = toPosition - fromPosition;
        if (Math.Abs(direction.x) > 0.01f && Math.Abs(direction.y) > 0.01f)
        {
            return false;
        }

        if (Math.Abs(direction.x) > 1.5 || Math.Abs(direction.y) > 1.5)
        {
            return false;
        }

        return true;
    }
}