/**
 * Call display apter find tang id
 * @param {string} _id 
 * @param {object} sseting
 * @return {void}
 */
async function callDisplay(_id, ...seting) {

    changLoading(true)
    try {
        await $.ajax({
            url: seting[0].url,
            data: seting[0].data,
            type: seting[0].type,
            dataType: seting[0].dataType,
            success: function (data) {
                changLoading(false)
                $('#' + _id).html(data);
                $('#' + seting[0].modal).modal('show');
                $('#save-' + seting[0].modal).click(function () {
                    const from = $("#" + $('#' + _id).find('form')[0].id);
                    console.log(_id);
                    const dataReturn = submitForm(from.attr('method'), from.attr('action'), from.getFormData())
                    Swal.fire({
                        position: 'top-end',
                        title: 'Thành Công',
                        showConfirmButton: false,
                        timer: 1000
                    })
                    $('#' + seting[0].modal).modal('hide');
                })
            },
            error: function (e) {
                changLoading(false)
                $('#' + _id).html('<p>' + e + '</p>');
            },
        });
    } catch (e) {
        console.log(e);
    }
}
/**
 * Submit form 
 * @param  {any} method 
 * @param  {any} action 
 * @param  {any} data 
 * @return {*} 
 */
async function submitForm(method, action, data) {
    let status = false
    await $.ajax({
        type: method,
        url: action,
        data: data,
        success: function (_data) {
            console.log(_data);
            status = true
        },
        error: function (_data) {
            status = _data
        }
    });
    return status
}
/**
 * Chang is show loading
 * @param {boolean} _isLoading 
 * @return {void}
 */
function changLoading(_isLoading) {
    const loading = document.getElementById("loading")
    if (_isLoading)
        loading.classList.add("loading")
    else
        loading.classList.remove("loading")
}
/**
 * Get data in from
 * @param {*} $
 * @return {object}
 */
(function ($) {
    $.fn.getFormData = function () {
        var data = {};
        var dataArray = $(this).serializeArray();
        for (var i = 0; i < dataArray.length; i++) {
            data[dataArray[i].name] = dataArray[i].value;
        }
        return data;
    }
})(jQuery);
/**
 * 
 */

$(document).ready(function onDocumentReady() {
});
