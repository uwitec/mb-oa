function Common() {
}

Common.clearIframe = function (ifm) {
    ifm.src = "javascript:false"; // "about:blank";  // 差不多，前者略好点
    //debugger;
    //ifm.document.write(""); //之后，很多属性拒绝访问。EXT出错
    //ifm.document.clear(); //没有权限
    //ifm.parentNode.removeChild(ifm); //加上此句，内存反而释放少了
    if (CollectGarbage) { //未验证
        CollectGarbage();
    }
}