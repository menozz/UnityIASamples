using UnityEngine;

public class char_script : MonoBehaviour
{
    private const int MaxCount = 10;
    private int _currentCount = 0;

    private Animator _animator;

    private string _nextState;


    void Start ()
    {
        _currentCount = MaxCount;
        this._animator = GetComponent<Animator>();
    }
	
	void Update ()
	{
	    if (Input.GetMouseButtonDown(0))
	    {
	        _currentCount--;
	        if (_currentCount == 0)
	        {
	            Debug.Log("AAAAA");
	            _animator.SetTrigger("next");
                this._currentCount = MaxCount;
	        }
	    }
	}
}
