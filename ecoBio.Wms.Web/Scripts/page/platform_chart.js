

var cmbu1, cmbu2, cmbu3, cmbu4;
var cmbp1, cmbp2, cmbp3, cmbp4;
var pdata1, pdata2, pdata3;
$(function () {


    cmbu1 = $("#ddlunit1").ligerComboBox({
        width: 200,
        data: sunit1, isMultiSelect: false,
        onSelected: function (newvalue, txt) {
            //$("#unit_1").text(txt);
            sel_uname1 = txt;
            sel_unit1 = newvalue; sel_p11 = ""; sel_p12 = "";
            $.post("../home/getunitcollections", { unit: newvalue, items: '7,8,9' }, function (data) {
                var newData = new Array();
                if (data != "null") { newData = eval(data); }
                pdata1 = newData;
                cmbp1.setData(newData);
            });
        }
    });
    //cmbu1.selectValue(sunit1[0].id);
    cmbp1 = $("#ddlpoints1").ligerComboBox({
        width: 200, data: null, isMultiSelect: true, isShowCheckBox: false, split: ',',
        onBeforeSelect: function (value, text) {
            if (sel_p11 == "") sel_p11 = value;
            else if (sel_p12 == "") sel_p12 = value;
            else
            {
                var td = $("td[value='" + sel_p12 + "']");
                var a = $(td[0]).find("a");
                a.removeClass("l-checkbox-checked");
                sel_p12 = value;
            }
        },
        onSelected: function (value, text) {
            var sel = value.split(',');
            if (sel.length == 0) {
                sel_p11 = "";
                sel_p12 = "";
            }
            else if (sel.length == 1) {
                sel_p11 = value;
                sel_p12 = "";
            }
            else if (sel.length == 2) {
                sel_p11 = sel[0];
                sel_p12 = sel[1];
            }
            else {
                var td = $("td[value='" + sel[0] + "']");
                cmbp1.selectValue(sel_p11 + "," + sel_p12);
            }
        }
    });
    $("#seli_1").click(function () {
        $("#chart_1").css("display", "none");
        //$("#flex1").
        $("#sel_1").css("display", "block");
    });




    //2
    cmbu2 = $("#ddlunit2").ligerComboBox({
        width: 200,
        data: sunit2, isMultiSelect: false,
        onSelected: function (newvalue, txt) {
            //$("#unit_2").text(txt);
            sel_uname2 = txt;
            sel_unit2 = newvalue; sel_p21 = ""; sel_p22 = "";
            $.post("../home/getunitcollections", { unit: newvalue, items: '10, 11, 12, 13' }, function (data) {
                var newData = new Array();
                if (data != "null") { newData = eval(data); }
                pdata2 = newData;
                cmbp2.setData(newData);
            });
        }
    });
    //cmbu1.selectValue(sunit1[0].id);
    cmbp2 = $("#ddlpoints2").ligerComboBox({
        width: 200, data: null, isMultiSelect: true, isShowCheckBox: false, split: ',',
        onBeforeSelect: function (value, text) {
            if (sel_p21 == "") sel_p21 = value;
            else if (sel_p22 == "") sel_p22 = value;
            else {
                var td = $("td[value='" + sel_p22 + "']");
                var a = $(td[0]).find("a");
                a.removeClass("l-checkbox-checked");
                sel_p22 = value;
            }
        },
        onSelected: function (value, text) {
            var sel = value.split(',');
            if (sel.length == 0) {
                sel_p21 = "";
                sel_p22 = "";
            }
            else if (sel.length == 1) {
                sel_p21 = value;
                sel_p22 = "";
            }
            else if (sel.length == 2) {
                sel_p21 = sel[0];
                sel_p22 = sel[1];
            }
            else {
                var td = $("td[value='" + sel[0] + "']");
                cmbp2.selectValue(sel_p21 + "," + sel_p22);
            }
        }
    });
    $("#seli_2").click(function () {
        $("#chart_2").css("display", "none");
        $("#sel_2").css("display", "block");
    });


    //3
    cmbu3 = $("#ddlunit3").ligerComboBox({
        width: 200,
        data: sunit3, isMultiSelect: false,
        onSelected: function (newvalue, txt) {
            //$("#unit_3").text(txt);
            sel_uname3 = txt;
            sel_unit3 = newvalue; sel_p31 = ""; sel_p32 = "";
            $.post("../home/getunitcollections", { unit: newvalue, items: '14, 15, 16' }, function (data) {
                var newData = new Array();
                if (data != "null") { newData = eval(data); }
                pdata3 = newData;
                cmbp3.setData(newData);
            });
        }
    });
    //cmbu1.selectValue(sunit1[0].id);
    cmbp3 = $("#ddlpoints3").ligerComboBox({
        width: 200, data: null, isMultiSelect: true, isShowCheckBox: false, split: ',',
        onBeforeSelect: function (value, text) {
            if (sel_p31 == "") sel_p31 = value;
            else if (sel_p32 == "") sel_p32 = value;
            else {
                var td = $("td[value='" + sel_p32 + "']");
                var a = $(td[0]).find("a");
                a.removeClass("l-checkbox-checked");
                sel_p32 = value;
            }
        },
        onSelected: function (value, text) {
            var sel = value.split(',');
            if (sel.length == 0) {
                sel_p31 = "";
                sel_p32 = "";
            }
            else if (sel.length == 1) {
                sel_p31 = value;
                sel_p32 = "";
            }
            else if (sel.length == 2) {
                sel_p31 = sel[0];
                sel_p32 = sel[1];
            }
            else {
                var td = $("td[value='" + sel[0] + "']");
                cmbp3.selectValue(sel_p31 + "," + sel_p32);
            }
        }
    });
    $("#seli_3").click(function () {
        $("#chart_3").css("display", "none");
        $("#sel_3").css("display", "block");
    });




    //4
    cmbu4 = $("#ddlunit4").ligerComboBox({
        width: 200,
        data: chart4data, isMultiSelect: false,
        onSelected: function (newvalue, txt) {
            chart4t = txt;
            //$("#chart4_a").text(txt);
            sel_cost = newvalue;

            var dd2 = $("#sel_4").find("div[class='l-text-wrapper']");
            $(dd2[1]).css("display", "block");

            if (newvalue == "CM05" || newvalue == "CC08") {
                cmbp4.setData(ddlm);
            }
            else if (newvalue == "CM06" || newvalue == "CC09") {
                cmbp4.setData(ddly);
            }
            else {          
                $(dd2[1]).css("display", "none");
                cmbp4.setData(new Array());
                $("#ddlpoints4").val("");
                cmbp4.selectValue('');
                $("#unit_4").text("");
                chart4name = "";
            }
        }
    });
    cmbp4 = $("#ddlpoints4").ligerComboBox({
        width: 200, data: null, isMultiSelect: false, isShowCheckBox: false,
        onSelected: function (value, text) {
            $("#unit_4").text(text);
            sel_material = value;
            chart4name = text;
        }
    });
    $("#seli_4").click(function () {
        $("#chart_4").css("display", "none");
        $("#sel_4").css("display", "block");
    });

});

