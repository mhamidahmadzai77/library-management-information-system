var edition = [];
var no = 0;
var segmentOrEditionChanged = true;
var editionFromNeedsToRecreate = true;


let previousNo = 0;
function segments() {
    var segments = document.getElementById('segment').value;
    
    var currentNo = segments;
    
    if (currentNo != previousNo) {
        segmentOrEditionChanged = true;
        editionFromNeedsToRecreate = true;
        
        previousNo = currentNo;
        // First remove created elements then create new elements
        // Get all elements with specified class names
        const elementsToRemove = document.getElementsByClassName('segment-elements');

        // Convert HTMLCollection to array then remove each one
        Array.from(elementsToRemove).forEach(element => {
            element.remove();
        });
        

        // Find the target element after which you want to insert new content
        const segmentElement = document.querySelector('.segment');

        for (var i = currentNo; i >= 1; i--) {
            // Create the HTML structure you want to insert
            const newHtmlContent = `
            <div class="form-group segment-elements segmentNo` + i + `">
                <label class="control-label col-md-3">
                    ` + i + `-  جلد نمبر 
                    <span class="required"> * </span>
                </label>
                <div class="col-md-6">
                    <input type="number" min="1" max="50" class="form-control segment-number" name="segmentNo" id="segment`+ i + `" value="` + i + `" />
                    <span class="help-block"> د جلد نمبر داخل کړئ </span>
                </div>
            </div>`;
            segmentElement.insertAdjacentHTML('afterend', newHtmlContent);

        }

        // All existance portlet must be deleted
        // Get all elements with specified class names
        const portletElementsToRemove = document.getElementsByClassName('portlet-tags');

        // Convert HTMLCollection to array then remove each one
        Array.from(portletElementsToRemove).forEach(element => {
            element.remove();
        });

        const endOfSegments = document.querySelector('.endOfSegments');

        // Get all elements with specified class names
        const elementsThatNeedsChilds = document.getElementsByClassName('segment-number');



        
        var editionLength = elementsThatNeedsChilds.length;
        // Convert HTMLCollection to array then remove each one
        no = editionLength;
            edition[no] = 1;
        for (var segmentNo = editionLength; segmentNo > 0; segmentNo--) {
            edition[segmentNo-1] = 0;

                const newHtmlContent = `
                <div class="form-group portlet-tags">
                    <div class="col-md-3"></div>
                    <div class="col-md-6">
                        <div class="portlet light bordered justify-content-center">

                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="icon-layers font-green-sharp"></i>
                                    <span class="caption-subject font-green-sharp bold uppercase">د ` + segmentNo + ` جلد مختلف چاپونه/ایډېشن مشخص کړئ</span>
                                </div>
                            </div>
                            <div class="portlet-body form">
                                <div class="form-group ">
                                    <label>د چاپ/ایډېشن مجموعي تعداد</label>
                                    <input type="number" min="1" max="30" class="form-control edition" name="editionOfSegment" value="1"  id="editionOfSegment` + segmentNo + `" />
                                    <div id="message"></div>
                                    <span class="help-block"> د چاپ/ایډېشن مجموعي تعداد مشخص کړئ. </span>
                                </div>
                                <hr id="afterEditionOfSegment`+ no + `" />
                                
                            </div>
                        </div>
                    </div>
                </div>`;
                no--;
                endOfSegments.insertAdjacentHTML('afterend', newHtmlContent);
            }
    }
}
setInterval(segments, 1);


