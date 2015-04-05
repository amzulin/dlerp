var ddlclient; var showclientcode; var showclientname;

var detailindex = 0;
var hmaterial = ""; var hname = ""; var hmodel = ""; var hth = ""; var hprice = 0;
var bomId = 0;
var parturl = "";
var page = 1;
var where = "";
var orderby = "";
var csparas = "";
var htmlids = "";


var supplierid, suppliername = '';

$(document).ready(function (e) {


    $(".table_common tr").mouseover(function () {
        $(this).addClass("over");
    }).mouseout(function () {
        $(this).removeClass("over");
    })
    $(".table_common tr:even").addClass("alt");

    $(".report tr").mouseover(function () {
        $(this).addClass("over");
    }).mouseout(function () {
        $(this).removeClass("over");
    })
    $(".report tr:even").addClass("alt");
    autopost();

    $("#ddlmc").change(function () {
        $("#txtmaterial").val("");
        hmaterial = "";
        hname = "";
        hmodel = "";
    });

    $("#ddlcate1").change(function () {
        var c = $("#ddlcate1").val();
        var uid = new Date();
        $("#ddlcatediv").load("../main/materialcate2?cate=" + c + "&uid=" + uid.getTime(), function (data) { });
    });

    $("#txtmaterial").autocomplete({
        source: function (request, response) {
            var key = $("#txtmaterial").val();
            var mc = $("#ddlmc").val();
            $.get("../main/querymaterial", { key: key, mc: mc }, function (data) {
                response($.map(data, function (item) {
                    return {
                        label: item.code + " 物料:" + item.name + " 型号:" + item.model,
                        name: item.name,
                        model: item.model,
                        tu: item.tu,
                        value: item.code,
                    }
                }));
            });
        },
        minLength: 1,
        select: function (event, ui) {
            var item = ui.item.name;
            $("#txtmaterial").val(ui.item.value + " " + ui.item.name + " " + ui.item.model);
            hmaterial = ui.item.value;
            hname = ui.item.name;
            hmodel = ui.item.model;
            hth = ui.item.tu;
            return false;
        }
    });
    $("#txtcatematerial").autocomplete({
        source: function (request, response) {
            var key = $("#txtcatematerial").val();
            $.get("../main/querymaterial", { key: key, mc: '', bigcate: '成品' }, function (data) {
                response($.map(data, function (item) {
                    return {
                        label: item.code + " 物料:" + item.name + " 型号:" + item.model,
                        name: item.name,
                        model: item.model,
                        tu: item.tu,
                        value: item.code,
                    }
                }));
            });
        },
        minLength: 1,
        select: function (event, ui) {
            var item = ui.item.name;
            $("#txtcatematerial").val(ui.item.value + " " + ui.item.name + " " + ui.item.model);
            hmaterial = ui.item.value;
            hname = ui.item.name;
            hmodel = ui.item.model;
            hth = ui.item.tu;
            return false;
        }
    });
    $("#txtsupplier").autocomplete({
        source: function (request, response) {
            var key = $("#txtsupplier").val();
            $.get("../main/querycustomer", { key: key,type:0 }, function (data) {
                response($.map(data, function (item) {
                    return {
                        label: item.no+item.name,
                        name: item.name,
                        no: item.no,
                        value: item.id,
                    }
                }));
            });
        },
        minLength: 1,
        select: function (event, ui) {
            var item = ui.item.name;
            $("#txtsupplier").val(ui.item.no+ui.item.name);
            supplierid = ui.item.value;
            suppliername = ui.item.name;
            return false;
        }
    });
    $("#txtcustomer").autocomplete({
        source: function (request, response) {
            var key = $("#txtcustomer").val();
            $.get("../main/querycustomer", { key: key, type: 1 }, function (data) {
                response($.map(data, function (item) {
                    return {
                        label: item.no + item.name,
                        name: item.name,
                        no: item.no,
                        value: item.id,
                    }
                }));
            });
        },
        minLength: 1,
        select: function (event, ui) {
            var item = ui.item.name;
            $("#txtcustomer").val(ui.item.no+ui.item.name);
            supplierid = ui.item.value;
            suppliername = ui.item.name;
            return false;
        }
    });

    $("#txtbom").autocomplete({
        source: function (request, response) {
            var key = $("#txtbom").val();
            if(supplierid==0){
            $.dialog.tips('请先选择客户');
            return false;
            }
            $.get("../main/querybom", { key: key,supplierid:supplierid }, function (data) {
                response($.map(data, function (item) {
                    return {
                        label:  item.code + " 物料:" + item.name + " 型号:" + item.model,
                        name: item.name,
                        model: item.model,
                        tu: item.tu,
                        code: item.code,
                        value: item.bomid,
                        price: item.price,
                    }
                }));
            });
        },
        minLength: 1,
        select: function (event, ui) {
            var item = ui.item.name;
            $("#txtbom").val(ui.item.code + " " + ui.item.name + " " + ui.item.model);
            $("#txtprice").val(ui.item.price);
            bomId = ui.item.value;
            hmaterial = ui.item.code;
            hname = ui.item.name;
            hmodel = ui.item.model;
            hth = ui.item.tu;
            hprice = ui.item.price;
            return false;
        }
    });

    $("#txtbomprice").autocomplete({
        source: function (request, response) {
            var key = $("#txtbomprice").val();
            if(supplierid==0){
            $.dialog.tips('请先选择客户');
            return false;
            }
            $.get("../main/querybomprice", { key: key,supplierid:supplierid }, function (data) {
                response($.map(data, function (item) {
                    return {
                        label: item.code + " 物料:" + item.name + " 型号:" + item.model + (item.version != "" ? (" 版本:" + item.version) : "") + " 单价:" + item.price + (item.remark != "" ? (" 备注:" + item.remark) : ""),
                        name: item.name,
                        model: item.model,
                        tu: item.tu,
                        code: item.code,
                        value: item.bomid,
                        price: item.price,
                        remark: item.remark,
                        version: item.version
                    }
                }));
            });
        },
        minLength: 1,
        select: function (event, ui) {
            var item = ui.item.name;
            $("#txtbomprice").val(ui.item.code + " " + ui.item.name + " " + ui.item.model + (ui.item.version != "" ? (" 版本:" + ui.item.version) : "") + (ui.item.remark != "" ? (" 备注:" + ui.item.remark) : ""));
            $("#txtprice").val(ui.item.price);
            bomId = ui.item.value;
            hmaterial = ui.item.code;
            hname = ui.item.name;
            hmodel = ui.item.model;
            hth = ui.item.tu;
            hprice = ui.item.price;
            return false;
        }
    });

    $("#txtmaterialprice").autocomplete({
        source: function (request, response) {
            var key = $("#txtmaterialprice").val();
            if (supplierid == 0) {
                $.dialog.tips('请先选择客户');
                return false;
            }
            $.get("../main/querymaterialprice", { key: key, supplierid: supplierid }, function (data) {
                response($.map(data, function (item) {
                    return {
                        label: item.code + " 物料:" + item.name + " 型号:" + item.model + " 单价:" + item.price + (item.remark != "" ? " 备注:" + item.remark : ""),
                        name: item.name,
                        model: item.model,
                        tu: item.tu,
                        value: item.code,
                        price: item.price,
                        remark: item.remark
                    }
                }));
            });
        },
        minLength: 1,
        select: function (event, ui) {
            var item = ui.item.name;
            $("#txtmaterialprice").val(ui.item.value + " " + ui.item.name + " " + ui.item.model + (ui.item.remark != "" ? (" 备注:" + ui.item.remark) : ""));
            $("#txtprice").val(ui.item.price);
            hmaterial = ui.item.value;
            hname = ui.item.name;
            hmodel = ui.item.model;
            hth = ui.item.tu;
            hprice = ui.item.price;
            return false;
        }
    });

});


