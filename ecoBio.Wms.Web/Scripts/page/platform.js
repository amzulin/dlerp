
/********************************/
/*  文字自动循环滚动  */
/*  IE6 FF1.0.4    */
/*  不支持xhtml声明的文档 */
/********************************/

//*********不要修改这部分***************
//scrollBodyId: String 内部滚动div的id
//scrollBoxId: String 外面限制div的id
//showHeight: Int 限制显示高度
//showWidth: Int 限制显示宽度
//lineHeight: Int 每行的高度
//stopTime:  Int 间隔停止的时间（毫秒）
//speed:  Int 滚动速度（毫秒，越小越快）
var ScrollObj = function(scrollBodyId,scrollBoxId,showHeight,showWidth,lineHeight,stopTime,speed) {
    this.obj = document.getElementById(scrollBodyId);
    this.box = document.getElementById(scrollBoxId);
 
    this.style = this.obj.style;
    this.defaultHeight = this.obj.offsetHeight;
 
    this.obj.innerHTML += this.obj.innerHTML;
    this.obj.style.position = "relative";
 
    this.box.style.height = showHeight;
    this.box.style.width = showWidth;
    this.box.style.overflow = "hidden";
 
    this.scrollUp = doScrollUp;

    this.stopScroll = false;
 
    this.curLineHeight = 0;
    this.lineHeight = lineHeight;
    this.curStopTime = 0;
    this.stopTime = stopTime;
    this.speed = speed;

    this.style.top = lineHeight;

    this.object = scrollBodyId + "Object";
    eval(this.object + "=this");
    setInterval(this.object+".scrollUp()",speed);
    this.obj.onmouseover=new Function(this.object+".stopScroll=true");
    this.obj.onmouseout=new Function(this.object+".stopScroll=false");
}
function doScrollUp() {
    if( this.stopScroll == true )
        return;
    this.curLineHeight += 1;
    if( this.curLineHeight >= this.lineHeight ) {
        this.curStopTime += 1;
        if( this.curStopTime >= this.stopTime ) {
            this.curLineHeight = 0;
            this.curStopTime = 0;
        }
    }
    else {   
        this.style.top = parseInt(this.style.top) - 1;
        if( -parseInt(this.style.top) >= this.defaultHeight ) {
            this.style.top = 0;
        }
    }
}
//***************这以上不要修改******************