var counter = 0;
var segmentNumber = 0;
function createEditions() {

    counter = 0;
    var afterEditionOfSegment = 1;
    const editions = document.getElementsByClassName('edition');
    segmentNumber = 1;
    Array.from(editions).forEach(element => {

        if (edition[counter] != element.value) {
            segmentOrEditionChanged = true;
            editionFromNeedsToRecreate = true;
            // All existance portlet must be deleted
            // Get all elements with specified class names
            const elementsToRemove = document.getElementsByClassName('subEdition' + counter);

            // Convert HTMLCollection to array then remove each one
            Array.from(elementsToRemove).forEach(element => {
                element.remove();
            });
            edition[counter] = element.value;
            var editionNo = element.value;
            
            for (var i = element.value; i >= 1; i--) {
                
                // Find the target element after which you want to insert new content
                var afterEdition = document.querySelector('#afterEditionOfSegment' + afterEditionOfSegment + '');

                // Create the HTML structure you want to insert
                const newHtmlContent = `<div class="form-group subEdition` + counter + `">
                                    <label style="padding-right:13px;">`+ editionNo + `- د چاپ نمبر</label>
                                    <input type="number" min="1" max="30" class="form-control editionNo`+ counter + `" name="editionNo" value="` + editionNo + `" id="editionNo` + segmentNumber + `` + editionNo +`" />
                                    <span class="help-block"> د چاپ نمبر مشخص کړئ </span>
                                </div>`;
                afterEdition.insertAdjacentHTML('afterend', newHtmlContent);

                editionNo--;
            }
        }
        segmentNumber++;

        counter++;
        afterEditionOfSegment++;
    });

    

}
setInterval(createEditions, 50);


function checkTotalSegments() {
    const totalSegments = document.getElementById('segment');
    return totalSegments.value;
}

