using System.Security.Cryptography;
using UnityEngine;

/*public enum PoolObjectType
{
    DefaultEnemy = 0,
}
*/
public class Factory : Singleton<Factory>
{
    DefaultEnemyPool defaultEnemy;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        // 풀 컴포넌트 찾고 찾으면 초기화하기
        defaultEnemy = GetComponentInChildren<DefaultEnemyPool>();
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

    public DefaultEnemy GetDefaultEnemy()
    {
        return defaultEnemy.GetObject();
    }

    public DefaultEnemy GetDefaultEnemy(Vector3 position, float angle = 0.0f)
    {
        return defaultEnemy.GetObject(position, angle * Vector3.forward);
    }

}
