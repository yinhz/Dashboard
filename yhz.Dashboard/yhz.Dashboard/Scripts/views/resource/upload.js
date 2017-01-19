function deleteImage(filename, ele) {
    $.post(
        "/Resource/DeleteImage",
        { fileName: filename },
    function (data, status) {
        if (data.Success) {
            $(ele).closest("tr").remove();
        }
        else {
            alert("Delete failed.Exception info：" + data.Info)
        }
    });
}

function deleteAudio(filename, ele) {
    $.post(
        "/Resource/DeleteAudio",
        { fileName: filename },
    function (data, status) {
        if (data.Success) {
            $(ele).closest("tr").remove();
        }
        else {
            alert("Delete failed.Exception info：" + data.Info)
        }
    });
}

function deleteVideo(filename, ele) {
    $.post(
        "/Resource/DeleteVideo",
        { fileName: filename },
    function (data, status) {
        if (data.Success) {
            $(ele).closest("tr").remove();
        }
        else {
            alert("Delete failed.Exception info：" + data.Info)
        }
    });
}

var file_input_id = 1;

function addImgFile() {
    file_input_id++;

    var file = $("#imgfile").clone();

    file.removeAttr("id");

    file.find("input[type=file]").attr("name", "file_img_" + file_input_id);

    file.css("display", "block")
        .appendTo("#imgfiles");
}

function addAudioFile() {
    file_input_id++;

    var file = $("#audiofile").clone();

    file.removeAttr("id");

    file.find("input[type=file]").attr("name", "file_audio_" + file_input_id);

    file.css("display", "block")
        .appendTo("#audiofiles");
}

function addVideoFile() {
    file_input_id++;

    var file = $("#videofile").clone();

    file.removeAttr("id");

    file.find("input[type=file]").attr("name", "file_video_" + file_input_id);

    file.css("display", "block")
        .appendTo("#videofiles");
}

function removeFile(btn) {
    $(btn).closest(".row").remove();
}

activeli($("#nav_li_upload"));