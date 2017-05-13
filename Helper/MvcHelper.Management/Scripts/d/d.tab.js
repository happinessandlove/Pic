/**************************************************
 * v1.1
 * by 丁浩
 * 2015-01-02
 **************************************************/
$(function ()
{
    $.fn.extend(
    {
        tab8:
            function (param)
            {
                var settings =
                    {
                        tabsContainer: '.tabs'
                        , tabItem: '.tab-item'
                        , currentItemClass: 'tab-item-current'
                        , tabpagesContainer: '.tabpages'
                        , tabpageItem: '.tabpage-item'
                        , triggerForm: 'click'
                        , defautItem: 1
						, focusEvent: null
						, blurEvent: null
                    };
                this.each(function ()
                {
                    var tab = $(this);
                    var attrs =
						{
						    tabsContainer: tab.attr('tabsContainer')
                            , tabItem: tab.attr('tabItem')
                            , currentItemClass: tab.attr('currentItemClass')
                            , tabpagesContainer: tab.attr('tabpagesContainer')
                            , tabpageItem: tab.attr('tabpageItem')
                            , defautItem: tab.attr('defautItem')
							, triggerForm: tab.attr('triggerform')
							, focusEvent: tab.attr('focusEvent')
							, blurEvent: tab.attr('blurEvent')
						};
                    var o = $.extend(true, {}, settings, attrs, param);

                    var tabsContainer = tab.find(o.tabsContainer);
                    var tabs = tab.find(o.tabItem);
                    var tabpagesContainer = tab.find(o.tabpagesContainer);
                    var tabpages = tab.find(o.tabpageItem);

                    if (o.triggerForm === 'hover')
                    {
                        tabs.hover(function ()
                        {
                            return focus($(this));
                        }, function ()
                        {
                            return blur($(this));
                        });
                    }
                    else if (o.triggerForm === 'click')
                    {
                        tabs.on('click', function ()
                        {
                            return focus($(this));
                        });
                    }
                    function focus(t)
                    {
                        if (!t.hasClass(o.currentItemClass))
                        {
                            tabs.removeClass(o.currentItemClass);
                            t.addClass(o.currentItemClass);
                            tabpages.hide();
                            var tabpage = tabpages.eq(tabs.index(t));
                            tabpage.show();
                            if (o.focusEvent != null) o.focusEvent(t, tabpage, tabs, tabpages, tab);
                        }
                        return false;
                    }
                    function blur(t)
                    {
                        if (o.blurEvent != null) o.blurEvent(t, tabs, tabpages, tab);
                        return false;
                    }
                    if (o.defautItem) focus(tabs.eq(o.defautItem - 1));
                });
            }
    })
});