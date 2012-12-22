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
using Fx.Entity;
using FxCacheService.FxSite;
using FxUserCenter.Models;

namespace FxUserCenter.Controllers
{
#if DEBUG
#else
    [Authorize]
#endif
    public class AdminController : Controller
    {
        #region ctor
        protected GlobalCache gloablCache;
        protected ICarUserCenter carUserCenter;
        protected IGoodsUserCenter goodsUserCenter;
        protected IHouseUserCenter houseUserCenter;
        protected ITopShow topShow;
        protected IBuyGoods buyGoods;
        protected ITransferGoods transferGoods;
        protected IBuyCar buyCar;
        protected ITransferCar transferCar;
        protected IBuyHouse buyHouse;
        protected ITransferHouse transferHouse;
        protected ICarBuyJob carBuyJob;
        protected ICarTransferJob carTransferJob;
        protected IHouseBuyJob houseBuyJob;
        protected IHouseTransferJob houseTransferJob;
        protected IGoodsBuyJob goodsBuyJob;
        protected IGoodsTransferJob goodsTransferJob;
        public AdminController(GlobalCache gloablCache,
            ICarUserCenter carUserCenter,
            IGoodsUserCenter goodsUserCenter,
            IHouseUserCenter houseUserCenter,
            ITopShow topShow,
            IBuyGoods buyGoods,
            ITransferGoods transferGoods,
            IBuyCar buyCar,
            ITransferCar transferCar,
            IBuyHouse buyHouse,
            ITransferHouse transferHouse,
            ICarBuyJob carBuyJob,
            ICarTransferJob carTransferJob,
            IHouseBuyJob houseBuyJob,
            IHouseTransferJob houseTransferJob,
            IGoodsBuyJob goodsBuyJob,
            IGoodsTransferJob goodsTransferJob)
        {
#if DEBUG

#else
            if (User == null || User.Identity == null || 
                User.Identity.Name == null || !User.Identity.Name.Equals("117822597@163.com"))
            {
                RedirectPermanent("http://yingtao.co.uk");
            }
#endif
            this.gloablCache = gloablCache;
            this.carUserCenter = carUserCenter;
            this.houseUserCenter = houseUserCenter;
            this.goodsUserCenter = goodsUserCenter;
            this.topShow = topShow;
            this.buyGoods = buyGoods;
            this.transferGoods = transferGoods;
            this.buyCar = buyCar;
            this.transferCar = transferCar;
            this.buyHouse = buyHouse;
            this.transferHouse = transferHouse;
            this.carBuyJob = carBuyJob;
            this.carTransferJob = carTransferJob;
            this.houseBuyJob = houseBuyJob;
            this.houseTransferJob = houseTransferJob;
            this.goodsBuyJob = goodsBuyJob;
            this.goodsTransferJob = goodsTransferJob;
        }
        #endregion

        //
        // GET: /Admin/

        #region Menu
        public ActionResult About()
        {
#if DEBUG
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
#else
            if (User.Identity.Name.Equals("117822597@163.com"))
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
            else
            {
                return RedirectToAction("About", "UserCenter");
            }
#endif

        }


        public ActionResult GoodsBuy(int? page)
        {
            if (page.HasValue)
            {

                return View(goodsUserCenter.GetAdminBuys(page.Value));
            }
            else
            {

                return View(goodsUserCenter.GetAdminBuys(0));
            }
        }


        public ActionResult GoodsTransfer(int? page)
        {
            if (page.HasValue)
            {

                return View(goodsUserCenter.GetAdminTransfers(page.Value));
            }
            else
            {
                return View(goodsUserCenter.GetAdminTransfers(0));
            }
        }


        public ActionResult CarBuy(int? page)
        {
            if (page.HasValue)
            {
                return View(carUserCenter.GetAdminBuys(page.Value));
            }
            else
            {
                return View(carUserCenter.GetAdminBuys(0));
            }
        }


        public ActionResult CarTransfer(int? page)
        {
            if (page.HasValue)
            {
                return View(carUserCenter.GetAdminTransfers(page.Value));
            }
            else
            {
                return View(carUserCenter.GetAdminTransfers(0));
            }
        }


        public ActionResult HouseBuy(int? page)
        {
            if (page.HasValue)
            {
                return View(houseUserCenter.GetAdminBuys(page.Value));
            }
            else
            {
                return View(houseUserCenter.GetAdminBuys(0));
            }
        }


