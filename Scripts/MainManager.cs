using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;
using UnityEngine.Events;
using UnityEngine.Purchasing;

public class MainManager : MonoBehaviour
{
    public IAPButton btn5000;

    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public GameObject popup;
    public GameObject inapppopup;
    public delegate void Callback(string msg, int val);

    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;

    private Callback callback;

    // Start is called before the first frame update
    void Start()
    {
        MobileAds.Initialize(initStatus => {
            Debug.Log("initialize end");
            this.RequestBanner();
            this.RequestInterstitial();
            this.RequestRewardAD();
        });
        button1.onClick.AddListener(ShowPopup);
        button2.onClick.AddListener(RunInApp);
        button3.onClick.AddListener(ShowAD);
        button4.onClick.AddListener(ShowRAD);

    }

    // Update is called once per frame
    void Update()
    {
    }
    void openPopup(string msg)
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject pp = Instantiate(popup, canvas.transform);
        GameObject text = pp.transform.Find("PopupBase").Find("Message").gameObject;
        text.GetComponent<Text>().text = msg;
        Button button = pp.transform.Find("PopupBase").Find("Button").GetComponent<Button>();
        button.onClick.AddListener(new UnityAction(() => {
            Destroy(pp);
        }));
    }
    void ShowPopup()
    {
        openPopup("팝업 테스트");
    }
    void RunInApp()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject pp = Instantiate(inapppopup, canvas.transform);
        IAPButton buy = pp.transform.Find("Button_Buy").GetComponent<IAPButton>();
        buy.onPurchaseComplete.AddListener(new UnityAction<Product>((product) =>
       {
           BuyInApp(5000);
           Destroy(pp);
       }));
        buy.onPurchaseFailed.AddListener(new UnityAction<Product, PurchaseFailureReason>((product, reason) =>
        {
            BuyInAppFailed(product.transactionID);
            Destroy(pp);
        }));
        Button button = pp.transform.Find("Button_Cancel").GetComponent<Button>();
        button.onClick.AddListener(new UnityAction(() =>
        {
            Destroy(pp);
        }));
    }
    public void BuyInApp(int credits)
    {
        Debug.Log("Buy In App");
        openPopup(string.Format("통행세 {0}원을 지불했습니다.", credits));
    }
    public void BuyInAppFailed(string msg)
    {
        openPopup(string.Format("구매 실패\nID:{0}", msg));
        Debug.Log("Buy In App");
    }

    void ShowAD()
    {
        //Show Interstitial Ad
        callback = (string msg, int val) =>
        {
            Debug.Log(msg + " " + val.ToString());
            if (msg == "Ad Closed")
                RequestInterstitial();
        };

        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
        else
        {
            callback("not loaded yet", 0);
        }
    }
    void ShowRAD()
    {
        //Show Reward Ad
        callback = (string msg, int val) =>
        {
            Debug.Log(msg + " " + val.ToString());
            switch(msg)
            {
                case "Ad Closed":
                    RequestRewardAD();
                    break;
                case "Reward Earned":
                    //Reward stock
                    GameObject canvas = GameObject.Find("Canvas");
                    GameObject pp = Instantiate(popup, canvas.transform);
                    GameObject text = pp.transform.Find("PopupBase").Find("Message").gameObject;
                    text.GetComponent<Text>().text = msg + " " + val.ToString();
                    RequestRewardAD();
                    break;
            }
        };

        if (this.rewardedAd.IsLoaded())
            this.rewardedAd.Show();
        else
            callback("not loaded yet", 0);

    }

    private void RequestRewardAD()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
#else
        string adUnitId = "unexpected platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);

        this.rewardedAd.OnAdLoaded += this.HandleOnAdLoaded;
        this.rewardedAd.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        this.rewardedAd.OnAdOpening += this.HandleOnAdOpened;
        this.rewardedAd.OnAdClosed += this.HandleOnAdClosed;
        this.rewardedAd.OnAdFailedToShow += this.HandleOnAdFailedToShow;
        this.rewardedAd.OnUserEarnedReward += this.HandleOnUserEarnedReward;

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
    }
    private void RequestInterstitial()
    {

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#else
        string adUnitId = "unexpected platform";
#endif
        this.interstitial = new InterstitialAd(adUnitId);

        this.interstitial.OnAdLoaded += this.HandleOnAdLoaded;
        this.interstitial.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        this.interstitial.OnAdOpening += this.HandleOnAdOpened;
        this.interstitial.OnAdClosed += this.HandleOnAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }

    private void RequestBanner()
    {
        Debug.Log("RequestBanner");

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/6300978111";
#else
        string adUnitId = "unexpected platform";
#endif
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.bannerView.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.ToString());
    }

    public void HandleOnAdOpened(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        //RequestInterstitial();
        if (callback != null)
            callback("Ad Closed", 1);
    }

    public void HandleOnAdLeavingApplication(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    public void HandleOnAdFailedToShow(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleOnAdFailedToShow event received");
    }
    public void HandleOnUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print("HandleOnUserEarnedReward event received for "
            + amount.ToString() + " " + type);

        if (callback != null)
            callback("Reward Earned", 2);
    }
}
