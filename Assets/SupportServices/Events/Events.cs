/*
Примеры событий
using System;

public class OnRepaintAwake { public IRepaint Marker; }
public class OnUpdateCoin { public BigNumber newCount; }
public class OnUpgrade { }
public class OnOpen { }
public class OnUpdatePassiveIncome { public BigNumber newCount; }
public class OnUpdateInvestor { public BigNumber newCount; }
public class OnUpfateFullMineContainer { }
public class OnUpfateFullLiftContainer { }
public class OnSelectLinz { public ILinzPanelService linzPanel; }

public class OnAddLinz
{
    public Type linzPanelService;
    public LinzComponent linzComponent;

    public OnAddLinz(Type linzPanelService, LinzComponent linzComponent)
    {
        this.linzPanelService = linzPanelService;
        this.linzComponent = linzComponent;
    }
}

*/

public class OnFocus { public bool status; }
public class OnMarkerAwake { public IMarker Marker; }
public class OnChangeAudioValue { }

public class OnDepositCoin { public BigNumber count; }

public class OnTakeGift { public GiftData giftData; }