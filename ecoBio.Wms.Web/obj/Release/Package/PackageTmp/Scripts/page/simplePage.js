/**
 * @author xing
 * @copyright GZbugu
 */
(function($){
    $.fn.simplePage = function(os){
        var options = {
            pager: '.pager',//表格控件的容器
            container: '.tableData',//放置表格数据的容器
            form: '#form',//放置查询条件的表单
            pageForm: '#pageForm',//放置隐藏与的Div
            url: '',//发送请求的地址
            success: null,//成功后执行的回调函数
            currentPage: 1,
            pageSize: 2,
            callbacks: null,
            hoverClass: 'tdHover',
            param: {},//附加参数
            type: null,//可选：action,
            pageStyle:'line',//slide//click
			pageShow:7,
            feilds:[]
			//pageUpdate:false//是否更新的依据,私有属性
        };
        var o = $.extend(options, os);
        return this.each(function(){
            o.pager = $(this).find(o.pager);
			o.pager.find('a.go').html('确 定');
            o.container = $(this).find(o.container);
			//首先清除click事件
			o.pager.unbind('click');
            //	console.log(o);
			$('div.line').click(function(){
				if($(o.form).is(':hidden')){
					$(o.form).slideDown();
				}else{
					$(o.form).slideUp();
				}
			});
            $('p.tips').ajaxStart(function(){
                $(this).fadeIn('slow');
            });
            $('p.tips').ajaxComplete(function(){
                $(this).fadeOut('slow');
            });
            if (o.type == 'action') {
                //指定的动作，比如删除时的事件，这时需要在当前页刷新数据
                o.currentPage = $.page.getPage(o);
                o.pageSize = $.page.getPerp(o);
                $.page.setPage(o);
                return;
            }
				$.page.setPage(o);
				if(o.pageStyle=='click'){
					$.page.handler(o);
				}else if(o.pageStyle=='line'){
					$.line.handleQueryLine(o);
				}
        })
    }
	/**
	 * @classDescription 执行line模式下的逻辑代码
	 * @param {Object} totalPage
	 * @param {Object} o
	 */
	$.line={
		setLine:function(totalPage,o){
			for(var i=0;i<totalPage;i++){
				var a=$('<a/>').html('<span>'+(i+1)+'</span>').addClass('pageA').bind('click',function(){
					var s=$(this);
					s.siblings().removeClass('pageActive');
					s.addClass('pageActive');
					o.currentPage=s.text();
					$.page.loadData(o);
				});
				if(o.currentPage==i+1){
					a.addClass('pageActive');
				}
				o.pager.append(a);
			}
			var limit=this.getLimit(o,totalPage);
			var aPage=o.pager.find('a.pageA').not('a.previous,a.nextAll,a.record');
			aPage.hide();
			aPage.slice(limit.start,limit.end).show();
			var prev=$('<a/>').html('<span>上一页</span>').addClass('pageA previous').unbind('click').bind('click',function(){
				var pageActive=o.pager.find('a.pageActive');
				var s=$(this);
				if(pageActive.prev().text()=='上一页'){
					alert('已经是第一页!');
					return false;
				}
				pageActive.removeClass('pageActive');
				pageActive.prev().addClass('pageActive');
				o.currentPage=pageActive.prev().text();
				$.page.loadData(o);
			});
			var next=$('<a/>').html('<span>下一页</span>').addClass('pageA nextAll').unbind('click').bind('click',function(){
				var pageActive=o.pager.find('a.pageActive');
				var s=$(this);
				if(pageActive.next().text()=='下一页'){
					alert('已经是最后一页!');
					return false;
				}
				pageActive.removeClass('pageActive');
				pageActive.next().addClass('pageActive');
				o.currentPage=pageActive.next().text();
				$.page.loadData(o);
			});
			var pageActiveText=o.pager.find('a.pageActive').text();
			var record=$('<a/>').html('<span>'+pageActiveText+'/'+totalPage+'</span>').addClass('pageA record');
			o.pager.prepend(prev).prepend(record).append(next);
		},
		handleQueryLine:function(o){
			$(o.form).find(".query").click(function(){
                //$(o.pageForm).append($(o.form).clone(true));
                $(o.pageForm).empty();
                $(o.form).find('input[type="text"]').each(function(){
                    var vals = $(this).val();
                    var s = $(this).clone().val(vals);
                    $(o.pageForm).append(s);
                });
                $(o.form).find('select').each(function(){
                    var vals = $(this).val();
                    var s = $(this).clone().val(vals);
                    $(o.pageForm).append(s);
                });
				//需要更新页码
				//o.pageUpdate=true;
                $.page.query(o);
            });
		},
		getLimit:function(o,totalPage){
			var start=parseInt(o.pager.find('a.pageActive').text());
			start=parseInt(start-o.pageShow/2)<0?0:parseInt(start-o.pageShow/2);
			if(start+o.pageShow>totalPage){
				start=totalPage-o.pageShow;
			}
			var end=(start+o.pageShow)>totalPage?totalPage:start+o.pageShow;
			return {
				start:start,
				end:end
			}
		}
	};
	/**
	 * @classDescription 执行click模式下的逻辑代码
	 * @param {Object} o
	 */
    $.page = {
        setPage: function(o){
            $(o.pageForm).hide();
            o.pager.find('input.page').val(o.currentPage);
            o.pager.find('input.perpage').val(o.pageSize);
            this.loadData(o);
        },
        getPage: function(o){
			if(o.pageStyle=="click"){
				var p = o.pager.find("input.page").val();
            	return p;
			}else if(o.pageStyle=="line"){
				var p = o.pager.find("a.pageActive").text();
				return p;
			}else{
				return o.currentPage;
			}
        },
        getLastPage: function(o){
            var lps = o.pager.find("code").text();
            var lastP = parseInt(lps);
            return lastP;
        },
        getPerp: function(o){
			if(o.pageStyle=="click"){
				 var perp = o.pager.find("input.perpage").val();
            	 return perp;
			}else if(o.pageStyle=="line"){
				 return o.pageSize; 
			}
        },
        genData: function (o) {
            var pdata = $.extend({}, {
                "page": o.currentPage,
                "pageSize": o.pageSize
            }, o.param);
            var sdata = $.extend({}, pdata, $.jsonObj(o.pageForm));
            //	console.log(sdata);
            return sdata;
        },
        loadData: function(o){
            var that = this;
            var data = that.genData(o);
            $.ajax({
                url: o.url,
                data: data,
                /*{
                 currentPage: o.currentPage,
                 pageSize: o.pageSize
                 },*/
                type: 'get',
                dataType: 'json',
                cache: false,
                success: function (result) {
                    var res = $(result).find('tbody').html();
                    if (result.TotalCount<1) {
                        var lengths = o.container.find('th').length;
                        o.container.find('tbody').html('<tr><td colspan="' + lengths + '"><span class="alert">没有需要显示的数据！</span></td></tr>');
                        //赋值总页数
                        var codeval = $(result).find('input:hidden').eq(0).val();
                        var code = o.pager.find('code');
                        code.text(codeval);
                        //赋值当前页
                        var currentPage = $(result).find('input:hidden').eq(1).val();
                        // console.log(currentPage);
                        o.pager.find('input.page').val(currentPage);
                    }
                    else {
                        o.container.find('tbody').empty();
                        for (var i = 0; i < result.Data.length; i++) {
                            var otr = result.Data[i];
                            var onetr = "<tr>";
                            for (var j = 0; j < o.feilds.length; j++) {
                                var feildname = o.feilds[j];
                                var s = otr.toString();
                                var otrj = eval(otr);
                                for (var x in otr) {
                                    if (x==feildname) {
                                        onetr += "<td>" + otr[x] + "</td>";
                                    }
                                }
                            }
                            onetr += "</tr>";
                            o.container.find('tbody').append(onetr);
                        }

                        //o.container.find('tbody').html(res);
                        if (o.pageStyle == 'click') {
                            //赋值总页数
                            var codeval = result.TotalPages;
                            var code = o.pager.find('code');
                            code.text(codeval);
                            //赋值当前页
                            var currentPage =result.PageIndex;
                            // console.log(currentPage);
                            o.pager.find('input.page').val(currentPage);
                            that.hover(o);
                            if (typeof o.callbacks == "function") {
                                o.callbacks();
                            }
                        }
                        else if (o.pageStyle == 'line') {
                            var totalPage = result.TotalPages;
                            var currentPage = result.PageIndex;
                            o.currentPage = currentPage;
                            if (o.pager.find('a').length < 1) {
                                $.line.setLine(totalPage, o);
                            }
                            //查询更新页码
                            //	if(o.pageUpdate==true){
                            o.pager.empty();
                            $.line.setLine(totalPage, o);
                            //	}
                        }
                    }

                },
                error: function(){
                    alert("error");
                }
            })
        },
        handler: function(o){
            var that = this;
            o.pager.find('.pager_first').bind('click', function(){
              //  o.form = o.pageForm;
                o.currentPage = 1;
                o.pageSize = that.getPerp(o);
                that.setPage(o);
            });
            o.pager.find('.pager_last').bind('click', function(){
               // o.form = o.pageForm;
                o.currentPage = that.getLastPage(o);
                o.pageSize = that.getPerp(o);
                /*if (o.currentPage == lastP) {
                 return;
                 }*/
                that.setPage(o);
            });
            o.pager.find(".pager_prev").click(function(){
               // o.form = o.pageForm;
                o.pageSize = that.getPerp(o);
                var p = that.getPage(o);
                o.currentPage = p - 1;
                if (o.currentPage <= 0) {
                    o.currentPage = 1;
                    return;
                }
                that.setPage(o);
            });
            o.pager.find(".pager_next").click(function(){
              //  o.form = o.pageForm;
                o.pageSize = that.getPerp(o);
                var pag = that.getPage(o);
                var _p = parseInt(pag);
                o.currentPage = _p + 1;
                var tot = parseInt(o.pager.find(".input_page").find("code").text());
                if (o.currentPage > tot) {
                    o.currentPage = tot;
                    return;
                }
                that.setPage(o);
            });
            o.pager.find(".go").click(function(){
                $.page.query(o);
            });
            $(o.form).find(".query").click(function(){
                //$(o.pageForm).append($(o.form).clone(true));
                $(o.pageForm).empty();
                $(o.form).find('input[type="text"]').each(function(){
                    var vals = $(this).val();
                    var s = $(this).clone().val(vals);
                    $(o.pageForm).append(s);
                });
                $(o.form).find('select').each(function(){
                    var vals = $(this).val();
                    var s = $(this).clone().val(vals);
                    $(o.pageForm).append(s);
                });
                $.page.query(o);
            });
            o.pager.keyup(function(e){
                var e = e || window.event;
                if (e.keyCode == '13') {
                    $.page.query(o);
                }
            })
        },
        hover: function(o){
            o.container.find("tr").hover(function(){
                $(this).addClass('tdHover');
            }, function(){
                $(this).removeClass('tdHover');
            });
        },
        query: function(o){
            var that = this;
            o.pageSize = that.getPerp(o);
            var pag = that.getPage(o);
            var total = parseInt(o.pager.find('code').text());
            if (parseInt(pag) > total) {
                o.currentPage = that.getLastPage(o);
            }
            else {
                o.currentPage = parseInt(pag);
            }
            that.setPage(o);
        }
    }
    $.jsonObj = function(form){
        //判断是否有序列化的东东
        if (!$(form).html() || $(form).html() == null || $.trim($(form).html()) == "") {
            return null;
        }
        var formEl = $(form).find('input[type="text"]');
        var formselect = $(form).find('select');
        var json = "{";
        for (var i = 0; i < formEl.length - 1; i++) {
            var name = formEl.eq(i).attr('name');
            var val = "'" + formEl.eq(i).val() + "'";
            json += name;
            json += ":";
            json += val;
            json += ",";
        }
        var lname = formEl.eq(formEl.length - 1).attr('name');
        var lval = "'" + formEl.eq(formEl.length - 1).val() + "'";
        json += lname;
        json += ":";
        json += lval;
        if (formselect) {
            json += ",";
            for (var i = 0; i < formselect.length - 1; i++) {
                var name = formselect.eq(i).attr('name');
                var val = "'" + formselect.eq(i).val() + "'";
                json += name;
                json += ":";
                json += val;
                json += ",";
            }
            var lname = formselect.eq(formselect.length - 1).attr('name');
            var lval = "'" + formselect.eq(formselect.length - 1).val() + "'";
            json += lname;
            json += ":";
            json += lval;
        }
        json += "}";
        var jsonObj = eval("(" + json + ")")
        return jsonObj;
    }
})(jQuery)
