function CreateUploadCtl(fileUploadDiv,hiddenID) {

    if (!document.getElementById("spanButtonPlaceholder")) {
        var spanButtonPlaceholder = document.createElement("div");
        spanButtonPlaceholder.id = "spanButtonPlaceholder";
        document.getElementById(fileUploadDiv).appendChild(spanButtonPlaceholder);
    }

    new SWFUpload({
        // Backend Settings
        upload_url: "/office/myoffice/fileUpload", //上传处理
        post_params: {},

        // 文件上传参数设置
        file_size_limit: "2 MB", // 动态修改设置中的file_size_limit，此修改针对之后的文件大小过滤有效。file_size_limit参数接收一个单位，有效的单位有B、KB、MB、GB，默认单位是KB。例如: 2147483648 B, 2097152, 2097152KB, 2048 MB, 2 GB
        file_types: "*.jpg;*.gif", // 默认值：*.*, 不限制类型
        file_types_description: "JPG/GIF 图片文件",
        file_upload_limit: "0",    // 上传文件数量限制, 0表示不限制

        // 事件处理函数
        /*
        file对象属性:
        index: 文件在整个队列中的位置,以0开始
        name: 文件名称
        size: 文件大小
        type: 文件名后缀,如".jpg"
        */
        // 队列错误
        file_queue_error_handler: function (file, errorCode, message) {
        },
        // 对话框选择文件完成
        file_dialog_complete_handler: function (numFilesSelected, numFilesQueued) {
            /*debugger;*/
            try {
                if (numFilesQueued > 0) {
                    this.startUpload();
                }
            } catch (ex) {
                this.debug(ex);
            }

        },
        upload_progress_handler: function (file, bytesLoaded) {
        },
        upload_error_handler: function (file, errorCode, message) {
        },
        upload_success_handler: function (file, serverData) { //上传成功,在upload_complete_handler之前执行
            //debugger;
            try {
                var status = document.getElementById("uploadStatus");
                if (!status) {
                    var status = document.createElement("span");
                    status.id = "uploadStatus";
                    document.getElementById(fileUploadDiv).appendChild(status);
                }
                //debugger;
                status.innerHTML = file.name + '上传完成;';
                this.customSettings.files[serverData] = file.name + "(" + Math.round(file.size / 1024) + "KB)";

            } catch (ex) {
                this.debug(ex);
            }
        },
        upload_complete_handler: function (file) {
            //debugger;
            try {
                this.customSettings.amount++; // = this.customSettings.amount + 1;
                this.customSettings.size = this.customSettings.size + file.size;
                if (this.getStats().files_queued > 0) {
                    this.startUpload();
                } else {
                    document.getElementById("uploadStatus").innerHTML = "上传完成!";
                    var uploadResult = document.getElementById("uploadResult");
                    if (!uploadResult) {
                        var uploadResult = document.createElement("div");
                        uploadResult.id = "uploadResult";
                        document.getElementById(fileUploadDiv).appendChild(uploadResult);
                    }

                    var hidFiles = document.getElementById(hiddenID);
                    /*if (!hidFiles) {
                        echkbox.setAttribute("type", "hidden");
                        echkbox.setAttribute("id", "hidFiles");
                        echkbox.setAttribute("name", "hidFiles");
                        document.getElementById(fileUploadDiv).appendChild(hidFiles);
                    }*/
                    document.getElementById("uploadStatus").innerHTML = "共上传了 " + this.customSettings.amount + "个文件, 共计 " + Math.round(this.customSettings.size / 1024) + "KB&nbsp;:&nbsp;&nbsp;";

                    for (var id in this.customSettings.files) {
                        uploadResult.innerHTML = uploadResult.innerHTML + "<span  style='margin-right:20px;display:inline-block;'><a href='" + id + "'>" + this.customSettings.files[id] + "</a><img src='/content/images/cross.gif' alt='删除' title='删除'/></span>";
                        hidFiles.value = hidFiles.value + "," + id;
                    }
                    //status.innerHTML = "共上传了 " + this.customSettings.amount + "个文件, 共计 " + Math.round(this.customSettings.size / 1024) + "KB";
                }
            } catch (ex) {
                this.debug(ex);
            }
        },

        // 按钮设置
        button_image_url: "images/XPButtonNoText_160x22.png",
        button_placeholder_id: "spanButtonPlaceholder", //按钮在哪个位置上,HTML中一定要有这个元素,不然会抛异常
        button_width: 160,
        button_height: 20,
        button_text: '<span class="button">选择图片<span class="buttonSmall">(最大 2MB)</span></span>',
        button_text_style: '.button { font-family: Helvetica, Arial, sans-serif; font-size: 12px; border: solid 1px } .buttonSmall { font-size: 12px; }',
        button_text_top_padding: 1,
        button_text_left_padding: 5,

        // Flash设置
        flash_url: "/content/swfupload/swfupload.swf", // flash文件的位置

        // 客户的设置, 设置完毕以后，可以通过实例的customSettings属性来访问
        custom_settings: {
            amount: 0,
            size: 0,
            files: {}
        },

        // Debug Settings
        debug: false
    });
}