function rf1()
{
    if (sel_p11 == "" && sel_p12 == "") {
        $.dialog.alert("请选择要查看的客户采集点！");
        return false;
    }
    else {
        $("#chart_1").css("display", "block");
        $("#sel_1").css("display", "none");
        $.post("../home/savenewconfig", { number: 1, unitname: sel_uname1, select1: sel_p11, select2: sel_p12 }, function (data) {
            var two = eval('(' + data + ')');
            var chart = two.chart;
            //chart.charttype = 'line3';
            //if (chart.processparms.indexOf(',') > 0) chart.charttype = 'line4';
            //if (c1h == 1 && chart.charttype == flextype1) {
            //    $("#unit_1").text(sel_uname1);
            //    var app = document.getElementById("flex1");
            //    var t = app.reload(chart.number, ct, 2, 4, chart.queryparms, chart.processparms,
            //   chart.needlower, chart.lowerlimit,
            //   chart.needlow, chart.lowlimit,
            //   chart.needup, chart.uplimit,
            //   chart.needuper, chart.uperlimit,
            //   chart.leftprecision, chart.rightprecision
            //   );
            //    //setTimeout("reloadflex(1," + chart + ")", 4000);
            //}
            //else {
            f1(chart);
            //}
        });
    }
}

function rf2() {
    if (sel_p21 == "" && sel_p22 == "") {
        $.dialog.alert("请选择要查看的客户采集点！");
        return false;
    }
    else {
        $("#chart_2").css("display", "block");
        $("#sel_2").css("display", "none");
        $.post("../home/savenewconfig", { number: 2, unitname: sel_uname2, select1: sel_p21, select2: sel_p22 }, function (data) {
            var two = eval('(' + data + ')');
            var chart = two.chart;
            //var ct =  'line3';
            //if (chart.processparms.indexOf(',') > 0) ct = 'line4'; 
            //if (c2h == 1 && ct == flextype2) {
            //    objchart2 = document.getElementById("flex2");
            //    var t = objchart2.reload(chart.number, ct, 2, 4, chart.queryparms, chart.processparms,
            //        chart.needlower, chart.lowerlimit,
            //        chart.needlow, chart.lowlimit,
            //        chart.needup, chart.uplimit,
            //        chart.needuper, chart.uperlimit,
            //        chart.leftprecision, chart.rightprecision
            //        );
            //}
            //else {
                f2(chart);
            //}
        });
    }
}