//展开明细
function loaddetail(i, url,key,funname) {
    if (i == detailindex) {
        var s = $("#detai_div_" + detailindex).css("display");
        if (s == "none") {
            $("#tr_" + detailindex).addClass("active");
            $("#detai_div_" + detailindex).css("display", "block");
        }
        else {
            $("#detai_div_" + detailindex).css("display", "none");
            $("#tr_" + detailindex).removeClass("active");
        }
        return false;
    }
    var where = '';
    if (typeof (key) != 'undefined') {
        var fun = eval(funname);
        where = fun();
    }
    $("#detai_div_" + detailindex).css("display", "none");
    $("#tr_" + detailindex).removeClass("active");
    detailindex = i;
    if (detail[i - 1] == 0) {//第一次加载数据
        var uid = new Date();
        $("#detai_div_" + i).load(url , { key: key, where: where }, function (data) { });
        detail[i - 1] = 1;
    }
    $("#tr_" + detailindex).addClass("active");
    $("#detai_div_" + detailindex).css("display", "block");
}

//审核,作废传-1
function changestatus(t, no, status, url)
{
    var opt = "";
    switch (status) {
        case 0:
            opt = "审核";
            break;
        case 1:
            opt = "返审";
            break;
        case 4:
            opt = "撤消作废";
            break;
        case -1:
            opt = "作废";
            break;
        default:
            opt = "类别错误";
            return false;
            break;
    }
    $.dialog.confirm('你确定要' + opt + '本条记录吗？', function () {
        $.post("../main/checkdata", { key: t, no: no, status: status }, function (data) {
            if (data.status == true) { $.dialog.tips(opt + "成功"); window.location.href = url; }
            else $.dialog.alert(data.message);
        });
    }, function () {
        //$.dialog.tips('执行取消操作');
    });
}
var printwin;
function print(title,key, no)
{
    printwin = $.dialog({
        id: 'win_repair', lock: true, max: false, min: false, width: 810, height: 650, resize: false,
        title: '打印窗口',
        content: 'url:../main/printwin?key=' + key + "&no=" + no + "&title=" +encodeURI(title)
    });
}

