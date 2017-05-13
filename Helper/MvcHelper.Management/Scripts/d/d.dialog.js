/**************************************************
 * v1.4
 * by 丁浩
 * 2015-03-19
 * 
 * 增加dialog-header，并调整相应css
 * 调整confirmDialog的esc为true
 * 调整maskerTarget的值
 * loadingDialog可以出现多个
 **************************************************/
var DialogIconType = { 'Loading': 'loading', 'Information': 'information', 'Success': 'success', 'Warning': 'warning', 'Confirm': 'confirm', 'Error': 'error' };
var DialogButtonType = { 'None': 'None', 'Close': 'Close', 'OkCancel': 'OkCancel' };
var DialogDisappearMethod = { 'Trigger': 'Trigger', 'AutoHide': 'AutoHide', 'None': 'None' };
//dialogParam:{ id,title,message,width,iconType,buttonType,disappearMethod }
//width属性每个项目不同
function createDialog(dialogParam, layerParam)
{
    var o =
        {
            id: false,
            title: false,
            message: false,
            width: 484,
            iconType: DialogIconType.Information,
            buttonType: DialogButtonType.None,
            disappearMethod: DialogDisappearMethod.AutoHide,
            okEvent: null
        };
    o = $.extend(true, o, dialogParam);
    var dialog, dialogHeader, dialogTitle, dialogBody, dialogIcon, dialogMessage, dialogButton, dialogFooter;

    if (typeof o.id === 'string') if ($('#' + o.id).length > 0) return;
    var dialog = $('<div class="dialog"></div>');
    if (typeof o.id === 'string') dialog.attr('id', o.id);
    dialog.addClass(o.iconType.toLowerCase()).css('width', o.width);
    dialogHeader = $('<div class="dialog-header"></div>');
    if (o.title == false) dialogTitle = null;
    else dialogTitle = $('<div class="dialog-title">' + o.title + '</div>');
    dialogBody = $('<div class="dialog-body"></div>');
    dialogIcon = $('<div class="dialog-icon"></div>');
    dialogMessage = $('<div class="dialog-message">{0}</div>'.replace('{0}', o.message));
    if (o.buttonType == DialogButtonType.None) dialogButton = null;
    else if (o.buttonType == DialogButtonType.Close) dialogButton = $('<div class="dialog-button"><a href="/" class="dialog-close"><span>关&nbsp;闭</span></a></div>');
    else if (o.buttonType == DialogButtonType.OkCancel)
    {
        dialogButton = $('<div class="dialog-button"><a href="/" class="dialog-ok"><span>确&nbsp;定</span></a><a href="/" class="dialog-close"><span>取&nbsp;消</span></a></div>');
        if (typeof o.okEvent == 'function') dialogButton.find('.dialog-ok').on('click', o.okEvent);
    }
    dialogFooter = $('<div class="dialog-footer"></div>');
    if (dialogTitle != null) dialogHeader.append(dialogTitle);
    dialog.append(dialogHeader).append(dialogBody.append(dialogIcon).append(dialogMessage).append('<div class="clr"></div>'));
    if (dialogButton != null) dialog.append(dialogButton);
    dialog.append(dialogFooter);
    dialog = dialog.float(layerParam);
    switch (o.disappearMethod)
    {
        case DialogDisappearMethod.AutoHide:
            setTimeout(function () { dialog.fadeOut(500, function () { dialog.Close(); }) }, 2000);
            break;
        default:
            break;
    }
    return dialog;
}

//param:id,follow,masker，width
function createLoadingDialog(param)
{
    var p =
        {
            id: param.id || false
        };
    var lp =
        {
            appendTo: param.appendTo || 'body'
            , follow: param.follow || document
            , left: param.left || 'center'
            , top: param.top || 'center'
            , right: param.right || false
            , bottom: param.bottom || false
            , zIndex: param.zIndex || 1000
            , maskerTarget: param.maskerTarget === false ? false : (param.maskerTarget || null)
            , show: param.show || true
            , esc: param.esc || false
            , doCloseEvent: param.doCloseEvent || false
            , closeButton: param.closeButton || null
            , afterCloseEvent: param.afterCloseEvent || null
            , okEvent: param.okEvent || null
            , adaptivePosition: param.adaptivePosition || false
        };
    var dialog = $('<div ' + (p.id ? ('id=' + p.id) : '') + ' class="loading"></div>');
    return dialog.float(lp);
}

//param:id,message,width
function createErrorDialog(param)
{
    var p =
        {
            id: param.id || false
            , title: param.title || false
            , message: param.message || null
            , width: param.width
            , iconType: DialogIconType.Error
            , buttonType: DialogButtonType.Close
            , disappearMethod: DialogDisappearMethod.Trigger
        };
    var lp =
        {
            appendTo: param.appendTo || 'body'
            , follow: param.follow || document
            , left: param.left || 'center'
            , top: param.top || 'center'
            , right: param.right || false
            , bottom: param.bottom || false
            , zIndex: param.zIndex || 1000
            , maskerTarget: param.maskerTarget === false ? false : (param.maskerTarget || null)
            , show: param.show || true
            , esc: param.esc || false
            , doCloseEvent: param.doCloseEvent || true
            , closeButton: param.closeButton || '.dialog-close'
            , afterCloseEvent: param.afterCloseEvent || null
            , okEvent: param.okEvent || null
            , adaptivePosition: param.adaptivePosition || false
        };
    return createDialog(p, lp);
}