function rf3() {
    if (sel_p31 == "" && sel_p32 == "") {
        $.dialog.alert("请选择要查看的客户采集点！");
        return false;
    }
    else {
        $("#chart_3").css("display", "block");
        $("#sel_3").css("display", "none");
        $.post("../home/savenewconfig", { number: 3, unitname: sel_uname3, select1: sel_p31, select2: sel_p32 }, function (data) {
            var two = eval('(' + data + ')');
            var chart = two.chart;
            //var ct = 'line3';
            //if (chart.processparms.indexOf(',') > 0) ct = 'line4';
            //if (c3h == 1 && ct == flextype3) {
            //    objchart3 = document.getElementById("flex3");
            //    var t = objchart3.reload(chart.number, ct, 2, 4, chart.queryparms, chart.processparms,
            //        chart.needlower, chart.lowerlimit,
            //        chart.needlow, chart.lowlimit,
            //        chart.needup, chart.uplimit,
            //        chart.needuper, chart.uperlimit,
            //        chart.leftprecision, chart.rightprecision
            //        );
            //}
            //else {
                f3(chart);
            //}
        });
    }
}

function rf4() {
    if (sel_cost == "") {
        $.dialog.alert("请选择图表类别！");
        return false;
    }
    else if (sel_cost != "CC07" && sel_cost != "CC10" && sel_material == "") {
        $.dialog.alert("请选择相应物料！");
        return false;
    }
    else {
        $("#chart_4").css("display", "block");
        $("#sel_4").css("display", "none");
        $.post("../home/savematerialconfig", { number: sel_cost, material: sel_material }, function (data) {
            var two = eval('(' + data + ')');
            var chart = two.chart;

            //chart.charttype = 'line4';
            //if (chart.number == "CC07") chart.charttype = 'column4';

            //if (c4h == 1 && chart.charttype==flextype4) {
            //    app = document.getElementById("flex4");
            //    //var t = app.reload(chart.number, 'line4', 2, 4, chart.queryparms, chart.processparms,
            //    //    chart.needlower, chart.lowerlimit,
            //    //    chart.needlow, chart.lowlimit,
            //    //    chart.needup, chart.uplimit,
            //    //    chart.needuper, chart.uperlimit,
            //    //    chart.leftprecision, chart.rightprecision
            //    //    );
            //    var t = app.reload(chart.number, chart.charttype, 2, 4, chart.queryparms, chart.processparms,
            //    chart.needlower, chart.lowerlimit,
            //    chart.needlow, chart.lowlimit,
            //    chart.needup, chart.uplimit,
            //    chart.needuper, chart.uperlimit,
            //    chart.leftprecision, chart.rightprecision
            //    );
            //}
            //else {
            f4(chart);
            // }
        });
    }
}

