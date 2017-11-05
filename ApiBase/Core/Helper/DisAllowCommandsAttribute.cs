namespace System
{
    /// <summary>
    /// 禁止使用命令合并
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class DisAllowCommandsAttribute : Attribute
    {

    }
}
