$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function (e) {
        $('#myModalContent').load(this.href, function () {
            $('#myModal').modal({
                keyboard: true
            }, 'show');
            bindForm(this);
        });
        return false;
    });
});
function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $('#progress').show();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.Success) {
                    $('#myModal').modal('hide');
                    $('#progress').hide();
                    location.reload();

                } else {
                    $('#progress').hide();
                    if (result.Message != null) {
                        alert(result.Message);
                    }

                    bindForm();

                }
            }
        });
        return false;
    });
}