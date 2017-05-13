/**************************************************
 * v3.7.1
 * by 丁浩
 * 2015-07-25
 * **************************************************/
var Fw = self.parent.framework;
var Subpage = function (param)
{
    var tid = 'tabid', mid = 'menuid';
    var SortDirection = { Ascending: 'Ascending', Descending: 'Descending' };
    var OpType = { Null: 0, Pager: 1, Query: 2, Sort: 3, Delete: 4, Deletes: 5, Showall: 6, RankUp: 7 };
    //var FwOpType = { RefreshTab: 'RefreshTab', OpenNewTab: 'OpenNewTab', LoadInSelfTab: 'LoadInSelfTab', SubmitForm: 'SubmitForm', ReturnParentTab: 'ReturnParentTab', Create: 'Create', Edit: 'Edit', Delete: 'Delete', Deletes: 'Deletes', Query: 'Query', Sort: 'Sort', ShowAll: 'ShowAll', RankUp: 'RankUp', Custom: 'Custom' };

    //#### 参数 ##########################################################################################################################################################################
    var o =
    {
        toolsContainerId: 'tools'
        , toolRefreshClass: 'tool-refresh'
        , toolCreateClass: 'tool-create'
        , toolDeletesClass: 'tool-deletes'
        , toolQueryClass: 'tool-query'
        , toolShowAllClass: 'tool-showAll'
        , toolReturnClass: 'tool-return'
        , toolCustomClass: 'tool-custom'
        , toolHilightClass: 'tool-hilight'
        , toolDisableClass: 'tool-disable'
        , linkPagerAttribute: 'page'
        , linkCustomClass: 'link-custom'
        , buttonDetailsClass: 'button-details'
        , buttonRankClass: 'button-rank'
        , buttonEditClass: 'button-edit'
        , buttonDeleteClass: 'button-delete'
        , buttonCustomClass: 'button-custom'
        , submitCreateClass: 'submit-create'
        , submitEditClass: 'submit-edit'
        , customPreEventName: 'preEvent'
        , customPostEventName: 'postEvent'
        , selectPagerClass: 'pager-select'
        , selectAllId: 'select-all'
        , selectItemClass: 'select-item'
        , listTableClass: 'list-table'
        , evenRowClass: 'even-row'
        , rowHoverClass: 'row-hover'
        , sortColumnHeaderClass: 'sort'
        , sortColumnPropertyAttribute: 'property'
        , textboxFocusClass: 'text-focus'
        , validationErrorMessageClass: 'field-validation-error'
        , validationErrorInputClass: 'input-validation-error'
        , validationTag: 'span'
        , datepickerClass: 'datepicker'
        , opParamId: { opType: 'OpType', opArgument: 'OpArgument', opPager: 'OpPager', opQueryString: 'OpQueryString', opSortProperty: 'OpSortProperty', opSortDirection: 'OpSortDirection' }
        , query:
        {
            dialogId: 'query-dialog'
            , dialogHeaderId: 'dialog-header'
            , itemContainerId: 'query-items'
            , itemClass: 'query-item'
            , buttonRemoveItemClass: 'remove-query-item'
            , buttonAddItemClass: 'add-query-item'
            , submitQueryId: 'submit-query'
            , logic: { elemClass: 'query-logic', containerClass: 'col-query-logic', nameAttribute: 'queryLogic', type: { And: 'And', Or: 'Or' } }
            , property: { elemClass: 'query-property', containerClass: 'col-query-property', nameAttribute: 'queryProperty', typeAttribute: 'type', type: { 'Gender': 'Gender', 'String': 'String', 'Numerical': 'Numerical', 'DateTime': 'DateTime', 'Boolean': 'Boolean', 'Select': 'Select' } }
            , condition: { elemClass: 'query-condition', containerClass: 'col-query-condition', nameAttribute: 'queryCondition', propertyTypeAttribute: 'propertytype', type: { 'Equal': 'Equal', 'Contains': 'Contains', 'StartsWith': 'StartsWith', 'EndsWith': 'EndsWith', 'LessThan': 'LessThan', 'LessThanOrEqual': 'LessThanOrEqual', 'GreaterThan': 'GreaterThan', 'GreaterThanOrEqual': 'GreaterThanOrEqual', 'Between': 'Between' } }
            , value: { elemClass: 'query-value', containerClass: 'col-query-value', nameAttribute: 'queryValue' }
            , value1: { elemClass: 'query-value1', containerClass: 'col-query-value', nameAttribute: 'queryValue1' }
            , value2: { elemClass: 'query-value2', containerClass: 'col-query-value', nameAttribute: 'queryValue2' }
            , templateId: 'query-template'
            , selectTemplate: { containerId: 'query-template-select', propertyNameAttribute: 'propertyname' }
        }
    };
    $.extend(true, o, param);

    //#### 选择器 ##########################################################################################################################################################################
    var selector =
    {
        toolsContainer: '#' + o.toolsContainerId
        , toolRefresh: '.' + o.toolRefreshClass
        , toolCreate: '.' + o.toolCreateClass
        , toolQuery: '.' + o.toolQueryClass
        , toolShowAll: '.' + o.toolShowAllClass
        , toolReturn: '.' + o.toolReturnClass
        , toolCustom: '.' + o.toolCustomClass
        , linkPager: '[' + o.linkPagerAttribute + ']'
        , linkCustom: '.' + o.linkCustomClass
        , buttonDetails: '.' + o.buttonDetailsClass
        , buttonRank: '.' + o.buttonRankClass
        , buttonEdit: '.' + o.buttonEditClass
        , buttonDelete: '.' + o.buttonDeleteClass
        , buttonDeletes: '.' + o.toolDeletesClass
        , buttonCustom: '.' + o.buttonCustomClass + ':not(' + '.' + o.buttonDetailsClass + ',.' + o.buttonRankClass + ',.' + o.buttonEditClass + ',.' + o.buttonDeleteClass + ',.' + o.toolDeletesClass + ')'
        , submitCreate: '.' + o.submitCreateClass
        , submitEdit: '.' + o.submitEditClass
        , selectPager: '.' + o.selectPagerClass
        , selectAll: '#' + o.selectAllId
        , selectItem: '.' + o.selectItemClass
        , listTable: '.' + o.listTableClass
        , sortColumnHeader: '.' + o.sortColumnHeaderClass
        , textbox: '.' + o.textbox
        , validationErrorMessage: '.' + o.validationErrorMessageClass
        , validationErrorInput: '.' + o.validationErrorInputClass
        , validationTag: o.validationTag
        , datepicker: '.' + o.datepickerClass
        , opParam: { opType: 'input#' + o.opParamId.opType, opArgument: 'input#' + o.opParamId.opArgument, opPager: 'input#' + o.opParamId.opPager, opQueryString: 'input#' + o.opParamId.opQueryString, opSortProperty: 'input#' + o.opParamId.opSortProperty, opSortDirection: 'input#' + o.opParamId.opSortDirection }
        , query:
            {
                dialog: '#' + o.query.dialogId
                , dialogHeader: '#' + o.query.dialogId + ' #' + o.query.dialogHeaderId
                , itemContainer: '#' + o.query.dialogId + ' #' + o.query.itemContainerId
                , item: '.' + o.query.itemClass
                , buttonRemoveItem: '.' + o.query.buttonRemoveItemClass
                , buttonAddItem: '#' + o.query.dialogId + ' .' + o.query.buttonAddItemClass
                , submitQuery: '#' + o.query.dialogId + ' #' + o.query.submitQueryId
                , logic: { elem: '.' + o.query.logic.elemClass, container: '.' + o.query.logic.containerClass }
                , property: { elem: '.' + o.query.property.elemClass, container: '.' + o.query.property.containerClass }
                , condition: { elem: '.' + o.query.condition.elemClass, container: '.' + o.query.condition.containerClass }
                , value: { elem: '.' + o.query.value.elemClass, container: '.' + o.query.value.containerClass }
                , value1: { elem: '.' + o.query.value1.elemClass, container: '.' + o.query.value1.containerClass }
                , value2: { elem: '.' + o.query.value2.elemClass, container: '.' + o.query.value2.containerClass }
                , template: '#' + o.query.templateId
                , selectTemplate: '#' + o.query.selectTemplate.containerId
            }
    };
    //#### 元素对象 ##########################################################################################################################################################################
    var elem =
        {
            window: $("window")
            , form: function (selector) { if (!selector) selector = ''; return $("form" + selector); }
            , toolsContainer: $(selector.toolsContainer)
            , toolRefresh: function () { return $(selector.toolRefresh); }
            , toolCreate: function () { return $(selector.toolCreate); }
            , toolQuery: function () { return $(selector.toolQuery); }
            , toolShowAll: function () { return $(selector.toolShowAll); }
            , toolReturn: function () { return $(selector.toolReturn); }
            , toolCustom: function () { return $(selector.toolCustom); }
            , linkPager: function () { return $(selector.linkPager); }
            , linkCustom: function () { return $(selector.linkCustom); }
            , buttonDetails: function () { return $(selector.buttonDetails); }
            , buttonRank: function () { return $(selector.buttonRank); }
            , buttonEdit: function () { return $(selector.buttonEdit); }
            , buttonDelete: function () { return $(selector.buttonDelete); }
            , buttonDeletes: function () { return $(selector.buttonDeletes); }
            , buttonCustom: function () { return $(selector.buttonCustom); }
            , submitCreate: function () { return $(selector.submitCreate); }
            , submitEdit: function () { return $(selector.submitEdit); }
            , selectPager: function () { return $(selector.selectPager); }
            , selectAll: function () { return $(selector.selectAll); }
            , selectItem: function () { return $(selector.selectItem); }
            , selectedItem: function () { return $(selector.selectItem + ':checked'); }
            , listTable: $(selector.listTable)
            , sortColumnHeader: function (propertyName) { if (propertyName) return $(selector.sortColumnHeader).filter('[' + o.sortColumnPropertyAttribute + '="' + propertyName + '"]'); else return $(selector.sortColumnHeader) }
            , validationErrorMessage: function () { return $(selector.validationErrorMessage); }
            , validationErrorInput: function () { return $(selector.validationErrorInput); }
            , datepicker: function () { return $(selector.datepicker); }
            , opParam:
            {
                getOpType: function () { return $(selector.opParam.opType).val(); }
                , getOpArgument: function () { return $(selector.opParam.opArgument).val(); }
                , getOpPager: function () { return $(selector.opParam.opPager).val(); }
                , getOpQueryString: function () { return $(selector.opParam.opQueryString).val(); }
                , getOpSortProperty: function () { return $(selector.opParam.opSortProperty).val(); }
                , getOpSortDirection: function () { return $(selector.opParam.opSortDirection).val(); }
                , setOpType: function (opType) { $(selector.opParam.opType).val(opType); }
                , setOpArgument: function (opArgument) { $(selector.opParam.opArgument).val(opArgument); }
                , setOpPager: function (opPager) { $(selector.opParam.opPager).val(opPager); }
                , setOpQueryString: function (opQueryString) { $(selector.opParam.opQueryString).val(opQueryString); }
                , setOpSortProperty: function (opSortProperty) { $(selector.opParam.opSortProperty).val(opSortProperty); }
                , setOpSortDirection: function (opSortDirection) { $(selector.opParam.opSortDirection).val(opSortDirection); }
                , set: function (opType, opArgument, opQueryString, opPager, opSortProperty, opSortDirection)
                {
                    if (opType != null) elem.opParam.setOpType(opType);
                    if (opArgument != null) elem.opParam.setOpArgument(opArgument);
                    if (opQueryString != null) elem.opParam.setOpQueryString(opQueryString);
                    if (opPager != null) elem.opParam.setOpPager(opPager);
                    if (opSortProperty != null) elem.opParam.setOpSortProperty(opSortProperty);
                    if (opSortDirection != null) elem.opParam.setOpSortDirection(opSortDirection);
                }
            }
            , query:
            {
                dialog: function () { return $(selector.query.dialog); }
                , dialogHeader: function () { return $(selector.query.dialogHeader); }
                , itemContainer: function () { return $(selector.query.itemContainer); }
                , item: function (index) { if (typeof index === 'number') return elem.query.dialog().find(selector.query.item).eq(index); else return elem.query.dialog().find(selector.query.item); }
                , buttonAddItem: function () { return $(selector.query.buttonAddItem); }
                , submitQuery: function () { return $(selector.query.submitQuery); }
                , logic: { container: function (row) { return row.find(selector.query.logic.container); }, elem: function (row) { if (row) return row.find(selector.query.logic.elem); else return elem.query.dialog().find(selector.query.logic.elem); } }
                , property: { container: function (row) { return row.find(selector.query.property.container); }, elem: function (row) { if (row) return row.find(selector.query.property.elem); else return elem.query.dialog().find(selector.query.property.elem); } }
                , condition: { container: function (row) { return row.find(selector.query.condition.container); }, elem: function (row) { if (row) return row.find(selector.query.condition.elem); else return elem.query.dialog().find(selector.query.condition.elem); } }
                , value: { container: function (row) { return row.find(selector.query.value.container); }, elem: function (row) { if (row) return row.find(selector.query.value.elem); else return elem.query.dialog().find(selector.query.value.elem); } }
                , value1: { container: function (row) { return row.find(selector.query.value1.container); }, elem: function (row) { if (row) return row.find(selector.query.value1.elem); else return elem.query.dialog().find(selector.query.value1.elem); } }
                , value2: { container: function (row) { return row.find(selector.query.value2.container); }, elem: function (row) { if (row) return row.find(selector.query.value2.elem); else return elem.query.dialog().find(selector.query.value2.elem); } }
                , template: function () { return $(selector.query.template + ' tr'); }
                , selectTemplate: function (propertyName) { return $(selector.query.selectTemplate + " [" + o.query.selectTemplate.propertyNameAttribute + "='" + propertyName + "']"); }
            }
        };
    //#### 事件注册 ##########################################################################################################################################################################
    //refresh
    elem.toolRefresh().on('click', function () { refresh(); return false; });
    //create
    elem.toolCreate().on('click', function () { Fw.CreateTab($(this)); return false; });
    //deletes
    elem.buttonDeletes().on('click', function () { deleteDatas(); return false; });
    //query & showAll
    initQuery();
    //return
    elem.toolReturn().on('click', function () { Fw.CreateTab($(this)); return false; });
    //custom
    elem.toolCustom().on('click', function () { Fw.CreateTab($(this)); return false; });
    //pager
    elem.linkPager().on('click', function () { pager($(this).attr(o.linkPagerAttribute)); return false; });
    elem.selectPager().on('change', function () { pager($(this).val()); return false; });
    //details
    elem.buttonDetails().on('click', function () { Fw.CreateTab($(this)); return false; });
    //rank
    elem.buttonRank().on('click', function () { rankUp($(this)); return false; });
    //edit
    elem.buttonEdit().on('click', function () { Fw.CreateTab($(this)); return false; });
    //delete
    elem.buttonDelete().on('click', function () { deleteData($(this)); return false; });
    //custom
    elem.buttonCustom().on('click', function () { Fw.CreateTab($(this)); return false; });
    //create-submit
    elem.submitCreate().on('click', function () { create($(this)); return false; });
    //edit-submit
    elem.submitEdit().on('click', function () { edit($(this)); return false; });
    //selectAll
    elem.selectAll().on('click', function () { var v = $(this).prop('checked'); elem.selectItem().prop('checked', v); });
    //selectItem
    elem.selectItem().on('click', function () { elem.selectAll().prop('checked', elem.selectItem().length == elem.selectedItem().length); });
    //listTable row-hover
    elem.listTable.find('tr').hover(function () { $(this).toggleClass(o.rowHoverClass); }).filter(':even').addClass(o.evenRowClass);
    //input
    $('.single-line,textarea,select').on('focus', function () { $(this).addClass(o.textboxFocusClass); }).on('blur', function () { $(this).removeClass(o.textboxFocusClass); });
    //sort
    elem.sortColumnHeader(elem.opParam.getOpSortProperty()).addClass('sort-' + elem.opParam.getOpSortDirection());
    elem.sortColumnHeader().on('click', function () { sort($(this)); return false; });
    //datepicker
    elem.datepicker().datepicker({ changeYear: true, changeMonth: true });
    //link-custom
    elem.linkCustom().on('click', function () { Fw.CreateTab($(this)); return false; });
    this.Refresh = refresh;

    //#### 页面初始化 ##########################################################################################################################################################################
    //处理服务器验证 bug
    $.each($(elem.validationErrorMessage()).not($(elem.validationErrorMessage().has(selector.validationTag))), function ()
    {
        $(this).wrapInner('<span></span>');
    });
    //处理操作返回结果
    if (returnValue)
    {
        switch (returnValue.type)
        {
            case ReturnType.Invalid: createWarningDialog({ message: returnValue.message }); break;
            case ReturnType.Success: createSuccessDialog({ message: returnValue.message }); break;
            case ReturnType.Failure: createFailureDialog({ message: returnValue.message }); break;
            case ReturnType.Error: createErrorDialog({ message: returnValue.message }); break;
            case ReturnType.NoPermission: createWarningDialog({ message: returnValue.message }); break;
            case ReturnType.NotLogin: createWarningDialog({ message: returnValue.message }); break;
            case ReturnType.Other: createInformationDialog({ message: returnValue.message }); break;
            case ReturnType.CreateSuccess: createSuccess(returnValue); break;
            case ReturnType.CreateFailure: createFailure(returnValue); break;
            case ReturnType.EditSuccess: editSuccess(returnValue); break;
            case ReturnType.EditFailure: editFailure(returnValue); break;
            case ReturnType.DeleteSuccess: deleteSuccess(returnValue); break;
            case ReturnType.DeleteFailure: deleteFailure(returnValue); break;
            case ReturnType.DeletesSuccess: deletesSuccess(returnValue); break;
            case ReturnType.DeletesFailure: deletesFailure(returnValue); break;
            case ReturnType.RankUpSuccess: rankUpSuccess(returnValue); break;
            case ReturnType.RankUpFailure: rankUpFailure(returnValue); break;
        }
    }
    //#### 刷新数据 ##########################################################################################################################################################################
    function refresh()
    {
        Fw.ShowProgress(pageId);
        if ($(selector.opParam.opType).length > 0) elem.form().submit();
        else location.replace(location.href);
    }
    //#### 添加数据 ##########################################################################################################################################################################
    function create(submitButton)
    {
        var form = elem.form();
        if (form.valid())
        {
            if (submitButton.attr(o.customPreEventName))
                if (!(eval(submitButton.attr(o.customPreEventName)))()) return false;
            Fw.ShowProgress(pageId);
            //if (typeof editor!='undefined') editor.sync();
            form[0].submit();
        }
        else
        {
            window.scrollTo(0, elem.validationErrorInput().first().offset().top - elem.toolsContainer.outerHeight(true));
        }
        return false;
    }
    function createSuccess()
    {
        createSuccessDialog({ top: '20%', title: '操作成功', message: returnValue.message });
        //刷新列表页
        if (indexPageId !== '') Fw.RefreshTab(indexPageId);
        var submitButton = $(selector.submitCreate);
        if (submitButton.length > 0 && submitButton.attr(o.customPostEventName))
            eval(submitButton.attr(o.customPostEventName))();
    }
    function createFailure()
    {
        createFailureDialog({ top: '20%', title: '操作失败', message: returnValue.message });
    }
    //#### 更新数据 ##########################################################################################################################################################################
    function edit(submitButton)
    {
        var form = elem.form();
        if (form.valid())
        {
            if (submitButton.attr(o.customPreEventName))
                if (!(eval(submitButton.attr(o.customPreEventName)))()) return false;
            Fw.ShowProgress(pageId);
            //if (typeof editor != 'undefined') editor.sync();
            form[0].submit();
        }
        else
        {
            window.scrollTo(0, elem.validationErrorInput().first().offset().top - elem.toolsContainer.outerHeight(true));
        }
        return false;
    }
    function editSuccess()
    {
        createSuccessDialog({ top: '20%', title: '操作成功', message: returnValue.message });
        //刷新列表页
        if (indexPageId !== '') Fw.RefreshTab(indexPageId);
        //刷新详情页
        if (detailsPageId !== '') Fw.RefreshTab(detailsPageId);
        var submitButton = $(selector.submitEdit);
        if (submitButton.length > 0 && submitButton.attr(o.customPostEventName))
            eval(submitButton.attr(o.customPostEventName))();
    }
    function editFailure()
    {
        createFailureDialog({ top: '20%', title: '操作失败', message: returnValue.message });
    }
    //#### 删除数据 ##########################################################################################################################################################################
    function deleteData(link)
    {
        var dialog = createConfirmDialog({
            top: '20%', title: '删除', message: '数据删除后无法恢复，请谨慎操作，确认删除？', okEvent: function ()
            {
                Fw.ShowProgress(pageId);
                dialog.Close();
                elem.opParam.setOpType(OpType.Delete);
                elem.opParam.setOpArgument(link.attr('itemid'));
                elem.form()[0].submit();
                return false;
            }
        });
    }
    function deleteSuccess(returnValue)
    {
        createSuccessDialog({ top: '20%', title: '操作成功', message: returnValue.message });
        //关闭详情、更新页面
        var id = elem.opParam.getOpArgument();
        if (actionPages.Edit)
        {
            var editPageId = actionPages.Edit + id;
            if (editPageId != pageId) Fw.CloseTab({ tabId: editPageId });
        }
        if (actionPages.Details)
        {
            var detailsPageId = actionPages.Details + id;
            if (detailsPageId != pageId) Fw.CloseTab({ tabId: detailsPageId });
        }
        elem.opParam.setOpArgument("");
    }
    function deleteFailure(returnValue)
    {
        createFailureDialog({ top: '20%', title: '操作失败', message: returnValue.message });
        elem.opParam.setOpArgument("");
    }
    //#### 批量删除数据 ##########################################################################################################################################################################
    function deleteDatas()
    {
        var selectedItem = elem.selectedItem();
        if (selectedItem.length <= 0)
        {
            var deletesTips = $('<span id="deletes-tips"><span>请选择批量删除的数据项</span></span>');
            deletesTips.float({ maskerTarget: false, left: 35, top: 37 });
            setTimeout(function () { deletesTips.remove(); }, 2000);
            return false;
        }
        var dialog = createConfirmDialog({
            top: '20%', title: '批量删除', message: '数据删除后无法恢复，请谨慎操作，确认删除？', okEvent: function ()
            {
                var ids = "";
                selectedItem.each(function ()
                {
                    ids += $(this).val() + ",";
                });
                Fw.ShowProgress(pageId);
                dialog.Close();
                elem.opParam.setOpType(OpType.Deletes);
                elem.opParam.setOpArgument(ids);
                elem.form()[0].submit();
                return false;
            }
        });
    }
    function deletesSuccess(returnValue)
    {
        createSuccessDialog({ top: '20%', title: '操作成功', message: returnValue.message });
        //关闭详情、更新页面
        var ids = elem.opParam.getOpArgument().split(",");

        for (var i = 0; i < ids.length; i++)
        {
            if (!ids[i]) continue;
            if (actionPages.Edit)
            {
                var editPageId = actionPages.Edit + ids[i];
                if (editPageId != pageId) Fw.CloseTab({ tabId: editPageId });
            }
            if (actionPages.Details)
            {
                var detailsPageId = actionPages.Details + ids[i];
                if (detailsPageId != pageId) Fw.CloseTab({ tabId: detailsPageId });
            }
        }
        elem.opParam.setOpArgument("");
        //if (submitButton.length > 0 && submitButton.attr(o.customPostEventName))
        //    eval(submitButton.attr(o.customPostEventName))();
    }
    function deletesFailure(returnValue)
    {
        createFailureDialog({ top: '20%', title: '操作失败', message: returnValue.message });
        elem.opParam.setOpArgument("");
    }
    //#### 排序上调 ##########################################################################################################################################################################
    function rankUp(link)
    {
        elem.opParam.set(OpType.RankUp, link.attr('href'), null, null, null, null);
        Fw.ShowProgress(pageId);
        elem.form()[0].submit();
    }
    function rankUpSuccess()
    {
        createSuccessDialog({ top: '20%', title: '操作成功', message: returnValue.message });
        elem.opParam.setOpArgument("");
    }
    function rankUpFailure()
    {
        createFailureDialog({ top: '20%', title: '操作失败', message: returnValue.message });
        elem.opParam.setOpArgument("");
    }
    //#### 查询 ##########################################################################################################################################################################
    function initQuery()
    {
        var dialog = elem.query.dialog();
        if (dialog.length <= 0) return false;
        if (elem.opParam.getOpQueryString())
        {
            elem.toolQuery().addClass(o.toolHilightClass);
            elem.toolShowAll().removeClass(o.toolDisableClass).off("click").on('click', function ()
            {
                Fw.ShowProgress(pageId);
                elem.opParam.set(OpType.Showall, "", "", 1, null, null);
                elem.form()[0].submit();
                return false;
            });
        }
        else
        {
            elem.toolQuery().removeClass(o.toolHilightClass);
            elem.toolShowAll().addClass(o.toolDisableClass).off("click").on('click', false);
        }
        elem.toolQuery().on('click', function ()
        {
            dialog.float({ top: '20%', esc: true });
            return false;
        });
        elem.query.buttonAddItem().on('click', function ()
        {
            var template = elem.query.template().clone();
            if (elem.query.item().length == 0)
                elem.query.logic.elem(template).remove();
            elem.query.itemContainer().append(template);
            return false;
        });
        $(dialog).on('click.queryRemoveItem', selector.query.buttonRemoveItem, function ()
        {
            var queryItem = $(this).closest(selector.query.item), index = queryItem.index(elem.query.item());
            $(this).closest(selector.query.item).remove();
            if (index == 0 && elem.query.item().length > 0) elem.query.logic.elem(elem.query.item(0)).remove();
            return false;
        });
        elem.query.submitQuery().on('click', function ()
        {
            Fw.ShowProgress(pageId);
            var queryString = "[";
            elem.query.item().each(function ()
            {
                var row = $(this), logicElem = elem.query.logic.elem(row), propertyElem = elem.query.property.elem(row), valueElem = elem.query.value.elem(row), value1Elem = elem.query.value1.elem(row), value2Elem = elem.query.value2.elem(row);
                var type = propertyElem.children(":selected").attr(o.query.property.typeAttribute);
                var logic = logicElem.length == 0 ? "" : logicElem.val();
                var property = propertyElem.val();
                var condition = elem.query.condition.elem(row).val();
                var value = valueElem.length > 0 ? valueElem.val() : "";
                var value1 = value1Elem.length > 0 ? value1Elem.val() : "";
                var value2 = value2Elem.length > 0 ? value2Elem.val() : "";
                queryString += '{"QueryPropertyType":"' + type + '","QueryLogic":"' + logic + '","QueryProperty":"' + property + '","QueryCondition":"' + condition + '","QueryValue":"' + value + '","QueryValue1":"' + value1 + '","QueryValue2":"' + value2 + '"},';
            });
            queryString = queryString.substr(0, queryString.length - 1) + "]";
            if (queryString == "]") queryString = "";
            elem.opParam.set(OpType.Query, "", queryString, 1, null, null);
            elem.form()[0].submit();
            return false;
        });
        var conditionStr = '<select ' + o.query.condition.propertyTypeAttribute + '="{0}" name="' + o.query.condition.nameAttribute + '" class="' + o.query.condition.elemClass + '">';
        var conditionOfStringTemplate = conditionStr.replace("{0}", o.query.property.type.String) + '<option value="' + o.query.condition.type.Contains + '">包含</option><option value="' + o.query.condition.type.Equal + '">等于</option><option value="' + o.query.condition.type.StartsWith + '">以此开头</option><option value="' + o.query.condition.type.EndsWith + '">以此结尾</option></select>';
        var conditionOfNumericalTemplate = conditionStr.replace("{0}", o.query.property.type.Numerical) + '<option value="' + o.query.condition.type.Equal + '">等于</option><option value="' + o.query.condition.type.LessThan + '">小于</option><option value="' + o.query.condition.type.LessThanOrEqual + 'l">小于等于</option><option value="' + o.query.condition.type.GreaterThan + '">大于</option><option value="' + o.query.condition.type.GreaterThanOrEqual + '">大于等于</option><option value="' + o.query.condition.type.Between + '">介于</option></select>';
        var conditionOfDateTimeTemplate = conditionStr.replace("{0}", o.query.property.type.DateTime) + '<option value="' + o.query.condition.type.GreaterThanOrEqual + '">起始于</option><option value="' + o.query.condition.type.LessThanOrEqual + '">终止于</option><option value="' + o.query.condition.type.Between + '">介于</option></select>';
        var conditionOfBooleanTemplate = conditionStr.replace("{0}", o.query.property.type.Boolean) + '<option value="' + o.query.condition.type.Equal + '">等于</option></select>';
        var conditionOfGenderTemplate = conditionStr.replace("{0}", o.query.property.type.Gender) + '<option value="' + o.query.condition.type.Equal + '">等于</option></select>';
        var conditionOfSelectTemplate = conditionStr.replace("{0}", o.query.property.type.Select) + '<option value="' + o.query.condition.type.Equal + '">等于</option></select>';
        var valueStr = 'class="' + o.query.value.elemClass + '" name="' + o.query.value.nameAttribute + '"';
        var value1Str = 'class="' + o.query.value1.elemClass + '" name="' + o.query.value1.nameAttribute + '"';
        var value2Str = 'class="' + o.query.value2.elemClass + '" name="' + o.query.value2.nameAttribute + '"';
        var valueOfOneTemplate = '<input type="text" ' + valueStr + '"/>';
        var valueOfTwoTemplate = '<input type="text" ' + value1Str + '/> - <input type="text" ' + value2Str + '/>';
        var valueOfBooleanTemplate = '<select ' + valueStr + '><option value="True">是</option><option value="False">否</option></select>';
        var valueOfGenderTemplate = '<select ' + valueStr + '><option value="男">男</option><option value="女">女</option><select>';
        var valueOfSelectTemplate = '<select ' + valueStr + '>{0}</select>';
        $(dialog).on('change.queryChangeProperty', selector.query.property.elem, function (e)
        {
            var t = $(this), row = t.closest(selector.query.item);
            var type = t.children(":selected").attr(o.query.property.typeAttribute);
            var conditionContainer = elem.query.condition.container(row), valueContainer = elem.query.value.container(row);
            var currentConditionType = elem.query.condition.elem(row).attr(o.query.condition.propertyTypeAttribute);
            switch (type)
            {
                case o.query.property.type.Gender:
                    if (currentConditionType != type)
                    {
                        conditionContainer.html(conditionOfGenderTemplate);
                        valueContainer.html(valueOfGenderTemplate);
                    }
                    break;
                case o.query.property.type.String:
                    if (currentConditionType != type)
                    {
                        conditionContainer.html(conditionOfStringTemplate);
                        valueContainer.html(valueOfOneTemplate);
                    }
                    break;
                case o.query.property.type.Numerical:
                    if (currentConditionType != type)
                    {
                        conditionContainer.html(conditionOfNumericalTemplate);
                        valueContainer.html(valueOfOneTemplate);
                    }
                    break;
                case o.query.property.type.DateTime:
                    if (currentConditionType != type)
                    {
                        conditionContainer.html(conditionOfDateTimeTemplate);
                        valueContainer.html(valueOfOneTemplate);
                        elem.query.value.elem(row).datepicker();
                        elem.query.value1.elem(row).datepicker();
                        elem.query.value2.elem(row).datepicker();
                    }
                    break;
                case o.query.property.type.Boolean:
                    if (currentConditionType != type)
                    {
                        conditionContainer.html(conditionOfBooleanTemplate);
                        valueContainer.html(valueOfBooleanTemplate);
                    }
                    break;
                case o.query.property.type.Select:
                    if (currentConditionType != type)
                    {
                        conditionContainer.html(conditionOfSelectTemplate);
                    }
                    var tmp = elem.query.selectTemplate(t.val()).clone();
                    valueContainer.html(tmp);
                    break;
            }
        });
        $(dialog).on('change.queryChangeCondition', "." + o.query.condition.elemClass, function (e)
        {
            var t = $(this), row = t.closest(selector.query.item), type = t.attr(o.query.condition.propertyTypeAttribute);
            if (type == o.query.property.type.DateTime || type == o.query.property.type.Numerical)
            {
                var valueContainer = elem.query.value.container(row);
                var value = elem.query.value.elem(row), value1 = elem.query.value1.elem(row), value2;
                if (t.val() == o.query.condition.type.Between && value1.length == 0) valueContainer.html(valueOfTwoTemplate);
                else if (t.val() != o.query.condition.type.Between && value1.length > 0) valueContainer.html(valueOfOneTemplate);
                if (type == o.query.property.type.DateTime)
                {
                    value1 = elem.query.value1.elem(row), value2 = elem.query.value2.elem(row);
                    if (value.length > 0) value.datepicker();
                    if (value1.length > 0) value1.datepicker();
                    if (value2.length > 0) value2.datepicker();
                }
            }
        });
        elem.query.item().each(function ()
        {
            var row = $(this);
            if (elem.query.property.elem(row).children(":selected").attr(o.query.property.typeAttribute) == o.query.property.type.DateTime)
            {
                elem.query.value.elem($(this)).datepicker();
                elem.query.value1.elem($(this)).datepicker();
                elem.query.value2.elem($(this)).datepicker();
            }
        });
        var _move = false;
        var _x, _y;
        elem.query.dialogHeader().on('mousedown', function (e)
        {
            _move = true;
            var p = $(this).parent().position();
            _x = e.pageX - parseInt(p.left);
            _y = e.pageY - parseInt(p.top);
        });
        $(document).on('mousemove.moveQueryDialog', function (e)
        {
            if (_move) { dialog.css({ top: e.pageY - _y, left: e.pageX - _x }); }
        }).mouseup(function ()
        {
            _move = false;
        });
        return false;
    }
    //#### 翻页 ##########################################################################################################################################################################
    function pager(pageNumber)
    {
        Fw.ShowProgress(pageId);
        elem.opParam.set(OpType.Pager, null, null, pageNumber, null, null);
        elem.form()[0].submit();
    }
    //#### 排序 ##########################################################################################################################################################################
    function sort(sortLink)
    {
        Fw.ShowProgress(pageId);
        var property = sortLink.attr(o.sortColumnPropertyAttribute);
        var direction = SortDirection.Ascending;
        var currentProperty = elem.opParam.getOpSortProperty();
        if (currentProperty == property)
        {
            if (elem.opParam.getOpSortDirection() == SortDirection.Ascending) direction = SortDirection.Descending;
        }
        elem.opParam.set(OpType.Sort, "", null, 1, property, direction);
        elem.form()[0].submit();
    }
}
var subpage;
$(function ()
{
    subpage = new Subpage();
});

