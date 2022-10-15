
$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function (e) {
        changLoading(true);
        $('#myModalContent').load(this.href, function () {
            $('#myModal').modal({
                keyboard: true
            }, 'show');
            changLoading(false);
            bindForm(this);
        });
        return false;
    });
});

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        changLoading(true);
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.Success) {
                    $('#myModal').modal('hide');
                    location.reload();
                } else {
                    if (result.Message != null) {
                        alert(result.Message);
                    }
                    bindForm();
                }
                changLoading(false);
            }
        });
        return false;
    });
}

