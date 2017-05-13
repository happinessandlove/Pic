/**************************************************
 * v3.4
 * by 丁浩
 * 2015-04-07 
 * **************************************************/
var Framework = function (param)
{
    //==== 参数
    //=======================================================================================================================================
    var tid = 'tabid', mid = 'menuid';
    var o =
    {
        pageHeaderId: 'header'
        , pageMainId: 'main'
        , pageMainLeftId: 'left'
        , pageMainRightId: 'right'
        , tabContainerId: 'tab-container'
        , tabClass: 'tab'
        , tabIdPrefix: 'tab-'
        , currentTabClass: 'tab-h'
        , defaultTabClass: 'tab-default'
        , tabBaseWidth: 0 //50
        , tabTitleClass: 'tab-title'
        , tabCloseClass: 'tab-close'
        , tabpageContainerId: 'tabpage-container'
        , tabpageClass: 'tabpage'
        , tabpageIdPrefix: 'tabpage-'
        , currentTabpageClass: 'tabpage-h'
        , defaultTabpageClass: 'tabpage-default'
        , menuContainerId: 'menu-container'
        , menuGroupClass: 'menu-group'
        , menuGroupIdPrefix: 'menu-group-'
        , currentMenuGroupClass: 'menu-group-h'
        , menuItemsContainerClass: 'menu-items-container'
        , menuItemClass: 'menu-item'
        , menuItemIdPrefix: 'menu-'
        , currentMenuItemClass: 'menu-item-h'
        , btnCloseAllId: 'btn-close-all'
        , btnLogoutId: 'btn-logout'
        , maskClass: 'tabpage-mask'
        , loadingClass: 'tabpage-loading'
        , tabTitleWidthArrayName: 'widthArray'
        , toggleMenuGroup: true
        , maskClass: 'mask'
    };
    $.extend(true, o, param);

    //==== 选择器
    //=======================================================================================================================================
    var selector =
    {
        pageHeader: '#' + o.pageHeaderId
        , pageMain: '#' + o.pageMainId
        , pageMainLeft: '#' + o.pageMainLeftId
        , pageMainRight: '#' + o.pageMainRightId
        , tabContainer: '#' + o.tabContainerId
        , tab: '.' + o.tabClass
        , tabIdPrefix: '#' + o.tabIdPrefix
        , tabTitle: '.' + o.tabTitleClass
        , tabClose: '.' + o.tabCloseClass
        , tabpageContainer: '#' + o.tabpageContainerId
        , tabpage: '.' + o.tabpageClass
        , tabpageIdPrefix: '#' + o.tabpageIdPrefix
        , menuContainer: '.' + o.menuContainerId
        , menuGroup: '.' + o.menuGroupClass
        , menuGroupIdPrefix: '#' + o.menuGroupIdPrefix
        , menuItemsContainer: '.' + o.menuItemsContainerClass
        , menuItem: '.' + o.menuItemClass
        , menuItemId: '#' + o.menuItemIdPrefix
        , btnCloseAll: '#' + o.btnCloseAllId
        , btnLogout: '#' + o.btnLogoutId
        , mask: '.' + o.maskClass
        , loading: '.' + o.loadingClass
        , defaultTab: '.' + o.defaultTabClass
        , defaultTabpage: '.' + o.defaultTabpageClass
        , currentTab: '.' + o.currentTabClass
        , currentTabpage: '.' + o.currentTabpageClass
        , currentMenuGroup: '.' + o.currentMenuGroupClass
        , currrentMenuItem: '.' + o.currentMenuItemClass
        , mask: '.' + o.maskClass
    };

    //==== 元素对象
    //=======================================================================================================================================
    var elem =
    {
        window: $(window)
        , pageHeader: $(selector.pageHeader)
        , pageMain: $(selector.pageMain)
        , pageMainLeft: $(selector.pageMainLeft)
        , pageMainRight: $(selector.pageMainRight)
        , tabContainer: $(selector.tabContainer)
        , allTabs: function () { return $(selector.tab); }
        , allTabsWithoutDefault: function () { return elem.allTabs().not(selector.defaultTab); }
        , allTabsClose: function () { return $(selector.tabClose); }
        , tabpageContainer: $(selector.tabpageContainer)
        , allTabpages: function () { return $(selector.tabpage); }
        , allTabpagesWithoutDefault: function () { return elem.allTabpages().not(selector.defaultTabpage); }
        , menuContainer: $(selector.menuContainer)
        , allMenuGroups: $(selector.menuGroup)
        , allMenuItems: $(selector.menuItem)
        , btnCloseAll: $(selector.btnCloseAll)
        , btnLogout: $(selector.btnLogout)
        , currentTab: function () { return $(selector.currentTab); }
        , currentTabpage: function () { return $(selector.currentTabpage); }
        , currentMenuGroup: function () { return $(selector.currentMenuGroup); }
        , currentMenuItemsContainer: function () { return elem.findMenuItemsContainerByMenuItem(elem.currentMenuItem()); }
        , currentMenuItem: function () { return $(selector.currrentMenuItem); }
        , defaultTabs: function () { return $(selector.defaultTab); }
        , defalutTabpages: function () { return $(selector.defaultTabpage); }
        , findTabByTabId: function (tabId) { return $(selector.tabIdPrefix + tabId); }
        , findTabTitleByTab: function (tab) { return tab.find(selector.tabTitle); }
        , findTabpageByTabId: function (tabId) { return $(selector.tabpageIdPrefix + tabId); }
        , findTabpageByTab: function (tab) { return elem.findTabpageByTabId(tab.attr(tid)); }
        , findLoadingByTabpage: function (tabpage) { return tabpage.find(selector.loading); }
        , findMaskByTabpage: function (tabpage) { return tabpage.find(selector.mask); }
        , findIframeByTabpage: function (tabpage) { return tabpage.find('iframe') }
        , findIframeBodyByTabpage: function (tabpage) { return elem.findIframeByTabpage(tabpage).contents().find('body'); }
        , findIframeBodyByIframe: function (iframe) { return iframe.contents().find('body'); }
        , findMenuByMenuId: function (menuId) { return $(selector.menuItemId + menuId); }
        , findMenuByTab: function (tab) { return elem.findMenuByMenuId(tab.attr(mid)); }
        , findMenuItemsContainerByMenuItem: function (menuItem) { return menuItem.closest(selector.menuItemsContainer); }
        , findMenuItemsContainerByMenuId: function (menuId) { return elem.findMenuItemsContainerByMenuItem(elem.findMenuByMenuId(menuId)); }
        , findMenuItemsContainerByMenuGroup: function (menuGroup) { return menuGroup.next(selector.menuItemsContainer); }
        , findMenuGroupByMenuItem: function (menuItem) { return elem.findMenuItemsContainerByMenuItem(menuItem).prev(selector.menuGroup); }
        , findMenuGroupByMenuId: function (menuId) { return elem.findMenuGroupByMenuItem(elem.findMenuByMenuId(menuId)); }
        , findMenuGroupByMenuItemsContainer: function (menuItemsContainer) { return menuItemsContainer.prev(selector.menuGroup); }
        , findMaskByTabpage: function (tabpage) { return tabpage.find(selector.mask); }
        , findMaskByTabId: function (tabId) { return elem.findMaskByTabpage(elem.findTabpageByTabId(tabId)); }
    };
    var data =
    {
        windowWidth: function () { return elem.window.width(); }//当前的宽度
        , windowHeight: function () { return elem.window.height(); }//当前的高度
        , pageMainRightWidth: function () { return Math.floor(data.windowWidth() - elem.pageMainLeft.outerWidth(true)) }//应有的宽度
        , pageMainRightHeight: function () { return Math.floor(data.windowHeight() - elem.pageHeader.outerHeight(true)) }//应有的高度
        , tabContainerWidth: function () { return data.windowWidth() - elem.pageMainLeft.outerWidth(true); }//应有的宽度
        , defaultTabWidth: 0
    };

    //==== 初始化
    //=======================================================================================================================================

    if (elem.defaultTabs().length > 0)
    {
        $.each(elem.defaultTabs(), function ()
        {
            data.defaultTabWidth += $(this).outerWidth(true);
        });
    }
    elem.window.on('resize.framework', resize);
    resize(false);
    elem.allMenuGroups.on('click', menuGroupClick);
    elem.allMenuItems.on('click', menuItemClick);
    $(selector.tabContainer).on('click', selector.tab, tabClick);
    $(selector.tabContainer).on('click', selector.tabClose, tabCloseClick);
    elem.btnCloseAll.on('click', closeAllTab).hover(function () { var btnCloseAllTip = $('<span class="tip" id="btnCloseAll-tip">关闭所有标签</span>'); var position = $(this).position(); $('body').append(btnCloseAllTip.css({ 'top': position.top - 25, 'left': position.left - 20 })); }, function () { $('#btnCloseAll-tip').remove(); });
    elem.btnLogout.hover(function () { var btnLogoutTip = $('<span class="tip" id="btnLogout-tip">退出系统</span>'); var position = $(this).position(); $('body').append(btnLogoutTip.css({ 'top': position.top + 25, 'left': position.left - 24 })); }, function () { $('#btnLogout-tip').remove(); });

    //==== 框架功能（对外开放）
    //=======================================================================================================================================
    this.CreateTab = createTab;
    this.CloseTab = closeTab;
    ///刷新标签页 - 1.iframe页面createSuccess()、editSuccess()调用
    this.RefreshTab = function (tabId)
    {
        var tabpage = elem.findTabpageByTabId(tabId);
        if (tabpage.length == 0) return false;
        elem.findIframeByTabpage(tabpage)[0].contentWindow.subpage.Refresh(tabId);
        return false;
    }
    ///页面加载 - 1.iframe页面所有操作均调用
    this.ShowProgress = function (tabId)
    {
        var tabpage = elem.findTabpageByTabId(tabId);
        if (tabpage.length == 0) alert('错误，目标标签页不存在。位置：ShowProgress()。标签id：' + tabId); //调试
        showProgress(tabpage);
    };
    //==== 事件
    //=======================================================================================================================================
    function menuItemClick()
    {
        createTab($(this));
        return false;
    }

    function menuGroupClick()
    {
        switchMenuGroup($(this), elem.currentMenuGroup(), true);
        return false;
    }

    function tabClick()
    {
        var tab = $(this);
        switchTab(tab, elem.currentTab());
        switchTabpage(elem.findTabpageByTab(tab), elem.currentTabpage());
        var menuItem = elem.findMenuByTab(tab);
        switchMenuItem(menuItem, elem.currentMenuItem());
        switchMenuGroup(elem.findMenuGroupByMenuItem(menuItem), elem.currentMenuGroup());
        return false;
    }

    function tabCloseClick()
    {
        closeTab({ tab: $(this).closest(selector.tab) });
        return false;
    }

    //==== 框架功能（不对外开放）
    //=======================================================================================================================================

    ///添加标签
    ///link:发起创建新标签的超链接jquery对象
    function createTab(link)
    {
        var tabId = link.attr(tid), menuId = link.attr(mid), tabTitle = link.attr('tabtitle'), href = link.attr('href');
        var newTab = elem.findTabByTabId(tabId);
        var newTabpage = null;
        //创建标签页
        if (newTab.length == 0)
        {
            newTab = $('<dd id="tab-{0}" class="tab" style="display:none" tabid="{0}" menuid="{1}" title="{2}"><ul><li class="tab-head"><span class="tab-icon"></span></li><li class="tab-title">{2}</li><li class="tab-tail"><a class="tab-close" href="/"></a></li></ul></dd>'.replace(/\{0\}/g, tabId).replace(/\{1\}/g, menuId).replace(/\{2\}/g, tabTitle));
            elem.tabContainer.append(newTab);
            newTab.data(o.tabTitleWidthArrayName, getStrWidthArray(tabTitle));
            newTab.show();
            newTabpage = $('<div id="tabpage-{0}" tabid="{0}" class="tabpage"><div style="display: none" class="mask"></div><iframe frameborder="0" scrolling="auto" src="{1}" width="100%" height="100%"></iframe></div>'.replace(/\{0\}/g, tabId).replace(/\{1\}/g, href));
            elem.tabpageContainer.append(newTabpage);
            adjustTabWidth();
            showProgress(newTabpage);
        }
        else
            newTabpage = elem.findTabpageByTabId(tabId);
        //切换标签页
        switchTab(newTab, elem.currentTab());
        switchTabpage(newTabpage, elem.currentTabpage());
        switchMenuItem(elem.findMenuByMenuId(menuId), elem.currentMenuItem());
        switchMenuGroup(elem.findMenuGroupByMenuId(menuId), elem.currentMenuGroup());
        return false;
    }

    ///关闭标签页 - 1.点击标签的关闭按钮 2.子页面删除数据时关闭相关详情、编辑页
    ///target:{tab; tabpage;  tabId;}
    function closeTab(target)
    {
        var targetTab, targetTabpage;
        if (target.tab) targetTab = target.tab;
        else if (target.tabId) targetTab = elem.findTabByTabId(target.tabId);
        if (targetTab.length == 0) return;

        if (target.tabpage) targetTabpage = target.tabpage;
        else if (target.tabId) targetTabpage = elem.findTabpageByTabId(target.tabId);
        else targetTabpage = elem.findTabpageByTab(targetTab);

        var tabId = targetTab.attr(tid);
        var needSwitchTab = false, newHilightTab = null;
        if (targetTab.hasClass(o.currentTabClass))//当前高亮
        {
            needSwitchTab = true;
            var next = targetTab.next()
            if (next.hasClass(o.tabClass))//后面有标签页
                newHilightTab = next;
            else
            {
                var prev = targetTab.prev();
                if (prev.hasClass(o.tabClass))//前面有标签页
                    newHilightTab = prev;
            }
        }
        //销毁flash
        //var destroyUploadify = targetTabpage.find("iframe")[0].contentWindow.destroyUploadify;
        //if (destroyUploadify) destroyUploadify();
        targetTabpage.remove();
        targetTab.remove();
        //关闭前为前台标签页，需切换标签页
        if (needSwitchTab)
        {
            if (newHilightTab != null)
            {
                switchTab(newHilightTab, null);
                switchTabpage(elem.findTabpageByTab(newHilightTab), null);
                var menuItem = elem.findMenuByTab(newHilightTab);
                switchMenuItem(menuItem, elem.currentMenuItem());
                switchMenuGroup(elem.findMenuGroupByMenuItem(menuItem), elem.currentMenuGroup());
            }
            else
            {
                switchMenuItem(null, elem.currentMenuItem());
            }
        }
        adjustTabWidth();
    }

    ///关闭所有标签页
    function closeAllTab()
    {
        elem.allTabsWithoutDefault().remove();
        elem.allTabpagesWithoutDefault().remove();
        var defaultTabs = elem.defaultTabs();
        if (defaultTabs.length > 0)
        {
            if (!elem.currentTab().hasClass(o.defaultTabClass))
            {
                var tab = defaultTabs.last(), tabId = tab.attr(tid);
                switchTab(tab, null);
                switchTabpage(elem.findTabpageByTabId(tabId), null);
                var menuItem = elem.findMenuByTab(tab);
                switchMenuItem(menuItem, null);
                switchMenuGroup(elem.findMenuGroupByMenuItem(menuItem), null);
            }
        }
        else
        {
            switchMenuItem(null, elem.currentMenuItem());
        }
        adjustTabWidth();
        return false;
    }

    ///调整页面布局 - 1.总页面初始加载时调用 2.调整窗口大小时调用
    ///doAdjustIe6IframeSize: 是否调整标签页大小 （情况1时，无任何标签页，不调用）
    function resize(doAdjustIe6IframeSize)
    {
        elem.tabContainer.width(data.tabContainerWidth());
        elem.pageMainLeft.height(data.pageMainRightHeight);
        elem.pageMainRight.height(data.pageMainRightHeight).width(data.pageMainRightWidth);
        //调整标签大小
        adjustTabWidth();
        //调整iframe大小
        if (IE6 && doAdjustIe6IframeSize !== false)
            adjustIe6IframeSize(elem.currentTabpage());
    }

    ///切换标签页的标签
    ///tab：目标标签；currentTab：前台标签（null表示没有前台标签）
    function switchTab(tab, currentTab)
    {
        if (currentTab && currentTab.length > 0)
            currentTab.removeClass(o.currentTabClass);
        if (tab && tab.length > 0)
            tab.addClass(o.currentTabClass);
    }

    ///切换标签页的页面
    ///tabpage：目标标签页面；currentTabpage：前台标签页面（null表示没有前台标签页面）
    function switchTabpage(tabpage, currentTabpage)
    {
        if (currentTabpage && currentTabpage.length > 0)
            currentTabpage.removeClass(o.currentTabpageClass);
        if (tabpage && tabpage.length > 0)
            tabpage.addClass(o.currentTabpageClass);
        if (IE6) adjustIe6IframeSize(tabpage);
    }

    ///切换菜单项
    ///menuItem：目标菜单项（null表示取消前台菜单项）；currentMenuItem：前台菜单项（null表示没有前台菜单项）
    function switchMenuItem(menuItem, currentMenuItem)
    {
        //取消前台菜单项
        if (menuItem === null && (currentMenuItem && currentMenuItem.length > 0))
        {
            currentMenuItem.removeClass(o.currentMenuItemClass);
            return;
        }
        if (currentMenuItem && currentMenuItem.length > 0)
            currentMenuItem.removeClass(o.currentMenuItemClass);
        if (menuItem && menuItem.length > 0)
            menuItem.addClass(o.currentMenuItemClass);
    }

    ///切换菜单项
    ///menuGroup：目标菜单组；currentMenuGroup：前台菜单组（null表示没有前台菜单组）；fromClickMenuGroup：是否是点击菜单组触发
    function switchMenuGroup(menuGroup, currentMenuGroup, fromClickMenuGroup)
    {
        if (menuGroup && menuGroup.length > 0 && menuGroup.hasClass(o.currentMenuGroupClass))
        {
            //点击已高亮的菜单组
            if (fromClickMenuGroup)
            {
                menuGroup.removeClass(o.currentMenuGroupClass);
                if (o.toggleMenuGroup) elem.findMenuItemsContainerByMenuGroup(menuGroup).slideUp(300);
            }
        }
        else
        {
            if (currentMenuGroup && currentMenuGroup.length > 0)
            {
                if (o.toggleMenuGroup) elem.findMenuItemsContainerByMenuGroup(currentMenuGroup).slideUp(300);
                currentMenuGroup.removeClass(o.currentMenuGroupClass);
            }
            if (menuGroup && menuGroup.length > 0) menuGroup.addClass(o.currentMenuGroupClass);
            if (o.toggleMenuGroup) elem.findMenuItemsContainerByMenuGroup(menuGroup).slideDown(300);
        }
    }

    ///显示操作等待浮动层
    ///tabpage：目标标签页面
    function showProgress(tabpage)
    {
        if (tabpage.length == 0) alert('错误，目标标签页不存在。位置：showProgress()。'); //调试
        var masker = elem.findMaskByTabpage(tabpage);
        masker.fadeTo(0, 0.8);
        var loading = createLoadingDialog({ appendTo: tabpage, maskerTarget: false, id: 'loading' + getRandom(), esc: true, follow: tabpage, top: '30%', adaptivePosition: true });
        elem.findIframeByTabpage(tabpage).one('load', function ()
        {
            adjustIe6IframeSize(tabpage);
            if (loading) loading.Close();
            if (masker) masker.hide();
        });
    };

    //==== 内部方法
    //=======================================================================================================================================

    ///调整标签页大小 - 1.resize()非总页面初始加载时调用 2.switchTabpage()前台显示标签页面时调用 3.refreshTab()页面加载完毕后调用 4.翻页、显示所有记录、增、查、改、删等操作完成页面重新加载完毕后均调用
    ///tabpage: 目标标签页
    function adjustIe6IframeSize(tabpage)
    {
        if (IE6 && tabpage)
        {
            var iframe = elem.findIframeByTabpage(tabpage);
            var th = iframe.height();
            var w = data.pageMainRightWidth(), h = data.pageMainRightHeight();
            var ibody = elem.findIframeBodyByTabpage(iframe), ih = ibody.height();
            if (th < ih) w -= 17;
            ibody.width(w).height(h);
        }
    }

    ///调整标签宽度 - 1.resize()调用 2.createTab()调用 3.closeTab()调用 4.closeAllTab()调用
    function adjustTabWidth()
    {
        var tabs = elem.allTabsWithoutDefault(), tabsCount = tabs.length;
        var pointWidth = 6;
        if (tabsCount > 0)
        {
            var tabContainerWidth = data.tabContainerWidth() - tabsCount * o.tabBaseWidth - data.defaultTabWidth;
            var everyTabWidth = tabContainerWidth <= 0 ? 0 : Math.floor(tabContainerWidth / tabsCount);
            var currentTabsWidth = 0, maxTabsWidth = 0, widthArrays = [];
            tabs.each(function (i)
            {
                var t = $(this);
                currentTabsWidth += elem.findTabTitleByTab(t).outerWidth(true);
                widthArrays[i] = t.data(o.tabTitleWidthArrayName);
                maxTabsWidth += widthArrays[i][widthArrays[i].length - 1];
            });
            if (currentTabsWidth >= maxTabsWidth && maxTabsWidth <= tabContainerWidth)
            { }
            else
            {
                tabs.each(function (i)
                {
                    var t = $(this), title = elem.findTabTitleByTab(t), titleText = t.attr('title');
                    if (tabContainerWidth >= maxTabsWidth) title.html(titleText);
                    else if (everyTabWidth >= widthArrays[i][widthArrays[i].length - 1]) title.html(titleText);
                    else if (everyTabWidth < pointWidth) title.html('');
                    else
                    {
                        for (var j = 0; j < widthArrays[i].length; j++)
                        {
                            if (pointWidth + widthArrays[i][j] > everyTabWidth) break;
                        }
                        title.html(titleText.substr(0, j) + '..');
                    }
                });
            }
        }
    }

    /////日期时间初始化
    //function getDatetime()
    //{
    //    var d = new Date(), y = d.getFullYear(), M = (d.getMonth() + 1), D = d.getDate(), day = ['日', '一', '二', '三', '四', '五', '六'], h = d.getHours(), m = d.getMinutes(), s = d.getSeconds();
    //    elem.datetime.text(y + '-' + (('' + M).length == 1 ? '0' + M : M) + '-' + (('' + D).length == 1 ? '0' + D : D) + ' ' + (('' + h).length == 1 ? '0' + h : h) + ':' + (('' + m).length == 1 ? '0' + m : m) + ':' + (('' + s).length == 1 ? '0' + s : s) + ' 星期' + day[d.getDay()]);
    //}

    ///获取字符串显示宽度
    function getStrWidthArray(str)
    {
        var cl = [];
        if (IE6) cl = [3, 3, 4, 7, 7, 11, 8, 2, 4, 4, 5, 7, 3, 4, 3, 3, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 3, 3, 7, 7, 7, 7, 12, 8, 8, 9, 9, 8, 7, 9, 9, 3, 6, 8, 7, 10, 9, 9, 8, 9, 9, 8, 7, 9, 8, 11, 8, 8, 7, 3, 3, 3, 6, 7, 4, 7, 7, 6, 7, 7, 3, 7, 7, 3, 3, 6, 3, 10, 7, 7, 7, 7, 4, 6, 3, 7, 6, 9, 6, 6, 6, 4, 3, 4, 7];
        else cl = [4, 4, 5, 8, 7, 11, 10, 3, 4, 4, 5, 9, 3, 5, 3, 5, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 3, 3, 9, 9, 9, 6, 12, 8, 8, 8, 9, 7, 6, 9, 9, 4, 5, 8, 6, 12, 10, 10, 7, 10, 8, 7, 7, 9, 8, 12, 8, 7, 7, 4, 5, 4, 9, 5, 4, 7, 8, 6, 8, 7, 4, 8, 7, 3, 3, 7, 3, 11, 7, 8, 8, 8, 5, 6, 4, 7, 6, 9, 6, 6, 6, 4, 3, 4, 9];
        var w = 0, i;
        var widthArray = [];
        for (i = 0; i < str.length; i++)
        {
            var asc = str.charCodeAt(i);
            if (asc > 255) w += 12;
            else if (asc < 127 && asc >= 32) w += cl[asc - 32];
            else w += 8;
            widthArray.push(w);
        }
        return widthArray;
    }
};