//param:id,message,width
function createFailureDialog(param)
{
    var p =
        {
            id: param.id || false
            , title: param.title || false
            , message: param.message || null
            , width: param.width
            , iconType: DialogIconType.Error
            , buttonType: DialogButtonType.Close
            , disappearMethod: DialogDisappearMethod.Trigger
        };
    var lp =
        {
            appendTo: param.appendTo || 'body'
            , follow: param.follow || document
            , left: param.left || 'center'
            , top: param.top || 'center'
            , right: param.right || false
            , bottom: param.bottom || false
            , zIndex: param.zIndex || 1000
            , maskerTarget: param.maskerTarget === false ? false : (param.maskerTarget || null)
            , show: param.show || true
            , esc: param.esc || false
            , doCloseEvent: param.doCloseEvent || true
            , closeButton: param.closeButton || '.dialog-close'
            , afterCloseEvent: param.afterCloseEvent || null
            , okEvent: param.okEvent || null
            , adaptivePosition: param.adaptivePosition || false
        };
    return createDialog(p, lp);
}

//param:id,message,width
function createWarningDialog(param)
{
    var p =
        {
            id: param.id || false
            , title: param.title || false
            , message: param.message || null
            , width: param.width
            , iconType: DialogIconType.Warning
            , buttonType: DialogButtonType.Close
            , disappearMethod: DialogDisappearMethod.Trigger
        };
    var lp =
        {
            appendTo: param.appendTo || 'body'
            , follow: param.follow || document
            , left: param.left || 'center'
            , top: param.top || 'center'
            , right: param.right || false
            , bottom: param.bottom || false
            , zIndex: param.zIndex || 1000
            , maskerTarget: param.maskerTarget === false ? false : (param.maskerTarget || null)
            , show: param.show || true
            , esc: param.esc || false
            , doCloseEvent: param.doCloseEvent || true
            , closeButton: param.closeButton || '.dialog-close'
            , afterCloseEvent: param.afterCloseEvent || null
            , okEvent: param.okEvent || null
            , adaptivePosition: param.adaptivePosition || false
        };
    return createDialog(p, lp);
}

//param:id,message,width
function createSuccessDialog(param)
{
    var p =
        {
            id: param.id || false
            , title: param.title || false
            , message: param.message || null
            , width: param.width
            , iconType: DialogIconType.Success
            , buttonType: DialogButtonType.None
            , disappearMethod: DialogDisappearMethod.AutoHide
        };
    var lp =
        {
            appendTo: param.appendTo || 'body'
            , follow: param.follow || document
            , left: param.left || 'center'
            , top: param.top || 'center'
            , right: param.right || false
            , bottom: param.bottom || false
            , zIndex: param.zIndex || 1000
            , maskerTarget: false
            , show: param.show || true
            , esc: param.esc || false
            , doCloseEvent: param.doCloseEvent || true
            , closeButton: param.closeButton || '.dialog-close'
            , afterCloseEvent: param.afterCloseEvent || null
            , okEvent: param.okEvent || null
            , adaptivePosition: param.adaptivePosition || false
        };
    return createDialog(p, lp);
}

//param:id,message,width
function createInformationDialog(param)
{
    var p =
        {
            id: param.id || false
            , title: param.title || false
            , message: param.message || null
            , width: param.width
            , iconType: DialogIconType.Information
            , buttonType: DialogButtonType.Close
            , disappearMethod: DialogDisappearMethod.Trigger
        };
    var lp =
        {
            appendTo: param.appendTo || 'body'
            , follow: param.follow || document
            , left: param.left || 'center'
            , top: param.top || 'center'
            , right: param.right || false
            , bottom: param.bottom || false
            , zIndex: param.zIndex || 1000
            , maskerTarget: param.maskerTarget === false ? false : (param.maskerTarget || null)
            , show: param.show || true
            , esc: param.esc || false
            , doCloseEvent: param.doCloseEvent || true
            , closeButton: param.closeButton || '.dialog-close'
            , afterCloseEvent: param.afterCloseEvent || null
            , okEvent: param.okEvent || null
            , adaptivePosition: param.adaptivePosition || false
        };
    return createDialog(p, lp);
}

//param:id,message,width
function createConfirmDialog(param)
{
    var p =
        {
            id: param.id || false
            , title: param.title || false
            , message: param.message || null
            , width: param.width
            , okEvent: param.okEvent || null
            , iconType: DialogIconType.Confirm
            , buttonType: DialogButtonType.OkCancel
            , disappearMethod: DialogDisappearMethod.Trigger
        };
    var lp =
        {
            appendTo: param.appendTo || 'body'
            , follow: param.follow || document
            , left: param.left || 'center'
            , top: param.top || 'center'
            , right: param.right || false
            , bottom: param.bottom || false
            , zIndex: param.zIndex || 1000
            , maskerTarget: param.maskerTarget === false ? false : (param.maskerTarget || null)
            , show: param.show || true
            , esc: param.esc || true
            , doCloseEvent: param.doCloseEvent || true
            , closeButton: param.closeButton || '.dialog-close'
            , afterCloseEvent: param.afterCloseEvent || null
            , okEvent: param.okEvent || null
            , adaptivePosition: param.adaptivePosition || false
        };
    return createDialog(p, lp);
}