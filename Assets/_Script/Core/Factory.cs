using System.Security.Cryptography;
using UnityEngine;

/*public enum PoolObjectType
{
    DefaultEnemy = 0,
}
*/
public class Factory : Singleton<Factory>
{
    Enemy_Slime_Pool defaultEnemy;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        // Ǯ ������Ʈ ã�� ã���� �ʱ�ȭ�ϱ�
        defaultEnemy = GetComponentInChildren<Enemy_Slime_Pool>();
        if(defaultEnemy != null)
            defaultEnemy.Initialize();
    }
    /*public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;
        switch (type)
        {
            case PoolObjectType.DefaultEnemy:
                result = defaultEnemy.GetObject(position, euler).gameObject;
                break;
        }

        return result;
    }*/

    public Enemy_Slime GetDefaultEnemy()
    {
        return defaultEnemy.GetObject();
    }

    public Enemy_Slime GetDefaultEnemy(Vector3 position, float angle = 0.0f)
    {
        return defaultEnemy.GetObject(position, angle * Vector3.forward);
    }

}
