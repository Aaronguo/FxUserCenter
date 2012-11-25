using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fx.Domain.FxCar.IService.UserCenter;
using Fx.Domain.FxGoods.IService.UserCenter;
using Fx.Domain.FxHouse.IService.UserCenter;
using FxCacheService.FxSite;
using FxUserCenter.Models;

namespace FxUserCenter.Controllers
{
#if DEBUG
#else
    [Authorize]
#endif
    public class UserCenterController : Controller
    {
        protected GlobalCache gloablCache;
        protected ICarUserCenter carUserCenter;
        protected IGoodsUserCenter goodsUserCenter;
        protected IHouseUserCenter houseUserCenter;
        public UserCenterController(GlobalCache gloablCache,
            ICarUserCenter carUserCenter,
            IGoodsUserCenter goodsUserCenter,
            IHouseUserCenter houseUserCenter)
        {
            this.gloablCache = gloablCache;
            this.carUserCenter = carUserCenter;
            this.houseUserCenter = houseUserCenter;
            this.goodsUserCenter = goodsUserCenter;
        }
        //
        // GET: /UserCenter/

        public ActionResult About()
        {
            if (User.Identity.Name.Equals("117822597@163.com"))
            {
                RedirectPermanent("~/Admin/About");
            }
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
            return View(goodsUserCenter.GetBuys(User.Identity.Name));
        }


        public ActionResult GoodsTransfer()
        {
            return View(goodsUserCenter.GetTransfers(User.Identity.Name));
        }


        public ActionResult CarBuy()
        {
            return View(carUserCenter.GetBuys(User.Identity.Name));
        }


        public ActionResult CarTransfer()
        {
            return View(carUserCenter.GetTransfers(User.Identity.Name));
        }


        public ActionResult HouseBuy()
        {
            return View(houseUserCenter.GetBuys(User.Identity.Name));
        }



        public ActionResult HouseTransfer()
        {
            return View(houseUserCenter.GetTransfers(User.Identity.Name));
        }

        public ActionResult ChangePassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            var account = new Fx.Domain.Account.UserAccountService();
            string name = User.Identity.Name;
            var domainResult = account.VaildUser(name, oldPassword);
            if (domainResult.isSuccess)
            {
                var res = account.ChangePassword(new Fx.Entity.MemberShip.Membership()
                {
                    Users = new Fx.Entity.MemberShip.Users()
                    {
                        UserName = name
                    },
                    Password = newPassword
                }, oldPassword);
                if (res.isSuccess)
                {
                    ViewBag.New = "您的密码已经成功修改";
                }
                else
                {
                    ViewBag.New = res.ResultMsg;
                }
            }
            else
            {
                ViewBag.Old = domainResult.ResultMsg;
            }
            return View();
        }



        public ActionResult LogOff()
        {
            return Redirect("http://yingtao.co.uk/Account/LoginOff");
        }
    }
}