var editionOfSegment = 0;
var segmentNo = 0;
var editionNo = 0;
var n = 0;
var e = null;
function provideEditionForms() {
    editionNo = 0;
    n = 0;
    // All existance portlet must be deleted
    // Get all elements with specified class names
    const elementsToRemove = document.getElementsByClassName('segmentPortlet');

    // Convert HTMLCollection to array then remove each one
    Array.from(elementsToRemove).forEach(element => {
        element.remove();
    });

    var totalSegments = document.getElementById('segment').value;
    /*
    for (var i = 1; i <= totalSegments.value; i++) {
        alert(document.getElementById('segment' + i + "").value);
    }*/


    editionNo = totalSegments;
    for (var i = totalSegments; i >= 1; i--) {
        editionNo--;
        e = document.querySelectorAll('.editionNo' + editionNo + '')
        n = 0;

        segmentNo = document.getElementById('segment' + i + "").value;
        // Find the target element after which you want to insert new content
        var afterTab3 = document.querySelector('#tabNo3');

        // Create the HTML structure you want to insert
        const newHtmlContent = `<div class="row segmentPortlet">
                                            <div class="col-md-12">
                                                <!-- BEGIN Portlet PORTLET-->
                                                <div class="portlet box red-sunglo">
                                                    <div class="portlet-title">
                                                        <div class="caption">
                                                            <i class="icon-layers"></i> د `+ segmentNo + ` جلد اړونده فورم ډک کړئ
                                                        </div>
                                                        <div class="tools">
                                                            <a href="javascript:;" class="collapse" data-original-title="" title=""> </a>
                                                            <a href="#" class="fullscreen" data-original-title="" title=""> </a>
                                                        </div>
                                                    </div>
                                                    <div class="portlet-body" style="height: auto; display: block;">
                                                        <p>

                                                            <br>
                                                            د `+ segmentNo + ` جلد د هر یو ایډېشن لپاره جدا جدا معلومات ولیکئ تر څو معلومات مو دقیق ذخیره او په اسانۍ سره یې ترلاسه کړئ
                                                        </p>
                                                        <hr />
                                                        <div id="segmentPortlet`+ i + `"></div>
                                                    </div>
                                                </div>
                                                <!-- END Portlet PORTLET-->
                                            </div>
                                        </div>`;
        afterTab3.insertAdjacentHTML('afterend', newHtmlContent);


        editionOfSegment = document.getElementById('editionOfSegment' + i + "").value;

        //var editionNo = document.getElementsByClassName('editionNo' + editionNo +'').value;

        for (var j = editionOfSegment; j >= 1; j--) {

            //editionNo = document.getElementById('editionNo' + j + "").value;
            // Find the target element after which you want to insert new content
            var afterSegmentPortlet = document.querySelector('#segmentPortlet' + i + '');

            // Create the HTML structure you want to insert
            const newHtmlContent = `<div class="row">
                    <div class="col-md-1"></div>
                    <div class="col-md-10">
                        <!-- BEGIN Portlet PORTLET-->
                        <div class="portlet box green">
                            <div class="portlet-title">
                                <div class="caption">
                                    <i class="icon-layers"></i> د `+ e[j-1].value + ` چاپ/ایدېشن اړونده فورم
                                </div>
                                <div class="tools">
                                    <a href="javascript:;" class="collapse" data-original-title="" title=""> </a>
                                    <a href="#" class="fullscreen" data-original-title="" title=""> </a>
                                </div>
                            </div>
                            <div class="portlet-body" style="display: block; ">
                                <div class="slimScrollDiv" style="position: relative;  width: auto; height: auto;">
                                    <div class="scroller" style="height: auto;  width: auto;" data-initialized="1">
                                        <br />
                                        <div class="form-group publicationNo">
                                            <label class="control-label col-md-3">
                                                د تأسیس ځل
                                                <span class="required"> * </span>
                                            </label>
                                            <div class="col-md-8">
                                                <input type="number" min="1" max="10" onclick="fillEditionForm(`+i+`,` +j +`)" class="form-control publicationNo" name="publicationNo" id="publicationNo`+i +``+ j+`" value="1" />
                                                <span class="help-block">  د تأسیس ځل وټاکئ </span>
                                            </div>
                                        </div>
                                        <div class="form-group publicationPlace">
                                            <label class="control-label col-md-3">
                                                د تأسیس ځای
                                            </label>
                                            <div class="col-md-8">
                                                <input type="text" class="form-control publicationPlace" name="publicationPlace" id="publicationPlace`+ i + `` + j +`" value="" />
                                                <span class="help-block">  د تأسیس ځای ولیکئ </span>
                                            </div>
                                        </div>
                                        <div class="form-group publisherName">
                                            <label class="control-label col-md-3">
                                                د مطبعې نوم
                                            </label>
                                            <div class="col-md-8">
                                                <input type="text" class="form-control publisherName" name="publisherName" id="publisherName`+ i + `` + j +`" value="" />
                                                <span class="help-block">  د مطبعې نوم ولیکئ </span>
                                            </div>
                                        </div>
                                        <div class="form-group publicationQuantity">
                                            <label class="control-label col-md-3">
                                                مقدار/تعداد
                                                <span class="required"> * </span>
                                            </label>
                                            <div class="col-md-8">
                                                <input type="number" min="1" max="50" class="form-control publicationQuantity" name="publicationQuantity" id="publicationQuantity`+ i + `` + j +`" value="1" />
                                                <span class="help-block">  مقدار/تعداد مشخص کړئ </span>
                                            </div>
                                        </div>
                                        <div class="form-group publicationPages">
                                            <label class="control-label col-md-3">
                                                صفحې
                                                <span class="required"> * </span>
                                            </label>
                                            <div class="col-md-8">
                                                <input type="number" min="10" max="5000" class="form-control publicationPages" name="publicationPages" id="publicationPages`+ i + `` + j +`" value="100" />
                                                <span class="help-block">  څو صفحې دی؟ </span>
                                            </div>
                                        </div>
                                        <div class="form-group CDQuantity">
                                            <label class="control-label col-md-3">
                                                د سي ډي/ CD تعداد
                                            </label>
                                            <div class="col-md-8">
                                                <input type="number" min="0" max="10" class="form-control CDQuantity" name="CDQuantity" id="CDQuantity`+ i + `` + j +`" value="0" />
                                                <span class="help-block">  د سي ډي/ CD تعداد </span>
                                            </div>
                                        </div>

                                        <div class="form-group registrationDateType">
                                            <label class="control-label col-md-3">
                                                د ثبت د تاریخ ډول
                                                <span class="required"> * </span>
                                            </label>
                                            <div class="col-md-8 ">
                                                <select name="registrationDateType" id="registrationDateType`+ i + `` + j +`" class="form-control registrationDateType">
                                                    <option selected value="شمسي">هجري شمسي</option>
                                                    <option value="قمري">هجري قمري</option>
                                                    <option value="میلادي">میلادي</option>
                                                </select>
                                                <span class="help-block"> د ثبت د تاریخ ډول انتخاب کړئ  </span>
                                            </div>
                                        </div>

                                        <div class="form-group registrationDate">
                                            <label class="control-label col-md-3">
                                                د ثبت تاریخ
                                                <span class="required"> * </span>
                                            </label>
                                            <div class="col-md-8">
                                                <input type="date" class="form-control registrationDate" name="registrationDate`+ i + `` + j +`" id="registrationDate`+ i + `` + j +`" value="" />
                                                <span class="help-block"> د ثبت تاریخ ولیکئ </span>
                                            </div>
                                        </div>
                                        <div class="form-group publicationDateType">
                                            <label class="control-label col-md-3">
                                                د تأسیس د تاریخ ډول
                                                <span class="required"> * </span>
                                            </label>
                                            <div class="col-md-8 ">
                                                <select name="publicationDateType" id="publicationDateType`+ i + `` + j +`" class="form-control publicationDateType">
                                                    <option selected value="شمسي">هجري شمسي</option>
                                                    <option value="قمري">هجري قمري</option>
                                                    <option value="میلادي">میلادي</option>
                                                </select>
                                                <span class="help-block"> د تأسیس د تاریخ ډول انتخاب کړئ  </span>
                                            </div>
                                        </div>
                                        <div class="form-group publicationYear">
                                            <label class="control-label col-md-3">
                                                د تأسیس کال
                                                <span class="required"> * </span>
                                            </label>
                                            <div class="col-md-8">
                                                <input type="number" min="1" max="4000" class="form-control publicationYear" name="publicationYear" id="publicationYear`+ i + `` + j +`" value="2000" />
                                                <span class="help-block"> د تأسیس کال ولیکئ </span>
                                            </div>
                                        </div>
                                        <div class="form-group publicationMonth">
                                            <label class="control-label col-md-3">
                                                د تأسیس میاشت

                                            </label>
                                            <div class="col-md-8">
                                                <input type="number" min="1" max="12" class="form-control publicationMonth" name="publicationMonth" id="publicationMonth`+ i + `` + j +`" value="" />
                                                <span class="help-block"> د تأسیس میاشت ولیکئ </span>
                                            </div>
                                        </div>
                                        <div class="form-group publicationDay">
                                            <label class="control-label col-md-3">
                                                د تأسیس ورځ

                                            </label>
                                            <div class="col-md-8">
                                                <input type="number" min="1" max="31" class="form-control publicationDay" name="publicationDay" id="publicationDay`+ i + `` + j +`" value="" />
                                                <span class="help-block"> د تأسیس ورځ ولیکئ </span>
                                            </div>
                                        </div>
                                        <div class="form-group cupboardNo">
                                            <label class="control-label col-md-3">
                                                د المارۍ نمبر

                                            </label>
                                            <div class="col-md-8">
                                                <input type="number" min="1" max="1000" class="form-control cupboardNo" name="cupboardNo" id="cupboardNo`+ i + `` + j +`" value="" />
                                                <span class="help-block"> د المارۍ نمبر مشخص کړئ </span>
                                            </div>
                                        </div>
                                        <div class="form-group cellNo">
                                            <label class="control-label col-md-3">
                                                د خانې نمبر

                                            </label>
                                            <div class="col-md-8">
                                                <input type="number" min="1" max="20" class="form-control cellNo" name="cellNo" id="cellNo`+ i + `` + j +`" value="" />
                                                <span class="help-block"> د خانې نمبر مشخص کړئ </span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="slimScrollBar" style="background: rgb(187, 187, 187); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; left: 1px; height: 41.6667px;">
                                    </div>
                                    <div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(234, 234, 234); opacity: 0.2; z-index: 90; left: 1px;">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- END Portlet PORTLET-->
                    </div>

                </div>`;
            afterSegmentPortlet.insertAdjacentHTML('afterend', newHtmlContent);
           
            n++;
        }

    }
    
}


