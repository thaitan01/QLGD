/**
 * Call and add data madal add
 * @return {void}
 */
function callModatAdd() {
    const setting = {
        url: "/HocKy/Create",
        data: {},
        type: "get",
        dataType: "html",
        modal: "exampleModalCenters",
    }
    document.getElementById("exampleModalLongTitle").innerText = "Thêm Mới Học Kỳ";
    callDisplays(1, setting)
}
/**
 * Call and add data madal add
 * @return {void}
 */
function callModatDetail(_id) {
    const setting = {
        url: "/HocKy/Details"+_id,
        data: {},
        type: "get",
        dataType: "html",
        modal: "exampleModalCenter",
    }
    document.getElementById("exampleModalLongTitle").innerText = "Chi Tiết";
    callDisplays(1, setting)
}
/**
 * Call and add data madal add
 * @return {void}
 */
function callModatUpdate(_id) {
    const setting = {
        url: "/HocKy/Edit/" + _id,
        data: {},
        type: "get",
        dataType: "html",
        modal: "exampleModalCenter",
    }
    document.getElementById("exampleModalLongTitle").innerText = "Cập Nhật";
    callDisplays(1, setting)
}
/**
 * Call and add data madal add
 * @return {void}
 */
function callModatDelete(_id) {
    const setting = {
        url: "/HocKy/Delete" + _id,
        data: {},
        type: "get",
        dataType: "html",
        modal: "exampleModalCenter",
    }
    document.getElementById("exampleModalLongTitle").innerText = "Xóa";
    callDisplays(1, setting)
}
/**
 * Call display apter find tang id
 * @param {string} _id 
 * @param {object} sseting
 * @return {void}
 */
async function callDisplays(_id, ...seting) {
    changLoading(true)
    try {
        await $.ajax({
            url: seting[0].url,
            data: seting[0].data,
            type: seting[0].type,
            dataType: seting[0].dataType,
            success: function (data) {
                changLoading(false)
                $('#' + seting[0].modal).html(data);
                $('#' + seting[0].modal).modal('show');
            },
            error: function (e) {
                changLoading(false)
               /* $('#' + _id).html('<p>' + e + '</p>');*/
            },
        });
    } catch (e) {
        changLoading(false)
        console.log(e);
    }
}