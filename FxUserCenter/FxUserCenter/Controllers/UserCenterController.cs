using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FxCacheService.FxSite;

namespace FxUserCenter.Controllers
{
    [Authorize]
    public class UserCenterController : Controller
    {
        GlobalCache gloablCache;
        public UserCenterController(GlobalCache gloablCache)
        {
            this.gloablCache = gloablCache;
        }
        //
        // GET: /UserCenter/

        public ActionResult About()
        {
            var about = new AboutModel()
            {
                UserCount = gloablCache.UserCount(),
                UserTodayCount = gloablCache.UserTodayCount(),
                InfoPublishAllCount = gloablCache.InfoPublishAllCount(),
                InfoPublishTodayCount = gloablCache.InfoPublishTodayCount(),
                InfoEndCount = gloablCache.InfoEndCount(),
                PrivateMessageCount = gloablCache.PrivateMessageCount(User.Identity.Name),
                PrivateMessageTodayCount = gloablCache.PrivateMessageTodayCount(User.Identity.Name)
            };
            return View(about);
        }



        public ActionResult GoodsBuy()
        {
            return View();
        }


        public ActionResult CarBuy()
        {
            return View();
        }



        public ActionResult GoodsTransfer()
        {
            return View();
        }


        public ActionResult CarTransfer()
        {
            return View();
        }


        public ActionResult HouseBuy()
        {
            return View();
        }



        public ActionResult HouseTransfer()
        {
            return View();
        }

    }
}