// This is when to call provideEditionForms() function

function continueButtonEvent(){
    var tab2 = document.getElementById('tab2');
    if (tab2.classList.contains('active')) {
        if (editionFromNeedsToRecreate == true) {
            provideEditionForms();
            editionFromNeedsToRecreate = false;
        }
    }

    var tab3 = document.getElementById('tab3');
    if (tab3.classList.contains('active')) {
        confirmDetail();
    }

}

var Edition = 0;
function confirmDetail() {


    var rowNo = 0;
        
    const elemetnsToRemove = document.getElementsByClassName('tableRow');
    Array.from(elemetnsToRemove).forEach(element => {
        element.remove();
    });

    var inTableBody = document.querySelector('#tableBody');

        var totalSegments = document.getElementById('segment').value;
    var segmentNo;
    for (var i = totalSegments; i >= 1; i--) {
        
            segmentNo = document.getElementById('segment' + i + "").value;
           
            var editionOfSegment = document.getElementById('editionOfSegment' + i + '').value;

        for (var j = editionOfSegment; j >= 1; j--) {
                rowNo++;
            Edition = document.querySelector('#editionNo' + i + '' + j + '').value;


                var newHtmlContent = `<tr class="tableRow">
                                                        <td> `+rowNo+` </td>
                                                        <td style="min-width:150px;" class="bookName"> `+$('#name').val()+` </td>
                                                        <td> `+ segmentNo + ` </td>
                                                        <td> `+ Edition +` </td>
                                                        <td> `+$('#publicationNo'+i + '' +j).val()+` </td>
                                                        <td> `+ $('#publicationPlace' + i + '' + j).val() +` </td>
                                                        <td style="min-width:100px;"> `+ $('#publisherName' + i + '' + j).val() +` </td>
                                                        <td> `+ $('#publicationQuantity' + i + '' + j).val() +` </td>
                                                        <td> `+ $('#publicationPages' + i + '' + j).val() +` </td>
                                                        <td> `+ $('#CDQuantity' + i + '' + j).val() +` </td>
                                                        <td> `+ $('#registrationDateType' + i + '' + j).val() +` </td>
                                                        <td style="min-width:100px;"> `+ $('#registrationDate' + i + '' + j).val() +` </td>
                                                        <td> `+ $('#publicationDateType' + i + '' + j).val() +` </td>
                                                        <td> `+ $('#publicationYear' + i + '' + j).val() +` </td>
                                                        <td> `+ $('#publicationMonth' + i + '' + j).val() +` </td>
                                                        <td> `+ $('#publicationDay' + i + '' + j).val() +` </td>
                                                        <td> `+ $('#cupboardNo' + i + '' + j).val() +` </td>
                                                        <td> `+ $('#cellNo' + i + '' + j).val() +` </td>
                                                    </tr>`;
                inTableBody.insertAdjacentHTML('afterend', newHtmlContent);
                
                n++;
            }
        }

}