function printsale(no)
{
    printwin = $.dialog({
        id: 'win_repair', lock: true, max: false, min: false, width: 810, height: 650, resize: false,
        title: '送（提）货单打印窗口',
        content: 'url:../main/printsale?no=' + no
    });
}

function printurl(url)
{
    window.open(url);
}

function autopost() {
    $.post("../main/autopost", { guid: new Date() }, function (data) {
        if (data.status == true) setTimeout("autopost();", 300000);//5分钟
    });
}

function KeyDown() { }
function Initialized() { }

function getparas()
{
    where = "";
    var ap = csparas.split(',');
    var idp = htmlids.split(',');
    for (var i = 0; i < ap.length; i++) {
        if ($("#" + idp[i]).val() != '') where += " and " + ap[i] + " like '%" + $("#" + idp[i]).val() + "%'";
    }
}

function orderlist(by) {
    if (orderby.indexOf('desc') != -1) orderby = by + " asc";
    else orderby = by + " desc";
    loadlistpart(page);
}

function loadlistpart(p)
{
    page = p;
    getparas();
    //where = "?page=" + page + where;
    //if (orderby != '') where += "&orderby=" + orderby;
    $("#partlistdiv").load(parturl, { page: page, where: where, orderby: orderby }, function () { });
}

function deleteone(url,dno,type) {
    if (confirm("确定删除该" + type + "？")) {
        $.post(url, { no: dno }, function (data) {
            if (data == "ok") {
                loadlistpart(1);
            }
            else {
                $.dialog.alert("删除失败！");
            }
        });
    }
    else
        return false;

}