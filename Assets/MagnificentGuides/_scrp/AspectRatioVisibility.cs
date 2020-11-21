using UnityEngine;


[ExecuteInEditMode]
public class AspectRatioVisibility : MonoBehaviour
{
    public GameObject[] HorizontalObjects;
    public GameObject[] VerticalObjects;

    [SerializeField]
    private float _horizontalAspectRatio = 1f;

    private float _aspectRatio;
    private bool _isHorizontal;
    
    
    private void Awake ()
    {
        _aspectRatio = UITools.GetAspectRatio();

        _isHorizontal = IsHorizontal();

        UpdateVisibility();
    }
    
    private void Update ()
    {
        var newAspectRatio = UITools.GetAspectRatio();
        
        if( newAspectRatio == _aspectRatio )
            return;

        _aspectRatio = newAspectRatio;

        CheckAspectRatio();
    }

    private void CheckAspectRatio ()
    {
        var isHorizontal = IsHorizontal();
        
        if( _isHorizontal == isHorizontal )
            return;
        
        _isHorizontal = isHorizontal;

        UpdateVisibility();
    }

    private bool IsHorizontal ()
    {
        return ( _aspectRatio > _horizontalAspectRatio );
    }

    private void UpdateVisibility ()
    {
        foreach( var horizontalObject in HorizontalObjects )
        {
            horizontalObject.SetActive( _isHorizontal );
        }

        foreach( var verticalObject in VerticalObjects )
        {
            verticalObject.SetActive( !_isHorizontal );
        }
    }
}
