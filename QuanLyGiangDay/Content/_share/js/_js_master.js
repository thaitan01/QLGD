/**
 * Call display apter find tang id
 * @param {string} _id 
 * @param {object} sseting
 * @return {void}
 */
function callDisplay(_id, ...seting) {
    try {
        $.ajax({
            url: seting[0].url,
            data: seting[0].data,
            type: seting[0].type,
            dataType: seting[0].dataType,
            success: function (data) {
                $('#' + _id).html(data);
                $('#' + seting[0].modal).modal('show');
            },
            error: function (e) {
                $('#' + _id).html('<p>' + e + '</p>');
            },
        });
    } catch (e) {
        console.log(e);
    }
}
/**
 * Chang tab
 * @param {*} param
 * @return {void}
 */
function changTab(...param) {
    // const tab = document.getElementsByClassName("heder-tab").stylpe
    const tab = document.getElementsByClassName("heder-tab")
    if (param[0] !== undefined)
        param[0].prototype.constructor(tab[0].getAttribute("href"))
    for (let i = 0; i < tab.length; i++) {
        tab[i].addEventListener('click', function (e) {
            for (let y = 0; y < tab.length; y++) {
                if (y === i) {
                    tab[y].classList.add("heder-tab-active");
                } else {
                    tab[y].classList.remove("heder-tab-active");
                }
            }
            const elematBody = document.getElementsByClassName("body-tab");
            for (const iterator of elematBody) {
                if (e.target.attributes.href.value !== iterator.id)
                    iterator.classList.add("none")
                else
                    iterator.classList.remove("none")
            }
            if (param[0] !== undefined)
                param[0].prototype.constructor(e.target.attributes.href.value)
        });
    }
}
//tab
$(document).ready(function () {
    console.log(document.getElementById("tabs"));
});
