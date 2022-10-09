/**
 * Call and add data madal add
 * @return {void}
 */
function callModatAddTT() {
    const setting = {
        url: "/QuanLyGiaoVien/Create",
        data: {},
        type: "get",
        dataType: "html",
        modal: "exampleModalCenter",
    }
    document.getElementById("exampleModalLongTitle").innerText = "Thêm Mới Giáo Viên";
    callDisplays("body-modal-qlgv", setting);
}
/**
 * Call and add data madal update
 * @param {String} _id
 * @return {void}
 */
function callModatUpdateTT(_id) {
    const setting = {
        url: "/QuanLyGiaoVien/Edit/" + _id,
        data: {},
        type: "get",
        dataType: "html",
        modal: "exampleModalCenter",
    }
    document.getElementById("exampleModalLongTitle").innerText = "Cập Nhật Giáo Viên";
    callDisplays("body-modal-qlgv", setting);
}
/**
 * Call and add data madal update
 * @param {String} _id
 * @return {void}
 */
function callModatDatailTT(_id) {
    const setting = {
        url: "/QuanLyGiaoVien/Details/" + _id,
        data: {},
        type: "get",
        dataType: "html",
        modal: "exampleModalCenter",
    }
    document.getElementById("exampleModalLongTitle").innerText = "Chi Tiết Giáo Viên";
    callDisplays("body-modal-qlgv", setting);
}

/**
 * Call and add data madal update 
 * @param {String} _id
 * @return {void}
 */
function callModatDeleteTT(_id) {
    const setting = {
        url: "/QuanLyGiaoVien/Delete/" + _id,
        data: {},
        type: "get",
        dataType: "html",
        modal: "exampleModalCenter",
    }
    document.getElementById("exampleModalLongTitle").innerText = "Xóa Giáo Viên";
    callDisplays("body-modal-qlgv", setting);
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
                $('#' + _id).html(data);
                $('#' + seting[0].modal).modal('show');
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


