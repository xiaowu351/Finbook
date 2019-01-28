namespace Recommend.API.Models
{
    public enum EnumRecommendType:int
    {
        /// <summary>
        /// 系统推荐
        /// </summary>
        Platform =1,
        /// <summary>
        /// 好友推荐
        /// </summary>
        Friend=2,
        /// <summary>
        /// 二度好友
        /// </summary>
        Foaf=3
    }
}