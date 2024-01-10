/*


 Copyright (c) 2015-2018 Google, Inc., Netflix, Inc., Microsoft Corp. and contributors

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
*/
gvjs__L.prototype.MB=gvjs_V(77,function(a,b,c,d){for(var e=a.call(d,this),f=[],g=this.getChildren(),h=0;h<g.length;h++){var k=g[h].MB(a,b,c,d);f.push(k)}a=b.call(d,e,f);c&&c.call(d,this,a);return a});gvjs_2L.prototype.MB=gvjs_V(76,function(a,b,c,d){for(var e=this.ep,f=0;f<e.length;f++)e[f].MB(a,b,c,d)});gvjs_jB.prototype.BT=gvjs_V(40,function(a){var b=this.j();b&&gvjs_cg(b,a)});function gvjs_BW(a,b){return gvjs_yh(b,function(c){return"SVG"==c.nodeName&&!0},!0,void 0)}
function gvjs_CW(a,b,c){c=a.HY.splice(c||0)[0];(c=a.Am=a.Am||c)?c.Dba?a.Fi=a.Lx||a.Ws:void 0!=c.hh&&a.Ws<c.hh?(a.Fi=c.hh,a.Am=null):a.Fi=a.Ws:a.Fi=b}function gvjs_Zja(a,b){var c=void 0;return new (c||(c=Promise))(function(d,e){function f(k){try{h(b.next(k))}catch(l){e(l)}}function g(k){try{h(b["throw"](k))}catch(l){e(l)}}function h(k){k.done?d(k.value):(new c(function(l){l(k.value)})).then(f,g)}h((b=b.apply(a,void 0)).next())})}function gvjs_DW(a){gvjs_u(a,function(){})}
function gvjs_EW(a,b){gvjs_DW(b);return 0==b.length?null===a?0:a:gvjs_Ee(b,function(c,d){return c+d},0)}function gvjs__ja(a,b){gvjs_DW(b);if(0==b.length)return a;b=gvjs_De(b,function(c){return null!=c});return 0==b.length?null:gvjs_EW(a,b)/b.length}var gvjs_FW=!0,gvjs_GW={set Ep(a){a?console.warn("DEPRECATED! RxJS was set to use deprecated synchronous error handling behavior by code at: \n"+Error().stack):gvjs_FW&&console.log("RxJS: Back to a better error behavior. Thank you. <3");gvjs_FW=a},get Ep(){return gvjs_FW}};var gvjs_HW=typeof Symbol===gvjs_d&&Symbol.observable||"@@observable";function gvjs_IW(a){setTimeout(function(){throw a;},0)};var gvjs_JW={closed:!0,next:function(){},error:function(a){if(gvjs_GW.Ep)throw a;gvjs_IW(a)},complete:function(){}};var gvjs_KW=function(){function a(b){this.message=b?b.length+" errors occurred during unsubscription:\n"+b.map(function(c,d){return d+1+") "+c.toString()}).join("\n  "):"";this.name="UnsubscriptionError";this.errors=b;return this}a.prototype=Object.create(Error.prototype);return a}();var gvjs_0ja=Array.isArray||function(a){return a&&typeof a.length===gvjs_g};function gvjs_LW(a){this.closed=!1;this.oB=this.xu=null;a&&(this.FV=a)}
gvjs_LW.prototype.unsubscribe=function(){if(!this.closed){var a=this.xu,b=this.FV,c=this.oB;this.closed=!0;this.oB=this.xu=null;if(a instanceof gvjs_LW)a.remove(this);else if(null!==a)for(var d=0;d<a.length;++d)a[d].remove(this);if(typeof b===gvjs_d)try{b.call(this)}catch(h){var e=h instanceof gvjs_KW?gvjs_MW(h.errors):[h]}if(gvjs_0ja(c)){d=-1;for(var f=c.length;++d<f;){var g=c[d];if(null!==g&&typeof g===gvjs_h)try{g.unsubscribe()}catch(h){e=e||[],h instanceof gvjs_KW?e=e.concat(gvjs_MW(h.errors)):
e.push(h)}}}if(e)throw new gvjs_KW(e);}};gvjs_LW.prototype.add=function(a){gvjs_1ja(this,a)};
function gvjs_1ja(a,b){var c=b;if(b){switch(typeof b){case gvjs_d:c=new gvjs_LW(b);case gvjs_h:if(c===a||c.closed||typeof c.unsubscribe!==gvjs_d)return;if(a.closed){c.unsubscribe();return}c instanceof gvjs_LW||(b=c,c=new gvjs_LW,c.oB=[b]);break;default:throw Error("unrecognized teardown "+b+" added to Subscription.");}var d=c.xu;if(null===d)c.xu=a;else if(d instanceof gvjs_LW){if(d===a)return;c.xu=[d,a]}else if(-1===d.indexOf(a))d.push(a);else return;b=a.oB;null===b?a.oB=[c]:b.push(c)}}
gvjs_LW.prototype.remove=function(a){var b=this.oB;b&&(a=b.indexOf(a),-1!==a&&b.splice(a,1))};var gvjs_NW=new gvjs_LW;gvjs_NW.closed=!0;gvjs_LW.EMPTY=gvjs_NW;function gvjs_MW(a){return a.reduce(function(b,c){return b.concat(c instanceof gvjs_KW?c.errors:c)},[])};var gvjs_OW=typeof Symbol===gvjs_d?Symbol("rxSubscriber"):"@@rxSubscriber_"+Math.random();function gvjs_PW(a,b,c){gvjs_LW.call(this);this.eU=null;this.Ef=this.Nn=this.dU=!1;switch(arguments.length){case 0:this.destination=gvjs_JW;break;case 1:if(!a){this.destination=gvjs_JW;break}if(typeof a===gvjs_h){a instanceof gvjs_PW?(this.Nn=a.Nn,this.destination=a,a.add(this)):(this.Nn=!0,this.destination=new gvjs_QW(this,a));break}default:this.Nn=!0,this.destination=new gvjs_QW(this,a,b,c)}}gvjs_o(gvjs_PW,gvjs_LW);gvjs_PW.EMPTY=gvjs_LW.EMPTY;gvjs_PW.prototype[gvjs_OW]=function(){return this};
gvjs_PW.create=function(a,b,c){a=new gvjs_PW(a,b,c);a.Nn=!1;return a};gvjs_=gvjs_PW.prototype;gvjs_.next=function(a){this.Ef||this.Xn(a)};gvjs_.error=function(a){this.Ef||(this.Ef=!0,this.nB(a))};gvjs_.complete=function(){this.Ef||(this.Ef=!0,this.xx())};gvjs_.unsubscribe=function(){this.closed||(this.Ef=!0,gvjs_LW.prototype.unsubscribe.call(this))};gvjs_.Xn=function(a){this.destination.next(a)};gvjs_.nB=function(a){this.destination.error(a);this.unsubscribe()};
gvjs_.xx=function(){this.destination.complete();this.unsubscribe()};gvjs_.$Ba=function(){var a=this.xu;this.xu=null;this.unsubscribe();this.Ef=this.closed=!1;this.xu=a;return this};
function gvjs_QW(a,b,c,d){gvjs_PW.call(this);this.bG=a;var e=this;if(typeof b===gvjs_d)var f=b;else b&&(f=b.next,c=b.error,d=b.complete,b!==gvjs_JW&&(e=Object.create(b),b&&typeof b.add===gvjs_d&&typeof b.unsubscribe===gvjs_d&&b.add(this.unsubscribe.bind(this)),e.unsubscribe=this.unsubscribe.bind(this)));this.NM=e;this.Xn=f;this.nB=c;this.xx=d}gvjs_o(gvjs_QW,gvjs_PW);gvjs_QW.EMPTY=gvjs_PW.EMPTY;gvjs_QW.create=gvjs_PW.create;gvjs_=gvjs_QW.prototype;
gvjs_.next=function(a){if(!this.Ef&&this.Xn){var b=this.bG;gvjs_GW.Ep&&b.Nn?this.CV(b,this.Xn,a)&&this.unsubscribe():this.DV(this.Xn,a)}};gvjs_.error=function(a){if(!this.Ef){var b=this.bG,c=gvjs_GW.Ep;if(this.nB)c&&b.Nn?this.CV(b,this.nB,a):this.DV(this.nB,a),this.unsubscribe();else if(b.Nn)c?(b.eU=a,b.dU=!0):gvjs_IW(a),this.unsubscribe();else{this.unsubscribe();if(c)throw a;gvjs_IW(a)}}};
gvjs_.complete=function(){var a=this;if(!this.Ef){var b=this.bG;if(this.xx){var c=function(){return a.xx.call(a.NM)};gvjs_GW.Ep&&b.Nn?this.CV(b,c):this.DV(c)}this.unsubscribe()}};gvjs_.DV=function(a,b){try{a.call(this.NM,b)}catch(c){this.unsubscribe();if(gvjs_GW.Ep)throw c;gvjs_IW(c)}};gvjs_.CV=function(a,b,c){if(!gvjs_GW.Ep)throw Error("bad call");try{b.call(this.NM,c)}catch(d){return gvjs_GW.Ep?(a.eU=d,a.dU=!0):gvjs_IW(d),!0}return!1};gvjs_.FV=function(){var a=this.bG;this.bG=this.NM=null;a.unsubscribe()};function gvjs_RW(a){return a};function gvjs_2ja(a){return 0===a.length?gvjs_RW:1===a.length?a[0]:function(b){return a.reduce(function(c,d){return d(c)},b)}};function gvjs_SW(a){this.ZBa=!1;a&&(this.cG=a)}gvjs_=gvjs_SW.prototype;gvjs_.EJ=function(a){var b=new gvjs_SW;b.source=this;b.operator=a;return b};gvjs_.subscribe=function(a,b,c){var d=this.operator;a:{if(a){if(a instanceof gvjs_PW)break a;if(a[gvjs_OW]){a=a[gvjs_OW]();break a}}a=a||b||c?new gvjs_PW(a,b,c):new gvjs_PW(gvjs_JW)}d?a.add(d.call(a,this.source)):a.add(this.source||gvjs_GW.Ep&&!a.Nn?this.cG(a):this.EV(a));if(gvjs_GW.Ep&&a.Nn&&(a.Nn=!1,a.dU))throw a.eU;return a};
gvjs_.EV=function(a){try{return this.cG(a)}catch(e){gvjs_GW.Ep&&(a.dU=!0,a.eU=e);var b;a:{for(b=a;b;){var c=b.destination,d=b.Ef;if(b.closed||d){b=!1;break a}b=c&&c instanceof gvjs_PW?c:null}b=!0}b?a.error(e):console.warn(e)}};gvjs_.forEach=function(a,b){var c=this;b=gvjs_3ja(b);return new b(function(d,e){var f=c.subscribe(function(g){try{a(g)}catch(h){e(h),f&&f.unsubscribe()}},e,d)})};gvjs_.cG=function(a){var b=this.source;return b&&b.subscribe(a)};gvjs_SW.prototype[gvjs_HW]=function(){return this};
gvjs_SW.prototype.l2=function(a){for(var b=[],c=0;c<arguments.length;++c)b[c-0]=arguments[c];return 0===b.length?this:gvjs_2ja(b)(this)};gvjs_SW.create=function(a){return new gvjs_SW(a)};function gvjs_3ja(a){a||(a=Promise);if(!a)throw Error("no Promise impl found");return a};function gvjs_TW(a,b){gvjs_LW.call(this);this.subject=a;this.Dxa=b;this.closed=!1}gvjs_o(gvjs_TW,gvjs_LW);gvjs_TW.EMPTY=gvjs_LW.EMPTY;gvjs_TW.prototype.unsubscribe=function(){if(!this.closed){this.closed=!0;var a=this.subject,b=a.xw;this.subject=null;!b||0===b.length||a.Ef||a.closed||(a=b.indexOf(this.Dxa),-1!==a&&b.splice(a,1))}};var gvjs_UW=function(){function a(){this.message="object unsubscribed";this.name="ObjectUnsubscribedError";return this}a.prototype=Object.create(Error.prototype);return a}();function gvjs_VW(a){gvjs_PW.call(this,a);this.destination=a}gvjs_o(gvjs_VW,gvjs_PW);gvjs_VW.EMPTY=gvjs_PW.EMPTY;gvjs_VW.create=gvjs_PW.create;function gvjs_WW(){gvjs_SW.call(this);this.xw=[];this.Paa=this.Ef=this.closed=!1;this.uga=null}gvjs_o(gvjs_WW,gvjs_SW);gvjs_WW.prototype[gvjs_OW]=function(){return new gvjs_VW(this)};gvjs_=gvjs_WW.prototype;gvjs_.EJ=function(a){var b=new gvjs_XW(this,this);b.operator=a;return b};
gvjs_.next=function(a){if(this.closed)throw new gvjs_UW;if(!this.Ef){var b=this.xw,c=b.length;b=b.slice();for(var d=0;d<c;d++)b[d].next(a)}};gvjs_.error=function(a){if(this.closed)throw new gvjs_UW;this.Paa=!0;this.uga=a;this.Ef=!0;var b=this.xw,c=b.length;b=b.slice();for(var d=0;d<c;d++)b[d].error(a);this.xw.length=0};gvjs_.complete=function(){if(this.closed)throw new gvjs_UW;this.Ef=!0;var a=this.xw,b=a.length;a=a.slice();for(var c=0;c<b;c++)a[c].complete();this.xw.length=0};
gvjs_.unsubscribe=function(){this.closed=this.Ef=!0;this.xw=null};gvjs_.EV=function(a){if(this.closed)throw new gvjs_UW;return gvjs_SW.prototype.EV.call(this,a)};gvjs_.cG=function(a){if(this.closed)throw new gvjs_UW;if(this.Paa)return a.error(this.uga),gvjs_LW.EMPTY;if(this.Ef)return a.complete(),gvjs_LW.EMPTY;this.xw.push(a);return new gvjs_TW(this,a)};gvjs_WW.create=function(a,b){return new gvjs_XW(a,b)};function gvjs_XW(a,b){gvjs_WW.call(this);this.destination=a;this.source=b}gvjs_o(gvjs_XW,gvjs_WW);
gvjs_XW.create=gvjs_WW.create;gvjs_XW.prototype.next=function(a){var b=this.destination;b&&b.next&&b.next(a)};gvjs_XW.prototype.error=function(a){var b=this.destination;b&&b.error&&this.destination.error(a)};gvjs_XW.prototype.complete=function(){var a=this.destination;a&&a.complete&&this.destination.complete()};gvjs_XW.prototype.cG=function(a){return this.source?this.source.subscribe(a):gvjs_LW.EMPTY};function gvjs_4ja(a,b){return new gvjs_SW(function(c){var d=new gvjs_LW,e=0;d.add(b.jr(function(){e===a.length?c.complete():(c.next(a[e++]),c.closed||d.add(this.jr()))}));return d})};function gvjs_YW(a){return function(b){for(var c=0,d=a.length;c<d&&!b.closed;c++)b.next(a[c]);b.complete()}};function gvjs_ZW(a,b){b=void 0===b?gvjs_ZW.now:b;this.Ija=a;this.now=b}gvjs_ZW.prototype.jr=function(a,b,c){b=void 0===b?0:b;return(new this.Ija(this,a)).jr(c,b)};gvjs_ZW.now=function(){return Date.now()};(function(){function a(){this.message="no elements in sequence";this.name="EmptyError";return this}a.prototype=Object.create(Error.prototype);return a})();function gvjs_5ja(a){return function(b){if(typeof a!==gvjs_d)throw new TypeError("argument is not a function. Are you looking for `mapTo()`?");return b.EJ(new gvjs__W(a))}}function gvjs__W(a){this.project=a;this.mu=void 0}gvjs__W.prototype.call=function(a,b){return b.subscribe(new gvjs_0W(a,this.project,this.mu))};function gvjs_0W(a,b,c){gvjs_PW.call(this,a);this.project=b;this.count=0;this.mu=c||this}gvjs_o(gvjs_0W,gvjs_PW);gvjs_0W.EMPTY=gvjs_PW.EMPTY;gvjs_0W.create=gvjs_PW.create;
gvjs_0W.prototype.Xn=function(a){try{var b=this.project.call(this.mu,a,this.count++)}catch(c){this.destination.error(c);return}this.destination.next(b)};function gvjs_1W(){gvjs_PW.apply(this,arguments)}gvjs_o(gvjs_1W,gvjs_PW);gvjs_1W.EMPTY=gvjs_PW.EMPTY;gvjs_1W.create=gvjs_PW.create;gvjs_1W.prototype.rda=function(a){this.destination.next(a)};gvjs_1W.prototype.qda=function(){this.destination.complete()};function gvjs_2W(a){gvjs_PW.call(this);this.parent=a;this.index=0}gvjs_o(gvjs_2W,gvjs_PW);gvjs_2W.EMPTY=gvjs_PW.EMPTY;gvjs_2W.create=gvjs_PW.create;gvjs_2W.prototype.Xn=function(a){this.parent.rda(a,this.index++)};gvjs_2W.prototype.nB=function(a){this.parent.destination.error(a);this.unsubscribe()};gvjs_2W.prototype.xx=function(){this.parent.qda(this);this.unsubscribe()};var gvjs_3W=typeof Symbol===gvjs_d&&Symbol.iterator?Symbol.iterator:"@@iterator";function gvjs_6ja(a){return function(b){gvjs_7ja(a,b).catch(function(c){return b.error(c)})}}
function gvjs_7ja(a,b){var c,d,e,f;return gvjs_Zja(this,function h(){var k,l;return gvjs_Dy(h,function(m){switch(m.Fi){case 1:m.Lx=2;m.Ws=3;if(!Symbol.asyncIterator)throw new TypeError("Symbol.asyncIterator is not defined.");var n=a[Symbol.asyncIterator];c=n?n.call(a):typeof __values===gvjs_d?__values(a):a[Symbol.iterator]();case 5:return gvjs_zy(m,c.next(),8);case 8:if(d=m.l6,d.done){m.hh(3);break}k=d.value;b.next(k);m.hh(5);break;case 3:m.HY=[m.Am];m.Lx=0;m.Ws=0;m.Lx=0;m.Ws=9;if(!d||d.done||!(f=
c.return)){m.hh(9);break}return gvjs_zy(m,f.call(c),9);case 9:m.HY[1]=m.Am;m.Lx=0;m.Ws=0;if(e)throw e.error;gvjs_CW(m,10,1);break;case 10:gvjs_CW(m,4);break;case 2:m.Lx=0;n=m.Am.a$;m.Am=null;l=n;e={error:l};m.hh(3);break;case 4:b.complete(),m.Fi=0}})})};function gvjs_8ja(a){return function(b){var c=a[gvjs_3W]();do{var d=void 0;try{d=c.next()}catch(e){b.error(e);return}if(d.done){b.complete();break}b.next(d.value);if(b.closed)break}while(1);typeof c.return===gvjs_d&&b.add(function(){c.return&&c.return()});return b}};function gvjs_9ja(a){return function(b){var c=a[gvjs_HW]();if(typeof c.subscribe!==gvjs_d)throw new TypeError("Provided object does not correctly implement Symbol.observable");return c.subscribe(b)}};function gvjs_$ja(a){return function(b){a.then(function(c){b.closed||(b.next(c),b.complete())},function(c){return b.error(c)}).then(null,gvjs_IW);return b}};function gvjs_4W(a){if(a&&typeof a[gvjs_HW]===gvjs_d)return gvjs_9ja(a);if(a&&typeof a.length===gvjs_g&&typeof a!==gvjs_d)return gvjs_YW(a);if(a&&typeof a.subscribe!==gvjs_d&&typeof a.then===gvjs_d)return gvjs_$ja(a);if(a&&typeof a[gvjs_3W]===gvjs_d)return gvjs_8ja(a);if(Symbol&&Symbol.asyncIterator&&a&&typeof a[Symbol.asyncIterator]===gvjs_d)return gvjs_6ja(a);throw new TypeError("You provided "+(null!==a&&typeof a===gvjs_h?"an invalid object":"'"+a+"'")+" where a stream was expected. You can provide an Observable, Promise, Array, or Iterable.");
};function gvjs_aka(a){return a instanceof gvjs_SW?a:new gvjs_SW(gvjs_4W(a))};function gvjs_5W(a,b){var c=void 0===c?Infinity:c;if(typeof b===gvjs_d)return function(d){return d.l2(gvjs_5W(function(e,f){return gvjs_aka(a(e,f)).l2(gvjs_5ja(function(g,h){return b(e,g,f,h)}))},c))};typeof b===gvjs_g&&(c=b);return function(d){return d.EJ(new gvjs_6W(a,c))}}function gvjs_6W(a,b){b=void 0===b?Infinity:b;this.project=a;this.iX=b}gvjs_6W.prototype.call=function(a,b){return b.subscribe(new gvjs_7W(a,this.project,this.iX))};
function gvjs_7W(a,b,c){c=void 0===c?Infinity:c;gvjs_1W.call(this,a);this.project=b;this.iX=c;this.Oaa=!1;this.buffer=[];this.index=this.active=0}gvjs_o(gvjs_7W,gvjs_1W);gvjs_7W.EMPTY=gvjs_1W.EMPTY;gvjs_7W.create=gvjs_1W.create;gvjs_=gvjs_7W.prototype;gvjs_.Xn=function(a){this.active<this.iX?this.bka(a):this.buffer.push(a)};gvjs_.bka=function(a){var b=this.index++;try{var c=this.project(a,b)}catch(d){this.destination.error(d);return}this.active++;this.Wja(c,a,b)};
gvjs_.Wja=function(a,b,c){b=new gvjs_2W(this,b,c);this.destination.add(b);b=void 0===b?new gvjs_2W(this,void 0,void 0):b;b.closed||(a instanceof gvjs_SW?a.subscribe(b):gvjs_4W(a)(b))};gvjs_.xx=function(){this.Oaa=!0;0===this.active&&0===this.buffer.length&&this.destination.complete();this.unsubscribe()};gvjs_.rda=function(a){this.destination.next(a)};gvjs_.qda=function(a){var b=this.buffer;this.remove(a);this.active--;0<b.length?this.Xn(b.shift()):0===this.active&&this.Oaa&&this.destination.complete()};function gvjs_8W(){gvjs_LW.call(this)}gvjs_o(gvjs_8W,gvjs_LW);gvjs_8W.EMPTY=gvjs_LW.EMPTY;gvjs_8W.prototype.jr=function(){return this};function gvjs_9W(a,b){gvjs_LW.call(this);this.nA=a;this.wha=b;this.pending=!1}gvjs_o(gvjs_9W,gvjs_8W);gvjs_9W.EMPTY=gvjs_8W.EMPTY;gvjs_9W.prototype.jr=function(a,b){b=void 0===b?0:b;if(this.closed)return this;this.state=a;var c=this.id;a=this.nA;null!=c&&(this.id=gvjs_$W(this,c,b));this.pending=!0;this.delay=b;(c=this.id)||(b=void 0===b?0:b,c=setInterval(a.flush.bind(a,this),b));this.id=c;return this};
function gvjs_$W(a,b,c){c=void 0===c?0:c;if(null!==c&&a.delay===c&&!1===a.pending)return b;clearInterval(b)}gvjs_9W.prototype.execute=function(a,b){if(this.closed)return Error("executing a cancelled action");this.pending=!1;if(a=this.Vja(a,b))return a;!1===this.pending&&null!=this.id&&(this.id=gvjs_$W(this,this.id,null))};gvjs_9W.prototype.Vja=function(a){var b=!1,c=void 0;try{this.wha(a)}catch(d){b=!0,c=!!d&&d||Error(d)}if(b)return this.unsubscribe(),c};
gvjs_9W.prototype.FV=function(){var a=this.id,b=this.nA.actions,c=b.indexOf(this);this.state=this.wha=null;this.pending=!1;this.nA=null;-1!==c&&b.splice(c,1);null!=a&&(this.id=gvjs_$W(this,a,null));this.delay=null};function gvjs_aX(a,b){b=void 0===b?gvjs_ZW.now:b;gvjs_ZW.call(this,a,function(){return gvjs_aX.FH&&gvjs_aX.FH!==c?gvjs_aX.FH.now():b()});var c=this;this.actions=[];this.active=!1}gvjs_o(gvjs_aX,gvjs_ZW);gvjs_aX.now=gvjs_ZW.now;gvjs_aX.prototype.jr=function(a,b,c){b=void 0===b?0:b;return gvjs_aX.FH&&gvjs_aX.FH!==this?gvjs_aX.FH.jr(a,b,c):gvjs_ZW.prototype.jr.call(this,a,b,c)};
gvjs_aX.prototype.flush=function(a){var b=this.actions;if(this.active)b.push(a);else{var c;this.active=!0;do if(c=a.execute(a.state,a.delay))break;while(a=b.shift());this.active=!1;if(c){for(;a=b.shift();)a.unsubscribe();throw c;}}};var gvjs_bka=new gvjs_aX(gvjs_9W);function gvjs_cka(a){for(var b=[],c=0;c<arguments.length;++c)b[c-0]=arguments[c];var d=Infinity;c=void 0;var e=b[b.length-1];e&&typeof e.jr===gvjs_d?(c=b.pop(),1<b.length&&typeof b[b.length-1]===gvjs_g&&(d=b.pop())):typeof e===gvjs_g&&(d=b.pop());!c&&1===b.length&&b[0]instanceof gvjs_SW?b=b[0]:(d=void 0===d?Infinity:d,b=gvjs_5W(gvjs_RW,d)(c?gvjs_4ja(b,c):new gvjs_SW(gvjs_YW(b))));return b};function gvjs_bX(a,b){return function(c){return c.EJ(new gvjs_cX(a,b))}}function gvjs_cX(a,b){this.t2=a;this.mu=b}gvjs_cX.prototype.call=function(a,b){return b.subscribe(new gvjs_dX(a,this.t2,this.mu))};function gvjs_dX(a,b,c){gvjs_PW.call(this,a);this.t2=b;this.mu=c;this.count=0}gvjs_o(gvjs_dX,gvjs_PW);gvjs_dX.EMPTY=gvjs_PW.EMPTY;gvjs_dX.create=gvjs_PW.create;gvjs_dX.prototype.Xn=function(a){try{var b=this.t2.call(this.mu,a,this.count++)}catch(c){this.destination.error(c);return}b&&this.destination.next(a)};function gvjs_dka(a){function b(){return!b.fva.apply(b.mu,arguments)}b.fva=a;b.mu=void 0;return b};function gvjs_eka(a,b){return[gvjs_bX(b,void 0)(new gvjs_SW(gvjs_4W(a))),gvjs_bX(gvjs_dka(b))(new gvjs_SW(gvjs_4W(a)))]};(function(){function a(){this.message="argument out of range";this.name="ArgumentOutOfRangeError";return this}a.prototype=Object.create(Error.prototype);return a})();(function(){function a(b){this.message=b;this.name="NotFoundError";return this}a.prototype=Object.create(Error.prototype);return a})();(function(){function a(b){this.message=b;this.name="SequenceError";return this}a.prototype=Object.create(Error.prototype);return a})();(function(){function a(){this.message="Timeout has occurred";this.name="TimeoutError";return this}a.prototype=Object.create(Error.prototype);return a})();function gvjs_fka(){var a=void 0===a?gvjs_bka:a;return function(b){return b.EJ(new gvjs_eX(a))}}function gvjs_eX(a){this.lY=250;this.nA=a}gvjs_eX.prototype.call=function(a,b){return b.subscribe(new gvjs_fX(a,this.lY,this.nA))};function gvjs_fX(a,b,c){gvjs_PW.call(this,a);this.lY=b;this.nA=c;this.n0=this.MX=null;this.Oh=!1}gvjs_o(gvjs_fX,gvjs_PW);gvjs_fX.EMPTY=gvjs_PW.EMPTY;gvjs_fX.create=gvjs_PW.create;
gvjs_fX.prototype.Xn=function(a){gvjs_gX(this);this.n0=a;this.Oh=!0;this.add(this.MX=this.nA.jr(gvjs_gka,this.lY,this))};gvjs_fX.prototype.xx=function(){gvjs_hX(this);this.destination.complete()};function gvjs_hX(a){gvjs_gX(a);if(a.Oh){var b=a.n0;a.n0=null;a.Oh=!1;a.destination.next(b)}}function gvjs_gX(a){var b=a.MX;null!==b&&(a.remove(b),b.unsubscribe(),a.MX=null)}function gvjs_gka(a){gvjs_hX(a)};var gvjs_iX=gvjs_8d(["altKey","ctrlKey","metaKey","shiftKey"]),gvjs_hka=gvjs_iX.next().value,gvjs_ika=gvjs_iX.next().value,gvjs_jka=gvjs_iX.next().value,gvjs_kka=gvjs_iX.next().value,gvjs_lka=["drilldown",gvjs_3u,"rollup",gvjs_yx],gvjs_jX=[gvjs_hka,gvjs_ika,gvjs_jka,gvjs_kka],gvjs_kX=[gvjs_Wt,gvjs_3t,gvjs_du,gvjs_kd,gvjs_ld];
function gvjs_lX(){var a=this;this.XJ={highlight:gvjs_mX([gvjs_ld]),unhighlight:gvjs_mX([gvjs_kd]),rollup:gvjs_mX([gvjs_3t]),drilldown:gvjs_mX([gvjs_Wt])};this.xba={highlight:function(){},unhighlight:function(){},rollup:function(){},drilldown:function(){}};this.dda=new gvjs_WW;var b=gvjs_8d(gvjs_eka(this.dda,function(d){return[gvjs_Wt,gvjs_du].includes(d.type)})),c=b.next().value;b=b.next().value;c=c.l2(gvjs_fka());gvjs_cka(c,b).subscribe(function(d){for(var e=gvjs_8d(gvjs_lka),f=e.next();!f.done;f=
e.next()){f=f.value;a:{var g=d;var h=a.XJ[f];if(h.enabled&&h.mouse===g.type){for(var k=gvjs_8d(gvjs_jX),l=k.next();!l.done;l=k.next())if(l=l.value,h.keys[l]!==!!g[l]){g=!1;break a}g=!0}else g=!1}if(g)a.xba[f](d)}})}function gvjs_mka(a,b){for(var c=gvjs_8d(Object.keys(a.XJ)),d=c.next();!d.done;d=c.next()){d=d.value;var e=gvjs_mX(b[d]);e&&(a.XJ[d]=e)}}
gvjs_lX.prototype.B0=function(a,b,c){for(var d=this,e=gvjs_8d(gvjs_kX),f=e.next();!f.done;f=e.next())a.ic(b,f.value,function(g){d.dda.next(g)});this.xba=c};
function gvjs_mX(a){if(!a)return null;var b={enabled:!1,mouse:"",keys:{}};if(0===a.length)return b;var c=[].concat(gvjs_9d(a)),d=c.shift()||"";if(!gvjs_kX.includes(d)||!c.every(function(e){return gvjs_jX.includes(e)}))return null;b.enabled=!0;b.mouse=d;d=gvjs_8d(gvjs_jX);for(c=d.next();!c.done;c=d.next())b.keys[c.value]=!1;a=gvjs_8d(a);for(c=a.next();!c.done;c=a.next())b.keys[c.value]=!0;return b};function gvjs_nX(a){gvjs_Qn.call(this,a);this.el=new gvjs_$n;this.Ufa=this.Es=this.tree=this.canvas=this.renderer=null;this.title="";this.GR=this.ja=this.Ega=null;this.Pfa=this.rL=!1;this.efa=null;this.Sfa=!0;this.Jga=null;this.l5="";this.ID=this.maxDepth=1;this.cba=.5;this.Kca=0;this.kha=!1;this.header=0;this.P7=this.borderColor="";this.IB=this.Qp=null;this.s$=this.r$="";this.fontSize=0;this.ZP=this.iR=this.oR=this.wR=this.bM=this.k_=this.fR=this.nR=this.rR=this.JU=this.fontFamily="";this.WK=!1;
this.fba=60/(1+Math.sqrt(5));this.c1=0;this.X0=.5;this.O0=1;this.y5=this.n_=null;this.$0=1;this.W0=.75;this.N0=.5;this.L0=this.Z0=this.x5=this.l_=null;this.GD=-1*Number.MAX_VALUE;this.LD=Number.MAX_VALUE;this.sb=this.options=this.Ta=null;this.height=this.width=0;this.zz=this.i0=null;this.Q7=this.K9=this.bba=!0;this.f3=this.Tx=this.Hh=this.Ux=null;this.WL=new gvjs_lX}gvjs_o(gvjs_nX,gvjs_Qn);gvjs_=gvjs_nX.prototype;
gvjs_.M=function(){gvjs_E(this.Es);delete this.Es;null!==this.renderer&&(this.renderer.clear(),delete this.renderer);gvjs_E(this.tree);delete this.tree;gvjs_E(this.sb);delete this.sb;gvjs_Qn.prototype.M.call(this)};gvjs_.He=function(){};gvjs_.getSelection=function(){return this.el.getSelection()};gvjs_.setSelection=function(a){if(Array.isArray(a)&&0!==a.length){var b=a[0].row;if(a=this.tree.uw[b]||null)this.el.pj=new Set,this.el.Kp(b)}else this.el.pj=new Set,a=this.tree.ep[0];a&&a.draw()};
gvjs_.Koa=function(a){if(null==a)return null;a=this.tree.uw[null==a.row?-1:a.row]||null;return null!==a?{value:a.color,color:gvjs_oX(a,!1,!1)}:null};gvjs_.Rd=function(a,b,c){this.options=new gvjs_Aj([c]);this.width=this.La(this.options);this.height=this.getHeight(this.options);c=new gvjs_A(this.width,this.height);if(null===this.sb){var d=gvjs_K(this.options,gvjs_Eu);this.sb=new gvjs_3B(this.container,c,a,d)}else this.sb.update(c,a);this.Ta=b;this.sb.rl(this.kna.bind(this),a)};
gvjs_.kna=function(){var a=this.sb.Oa();this.renderer=a;a.getContainer().oncontextmenu=function(){return!1};gvjs_E(this.Es);this.Es=null;gvjs_E(this.tree);this.tree=null;gvjs_nka(this,this.options);gvjs_oka(this,this.Ta);this.LD=Number.MAX_VALUE;this.GD=-1*Number.MAX_VALUE;this.tree=this.tW(this.Es);null!==this.Z0&&(this.LD=this.Z0);null!==this.L0&&(this.GD=this.L0);if(a=this.tree.ep[0])a.normalize(this.LD,this.GD),null!==this.Tx&&(this.Tx=null);this.setSelection([]);gvjs_I(this,gvjs_i,{})};
gvjs_.X$=function(){var a=this.Ufa,b=a.getParent();if(!b)return!1;null!==a.tooltip&&a.tooltip.detach();this.setSelection([{row:b.row}]);return!0};gvjs_.Roa=function(){return this.Kca};
function gvjs_nka(a,b){function c(f,g){return""===f||f===gvjs_f?gvjs_uj(gvjs_2z(gvjs_vj(gvjs_qj(g).hex),.35)):f}a.title=gvjs_J(b,gvjs_fx);a.Z0=b.Aa("minColorValue");a.L0=b.Aa("maxColorValue");a.WK=gvjs_K(b,"rotatingHue",!1);a.WK&&(a.fba=gvjs_L(b,"hueStep",60/(1+Math.sqrt(5))),a.c1=gvjs_L(b,"minSaturation",0),a.X0=gvjs_L(b,"midSaturation",.5),a.O0=gvjs_L(b,"maxSaturation",1),a.n_=b.Aa("headerSaturation"),a.y5=b.Aa("noSaturation"),a.$0=gvjs_L(b,"minLightness",1),a.W0=gvjs_L(b,"midLightness",.75),a.N0=
gvjs_L(b,"maxLightness",.5),a.l_=b.Aa("headerLightness"),a.x5=b.Aa("noLightness"));a.rL=gvjs_K(b,"showScale",!1);a.Pfa=gvjs_K(b,"scale.showText",!1);a.maxDepth=gvjs_Oj(b,gvjs_Kv,1);a.ID=gvjs_Oj(b,"maxPostDepth",0);a.kha=gvjs_K(b,"useWeightedAverageForAggregation",!1);a.cba=gvjs_Oj(b,"hintOpacity",.5);1>a.maxDepth&&(a.maxDepth=1);0>a.ID&&(a.ID=0);a.bba=gvjs_K(b,"highlightOnMouseOver",!1);a.K9=gvjs_K(b,"enableHighlight",!1);a.Q7=gvjs_K(b,"borderOnMouseOver",!0);a.Sfa=gvjs_K(b,gvjs_Vw,!0);a.l5=gvjs_J(b,
"tooltipClass");var d=b.fa("generateTooltip",null);d&&(a.E$=d);a.borderColor=gvjs_oy(b,"borderColor",gvjs_ea);a.P7=gvjs_oy(b,"borderMouseOverColor",gvjs_ea);d=[5,4,3];var e=[.2,.4,.6];a.Qp=b.fa("borderMouseOverSizes",d);a.IB=b.fa("borderMouseOverOpacities",e);a.Qp.length!==a.IB.length&&(alert("The arrays' lengths for border mouse over sizes and opacities are not equal. We will use default borders."),a.Qp=d,a.IB=e);for(d=0;d<a.Qp.length;d++)a.Qp[d]=Number(a.Qp[d]),a.IB[d]=Number(a.IB[d]);a.fontSize=
gvjs_Oj(b,gvjs_zp,12);a.fontFamily=gvjs_J(b,gvjs_xp,gvjs_2r);a.r$=gvjs_oy(b,gvjs_Du,gvjs_mr);a.s$=gvjs_oy(b,"fontMouseOverColor",gvjs_ca);0<=a.fontFamily.indexOf("/")&&(alert("Bad font family! We will use Arial"),a.fontFamily=gvjs_2r);a.ja=gvjs_ry(b,gvjs_bx,{color:a.r$,bb:a.fontFamily,fontSize:a.fontSize});d=gvjs_x(a.ja);d.color=a.s$;a.GR=gvjs_ry(b,"mouseOverTextStyle",d);d=gvjs_x(a.ja);d.bold=!0;a.Ega=gvjs_ry(b,gvjs_ix,d);a.efa=gvjs_ry(b,"scale.textStyle",gvjs_x(a.ja));d=gvjs_Kz({background:gvjs_dv,
color:"infotext",padding:gvjs_Pr,border:gvjs_Qr,"font-size":a.ja.fontSize+"px; ","font-family":a.ja.bb});d=gvjs_J(b,"tooltipStyleString",d);a.Jga=gvjs_Jz(d);a.JU=gvjs_oy(b,"noColor",gvjs_ca);a.rR=gvjs_oy(b,gvjs_Pv,"#dc3912");a.nR=gvjs_oy(b,gvjs_Mv,"#efe6dc");a.fR=gvjs_oy(b,gvjs_Jv,gvjs_lr);a.k_=gvjs_oy(b,gvjs_Zu,gvjs_zr);a.bM=gvjs_oy(b,"noHighlightColor","");a.wR=gvjs_oy(b,"minHighlightColor","");a.oR=gvjs_oy(b,"midHighlightColor","");a.iR=gvjs_oy(b,"maxHighlightColor","");a.ZP=gvjs_oy(b,"headerHighlightColor",
"");a.bM=c(a.bM,a.JU);a.wR=c(a.wR,a.rR);a.iR=c(a.iR,a.fR);a.oR=c(a.oR,a.nR);a.ZP=c(a.ZP,a.k_);a.header=gvjs_Oj(b,gvjs__u,22);a.f3=b.cb("residualNode");gvjs_mka(a.WL,b.pb("eventsConfig",{}))}function gvjs_oka(a,b){if(2===b.$()){b=new gvjs_N(b);var c={};b.Hn([0,1,(c.type=gvjs_g,c.calc=function(){return 1},c)])}gvjs_E(a.Es);a.Es=new gvjs_3L(b);if(1<a.Es.ep.length)throw Error("Found "+a.Es.ep.length+" root nodes. Only a single root node is allowed.");}
gvjs_.tW=function(a){var b=this;a=this.WK?new gvjs_4L(a,function(c){return new gvjs_pX(b,c)},void 0,0,this.fba):new gvjs_4L(a,function(c){return new gvjs_pX(b,c)});this.Kca=a.getHeight();a.MB(function(c){var d=c.wea;d=typeof d===gvjs_g&&0<=d?d:0;return{primary:d,secondary:0<d?c.secondary:null,use_weighted_avg:c.treemap.kha}},function(c,d){if(0===d.length)return c;for(var e=[],f=[],g=0;g<d.length;g++)e.push(d[g].primary),f.push(d[g].secondary);e=gvjs_EW(c.primary,e);if(c.use_weighted_avg)for(f=c=0;f<
d.length;f++){if(g=d[f].primary,0!==g){var h=d[f].secondary;h=null===h?0:h;c+=g/e*h}}else c=gvjs__ja(c.secondary,f);return{primary:e,secondary:c}},function(c,d){c.area=d.primary;c.color=0<c.area?d.secondary:null});a.VL(function(c){null!==c.color&&(c.color<b.LD&&(b.LD=c.color),c.color>b.GD&&(b.GD=c.color))});return a};
gvjs_.OH=function(a){var b=this.Ux;gvjs_qX(this);this.Hh=this.renderer.Sa();this.renderer.appendChild(this.canvas,this.Hh);var c=b.top+7,d=b.height-7,e=this.rR,f=this.nR,g=this.fR;this.WK&&null!=a&&(e=gvjs_uj(gvjs__z(a,this.c1,this.$0)),f=gvjs_uj(gvjs__z(a,this.X0,this.W0)),g=gvjs_uj(gvjs__z(a,this.O0,this.N0)));a=b.width/2;e=new gvjs_3({gradient:{x1:b.left,y1:0,x2:b.left+a+1,y2:0,Vf:e,sf:f}});f=new gvjs_3({gradient:{x1:b.left+a,y1:0,x2:b.left+b.width,y2:0,Vf:f,sf:g}});this.renderer.yb(b.left,c,b.width,
d,new gvjs_3({stroke:gvjs_tr,strokeWidth:2}),this.Hh);this.renderer.yb(b.left,c,a+1,d,e,this.Hh);this.renderer.yb(b.left+a,c,b.width-a,d,f,this.Hh);if(!this.Pfa)return 0;c=gvjs_x(this.efa);d=this.renderer.ce(this.LD.toString(),b.left,b.top+b.height+3,0,gvjs_0,gvjs_2,c,this.Hh);f=d.offsetHeight;d=this.renderer.ce(this.GD.toString(),b.left+b.width,b.top+b.height+3,0,gvjs_R,gvjs_2,c,this.Hh);return f=Math.max(f,d.offsetHeight)};
function gvjs_qX(a){if(null!==a.Hh){var b=a.Hh.j();b.parentNode.removeChild(b);a.Hh=null}}function gvjs_rX(a){null!==a.Tx&&(a.Tx.parentNode.removeChild(a.Tx),a.Tx=null)}
function gvjs_pX(a,b){gvjs__L.call(this,b.getId(),b.getName());this.treemap=a;this.hue=this.color=this.uf=this.Kc=this.KI=this.HO=this.Ks=this.jC=null;this.area=0;this.opacity=1;this.highlight=this.Cw=!1;this.z1=this.aba="";this.collapsed=!1;this.WL=new gvjs_lX;this.row=b.getId();this.lI=gvjs_1L(b);this.tooltip=new gvjs_jB(null,null,a.renderer?gvjs_3g(a.renderer.getContainer()):null);this.wea=b.getValue(2);this.secondary=4<=b.mb().$()?b.getValue(3):this.wea;this.WL.XJ=a.WL.XJ}gvjs_o(gvjs_pX,gvjs__L);
gvjs_=gvjs_pX.prototype;gvjs_.M=function(){delete this.uf;null!==this.tooltip&&(gvjs_E(this.tooltip),delete this.tooltip);gvjs__L.prototype.M.call(this)};
gvjs_.normalize=function(a,b){if(null!==this.color){var c=b-a;0!==c?(this.color=gvjs_0g(this.color,a,b),this.color-=a,this.color/=c):this.color=.5}c=0;for(var d=this.ze();c<d;c++)this.getChildren()[c].normalize(a,b);this.treemap.Sfa&&(b=this.treemap,c=this.row,d=this.lI,a=this.tooltip,null!==c&&null!=b.E$?(b=b.E$(c,this.area,this.color),a.BT(gvjs_OA(b))):""===b.l5?a.BT(gvjs_5f(gvjs_Ob,{style:gvjs_Hf(b.Jga)},gvjs_2f(d))):(a.className=b.l5,a.du(d)))};
function gvjs_oX(a,b,c){var d=null;if(null===a.hue)if(c){var e=gvjs_vj(gvjs_qj(a.treemap.iR).hex);var f=gvjs_vj(gvjs_qj(a.treemap.wR).hex);var g=gvjs_vj(gvjs_qj(a.treemap.oR).hex);d=gvjs_vj(gvjs_qj(a.treemap.ZP).hex);var h=gvjs_vj(gvjs_qj(a.treemap.bM).hex)}else e=gvjs_vj(gvjs_qj(a.treemap.fR).hex),f=gvjs_vj(gvjs_qj(a.treemap.rR).hex),g=gvjs_vj(gvjs_qj(a.treemap.nR).hex),d=gvjs_vj(gvjs_qj(a.treemap.k_).hex),h=gvjs_vj(gvjs_qj(a.treemap.JU).hex);else e=gvjs__z(a.hue,a.treemap.O0,a.treemap.N0),f=gvjs__z(a.hue,
a.treemap.c1,a.treemap.$0),g=gvjs__z(a.hue,a.treemap.X0,a.treemap.W0),c&&(e=gvjs_2z(e,.35),f=gvjs_2z(f,.35),g=gvjs_2z(g,.35)),null!==a.treemap.n_&&null!==a.treemap.l_&&(d=gvjs__z(a.hue,a.treemap.n_,a.treemap.l_),c&&(d=gvjs_2z(d,.35))),null!==a.treemap.y5&&null!==a.treemap.x5?(h=gvjs__z(a.hue,a.treemap.y5,a.treemap.x5),c&&(h=gvjs_2z(h,.35))):h=c?gvjs_vj(gvjs_qj(a.treemap.bM).hex):gvjs_vj(gvjs_qj(a.treemap.JU).hex);return gvjs_uj(b&&null!==d?d:null===a.color?h:.5>a.color?gvjs_xj(g,f,2*a.color):gvjs_xj(e,
g,2*(a.color-.5)))}function gvjs_sX(a){delete a.uf;a.uf=null;for(var b=0,c=a.ze();b<c;b++)gvjs_sX(a.getChildren()[b])}function gvjs_tX(a,b){null!==a.jC&&(a.jC.parentNode.removeChild(a.jC),a.jC=null);if(a.Ks){b=gvjs_x(b);b.bold=b.bold||a.Ks.header;var c=gvjs_DG(a.treemap.renderer.me.bind(a.treemap.renderer),a.Ks.text,b,a.Ks.width,1),d="";0<c.lines.length&&(d=c.lines[0]);a.jC=a.treemap.renderer.ce(d,a.Ks.Qxa+a.Ks.width/2,a.Ks.Rxa,a.Ks.width,gvjs_0,gvjs_0,b,a.Kc);a.jC.setAttribute(gvjs_lw,gvjs_f)}}
function gvjs_uX(a,b){if(null!==a.treemap.zz){var c=a.treemap.i0,d=gvjs_Oh();(b=gvjs_BW(0,b.relatedTarget))&&d.tP(b)&&gvjs_tX(c,a.treemap.ja);a.treemap.zz.H&&(c=a.treemap.zz.j(),c.parentNode.removeChild(c));a.treemap.zz=null;a.treemap.i0=null}}
gvjs_.draw=function(){var a=Math.max.apply(Math,this.treemap.Qp),b=this.treemap.width-2*a,c=this.treemap.height,d=this.treemap.header;gvjs_sX(this);var e=0;this.treemap.renderer.clear();this.treemap.canvas=this.treemap.renderer.Lm(b,c);var f=Math.pow(2*(b+c),1/3),g=(1+Math.sqrt(5))/2*f,h=b/3;""!==this.treemap.title&&(this.treemap.renderer.ce(this.treemap.title,a,g/2,b-h,gvjs_2,gvjs_0,this.treemap.Ega,this.treemap.canvas),e=g);this.treemap.rL&&(e=(g-f)/2,this.treemap.Ux=new gvjs_5(b-h-a,e,h,gvjs_0g(g-
2*e,0,g)),h=this.treemap.OH(),e=g+h,this.treemap.WK&&gvjs_qX(this.treemap));this.uf=new gvjs_5(a,d+e,b-2*a,c-d-e-a);gvjs_vX(this,d,0);this.Cw=!1;this.Rd(d,0);this.treemap.Ufa=this;this.treemap.renderer.ic(this.treemap.canvas,gvjs_kd,this.Cta.bind(this))};
function gvjs_wX(a,b,c,d,e,f,g){a.aba=gvjs_oX(a,g,!0);a.z1=gvjs_oX(a,g,!1);if(0>=e||0>=d||a.Cw&&g)return null;null===a.Kc&&(a.Kc=a.treemap.renderer.Sa(),a.treemap.renderer.appendChild(a.treemap.canvas,a.Kc));var h=new gvjs_3({fill:a.z1,fillOpacity:a.opacity});a.Cw||h.rd(a.treemap.borderColor,1);h=a.treemap.renderer.yb(b,c,d,e,h,a.Kc);a.Cw||(a.Ks={text:f,Qxa:b,Rxa:c+e/2,width:d,height:e,header:g},gvjs_tX(a,a.treemap.ja));return h}
function gvjs_xX(a){a.HO=gvjs_wX(a,a.uf.left,a.uf.top,a.uf.width,a.uf.height,a.lI,!1)}function gvjs_yX(a,b){a.KI=gvjs_wX(a,a.uf.left,a.uf.top-b,a.uf.width,b,a.lI,!0)}
gvjs_.Rd=function(a,b){this.opacity=1;this.highlight=!1;var c=this.ze();this.KI=this.HO=this.Kc=null;if(0===c)gvjs_xX(this);else if(b>=this.treemap.maxDepth||this.collapsed){0!==a&&this.uf.height>=a&&!this.collapsed&&b<this.treemap.maxDepth&&gvjs_yX(this,a);if(!this.collapsed&&b<this.treemap.maxDepth+this.treemap.ID){for(var d=0;d<c;d++){var e=this.getChildren()[d];e.highlight=this.highlight;e.Cw=!0;e.Rd(a,b+1)}this.opacity=(this.treemap.maxDepth+this.treemap.ID-b)/this.treemap.ID*this.treemap.cba}gvjs_xX(this)}else{for(d=
0;d<c;d++)e=this.getChildren()[d],e.Cw=!1,e.highlight=this.highlight,e.Rd(a,b+1);0!==a&&gvjs_yX(this,a)}this.B0()};gvjs_.vxa=function(a,b){if(null!==a.treemap.f3){var c=a.treemap.f3;if(b.getName()===c)return-1;if(a.getName()===c)return 1}return b.area===a.area?a.row-b.row:b.area-a.area};
function gvjs_vX(a,b,c){c>=a.treemap.maxDepth-1&&(b=0);a.collapsed=!1;var d=gvjs_Le(a.getChildren());d.sort(a.vxa);gvjs_zX(a,d,gvjs_x(a.uf),0,d.length,a.area);a=0;for(var e=d.length;a<e;a++)d[a].ze()&&(d[a].uf.height>2*b?(d[a].uf.top+=b,d[a].uf.height-=b,gvjs_vX(d[a],b,c+1)):d[a].collapsed=!0)}
function gvjs_zX(a,b,c,d,e,f){if(!(0>=e))if(1===e)b[d].uf=new gvjs_5(Number(c.left),Number(c.top),0===f?0:Number(c.width),0===f?0:Number(c.height));else{var g=b[d].area,h,k=1;for(h=d+1;h<d+e&&2*(g+b[h].area)<f;h++)g+=b[h].area,k++;h=f-g;e-=k;var l=c.left,m=c.top,n=c.width;c=c.height;if(0>=g+h)var p=f=0;else n>=c?(f=Math.ceil(g*n/f),p=0):(f=Math.ceil(g*c/f),p=1);0===p?(gvjs_zX(a,b,new gvjs_5(l,m,f,c),d,k,g),gvjs_zX(a,b,new gvjs_5(l+f,m,n-f,c),d+k,e,h)):(gvjs_zX(a,b,new gvjs_5(l,m,n,f),d,k,g),gvjs_zX(a,
b,new gvjs_5(l,m+f,n,c-f),d+k,e,h))}}function gvjs_AX(a){if(a.treemap.bba||a.treemap.K9){var b=a.z1;a.highlight&&(b=a.aba);b=new gvjs_3({fill:b,fillOpacity:a.opacity});a.Cw||b.rd(a.treemap.borderColor,1);null!==a.HO&&a.treemap.renderer.rj(a.HO,b);null!==a.KI&&a.treemap.renderer.rj(a.KI,b);b=0;for(var c=a.ze();b<c;b++){var d=a.getChildren()[b];d.highlight=a.highlight;gvjs_AX(d)}}}
gvjs_.mna=function(){this.treemap.setSelection([{row:this.row}]);gvjs_I(this.treemap,gvjs_k,null);gvjs_I(this.treemap,"drilldown",{row:this.row})};gvjs_.Tva=function(){this.treemap.X$()&&(gvjs_I(this.treemap,"rollup",{row:this.row}),gvjs_I(this.treemap,gvjs_k,null))};
gvjs_.tra=function(a){this.highlight=!0;gvjs_AX(this);if(this.treemap.Q7){gvjs_uX(this,a);a=0;null!==this.KI&&(a=this.treemap.header);this.treemap.zz=this.treemap.renderer.Sa();this.treemap.renderer.appendChild(this.treemap.canvas,this.treemap.zz);for(var b=this.treemap.Qp.length,c=0;c<b;c++){var d=this.treemap.Qp[c];this.treemap.renderer.yb(this.uf.left-d/2,this.uf.top-a-d/2,this.uf.width+d,this.uf.height+a+d,new gvjs_3({stroke:this.treemap.P7,strokeWidth:this.treemap.Qp[c],strokeOpacity:this.treemap.IB[c]}),
this.treemap.zz)}this.treemap.i0=this;gvjs_tX(this,this.treemap.GR)}this.treemap.GR&&gvjs_tX(this,this.treemap.GR);this.treemap.rL&&(null!==this.hue&&(typeof this.secondary===gvjs_g?this.treemap.OH(this.hue):gvjs_qX(this.treemap)),b=0,null!==this.color&&(b=this.color),typeof this.secondary===gvjs_g?(a=this.treemap,b=gvjs_0g(b,0,1),b*=a.Ux.width,b+=a.Ux.left,gvjs_rX(a),b=gvjs_UA([{x:b-5,y:a.Ux.top},{x:b,y:a.Ux.top+5},{x:b+5,y:a.Ux.top}]),a.Tx=a.renderer.Ia(b,new gvjs_3({fill:"#777777"}),a.canvas)):
gvjs_rX(this.treemap));gvjs_I(this.treemap,gvjs_3u,{row:this.row})};gvjs_.Vga=function(){gvjs_rX(this.treemap);null!==this.hue&&gvjs_qX(this.treemap);this.highlight=!1;gvjs_AX(this);this.treemap.ja&&gvjs_tX(this,this.treemap.ja);gvjs_I(this.treemap,gvjs_yx,{row:this.row})};gvjs_.Cta=function(a){gvjs_BW(gvjs_Oh(),a.relatedTarget)&&gvjs_uX(this,a);gvjs_I(this.treemap,gvjs_6v,{row:this.row});this.Vga(a)};
gvjs_.B0=function(){var a=this;null!==this.tooltip&&this.tooltip.detach();if(null!==this.Kc&&!this.Cw){var b=this.Kc.j();null!==this.tooltip&&""!==this.tooltip.qP()&&this.tooltip.CB(b);this.$v()?gvjs_C(b,"cursor",gvjs_eu):gvjs_C(b,"cursor",gvjs_kw);this.treemap.renderer.ic(b,gvjs_ld,function(){gvjs_I(a.treemap,gvjs_7v,{row:a.row})});this.treemap.renderer.ic(b,gvjs_kd,function(){gvjs_I(a.treemap,gvjs_6v,{row:a.row})});this.WL.B0(this.treemap.renderer,b,{highlight:this.tra.bind(this),unhighlight:this.Vga.bind(this),
rollup:this.Tva.bind(this),drilldown:this.mna.bind(this)})}};gvjs_q(gvjs_Yc,gvjs_nX,void 0);gvjs_nX.prototype.draw=gvjs_nX.prototype.draw;gvjs_nX.prototype.getSelection=gvjs_nX.prototype.getSelection;gvjs_nX.prototype.setSelection=gvjs_nX.prototype.setSelection;gvjs_nX.prototype.goUpAndDraw=gvjs_nX.prototype.X$;gvjs_nX.prototype.getMaxPossibleDepth=gvjs_nX.prototype.Roa;gvjs_nX.prototype.clearChart=gvjs_nX.prototype.Jb;gvjs_nX.prototype.getElementColor=gvjs_nX.prototype.Koa;