//var editor = null;

//function initUploadify(param)
//{
//    var normalFile = '*.doc;*.docx;*.xls;*.xlsx;*.ppt;*.pptx;*.htm;*.html;*.txt;*.zip;*.rar;*.gz;*.bz2;*.gif;*.jpg;*.jpeg;*.png;*.bmp';
//    var imageFile = '*.gif;*.jpg;*.jpeg;*.png;*.bmp';
//    if (param.fileTypeExts == 'image')
//        param.fileTypeExts = imageFile;
//    var o =
//    {
//        target: null,
//        auto: true,
//        uploader: null,
//        swf: '/Scripts/uploadify-v3.1/uploadify.swf',
//        buttonImage: '/Scripts/uploadify-v3.1/browse-btn.png',
//        width: 65,
//        height: 23,
//        fileTypeExts: normalFile,
//        fileSizeLimit: '10MB',
//        multi: true,
//        removeTimeout: 0,
//        onUploadSuccess: function (file, data, response)
//        {
//            var f = new Function('return ' + data);
//            var d = f();
//            if (d.message != 'ok') alert(d.message);
//            else if (o.success) o.success(d);
//        },
//        onUploadError: function (file, errorCode, errorMsg, errorString)
//        {
//            alert('文件 ' + file.name + ' 未能上传: ' + errorString);
//        }
//    };
//    $.extend(true, o, param);
//    $(o.target).uploadify(o);
//}

//var destroyUploadify = function ()
//{
//    if ($(".uploadify").length > 0)
//        $("#" + $(".uploadify").attr("id")).uploadify('destroy');
//};

//function initEditor(param)
//{
//    var o = param.kindEditor,
//    editorItems = ['source', '|', 'undo', 'redo', '|', 'preview', 'selectall', 'cut', 'copy', 'paste', 'plainpaste', 'wordpaste', '|', 'table', 'hr', 'link', 'unlink', '|', 'image', 'insertfile', 'flash', 'media', '|', 'clearhtml', 'removeformat', 'quickformat', '|', 'fullscreen', '/',
//        'justifyleft', 'justifycenter', 'justifyright', 'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', '|', 'formatblock', 'fontname', 'fontsize', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline', 'strikethrough', 'subscript', 'superscript', 'lineheight'];
//    KindEditor.ready(function (K)
//    {
//        editor = K.create(o.target, { 'items': editorItems, 'resizeType': 1, 'indentChar': '', 'uploadJson': o.uploader, 'afterUpload': null || o.success });
//        //editor.sync();
//        //param.kindEditor.editor = editor;
//        //if (param.create) initCreate(param);
//        //else if (param.edit) initEdit(param);
//    });
//}