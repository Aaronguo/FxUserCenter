using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fx.Domain.FxAggregate.IService;
using Fx.Domain.FxCar.IService;
using Fx.Domain.FxCar.IService.UserCenter;
using Fx.Domain.FxGoods.IService;
using Fx.Domain.FxGoods.IService.UserCenter;
using Fx.Domain.FxHouse.IService;
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
        protected IPrivateMessage privateMessage;
        protected IFavorite favorite;
        public UserCenterController(GlobalCache gloablCache,
            ICarUserCenter carUserCenter,
            IGoodsUserCenter goodsUserCenter,
            IHouseUserCenter houseUserCenter,
            IPrivateMessage privateMessage,
            IFavorite favorite)
        {
            this.gloablCache = gloablCache;
            this.carUserCenter = carUserCenter;
            this.houseUserCenter = houseUserCenter;
            this.goodsUserCenter = goodsUserCenter;
            this.privateMessage = privateMessage;
            this.favorite = favorite;
        }
        //
        // GET: /UserCenter/

        public ActionResult About()
        {
            if (User != null && User.Identity != null &&
                User.Identity.Name != null && User.Identity.Name.Equals("117822597@163.com"))
            {
                return Redirect("~/Admin/About");
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

        public ActionResult Favorite()
        {
#if DEBUG
            var list = favorite.GetFavorite("117822597@163.com");
#else
            var list = favorite.GetFavorite(User.Identity.Name);
#endif
            return View(list);
        }

        public ActionResult FavoriteDelete(int id)
        {
            var info = favorite.GetById(id);
            if (info != null && info.UserAccount == User.Identity.Name)
            {
                favorite.DeleteFavorite(new Fx.Entity.FxAggregate.Favorite()
                {
                    FavoriteId = id
                });
            }
            return RedirectToAction("Favorite");
        }



        public ActionResult PrivateMessage()
        {
#if DEBUG
            var infos = privateMessage.GetByUser("117822597@163.com");
#else
            var infos = privateMessage.GetByUser(User.Identity.Name);
#endif
            return View(infos);
        }

        public ActionResult PrivateMessageDelete(int id)
        {
            var info = privateMessage.GetById(id);
            if (info != null && info.UserAccount == User.Identity.Name)
            {
                privateMessage.RemovePrivateMessage(new Fx.Entity.FxAggregate.PrivateMessage()
                {
                    PrivateMessageId = id
                });
            }
            return RedirectToAction("PrivateMessage");
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

        #region 帖子信息获取
        public ActionResult GoodsBuy()
        {
#if DEBUG
            return View(goodsUserCenter.GetBuys(User.Identity.Name));
#else
            return View(goodsUserCenter.GetBuys(User.Identity.Name));
#endif
        }


        public ActionResult GoodsTransfer()
        {
#if DEBUG
            return View(goodsUserCenter.GetTransfers("117822597@163.com"));
#else
            return View(goodsUserCenter.GetTransfers(User.Identity.Name));
#endif
        }


        public ActionResult CarBuy()
        {
#if DEBUG
            return View(carUserCenter.GetBuys("117822597@163.com"));
#else
            return View(carUserCenter.GetBuys(User.Identity.Name));
#endif
        }


        public ActionResult CarTransfer()
        {
#if DEBUG
            return View(carUserCenter.GetTransfers("117822597@163.com"));
#else
            return View(carUserCenter.GetTransfers(User.Identity.Name));
#endif
        }


        public ActionResult HouseBuy()
        {
#if DEBUG
            return View(houseUserCenter.GetBuys("117822597@163.com"));
#else
            return View(houseUserCenter.GetBuys(User.Identity.Name));
#endif
        }



        public ActionResult HouseTransfer()
        {
#if DEBUG
            return View(houseUserCenter.GetTransfers("117822597@163.com"));
#else
            return View(houseUserCenter.GetTransfers(User.Identity.Name));
#endif
        }
        #endregion

        #region 帖子成交
        public ActionResult CarBuyEnd(int id)
        {
            IBuyCar buy = System.Web.Mvc.DependencyResolver.Current.GetService<IBuyCar>();
            var info = buy.Get(id);
            if (info.PublishUserEmail == User.Identity.Name)
            {
                ICarBuyJob car = System.Web.Mvc.DependencyResolver.Current.GetService<ICarBuyJob>();
                car.End(id);

            }
            return RedirectToAction("CarBuy");
        }


        public ActionResult CarTransferEnd(int id)
        {
            ITransferCar transfer = System.Web.Mvc.DependencyResolver.Current.GetService<ITransferCar>();
            var info = transfer.Get(id);
            if (info.PublishUserEmail == User.Identity.Name)
            {
                ICarTransferJob car = System.Web.Mvc.DependencyResolver.Current.GetService<ICarTransferJob>();
                car.End(id);
            }
            return RedirectToAction("CarTransfer");
        }



        public ActionResult GoodsBuyEnd(int id)
        {
            IBuyGoods buy = System.Web.Mvc.DependencyResolver.Current.GetService<IBuyGoods>();
            var info = buy.Get(id);
            if (info.PublishUserEmail == User.Identity.Name)
            {
                IGoodsBuyJob goods = System.Web.Mvc.DependencyResolver.Current.GetService<IGoodsBuyJob>();
                goods.End(id);
            }
            return RedirectToAction("GoodsBuy");
        }


        public ActionResult GoodsTransferEnd(int id)
        {
            ITransferGoods transfer = System.Web.Mvc.DependencyResolver.Current.GetService<ITransferGoods>();
            var info = transfer.Get(id);
            if (info.PublishUserEmail == User.Identity.Name)
            {
                IGoodsTransferJob goods = System.Web.Mvc.DependencyResolver.Current.GetService<IGoodsTransferJob>();
                goods.End(id);
            }
            return RedirectToAction("GoodsTransfer");
        }



        public ActionResult HouseBuyEnd(int id)
        {
            IBuyHouse buy = System.Web.Mvc.DependencyResolver.Current.GetService<IBuyHouse>();
            var info = buy.Get(id);
            if (info.PublishUserEmail == User.Identity.Name)
            {
                IHouseBuyJob house = System.Web.Mvc.DependencyResolver.Current.GetService<IHouseBuyJob>();
                house.End(id);
            }
            return RedirectToAction("HouseBuy");
        }


        public ActionResult HouseTransferEnd(int id)
        {
            ITransferHouse transfer = System.Web.Mvc.DependencyResolver.Current.GetService<ITransferHouse>();
            var info = transfer.Get(id);
            if (info.PublishUserEmail == User.Identity.Name)
            {
                IHouseTransferJob car = System.Web.Mvc.DependencyResolver.Current.GetService<IHouseTransferJob>();
                car.End(id);
            }
            return RedirectToAction("HouseTransfer");
        } 
        #endregion


        #region 帖子删除
        public ActionResult CarBuyDelete(int id)
        {
            IBuyCar buy = System.Web.Mvc.DependencyResolver.Current.GetService<IBuyCar>();
            var info = buy.Get(id);
            if (info.PublishUserEmail == User.Identity.Name)
            {
                ICarBuyJob car = System.Web.Mvc.DependencyResolver.Current.GetService<ICarBuyJob>();
                car.Delete(id);
            }
            return RedirectToAction("CarBuy");

        }


        public ActionResult CarTransferDelete(int id)
        {
            ITransferCar transfer = System.Web.Mvc.DependencyResolver.Current.GetService<ITransferCar>();
            var info = transfer.Get(id);
            if (info.PublishUserEmail == User.Identity.Name)
            {
                ICarTransferJob car = System.Web.Mvc.DependencyResolver.Current.GetService<ICarTransferJob>();
                car.Delete(id);
            }
            return RedirectToAction("CarTransfer");
        }



        public ActionResult GoodsBuyDelete(int id)
        {
            IBuyGoods buy = System.Web.Mvc.DependencyResolver.Current.GetService<IBuyGoods>();
            var info = buy.Get(id);
            if (info.PublishUserEmail == User.Identity.Name)
            {
                IGoodsBuyJob goods = System.Web.Mvc.DependencyResolver.Current.GetService<IGoodsBuyJob>();
                goods.Delete(id);
            }
            return RedirectToAction("GoodsBuy");
        }


        public ActionResult GoodsTransferDelete(int id)
        {
            ITransferGoods transfer = System.Web.Mvc.DependencyResolver.Current.GetService<ITransferGoods>();
            var info = transfer.Get(id);
            if (info.PublishUserEmail == User.Identity.Name)
            {
                IGoodsTransferJob goods = System.Web.Mvc.DependencyResolver.Current.GetService<IGoodsTransferJob>();
                goods.Delete(id);
            }
            return RedirectToAction("GoodsTransfer");
        }



        public ActionResult HouseBuyDelete(int id)
        {
            IBuyHouse buy = System.Web.Mvc.DependencyResolver.Current.GetService<IBuyHouse>();
            var info = buy.Get(id);
            if (info.PublishUserEmail == User.Identity.Name)
            {
                IHouseBuyJob house = System.Web.Mvc.DependencyResolver.Current.GetService<IHouseBuyJob>();
                house.Delete(id);
            }
            return RedirectToAction("HouseBuy");
        }


        public ActionResult HouseTransferDelete(int id)
        {
            ITransferHouse transfer = System.Web.Mvc.DependencyResolver.Current.GetService<ITransferHouse>();
            var info = transfer.Get(id);
            if (info.PublishUserEmail == User.Identity.Name)
            {
                IHouseTransferJob car = System.Web.Mvc.DependencyResolver.Current.GetService<IHouseTransferJob>();
                car.Delete(id);
            }
            return RedirectToAction("HouseTransfer");
        } 
        #endregion
    }
}
