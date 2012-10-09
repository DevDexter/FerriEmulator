using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferri_Emulator.Messages
{
    internal struct Opcodes
    {
        internal struct OpcodesIn
        {
            internal const short ReadRelease = 4000; 
            internal const short InitCrypto = 3738; 
            internal const short InitRC4 = 2566;
            internal const short AuthenticateUser = 1418;
            internal const short GetUser = 1021;
            internal const short GetHotelview = 1844;
            internal const short GetUNK1 = 435; 
            internal const short UnlockSafetyQuiz = 2533; //
            internal const short GetPurse = 2843;
            internal const short GetCampaignWinners = 1256;
            internal const short GetNavigatorFrontpage = 2444;
            internal const short GetOwnRooms = 2549;
            internal const short GetShopTabs = 1073;
            internal const short GetRecyclerRewards = 2792;
            internal const short GetShopData = 3231;
            internal const short GetGiftWrappings = 3302;
            internal const short GetBundleDiscount = 502;
            internal const short GetShopPage = 2591;
            internal const short GetVIPBuyDialog = 3962;
            internal const short EnterRoom = 724;
            internal const short GetSearchResults = 1691;
            internal const short GetRoomGroupData = 267;
            internal const short GetRoomModelData = 3909;
            internal const short GetEndEnterRoom = 1085;
        }

        internal struct OpcodesOut
        {
            internal const short SendToken = 3984;
            internal const short SendPublicKey = 2432;
            internal const short SendInterface = 2810;
            internal const short SendMinimailCount = 990;
            internal const short SendHomeRoom = 1607;
            internal const short SendFavourites = 1477; 
            internal const short SendFuserights = 2249; 
            internal const short SendActivityPoints = 3709; //
            internal const short SendFriends = 1564; 
            internal const short SendEffects = 2721; // 
            internal const short SendUserInformation = 3060;
            internal const short SendInventory = 2011; 
            internal const short SendCreditBalance = 3398;
            internal const short SendSubscriptionData = 1351;
            internal const short SendBadgePointLimits = 3903;
            internal const short SendHotelView = 2426; 
            internal const short SendGamesDataUNK = 3008; // 
            internal const short SendPrivateCategories = 2319; 
            internal const short SendFriendbarUpdate = 1473; //
            internal const short SendIlluminaAlert = 764; 
            internal const short SendModTool = 2714; //
            internal const short SendMessageOfTheDay = 1137;
            internal const short SendCampaignWinners = 803;
            internal const short SendNavigatorFrontpage = 415;
            internal const short SendWelcomeContainer = 951;
            internal const short SendRoomDataNavigator = 2052;
            internal const short SendShopTabs = 3261;
            internal const short SendRecyclerRewards = 3232;
            internal const short SendShopData = 822;
            internal const short SendGiftWrappings = 46;
            internal const short SendBundleDiscount = 963;
            internal const short SendShopPage = 1323;
            internal const short SendVIPBuyDialog = 3173;
            internal const short SendCitizenship = 3404;
            internal const short SendAchievementUpdate = 1343;
            internal const short SendRoomInitialize = 1125;
            internal const short SendRoomModelInfo = 1618;
            internal const short SendRoomDecoration = 1841;
            internal const short SendRoomScore = 129;
            internal const short SendRoomEventData = 1953;
            internal const short SendAdsFurni = 1862; // ???
            internal const short SendRoomGroupBadge = 824;
            internal const short SendRoomUserRightLevel = 1292;
            internal const short SendRoomOwnerShip = 303;
            internal const short SendRoomModeldata = 2658;
            internal const short SendRoomRelativeModeldata = 3061;
            internal const short SendUserOutofRoom = 1125;
            internal const short SendDoorbellNoAnswer = 354;
            internal const short SendWrongPassword = 565;
            internal const short SendUNK1 = 3888;
        }
    }
}