function cancel(i) {
    $("#chart_" + i).css("display", "block");
    $("#sel_" + i).css("display", "none");
}

function fullscreen(i) {
    var s1 = "";
    var s2 = "";
    var title = "";
    switch (i) {
        case 1:
            s1 = sel_p11;
            s2 = sel_p12;
            title = sel_uname1;
            break;
        case 2:
            s1 = sel_p21;
            s2 = sel_p22;
            title = sel_uname2;
            break;
        case 3:
            s1 = sel_p31;
            s2 = sel_p32;
            title = sel_uname3;
            break;
        default:

    }
    var m = "";
    if (s2 == "" && s1 == "") {
        $.dialog.alert("请选择要查看的客户采集点！");
        return false;
    }
    var ww = document.documentElement.clientWidth;// ==> BODY对象宽度

    var wh = document.documentElement.clientHeight;// ==> BODY对象高度

    var w = ww - 200;
    var h = wh - 200;
    title = title.replace('#', '');
    var nt=encodeURI(title);
    var url = 'url:../datacenter/chartfullscreen?t=0&title=' + nt + '&s1=' + s1 + ',' + s2 + '&w=' + w + '&h=' + h + "&m=" + m;
    $.dialog({ id: 'win_repair', lock: true, max: false, min: false, width: ww - 150, height: wh - 150, resize: false, title: title, content: 'url:../datacenter/chartfullscreen?t=0&title=' + nt + '&s1=' + s1 + '&s2=' + s2 + '&w=' + w + '&h=' + h + "&m=" + m });

}

function chart4full() {
    //var sel_cost = '@chart4_2', sel_material = '@chart4_1';
    if (sel_cost == "") return false;
    var ww = document.documentElement.clientWidth;// ==> BODY对象宽度
    var wh = document.documentElement.clientHeight;// ==> BODY对象高度
    var w = ww - 200;
    var h = wh - 200;
    var nt = encodeURI(chart4t);
    if (sel_cost == "CM05" || sel_cost == "CM06") {//物料趋势
        $.dialog({ id: 'win_repair2', lock: true, max: false, min: false, width: ww - 150, height: wh - 150, resize: false, title: chart4t, content: 'url:../material/fullscreen?line=line5&acl=2&acr=3&t=' + nt + '&n=' + sel_cost + '&w=' + w + '&h=' + h + "&m=" + sel_material });
    }
    else if (sel_cost == "CC08" || sel_cost == "CC09") {//成本趋势
        $.dialog({ id: 'win_repair2', lock: true, max: false, min: false, width: ww - 150, height: wh - 150, resize: false, title: chart4t, content: 'url:../costanalysis/fullscreen?line=line5&acl=2&acr=4&t=' + nt + '&n=' + sel_cost + '&w=' + w + '&h=' + h + "&m=" + sel_material });
    }
    else if (sel_cost == "CC07") {//成本结构
        $.dialog({ id: 'win_repair2', lock: true, max: false, min: false, width: ww - 150, height: wh - 150, resize: false, title: chart4t, content: 'url:../costanalysis/costfullscreen?line=column3&acl=3&acr=3&t=' + nt + '&n=' + sel_cost + '&w=' + w + '&h=' + h });
    }
    else if (sel_cost == "CC10") {//沼气效益
        $.dialog({ id: 'win_repair2', lock: true, max: false, min: false, width: ww - 150, height: wh - 150, resize: false, title: chart4t, content: 'url:../costanalysis/biogasfullscreen?line=line5&acl=2&acr=3&t=' + nt + '&n=' + sel_cost + '&w=' + w + '&h=' + h });
    }
    else return false;
   
}

function reloadflex(i,chart)
{
    var app = document.getElementById("flex" + i);
    var t = app.reload(chart.number, chart.charttype, 2, 4, chart.queryparms, chart.processparms,
                    chart.needlower, chart.lowerlimit,
                    chart.needlow, chart.lowlimit,
                    chart.needup, chart.uplimit,
                    chart.needuper, chart.uperlimit,
                    chart.leftprecision, chart.rightprecision
                    );
}

function change(v)
{
    $("#sel_4").css("padding", v);
}