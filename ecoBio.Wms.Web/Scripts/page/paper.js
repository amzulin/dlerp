
function Paper(action,column)
{
    function query() {
        var txt = $("#txtid").val();
        $("#datatable").empty();
        $("#datatable").html("查询中...");
        $.get("waterloaddata", { Column: '', Direction: '', page: pageindex, search: txt }, function (data) {
            //数据部分
            var trs = ""; $("#datatable").empty();
            for (var i = 0; i < data.Data.length; i++) {
                var otr = data.Data[i];
                trs = "<tr><td>" + otr.sn + "</td>"
                    + "<td>" + otr.sn + "</td>"
                    + "<td>" + otr.guid + "</td>"
                    + "<td>" + otr.name + "</td>"
                    + "<td>" + otr.value + "</td>"
                    + "<td>" + otr.date + "</td></tr>";
                $("#datatable").append(trs);
            }
            //分页部分

            pagetotal = data.TotalPages;
            $("#pindex").text(data.PageIndex);
            $("#pcount").text(data.TotalPages);
            $("#pfirst").css("cursor", data.PageIndex > 1 && data.TotalPages > 1 ? "pointer" : "default").css("font-weight", data.PageIndex > 1 && data.TotalPages > 1 ? "bold" : "normal");
            $("#pprev").css("cursor", data.PageIndex > 1 && data.TotalPages > 1 ? "pointer" : "default").css("font-weight", data.PageIndex > 1 && data.TotalPages > 1 ? "bold" : "normal");
            $("#pnext").css("cursor", data.PageIndex < data.TotalPages && data.TotalPages > 1 ? "pointer" : "default").css("font-weight", data.PageIndex < data.TotalPages && data.TotalPages > 1 ? "bold" : "normal");
            $("#plast").css("cursor", data.PageIndex < data.TotalPages && data.TotalPages > 1 ? "pointer" : "default").css("font-weight", data.PageIndex < data.TotalPages && data.TotalPages > 1 ? "bold" : "normal");
            querying = 0;
        });
    }
    function firstpage() { if (pageindex == 1 || querying == 1) return false; else { pageindex = 1; querying = 1; query(); } }
    function nextpage() { if (pageindex < pagetotal && querying != 1) { pageindex += 1; querying = 1; query(); } else return false; }
    function prevpage() { if (pageindex > 1 && querying != 1) { pageindex -= 1; querying = 1; query(); } else return false; }
    function lastpage() { if (pageindex == pagetotal || querying == 1) return false; else { pageindex = pagetotal; querying = 1; query(); } }

}