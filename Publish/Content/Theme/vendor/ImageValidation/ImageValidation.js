//ImgType => 0 for Any Size
//        => 1 for Fix Size
//        => 2 for Bigger Then
function FunImgUploader(ImgType, ImgID, Files, ImgWidth, ImgHeight, ImgErrorID, ImgTypeErrorID) {

    $("#" + ImgID).removeClass("input-validation-error");
    $("#" + ImgErrorID).removeClass("field-validation-error");
    $("#" + ImgTypeErrorID).removeClass("field-validation-error");

    var F = Files;
    if (F && F[0]) for (var i = 0; i < F.length; i++) readImage(ImgType, F[i], ImgID, ImgWidth, ImgHeight, ImgErrorID, ImgTypeErrorID);
}

function readImage(ImgType, file, ImgID, ImgWidth, ImgHeight, ImgErrorID, ImgTypeErrorID) {
    var reader = new FileReader();
    var image = new Image();
    reader.readAsDataURL(file);
    reader.onload = function (_file) {
        image.src = _file.target.result;
        image.onload = function () {

            if (ImgType == 1) {

                if (this.width == ImgWidth && this.height == ImgHeight) {
                    $("#" + ImgID).removeClass("input-validation-error");
                    $("#" + ImgErrorID).removeClass("field-validation-error");
                }
                else {
                    $("#" + ImgID).val("");
                    $("#" + ImgID).addClass("input-validation-error");
                    $("#" + ImgErrorID).addClass("field-validation-error");
                }
            }
            else if (ImgType == 2) {
                if (this.width >= ImgWidth && this.height >= ImgHeight) {
                    $("#" + ImgID).removeClass("input-validation-error");
                    $("#" + ImgErrorID).removeClass("field-validation-error");
                }
                else {
                    $("#" + ImgID).val("");
                    $("#" + ImgID).addClass("input-validation-error");
                    $("#" + ImgErrorID).addClass("field-validation-error");

                }
            }
        };
        image.onerror = function () {
            $("#" + ImgID).val("");
            $("#" + ImgID).addClass("input-validation-error");
            $("#" + ImgTypeErrorID).addClass("field-validation-error");
        };
    };
}