        public ActionResult HouseTransfer(int? page)
        {
            if (page.HasValue)
            {
                return View(houseUserCenter.GetAdminTransfers(page.Value));
            }
            else
            {
                return View(houseUserCenter.GetAdminTransfers(0));
            }
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
        #endregion




        #region 置顶
        public ActionResult CarTransferTopShow(int? page)
        {
            if (page.HasValue)
            {
                var car = transferCar.Get(page.Value);
                if (car != null)
                {
                    carTransferJob.NoDelete(page.Value);
                    this.topShow.TopShow(new Fx.Entity.FxAggregate.TopShow()
                    {
                        ChannelCatagroy = (int)ChannelCatagroy.FxCarTransfer,
                        HeadPicture = car.Pictures.Where(r => r.TransferPictureCatagroy == (int)PictureCatagroy.Head).First().ImageUrl,
                        InfoId = car.CarTransferInfoId,
                        Price = car.Price,
                        Title = car.PublishTitle,
                    });
                    return Content("成功置顶");
                }
            }
            return Content("置顶失败");
        }


        public ActionResult CarBuyTopShow(int? page)
        {
            if (page.HasValue)
            {
                var car = buyCar.Get(page.Value);
                if (car != null)
                {
                    carBuyJob.NoDelete(page.Value);
                    this.topShow.TopShow(new Fx.Entity.FxAggregate.TopShow()
                    {
                        ChannelCatagroy = (int)ChannelCatagroy.FxCarBuy,
                        HeadPicture = "",
                        InfoId = car.CarBuyInfoId,
                        Price = car.Price,
                        Title = car.PublishTitle,
                    });
                    return Content("此置顶无图片,成功置顶");
                }
            }
            return Content("置顶失败");
        }


        public ActionResult HouseTransferTopShow(int? page)
        {
            if (page.HasValue)
            {
                var house = transferHouse.Get(page.Value);
                if (house != null)
                {
                    houseTransferJob.NoDelete(page.Value);
                    this.topShow.TopShow(new Fx.Entity.FxAggregate.TopShow()
                    {
                        ChannelCatagroy = (int)ChannelCatagroy.FxHouseTrasnfer,
                        HeadPicture = house.Pictures.Where(r => r.TransferPictureCatagroy == (int)PictureCatagroy.Head).First().ImageUrl,
                        InfoId = house.HouseTransferInfoId,
                        Price = house.Price,
                        Title = house.PublishTitle,
                    });
                    return Content("成功置顶");
                }
            }
            return Content("置顶失败");
        }


        public ActionResult HouseBuyTopShow(int? page)
        {
            if (page.HasValue)
            {
                var house = buyHouse.Get(page.Value);
                if (house != null)
                {
                    houseBuyJob.NoDelete(page.Value);
                    this.topShow.TopShow(new Fx.Entity.FxAggregate.TopShow()
                    {
                        ChannelCatagroy = (int)ChannelCatagroy.FxHouseBuy,
                        HeadPicture = "",
                        InfoId = house.HouseBuyInfoId,
                        Price = house.Price,
                        Title = house.PublishTitle,
                    });
                    return Content("此置顶无图片,成功置顶");
                }
            }
            return Content("置顶失败");
        }

        public ActionResult GoodsTransferTopShow(int? page)
        {
            if (page.HasValue)
            {
                var goods = transferGoods.Get(page.Value);
                if (goods != null)
                {
                    goodsTransferJob.NoDelete(page.Value);
                    this.topShow.TopShow(new Fx.Entity.FxAggregate.TopShow()
                    {
                        ChannelCatagroy = (int)ChannelCatagroy.FxGoodsTransfer,
                        HeadPicture = goods.Pictures.Where(r => r.TransferPictureCatagroy == (int)PictureCatagroy.Head).First().ImageUrl,
                        InfoId = goods.GoodsTransferInfoId,
                        Price = goods.Price,
                        Title = goods.PublishTitle,
                    });
                    return Content("成功置顶");
                }
            }
            return Content("置顶失败");
        }

        public ActionResult GoodsBuyTopShow(int? page)
        {
            if (page.HasValue)
            {
                var goods = buyGoods.Get(page.Value);
                if (goods != null)
                {
                    goodsBuyJob.NoDelete(page.Value);
                    this.topShow.TopShow(new Fx.Entity.FxAggregate.TopShow()
                    {
                        ChannelCatagroy = (int)ChannelCatagroy.FxGoodsBuy,
                        HeadPicture = goods.Pictures.Where(r => r.BuyPictureCatagroy == (int)PictureCatagroy.Head).First().ImageUrl,
                        InfoId = goods.GoodsBuyInfoId,
                        Price = goods.Price,
                        Title = goods.PublishTitle,
                    });
                    return Content("成功置顶");
                }
            }
            return Content("置顶失败");
        }

        #endregion



        public ActionResult TopShow()
        {
            var model = new TopShowModel();
            model.TopShows = topShow.GetAllTopShow();
            return View(model);
        }



        public ActionResult CancleTopShow(int? page)
        {
            if (page.HasValue)
            {
                var entity = topShow.GetById(page.Value);
                if (entity != null)
                {
                    #region 还原帖子状态为publish
                    if (entity.ChannelCatagroy == (int)ChannelCatagroy.FxCarBuy)
                    {
                        carBuyJob.Publish(entity.InfoId);
                    }
                    else if (entity.ChannelCatagroy == (int)ChannelCatagroy.FxCarTransfer)
                    {
                        carTransferJob.Publish(entity.InfoId);
                    }
                    else if (entity.ChannelCatagroy == (int)ChannelCatagroy.FxGoodsBuy)
                    {
                        goodsBuyJob.Publish(entity.InfoId);
                    }
                    else if (entity.ChannelCatagroy == (int)ChannelCatagroy.FxGoodsTransfer)
                    {
                        goodsTransferJob.Publish(entity.InfoId);
                    }
                    else if (entity.ChannelCatagroy == (int)ChannelCatagroy.FxHouseBuy)
                    {
                        houseBuyJob.Publish(entity.InfoId);
                    }
                    else if (entity.ChannelCatagroy == (int)ChannelCatagroy.FxHouseTrasnfer)
                    {
                        houseTransferJob.Publish(entity.InfoId);
                    }
                    #endregion
                    //删除置顶信息
                    topShow.TopShowCancel(new Fx.Entity.FxAggregate.TopShow()
                    {
                        TopShowId = page.Value
                    });
                    return Content("置顶取消成功");
                }
            }
            return Content("置顶取消失败");
        }



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
