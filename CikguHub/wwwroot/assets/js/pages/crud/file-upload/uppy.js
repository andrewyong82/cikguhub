"use strict";var KTUppy=function(){const e=Uppy.Tus,t=(Uppy.ProgressBar,Uppy.StatusBar,Uppy.FileInput,Uppy.Informer,Uppy.Dashboard),o=Uppy.Dropbox,p=Uppy.GoogleDrive,i=Uppy.Instagram,n=Uppy.Webcam;return{init:function(){var s,r;s={proudlyDisplayPoweredByUppy:!1,target:"#kt_uppy_1",inline:!0,replaceTargetContent:!0,showProgressDetails:!0,note:"No filetype restrictions.",height:470,metaFields:[{id:"name",name:"Name",placeholder:"file name"},{id:"caption",name:"Caption",placeholder:"describe what the image is about"}],browserBackButtonClose:!0},(r=Uppy.Core({autoProceed:!0,restrictions:{maxFileSize:1e6,maxNumberOfFiles:5,minNumberOfFiles:1}})).use(t,s),r.use(e,{endpoint:"https://master.tus.io/files/"}),r.use(p,{target:t,companionUrl:"https://companion.uppy.io"}),r.use(o,{target:t,companionUrl:"https://companion.uppy.io"}),r.use(i,{target:t,companionUrl:"https://companion.uppy.io"}),r.use(n,{target:t}),setTimeout((function(){swal.fire({title:"Notice",html:"Uppy demos uses <b>https://master.tus.io/files/</b> URL for resumable upload examples and your uploaded files will be temporarely stored in <b>tus.io</b> servers.",type:"info",buttonsStyling:!1,confirmButtonClass:"btn btn-primary",confirmButtonText:"Ok, I understand",onClose:function(e){console.log("on close event fired!")}})}),2e3)}}}();KTUtil.ready((function(){KTUppy.init()}));