function fillEditionForm(i, j) {
    
    document.getElementById("publicationNo" + i + "" + j).value = document.getElementById("publicationNo11").value;
    document.getElementById("publicationPlace" + i + "" + j).value = document.getElementById("publicationPlace11").value;
    document.getElementById("publisherName" + i + "" + j).value = document.getElementById("publisherName11").value;
    document.getElementById("publicationQuantity" + i + "" + j).value = document.getElementById("publicationQuantity11").value;
    document.getElementById("publicationPages" + i + "" + j).value = document.getElementById("publicationPages11").value;
    document.getElementById("CDQuantity" + i + "" + j).value = document.getElementById("CDQuantity11").value;
    document.getElementById("registrationDateType" + i + "" + j).value = document.getElementById("registrationDateType11").value;
    document.getElementById("registrationDate" + i + "" + j).value = document.getElementById("registrationDate11").value;
    document.getElementById("publicationDateType" + i + "" + j).value = document.getElementById("publicationDateType11").value;
    document.getElementById("publicationYear" + i + "" + j).value = document.getElementById("publicationYear11").value;
    document.getElementById("publicationMonth" + i + "" + j).value = document.getElementById("publicationMonth11").value;
    document.getElementById("publicationDay" + i + "" + j).value = document.getElementById("publicationDay11").value;
    document.getElementById("cupboardNo" + i + "" + j).value = document.getElementById("cupboardNo11").value;
    document.getElementById("cellNo" + i + "" + j).value = document.getElementById("cellNo11").value;

}