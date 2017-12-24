(function () {
    var dropzone = document.getElementById('dropzone');

    var upload = function (files) {

        var formData = new FormData();

        formData.append('file', files[0]);

        $.ajax({
            type: "Post",
            url: "ChangePicture",
            contentType: false,
            processData: false,
            data: formData,
            success: function () {
                alert("Successfully uploaded your new picture!");
                window.location.href="@Url.Action(\"Details\",\"User\")";
            },
            error: function () {
                alert("There was error uploading files!");
            }
        });
    };
    dropzone.ondrop = function (e) {
        e.preventDefault();
        this.className = 'dropzone';
        upload(e.dataTransfer.files);
    };

    dropzone.ondragover = function () {
        this.className = 'dropzone dragover';
        return false;
    };

    dropzone.ondragleave = function () {
        this.className = 'dropzone';
        return false;
    };
}());
