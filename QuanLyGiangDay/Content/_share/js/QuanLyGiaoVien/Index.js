window.onload = function () {
    let idForm = "";
    function changIdForm(e) {
        idForm = e
    }

    document.getElementById("save-t").addEventListener("click", function (e) {
        document.getElementById(idForm + "_table").submit();
    })
    document.getElementById("btn-call-add-gv").addEventListener("click", function () {
        callModatAddTT();
    })
    // update Tech
    const updateGV = document.getElementsByClassName("button-update-gv")
    for (let index = 0; index < updateGV.length; index++) {
        updateGV[index].addEventListener("click", function (e) {
            callModatUpdateTT(updateGV[index].dataset.id);
        })

    }
    // detail Tech
    const detailGV = document.getElementsByClassName("button-detail-gv")
    for (let index = 0; index < updateGV.length; index++) {
        detailGV[index].addEventListener("click", function (e) {
            callModatDatailTT(detailGV[index].dataset.id);
        })

    }
    // delete Tech
    const deleteleGV = document.getElementsByClassName("button-delete-gv")
    for (let index = 0; index < updateGV.length; index++) {
        deleteleGV[index].addEventListener("click", function (e) {
            callModatDeleteTT(deleteleGV[index].dataset.id);
        })

    }
    //
    const trTableHover = document.getElementsByClassName("c-tr-body");
    for (let i = 0; i < trTableHover.length; i++) {
        trTableHover[i].addEventListener("mouseover", function (e) {
            console.log(trTableHover[i].dataset.id);
            document.getElementById(trTableHover[i].dataset.id).style.display = "block";
        })
    }
    for (let i = 0; i < trTableHover.length; i++) {
        trTableHover[i].addEventListener("mouseout", function (e) {
            document.getElementById(trTableHover[i].dataset.id).style.display = "none";
        })
    }
    //
    document.getElementById("dataTable_previous").getElementsByTagName("a")[0].text = "Về Trước"
    document.getElementById("dataTable_next").getElementsByTagName("a")[0].text = "Về Sau"
    //
    document.getElementById("dataTable_filter").getElementsByTagName("label")[0]
        .getElementsByTagName("input")[0].placeholder = "Nhập nọi dung  cần tìm";
    document.getElementById("dataTable_filter").getElementsByTagName("label")[0]
        .getElementsByTagName("input")[0].style.width = "400px";
    document.getElementById("dataTable_filter").getElementsByTagName("label")[0]
        .getElementsByTagName("input")[0].style.height = "40px";
    // console.log(document.getElementById("dataTable_filter").getElementsByTagName("label"));
    // document.getElementById("dataTable_filter").getElementsByTagName("label")[0].innerText = "sss";
};
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
    callDisplay("body-modal-qlgv", setting);
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
    callDisplay("body-modal-qlgv", setting);
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
    callDisplay("body-modal-qlgv", setting);
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
    callDisplay("body-modal-qlgv", setting);
}

