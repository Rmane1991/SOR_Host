﻿/*
 Highcharts JS v9.1.2 (2021-06-16)

 (c) 2009-2021 Torstein Honsi

 License: www.highcharts.com/license
*/
'use strict'; (function (X, L) { "object" === typeof module && module.exports ? (L["default"] = L, module.exports = X.document ? L(X) : L) : "function" === typeof define && define.amd ? define("highcharts/highcharts", function () { return L(X) }) : (X.Highcharts && X.Highcharts.error(16, !0), X.Highcharts = L(X)) })("undefined" !== typeof window ? window : this, function (X) {
    function L(v, a, E, H) { v.hasOwnProperty(a) || (v[a] = H.apply(null, E)) } var a = {}; L(a, "Core/Globals.js", [], function () {
        var v = "undefined" !== typeof X ? X : "undefined" !== typeof window ? window :
{}, a; (function (a) {
    a.SVG_NS = "http://www.w3.org/2000/svg"; a.product = "Highcharts"; a.version = "9.1.2"; a.win = v; a.doc = a.win.document; a.svg = a.doc && a.doc.createElementNS && !!a.doc.createElementNS(a.SVG_NS, "svg").createSVGRect; a.userAgent = a.win.navigator && a.win.navigator.userAgent || ""; a.isChrome = -1 !== a.userAgent.indexOf("Chrome"); a.isFirefox = -1 !== a.userAgent.indexOf("Firefox"); a.isMS = /(edge|msie|trident)/i.test(a.userAgent) && !a.win.opera; a.isSafari = !a.isChrome && -1 !== a.userAgent.indexOf("Safari"); a.isTouchDevice =
    /(Mobile|Android|Windows Phone)/.test(a.userAgent); a.isWebKit = -1 !== a.userAgent.indexOf("AppleWebKit"); a.deg2rad = 2 * Math.PI / 360; a.hasBidiBug = a.isFirefox && 4 > parseInt(a.userAgent.split("Firefox/")[1], 10); a.hasTouch = !!a.win.TouchEvent; a.marginNames = ["plotTop", "marginRight", "marginBottom", "plotLeft"]; a.noop = function () { }; a.supportsPassiveEvents = function () {
        var v = !1; if (!a.isMS) {
            var u = Object.defineProperty({}, "passive", { get: function () { v = !0 } }); a.win.addEventListener && a.win.removeEventListener && (a.win.addEventListener("testPassive",
            a.noop, u), a.win.removeEventListener("testPassive", a.noop, u))
        } return v
    }(); a.charts = []; a.dateFormats = {}; a.seriesTypes = {}; a.symbolSizes = {}; a.chartCount = 0
})(a || (a = {})); return a
    }); L(a, "Core/Utilities.js", [a["Core/Globals.js"]], function (a) {
        function v(b, d, g, r) {
            var D = d ? "Highcharts error" : "Highcharts warning"; 32 === b && (b = D + ": Deprecated member"); var C = h(b), M = C ? D + " #" + b + ": www.highcharts.com/errors/" + b + "/" : b.toString(); if ("undefined" !== typeof r) {
                var l = ""; C && (M += "?"); J(r, function (b, Q) {
                    l += "\n - " + Q + ": " + b; C &&
                    (M += encodeURI(Q) + "=" + encodeURI(b))
                }); M += l
            } A(a, "displayError", { chart: g, code: b, message: M, params: r }, function () { if (d) throw Error(M); k.console && -1 === v.messages.indexOf(M) && console.warn(M) }); v.messages.push(M)
        } function E(b, d) { var D = {}; J(b, function (g, k) { if (G(b[k], !0) && !b.nodeType && d[k]) g = E(b[k], d[k]), Object.keys(g).length && (D[k] = g); else if (G(b[k]) || b[k] !== d[k]) D[k] = b[k] }); return D } function H(b, d) { return parseInt(b, d || 10) } function x(b) { return "string" === typeof b } function y(b) {
            b = Object.prototype.toString.call(b);
            return "[object Array]" === b || "[object Array Iterator]" === b
        } function G(b, d) { return !!b && "object" === typeof b && (!d || !y(b)) } function B(b) { return G(b) && "number" === typeof b.nodeType } function q(b) { var d = b && b.constructor; return !(!G(b, !0) || B(b) || !d || !d.name || "Object" === d.name) } function h(b) { return "number" === typeof b && !isNaN(b) && Infinity > b && -Infinity < b } function f(b) { return "undefined" !== typeof b && null !== b } function c(b, d, g) {
            var k; x(d) ? f(g) ? b.setAttribute(d, g) : b && b.getAttribute && ((k = b.getAttribute(d)) || "class" !==
            d || (k = b.getAttribute(d + "Name"))) : J(d, function (d, g) { b.setAttribute(g, d) }); return k
        } function e(b, d) { var g; b || (b = {}); for (g in d) b[g] = d[g]; return b } function t() { for (var b = arguments, d = b.length, g = 0; g < d; g++) { var k = b[g]; if ("undefined" !== typeof k && null !== k) return k } } function m(b, d) { a.isMS && !a.svg && d && "undefined" !== typeof d.opacity && (d.filter = "alpha(opacity=" + 100 * d.opacity + ")"); e(b.style, d) } function w(b, g, k, r, l) {
            b = d.createElement(b); g && e(b, g); l && m(b, { padding: "0", border: "none", margin: "0" }); k && m(b, k); r && r.appendChild(b);
            return b
        } function n(b, d) { return parseFloat(b.toPrecision(d || 14)) } function l(b, d, g) {
            var D = a.getStyle || l; if ("width" === d) return d = Math.min(b.offsetWidth, b.scrollWidth), g = b.getBoundingClientRect && b.getBoundingClientRect().width, g < d && g >= d - 1 && (d = Math.floor(g)), Math.max(0, d - (D(b, "padding-left", !0) || 0) - (D(b, "padding-right", !0) || 0)); if ("height" === d) return Math.max(0, Math.min(b.offsetHeight, b.scrollHeight) - (D(b, "padding-top", !0) || 0) - (D(b, "padding-bottom", !0) || 0)); k.getComputedStyle || v(27, !0); if (b = k.getComputedStyle(b,
            void 0)) { var r = b.getPropertyValue(d); t(g, "opacity" !== d) && (r = H(r)) } return r
        } function J(b, d, g) { for (var k in b) Object.hasOwnProperty.call(b, k) && d.call(g || b[k], b[k], k, b) } function K(b, d, g) {
            function k(d, z) { var Q = b.removeEventListener || a.removeEventListenerPolyfill; Q && Q.call(b, d, z, !1) } function r(g) { var z; if (b.nodeName) { if (d) { var Q = {}; Q[d] = !0 } else Q = g; J(Q, function (b, d) { if (g[d]) for (z = g[d].length; z--;) k(d, g[d][z].fn) }) } } var D = "function" === typeof b && b.prototype || b; if (Object.hasOwnProperty.call(D, "hcEvents")) {
                var l =
                D.hcEvents; d ? (D = l[d] || [], g ? (l[d] = D.filter(function (b) { return g !== b.fn }), k(d, g)) : (r(l), l[d] = [])) : (r(l), delete D.hcEvents)
            }
        } function A(b, g, k, r) {
            k = k || {}; if (d.createEvent && (b.dispatchEvent || b.fireEvent && b !== a)) { var D = d.createEvent("Events"); D.initEvent(g, !0, !0); k = e(D, k); b.dispatchEvent ? b.dispatchEvent(k) : b.fireEvent(g, k) } else if (b.hcEvents) {
                k.target || e(k, { preventDefault: function () { k.defaultPrevented = !0 }, target: b, type: g }); D = []; for (var l = b, C = !1; l.hcEvents;) Object.hasOwnProperty.call(l, "hcEvents") && l.hcEvents[g] &&
                (D.length && (C = !0), D.unshift.apply(D, l.hcEvents[g])), l = Object.getPrototypeOf(l); C && D.sort(function (b, z) { return b.order - z.order }); D.forEach(function (d) { !1 === d.fn.call(b, k) && k.preventDefault() })
            } r && !k.defaultPrevented && r.call(b, k)
        } var p = a.charts, d = a.doc, k = a.win; ""; (v || (v = {})).messages = []; var b; Math.easeInOutSine = function (b) { return -.5 * (Math.cos(Math.PI * b) - 1) }; var g = Array.prototype.find ? function (b, d) { return b.find(d) } : function (b, d) { var g, k = b.length; for (g = 0; g < k; g++) if (d(b[g], g)) return b[g] }; J({
            map: "map",
            each: "forEach", grep: "filter", reduce: "reduce", some: "some"
        }, function (b, d) { a[d] = function (g) { var k; v(32, !1, void 0, (k = {}, k["Highcharts." + d] = "use Array." + b, k)); return Array.prototype[b].apply(g, [].slice.call(arguments, 1)) } }); var r, F = function () { var b = Math.random().toString(36).substring(2, 9) + "-", d = 0; return function () { return "highcharts-" + (r ? "" : b) + d++ } }(); k.jQuery && (k.jQuery.fn.highcharts = function () {
            var b = [].slice.call(arguments); if (this[0]) return b[0] ? (new (a[x(b[0]) ? b.shift() : "Chart"])(this[0], b[0], b[1]),
            this) : p[c(this[0], "data-highcharts-chart")]
        }); return {
            addEvent: function (b, d, g, k) {
                void 0 === k && (k = {}); var r = "function" === typeof b && b.prototype || b; Object.hasOwnProperty.call(r, "hcEvents") || (r.hcEvents = {}); r = r.hcEvents; a.Point && b instanceof a.Point && b.series && b.series.chart && (b.series.chart.runTrackerClick = !0); var l = b.addEventListener || a.addEventListenerPolyfill; l && l.call(b, d, g, a.supportsPassiveEvents ? { passive: void 0 === k.passive ? -1 !== d.indexOf("touch") : k.passive, capture: !1 } : !1); r[d] || (r[d] = []); r[d].push({
                    fn: g,
                    order: "number" === typeof k.order ? k.order : Infinity
                }); r[d].sort(function (b, d) { return b.order - d.order }); return function () { K(b, d, g) }
            }, arrayMax: function (b) { for (var d = b.length, g = b[0]; d--;) b[d] > g && (g = b[d]); return g }, arrayMin: function (b) { for (var d = b.length, g = b[0]; d--;) b[d] < g && (g = b[d]); return g }, attr: c, clamp: function (b, d, g) { return b > d ? b < g ? b : g : d }, cleanRecursively: E, clearTimeout: function (b) { f(b) && clearTimeout(b) }, correctFloat: n, createElement: w, css: m, defined: f, destroyObjectProperties: function (b, d) {
                J(b, function (g,
                k) { g && g !== d && g.destroy && g.destroy(); delete b[k] })
            }, discardElement: function (d) { b || (b = w("div")); d && b.appendChild(d); b.innerHTML = "" }, erase: function (b, d) { for (var g = b.length; g--;) if (b[g] === d) { b.splice(g, 1); break } }, error: v, extend: e, extendClass: function (b, d) { var g = function () { }; g.prototype = new b; e(g.prototype, d); return g }, find: g, fireEvent: A, getMagnitude: function (b) { return Math.pow(10, Math.floor(Math.log(b) / Math.LN10)) }, getNestedProperty: function (b, d) {
                for (b = b.split(".") ; b.length && f(d) ;) {
                    var g = b.shift(); if ("undefined" ===
                    typeof g || "__proto__" === g) return; d = d[g]; if (!f(d) || "function" === typeof d || "number" === typeof d.nodeType || d === k) return
                } return d
            }, getStyle: l, inArray: function (b, d, g) { v(32, !1, void 0, { "Highcharts.inArray": "use Array.indexOf" }); return d.indexOf(b, g) }, isArray: y, isClass: q, isDOMElement: B, isFunction: function (b) { return "function" === typeof b }, isNumber: h, isObject: G, isString: x, keys: function (b) { v(32, !1, void 0, { "Highcharts.keys": "use Object.keys" }); return Object.keys(b) }, merge: function () {
                var b, d = arguments, g = {}, k = function (b,
                d) { "object" !== typeof b && (b = {}); J(d, function (g, z) { "__proto__" !== z && "constructor" !== z && (!G(g, !0) || q(g) || B(g) ? b[z] = d[z] : b[z] = k(b[z] || {}, g)) }); return b }; !0 === d[0] && (g = d[1], d = Array.prototype.slice.call(d, 2)); var r = d.length; for (b = 0; b < r; b++) g = k(g, d[b]); return g
            }, normalizeTickInterval: function (b, d, g, k, r) {
                var l = b; g = t(g, 1); var c = b / g; d || (d = r ? [1, 1.2, 1.5, 2, 2.5, 3, 4, 5, 6, 8, 10] : [1, 2, 2.5, 5, 10], !1 === k && (1 === g ? d = d.filter(function (b) { return 0 === b % 1 }) : .1 >= g && (d = [1 / g]))); for (k = 0; k < d.length && !(l = d[k], r && l * g >= b || !r && c <= (d[k] +
                (d[k + 1] || d[k])) / 2) ; k++); return l = n(l * g, -Math.round(Math.log(.001) / Math.LN10))
            }, objectEach: J, offset: function (b) { var g = d.documentElement; b = b.parentElement || b.parentNode ? b.getBoundingClientRect() : { top: 0, left: 0, width: 0, height: 0 }; return { top: b.top + (k.pageYOffset || g.scrollTop) - (g.clientTop || 0), left: b.left + (k.pageXOffset || g.scrollLeft) - (g.clientLeft || 0), width: b.width, height: b.height } }, pad: function (b, d, g) { return Array((d || 2) + 1 - String(b).replace("-", "").length).join(g || "0") + b }, pick: t, pInt: H, relativeLength: function (b,
            d, g) { return /%$/.test(b) ? d * parseFloat(b) / 100 + (g || 0) : parseFloat(b) }, removeEvent: K, splat: function (b) { return y(b) ? b : [b] }, stableSort: function (b, d) { var g = b.length, k, r; for (r = 0; r < g; r++) b[r].safeI = r; b.sort(function (b, g) { k = d(b, g); return 0 === k ? b.safeI - g.safeI : k }); for (r = 0; r < g; r++) delete b[r].safeI }, syncTimeout: function (b, d, g) { if (0 < d) return setTimeout(b, d, g); b.call(0, g); return -1 }, timeUnits: { millisecond: 1, second: 1E3, minute: 6E4, hour: 36E5, day: 864E5, week: 6048E5, month: 24192E5, year: 314496E5 }, uniqueKey: F, useSerialIds: function (b) {
                return r =
                t(b, r)
            }, wrap: function (b, d, g) { var k = b[d]; b[d] = function () { var b = Array.prototype.slice.call(arguments), d = arguments, r = this; r.proceed = function () { k.apply(r, arguments.length ? arguments : d) }; b.unshift(k); b = g.apply(this, b); r.proceed = null; return b } }
        }
    }); L(a, "Core/Color/Palette.js", [], function () {
        return {
            colors: "#7cb5ec #434348 #90ed7d #f7a35c #8085e9 #f15c80 #e4d354 #2b908f #f45b5b #91e8e1".split(" "), backgroundColor: "#ffffff", neutralColor100: "#000000", neutralColor80: "#333333", neutralColor60: "#666666", neutralColor40: "#999999",
            neutralColor20: "#cccccc", neutralColor10: "#e6e6e6", neutralColor5: "#f2f2f2", neutralColor3: "#f7f7f7", highlightColor100: "#003399", highlightColor80: "#335cad", highlightColor60: "#6685c2", highlightColor20: "#ccd6eb", highlightColor10: "#e6ebf5", positiveColor: "#06b535", negativeColor: "#f21313"
        }
    }); L(a, "Core/Chart/ChartDefaults.js", [a["Core/Color/Palette.js"]], function (a) {
        return {
            panning: { enabled: !1, type: "x" }, styledMode: !1, borderRadius: 0, colorCount: 10, defaultSeriesType: "line", ignoreHiddenSeries: !0, spacing: [10, 10,
            15, 10], resetZoomButton: { theme: { zIndex: 6 }, position: { align: "right", x: -10, y: 10 } }, zoomBySingleTouch: !1, width: null, height: null, borderColor: a.highlightColor80, backgroundColor: a.backgroundColor, plotBorderColor: a.neutralColor20
        }
    }); L(a, "Core/Color/Color.js", [a["Core/Globals.js"], a["Core/Utilities.js"]], function (a, u) {
        var v = u.isNumber, H = u.merge, x = u.pInt; u = function () {
            function u(v) {
                this.parsers = [{
                    regex: /rgba\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]?(?:\.[0-9]+)?)\s*\)/, parse: function (q) {
                        return [x(q[1]),
                        x(q[2]), x(q[3]), parseFloat(q[4], 10)]
                    }
                }, { regex: /rgb\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*\)/, parse: function (q) { return [x(q[1]), x(q[2]), x(q[3]), 1] } }]; this.rgba = []; var B = a.Color; if (B && B !== u) return new B(v); if (!(this instanceof u)) return new u(v); this.init(v)
            } u.parse = function (a) { return new u(a) }; u.prototype.init = function (a) {
                var B, q; if ((this.input = a = u.names[a && a.toLowerCase ? a.toLowerCase() : ""] || a) && a.stops) this.stops = a.stops.map(function (c) { return new u(c[1]) }); else {
                    if (a && a.charAt &&
                    "#" === a.charAt()) { var h = a.length; a = parseInt(a.substr(1), 16); 7 === h ? B = [(a & 16711680) >> 16, (a & 65280) >> 8, a & 255, 1] : 4 === h && (B = [(a & 3840) >> 4 | (a & 3840) >> 8, (a & 240) >> 4 | a & 240, (a & 15) << 4 | a & 15, 1]) } if (!B) for (q = this.parsers.length; q-- && !B;) { var f = this.parsers[q]; (h = f.regex.exec(a)) && (B = f.parse(h)) }
                } this.rgba = B || []
            }; u.prototype.get = function (a) {
                var B = this.input, q = this.rgba; if ("undefined" !== typeof this.stops) { var h = H(B); h.stops = [].concat(h.stops); this.stops.forEach(function (f, c) { h.stops[c] = [h.stops[c][0], f.get(a)] }) } else h =
                q && v(q[0]) ? "rgb" === a || !a && 1 === q[3] ? "rgb(" + q[0] + "," + q[1] + "," + q[2] + ")" : "a" === a ? q[3] : "rgba(" + q.join(",") + ")" : B; return h
            }; u.prototype.brighten = function (a) { var B, q = this.rgba; if (this.stops) this.stops.forEach(function (h) { h.brighten(a) }); else if (v(a) && 0 !== a) for (B = 0; 3 > B; B++) q[B] += x(255 * a), 0 > q[B] && (q[B] = 0), 255 < q[B] && (q[B] = 255); return this }; u.prototype.setOpacity = function (a) { this.rgba[3] = a; return this }; u.prototype.tweenTo = function (a, B) {
                var q = this.rgba, h = a.rgba; h.length && q && q.length ? (a = 1 !== h[3] || 1 !== q[3], B = (a ?
                "rgba(" : "rgb(") + Math.round(h[0] + (q[0] - h[0]) * (1 - B)) + "," + Math.round(h[1] + (q[1] - h[1]) * (1 - B)) + "," + Math.round(h[2] + (q[2] - h[2]) * (1 - B)) + (a ? "," + (h[3] + (q[3] - h[3]) * (1 - B)) : "") + ")") : B = a.input || "none"; return B
            }; u.names = { white: "#ffffff", black: "#000000" }; return u
        }(); ""; return u
    }); L(a, "Core/Time.js", [a["Core/Globals.js"], a["Core/Utilities.js"]], function (a, u) {
        var v = a.win, H = u.defined, x = u.error, y = u.extend, G = u.isObject, B = u.merge, q = u.objectEach, h = u.pad, f = u.pick, c = u.splat, e = u.timeUnits, t = a.isSafari && Intl && Intl.DateTimeFormat.prototype.formatRange,
        m = a.isSafari && Intl && !Intl.DateTimeFormat.prototype.formatRange; u = function () {
            function w(c) { this.options = {}; this.variableTimezone = this.useUTC = !1; this.Date = v.Date; this.getTimezoneOffset = this.timezoneOffsetFunction(); this.update(c) } w.prototype.get = function (c, l) { if (this.variableTimezone || this.timezoneOffset) { var e = l.getTime(), n = e - this.getTimezoneOffset(l); l.setTime(n); c = l["getUTC" + c](); l.setTime(e); return c } return this.useUTC ? l["getUTC" + c]() : l["get" + c]() }; w.prototype.set = function (c, l, e) {
                if (this.variableTimezone ||
                this.timezoneOffset) { if ("Milliseconds" === c || "Seconds" === c || "Minutes" === c && 0 === this.getTimezoneOffset(l) % 36E5) return l["setUTC" + c](e); var n = this.getTimezoneOffset(l); n = l.getTime() - n; l.setTime(n); l["setUTC" + c](e); c = this.getTimezoneOffset(l); n = l.getTime() + c; return l.setTime(n) } return this.useUTC || t && "FullYear" === c ? l["setUTC" + c](e) : l["set" + c](e)
            }; w.prototype.update = function (c) {
                var l = f(c && c.useUTC, !0); this.options = c = B(!0, this.options || {}, c); this.Date = c.Date || v.Date || Date; this.timezoneOffset = (this.useUTC =
                l) && c.timezoneOffset; this.getTimezoneOffset = this.timezoneOffsetFunction(); this.variableTimezone = l && !(!c.getTimezoneOffset && !c.timezone)
            }; w.prototype.makeTime = function (c, l, e, h, t, p) { if (this.useUTC) { var d = this.Date.UTC.apply(0, arguments); var k = this.getTimezoneOffset(d); d += k; var b = this.getTimezoneOffset(d); k !== b ? d += b - k : k - 36E5 !== this.getTimezoneOffset(d - 36E5) || m || (d -= 36E5) } else d = (new this.Date(c, l, f(e, 1), f(h, 0), f(t, 0), f(p, 0))).getTime(); return d }; w.prototype.timezoneOffsetFunction = function () {
                var c = this,
                l = this.options, e = l.moment || v.moment; if (!this.useUTC) return function (l) { return 6E4 * (new Date(l.toString())).getTimezoneOffset() }; if (l.timezone) { if (e) return function (c) { return 6E4 * -e.tz(c, l.timezone).utcOffset() }; x(25) } return this.useUTC && l.getTimezoneOffset ? function (c) { return 6E4 * l.getTimezoneOffset(c.valueOf()) } : function () { return 6E4 * (c.timezoneOffset || 0) }
            }; w.prototype.dateFormat = function (c, l, e) {
                if (!H(l) || isNaN(l)) return a.defaultOptions.lang && a.defaultOptions.lang.invalidDate || ""; c = f(c, "%Y-%m-%d %H:%M:%S");
                var n = this, t = new this.Date(l), p = this.get("Hours", t), d = this.get("Day", t), k = this.get("Date", t), b = this.get("Month", t), g = this.get("FullYear", t), r = a.defaultOptions.lang, F = r && r.weekdays, D = r && r.shortWeekdays; t = y({ a: D ? D[d] : F[d].substr(0, 3), A: F[d], d: h(k), e: h(k, 2, " "), w: d, b: r.shortMonths[b], B: r.months[b], m: h(b + 1), o: b + 1, y: g.toString().substr(2, 2), Y: g, H: h(p), k: p, I: h(p % 12 || 12), l: p % 12 || 12, M: h(this.get("Minutes", t)), p: 12 > p ? "AM" : "PM", P: 12 > p ? "am" : "pm", S: h(t.getSeconds()), L: h(Math.floor(l % 1E3), 3) }, a.dateFormats); q(t,
                function (b, d) { for (; -1 !== c.indexOf("%" + d) ;) c = c.replace("%" + d, "function" === typeof b ? b.call(n, l) : b) }); return e ? c.substr(0, 1).toUpperCase() + c.substr(1) : c
            }; w.prototype.resolveDTLFormat = function (e) { return G(e, !0) ? e : (e = c(e), { main: e[0], from: e[1], to: e[2] }) }; w.prototype.getTimeTicks = function (c, l, t, h) {
                var n = this, p = [], d = {}, k = new n.Date(l), b = c.unitRange, g = c.count || 1, r; h = f(h, 1); if (H(l)) {
                    n.set("Milliseconds", k, b >= e.second ? 0 : g * Math.floor(n.get("Milliseconds", k) / g)); b >= e.second && n.set("Seconds", k, b >= e.minute ? 0 : g *
                    Math.floor(n.get("Seconds", k) / g)); b >= e.minute && n.set("Minutes", k, b >= e.hour ? 0 : g * Math.floor(n.get("Minutes", k) / g)); b >= e.hour && n.set("Hours", k, b >= e.day ? 0 : g * Math.floor(n.get("Hours", k) / g)); b >= e.day && n.set("Date", k, b >= e.month ? 1 : Math.max(1, g * Math.floor(n.get("Date", k) / g))); if (b >= e.month) { n.set("Month", k, b >= e.year ? 0 : g * Math.floor(n.get("Month", k) / g)); var F = n.get("FullYear", k) } b >= e.year && n.set("FullYear", k, F - F % g); b === e.week && (F = n.get("Day", k), n.set("Date", k, n.get("Date", k) - F + h + (F < h ? -7 : 0))); F = n.get("FullYear",
                    k); h = n.get("Month", k); var D = n.get("Date", k), m = n.get("Hours", k); l = k.getTime(); !n.variableTimezone && n.useUTC || !H(t) || (r = t - l > 4 * e.month || n.getTimezoneOffset(l) !== n.getTimezoneOffset(t)); l = k.getTime(); for (k = 1; l < t;) p.push(l), l = b === e.year ? n.makeTime(F + k * g, 0) : b === e.month ? n.makeTime(F, h + k * g) : !r || b !== e.day && b !== e.week ? r && b === e.hour && 1 < g ? n.makeTime(F, h, D, m + k * g) : l + b * g : n.makeTime(F, h, D + k * g * (b === e.day ? 1 : 7)), k++; p.push(l); b <= e.hour && 1E4 > p.length && p.forEach(function (b) {
                        0 === b % 18E5 && "000000000" === n.dateFormat("%H%M%S%L",
                        b) && (d[b] = "day")
                    })
                } p.info = y(c, { higherRanks: d, totalRange: b * g }); return p
            }; return w
        }(); ""; return u
    }); L(a, "Core/DefaultOptions.js", [a["Core/Globals.js"], a["Core/Chart/ChartDefaults.js"], a["Core/Color/Color.js"], a["Core/Color/Palette.js"], a["Core/Time.js"], a["Core/Utilities.js"]], function (a, u, E, H, x, y) {
        var v = a.isTouchDevice, B = a.svg; E = E.parse; var q = y.merge; ""; var h = {
            colors: H.colors, symbols: ["circle", "diamond", "square", "triangle", "triangle-down"], lang: {
                loading: "Loading...", months: "January February March April May June July August September October November December".split(" "),
                shortMonths: "Jan Feb Mar Apr May Jun Jul Aug Sep Oct Nov Dec".split(" "), weekdays: "Sunday Monday Tuesday Wednesday Thursday Friday Saturday".split(" "), decimalPoint: ".", numericSymbols: "kMGTPE".split(""), resetZoom: "Reset zoom", resetZoomTitle: "Reset zoom level 1:1", thousandsSep: " "
            }, global: {}, time: { Date: void 0, getTimezoneOffset: void 0, timezone: void 0, timezoneOffset: 0, useUTC: !0 }, chart: u, title: { text: "Chart title", align: "center", margin: 15, widthAdjust: -44 }, subtitle: { text: "", align: "center", widthAdjust: -44 },
            caption: { margin: 15, text: "", align: "left", verticalAlign: "bottom" }, plotOptions: {}, labels: { style: { position: "absolute", color: H.neutralColor80 } }, legend: {
                enabled: !0, align: "center", alignColumns: !0, className: "highcharts-no-tooltip", layout: "horizontal", labelFormatter: function () { return this.name }, borderColor: H.neutralColor40, borderRadius: 0, navigation: { activeColor: H.highlightColor100, inactiveColor: H.neutralColor20 }, itemStyle: { color: H.neutralColor80, cursor: "pointer", fontSize: "12px", fontWeight: "bold", textOverflow: "ellipsis" },
                itemHoverStyle: { color: H.neutralColor100 }, itemHiddenStyle: { color: H.neutralColor20 }, shadow: !1, itemCheckboxStyle: { position: "absolute", width: "13px", height: "13px" }, squareSymbol: !0, symbolPadding: 5, verticalAlign: "bottom", x: 0, y: 0, title: { style: { fontWeight: "bold" } }
            }, loading: { labelStyle: { fontWeight: "bold", position: "relative", top: "45%" }, style: { position: "absolute", backgroundColor: H.backgroundColor, opacity: .5, textAlign: "center" } }, tooltip: {
                enabled: !0, animation: B, borderRadius: 3, dateTimeLabelFormats: {
                    millisecond: "%A, %b %e, %H:%M:%S.%L",
                    second: "%A, %b %e, %H:%M:%S", minute: "%A, %b %e, %H:%M", hour: "%A, %b %e, %H:%M", day: "%A, %b %e, %Y", week: "Week from %A, %b %e, %Y", month: "%B %Y", year: "%Y"
                }, footerFormat: "", padding: 8, snap: v ? 25 : 10, headerFormat: '<span style="font-size: 10px">{point.key}</span><br/>', pointFormat: '<span style="color:{point.color}">\u25cf</span> {series.name}: <b>{point.y}</b><br/>', backgroundColor: E(H.neutralColor3).setOpacity(.85).get(), borderWidth: 1, shadow: !0, style: {
                    color: H.neutralColor80, cursor: "default", fontSize: "12px",
                    whiteSpace: "nowrap"
                }
            }, credits: { enabled: !0, href: "https://www.highcharts.com?credits", position: { align: "right", x: -10, verticalAlign: "bottom", y: -5 }, style: { cursor: "pointer", color: H.neutralColor40, fontSize: "9px" }, text: "Highcharts.com" }
        }; h.chart.styledMode = !1; ""; var f = new x(q(h.global, h.time)); return { defaultOptions: h, defaultTime: f, getOptions: function () { return h }, setOptions: function (c) { q(!0, h, c); if (c.time || c.global) a.time ? a.time.update(q(h.global, h.time, c.global, c.time)) : a.time = f; return h } }
    }); L(a, "Core/Animation/Fx.js",
    [a["Core/Color/Color.js"], a["Core/Globals.js"], a["Core/Utilities.js"]], function (a, u, E) {
        var v = a.parse, x = u.win, y = E.isNumber, G = E.objectEach; return function () {
            function a(a, h, f) { this.pos = NaN; this.options = h; this.elem = a; this.prop = f } a.prototype.dSetter = function () {
                var a = this.paths, h = a && a[0]; a = a && a[1]; var f = this.now || 0, c = []; if (1 !== f && h && a) if (h.length === a.length && 1 > f) for (var e = 0; e < a.length; e++) {
                    for (var t = h[e], m = a[e], w = [], n = 0; n < m.length; n++) {
                        var l = t[n], J = m[n]; y(l) && y(J) && ("A" !== m[0] || 4 !== n && 5 !== n) ? w[n] = l + f * (J -
                        l) : w[n] = J
                    } c.push(w)
                } else c = a; else c = this.toD || []; this.elem.attr("d", c, void 0, !0)
            }; a.prototype.update = function () { var a = this.elem, h = this.prop, f = this.now, c = this.options.step; if (this[h + "Setter"]) this[h + "Setter"](); else a.attr ? a.element && a.attr(h, f, null, !0) : a.style[h] = f + this.unit; c && c.call(a, f, this) }; a.prototype.run = function (q, h, f) {
                var c = this, e = c.options, t = function (e) { return t.stopped ? !1 : c.step(e) }, m = x.requestAnimationFrame || function (c) { setTimeout(c, 13) }, w = function () {
                    for (var c = 0; c < a.timers.length; c++) a.timers[c]() ||
                    a.timers.splice(c--, 1); a.timers.length && m(w)
                }; q !== h || this.elem["forceAnimate:" + this.prop] ? (this.startTime = +new Date, this.start = q, this.end = h, this.unit = f, this.now = this.start, this.pos = 0, t.elem = this.elem, t.prop = this.prop, t() && 1 === a.timers.push(t) && m(w)) : (delete e.curAnim[this.prop], e.complete && 0 === Object.keys(e.curAnim).length && e.complete.call(this.elem))
            }; a.prototype.step = function (a) {
                var h = +new Date, f = this.options, c = this.elem, e = f.complete, t = f.duration, m = f.curAnim; if (c.attr && !c.element) a = !1; else if (a ||
                h >= t + this.startTime) { this.now = this.end; this.pos = 1; this.update(); var w = m[this.prop] = !0; G(m, function (c) { !0 !== c && (w = !1) }); w && e && e.call(c); a = !1 } else this.pos = f.easing((h - this.startTime) / t), this.now = this.start + (this.end - this.start) * this.pos, this.update(), a = !0; return a
            }; a.prototype.initPath = function (a, h, f) {
                function c(c, l) { for (; c.length < K;) { var d = c[0], k = l[K - c.length]; k && "M" === d[0] && (c[0] = "C" === k[0] ? ["C", d[1], d[2], d[1], d[2], d[1], d[2]] : ["L", d[1], d[2]]); c.unshift(d); w && (d = c.pop(), c.push(c[c.length - 1], d)) } }
                function e(c, l) { for (; c.length < K;) if (l = c[Math.floor(c.length / n) - 1].slice(), "C" === l[0] && (l[1] = l[5], l[2] = l[6]), w) { var d = c[Math.floor(c.length / n)].slice(); c.splice(c.length / 2, 0, l, d) } else c.push(l) } var t = a.startX, m = a.endX; f = f.slice(); var w = a.isArea, n = w ? 2 : 1; h = h && h.slice(); if (!h) return [f, f]; if (t && m && m.length) {
                    for (a = 0; a < t.length; a++) if (t[a] === m[0]) { var l = a; break } else if (t[0] === m[m.length - t.length + a]) { l = a; var J = !0; break } else if (t[t.length - 1] === m[m.length - t.length + a]) { l = t.length - a; break } "undefined" === typeof l &&
                    (h = [])
                } if (h.length && y(l)) { var K = f.length + l * n; J ? (c(h, f), e(f, h)) : (c(f, h), e(h, f)) } return [h, f]
            }; a.prototype.fillSetter = function () { a.prototype.strokeSetter.apply(this, arguments) }; a.prototype.strokeSetter = function () { this.elem.attr(this.prop, v(this.start).tweenTo(v(this.end), this.pos), null, !0) }; a.timers = []; return a
        }()
    }); L(a, "Core/Animation/AnimationUtilities.js", [a["Core/Animation/Fx.js"], a["Core/Utilities.js"]], function (a, u) {
        function v(c) { return q(c) ? h({ duration: 500, defer: 0 }, c) : { duration: c ? 500 : 0, defer: 0 } }
        function H(c, f) { for (var e = a.timers.length; e--;) a.timers[e].elem !== c || f && f !== a.timers[e].prop || (a.timers[e].stopped = !0) } var x = u.defined, y = u.getStyle, G = u.isArray, B = u.isNumber, q = u.isObject, h = u.merge, f = u.objectEach, c = u.pick; return {
            animate: function (c, t, m) {
                var e, n = "", l, J; if (!q(m)) { var K = arguments; m = { duration: K[2], easing: K[3], complete: K[4] } } B(m.duration) || (m.duration = 400); m.easing = "function" === typeof m.easing ? m.easing : Math[m.easing] || Math.easeInOutSine; m.curAnim = h(t); f(t, function (f, p) {
                    H(c, p); J = new a(c, m,
                    p); l = void 0; "d" === p && G(t.d) ? (J.paths = J.initPath(c, c.pathArray, t.d), J.toD = t.d, e = 0, l = 1) : c.attr ? e = c.attr(p) : (e = parseFloat(y(c, p)) || 0, "opacity" !== p && (n = "px")); l || (l = f); "string" === typeof l && l.match("px") && (l = l.replace(/px/g, "")); J.run(e, l, n)
                })
            }, animObject: v, getDeferredAnimation: function (c, a, f) {
                var e = v(a), n = 0, l = 0; (f ? [f] : c.series).forEach(function (c) { c = v(c.options.animation); n = a && x(a.defer) ? e.defer : Math.max(n, c.duration + c.defer); l = Math.min(e.duration, c.duration) }); c.renderer.forExport && (n = 0); return {
                    defer: Math.max(0,
                    n - l), duration: Math.min(n, l)
                }
            }, setAnimation: function (e, a) { a.renderer.globalAnimation = c(e, a.options.chart.animation, !0) }, stop: H
        }
    }); L(a, "Core/Renderer/HTML/AST.js", [a["Core/Globals.js"], a["Core/Utilities.js"]], function (a, u) {
        var v = a.SVG_NS, H = u.attr, x = u.createElement, y = u.discardElement, G = u.error, B = u.isString, q = u.objectEach, h = u.splat; try { var f = !!(new DOMParser).parseFromString("", "text/html") } catch (c) { f = !1 } u = function () {
            function c(c) { this.nodes = "string" === typeof c ? this.parseMarkup(c) : c } c.filterUserAttributes =
            function (e) { q(e, function (a, f) { var h = !0; -1 === c.allowedAttributes.indexOf(f) && (h = !1); -1 !== ["background", "dynsrc", "href", "lowsrc", "src"].indexOf(f) && (h = B(a) && c.allowedReferences.some(function (c) { return 0 === a.indexOf(c) })); h || (G("Highcharts warning: Invalid attribute '" + f + "' in config"), delete e[f]) }); return e }; c.setElementHTML = function (e, a) { e.innerHTML = ""; a && (new c(a)).addToDOM(e) }; c.prototype.addToDOM = function (e) {
                function f(e, t) {
                    var n; h(e).forEach(function (l) {
                        var e = l.tagName, h = l.textContent ? a.doc.createTextNode(l.textContent) :
                        void 0; if (e) if ("#text" === e) var m = h; else if (-1 !== c.allowedTags.indexOf(e)) { e = a.doc.createElementNS("svg" === e ? v : t.namespaceURI || v, e); var p = l.attributes || {}; q(l, function (d, k) { "tagName" !== k && "attributes" !== k && "children" !== k && "textContent" !== k && (p[k] = d) }); H(e, c.filterUserAttributes(p)); h && e.appendChild(h); f(l.children || [], e); m = e } else G("Highcharts warning: Invalid tagName '" + e + "' in config"); m && t.appendChild(m); n = m
                    }); return n
                } return f(this.nodes, e)
            }; c.prototype.parseMarkup = function (c) {
                var e = []; if (f) c =
                (new DOMParser).parseFromString(c, "text/html"); else { var a = x("div"); a.innerHTML = c; c = { body: a } } var h = function (c, l) { var e = c.nodeName.toLowerCase(), a = { tagName: e }; if ("#text" === e) { e = c.textContent || ""; if (/^[\s]*$/.test(e)) return; a.textContent = e } if (e = c.attributes) { var f = {};[].forEach.call(e, function (d) { f[d.name] = d.value }); a.attributes = f } if (c.childNodes.length) { var p = [];[].forEach.call(c.childNodes, function (d) { h(d, p) }); p.length && (a.children = p) } l.push(a) };[].forEach.call(c.body.childNodes, function (c) {
                    return h(c,
                    e)
                }); a && y(a); return e
            }; c.allowedTags = "a b br button caption circle clipPath code dd defs div dl dt em feComponentTransfer feFuncA feFuncB feFuncG feFuncR feGaussianBlur feOffset feMerge feMergeNode filter h1 h2 h3 h4 h5 h6 hr i img li linearGradient marker ol p path pattern pre rect small span stop strong style sub sup svg table text thead tbody tspan td th tr u ul #text".split(" "); c.allowedAttributes = "aria-controls aria-describedby aria-expanded aria-haspopup aria-hidden aria-label aria-labelledby aria-live aria-pressed aria-readonly aria-roledescription aria-selected class clip-path color colspan cx cy d dx dy disabled fill height href id in markerHeight markerWidth offset opacity orient padding paddingLeft paddingRight patternUnits r refX refY role scope slope src startOffset stdDeviation stroke stroke-linecap stroke-width style tableValues result rowspan summary target tabindex text-align textAnchor textLength type valign width x x1 x2 y y1 y2 zIndex".split(" ");
            c.allowedReferences = "https:// http:// mailto: / ../ ./ #".split(" "); return c
        }(); ""; return u
    }); L(a, "Core/FormatUtilities.js", [a["Core/DefaultOptions.js"], a["Core/Utilities.js"]], function (a, u) {
        function v(a, f, c, e) {
            a = +a || 0; f = +f; var h = H.lang, m = (a.toString().split(".")[1] || "").split("e")[0].length, w = a.toString().split("e"), n = f; if (-1 === f) f = Math.min(m, 20); else if (!G(f)) f = 2; else if (f && w[1] && 0 > w[1]) {
                var l = f + +w[1]; 0 <= l ? (w[0] = (+w[0]).toExponential(l).split("e")[0], f = l) : (w[0] = w[0].split(".")[0] || 0, a = 20 > f ? (w[0] *
                Math.pow(10, w[1])).toFixed(f) : 0, w[1] = 0)
            } l = (Math.abs(w[1] ? w[0] : a) + Math.pow(10, -Math.max(f, m) - 1)).toFixed(f); m = String(q(l)); var J = 3 < m.length ? m.length % 3 : 0; c = B(c, h.decimalPoint); e = B(e, h.thousandsSep); a = (0 > a ? "-" : "") + (J ? m.substr(0, J) + e : ""); a = 0 > +w[1] && !n ? "0" : a + m.substr(J).replace(/(\d{3})(?=\d)/g, "$1" + e); f && (a += c + l.slice(-f)); w[1] && 0 !== +a && (a += "e" + w[1]); return a
        } var H = a.defaultOptions, x = a.defaultTime, y = u.getNestedProperty, G = u.isNumber, B = u.pick, q = u.pInt; return {
            dateFormat: function (a, f, c) {
                return x.dateFormat(a,
                f, c)
            }, format: function (a, f, c) { var e = "{", h = !1, m = /f$/, w = /\.([0-9])/, n = H.lang, l = c && c.time || x; c = c && c.numberFormatter || v; for (var J = []; a;) { var K = a.indexOf(e); if (-1 === K) break; var A = a.slice(0, K); if (h) { A = A.split(":"); e = y(A.shift() || "", f); if (A.length && "number" === typeof e) if (A = A.join(":"), m.test(A)) { var p = parseInt((A.match(w) || ["", "-1"])[1], 10); null !== e && (e = c(e, p, n.decimalPoint, -1 < A.indexOf(",") ? n.thousandsSep : "")) } else e = l.dateFormat(A, e); J.push(e) } else J.push(A); a = a.slice(K + 1); e = (h = !h) ? "}" : "{" } J.push(a); return J.join("") },
            numberFormat: v
        }
    }); L(a, "Core/Renderer/SVG/SVGElement.js", [a["Core/Animation/AnimationUtilities.js"], a["Core/Renderer/HTML/AST.js"], a["Core/Color/Color.js"], a["Core/Globals.js"], a["Core/Color/Palette.js"], a["Core/Utilities.js"]], function (a, u, E, H, x, y) {
        var v = a.animate, B = a.animObject, q = a.stop, h = H.deg2rad, f = H.doc, c = H.noop, e = H.svg, t = H.SVG_NS, m = H.win, w = y.addEvent, n = y.attr, l = y.createElement, J = y.css, K = y.defined, A = y.erase, p = y.extend, d = y.fireEvent, k = y.isArray, b = y.isFunction, g = y.isNumber, r = y.isString, F = y.merge,
        D = y.objectEach, M = y.pick, C = y.pInt, O = y.syncTimeout, S = y.uniqueKey; a = function () {
            function a() { this.element = void 0; this.onEvents = {}; this.opacity = 1; this.renderer = void 0; this.SVG_NS = t; this.symbolCustomAttribs = "x y width height r start end innerR anchorX anchorY rounded".split(" ") } a.prototype._defaultGetter = function (b) { b = M(this[b + "Value"], this[b], this.element ? this.element.getAttribute(b) : null, 0); /^[\-0-9\.]+$/.test(b) && (b = parseFloat(b)); return b }; a.prototype._defaultSetter = function (b, d, g) {
                g.setAttribute(d,
                b)
            }; a.prototype.add = function (b) { var d = this.renderer, g = this.element; b && (this.parentGroup = b); this.parentInverted = b && b.inverted; "undefined" !== typeof this.textStr && "text" === this.element.nodeName && d.buildText(this); this.added = !0; if (!b || b.handleZ || this.zIndex) var k = this.zIndexSetter(); k || (b ? b.element : d.box).appendChild(g); if (this.onAdd) this.onAdd(); return this }; a.prototype.addClass = function (b, d) {
                var g = d ? "" : this.attr("class") || ""; b = (b || "").split(/ /g).reduce(function (b, d) { -1 === g.indexOf(d) && b.push(d); return b },
                g ? [g] : []).join(" "); b !== g && this.attr("class", b); return this
            }; a.prototype.afterSetters = function () { this.doTransform && (this.updateTransform(), this.doTransform = !1) }; a.prototype.align = function (b, d, g) {
                var z = {}, k = this.renderer, c = k.alignedObjects, a, I, l; if (b) { if (this.alignOptions = b, this.alignByTranslate = d, !g || r(g)) this.alignTo = a = g || "renderer", A(c, this), c.push(this), g = void 0 } else b = this.alignOptions, d = this.alignByTranslate, a = this.alignTo; g = M(g, k[a], "scrollablePlotBox" === a ? k.plotBox : void 0, k); a = b.align; var e =
                b.verticalAlign; k = (g.x || 0) + (b.x || 0); c = (g.y || 0) + (b.y || 0); "right" === a ? I = 1 : "center" === a && (I = 2); I && (k += (g.width - (b.width || 0)) / I); z[d ? "translateX" : "x"] = Math.round(k); "bottom" === e ? l = 1 : "middle" === e && (l = 2); l && (c += (g.height - (b.height || 0)) / l); z[d ? "translateY" : "y"] = Math.round(c); this[this.placed ? "animate" : "attr"](z); this.placed = !0; this.alignAttr = z; return this
            }; a.prototype.alignSetter = function (b) {
                var d = { left: "start", center: "middle", right: "end" }; d[b] && (this.alignValue = b, this.element.setAttribute("text-anchor",
                d[b]))
            }; a.prototype.animate = function (b, d, g) { var k = this, z = B(M(d, this.renderer.globalAnimation, !0)); d = z.defer; M(f.hidden, f.msHidden, f.webkitHidden, !1) && (z.duration = 0); 0 !== z.duration ? (g && (z.complete = g), O(function () { k.element && v(k, b, z) }, d)) : (this.attr(b, void 0, g), D(b, function (b, d) { z.step && z.step.call(this, b, { prop: d, pos: 1, elem: this }) }, this)); return this }; a.prototype.applyTextOutline = function (b) {
                var d = this.element; -1 !== b.indexOf("contrast") && (b = b.replace(/contrast/g, this.renderer.getContrast(d.style.fill)));
                var g = b.split(" "); b = g[g.length - 1]; if ((g = g[0]) && "none" !== g && H.svg) {
                    this.fakeTS = !0; this.ySetter = this.xSetter; g = g.replace(/(^[\d\.]+)(.*?)$/g, function (b, d, g) { return 2 * Number(d) + g }); this.removeTextOutline(); var k = f.createElementNS(t, "tspan"); n(k, { "class": "highcharts-text-outline", fill: b, stroke: b, "stroke-width": g, "stroke-linejoin": "round" });[].forEach.call(d.childNodes, function (b) {
                        var d = b.cloneNode(!0); d.removeAttribute && ["fill", "stroke", "stroke-width", "stroke"].forEach(function (b) { return d.removeAttribute(b) });
                        k.appendChild(d)
                    }); var c = f.createElementNS(t, "tspan"); c.textContent = "\u200b";["x", "y"].forEach(function (b) { var g = d.getAttribute(b); g && c.setAttribute(b, g) }); k.appendChild(c); d.insertBefore(k, d.firstChild)
                }
            }; a.prototype.attr = function (b, d, g, k) {
                var z = this.element, Q = this.symbolCustomAttribs, c, I = this, r, a; if ("string" === typeof b && "undefined" !== typeof d) { var l = b; b = {}; b[l] = d } "string" === typeof b ? I = (this[b + "Getter"] || this._defaultGetter).call(this, b, z) : (D(b, function (d, g) {
                    r = !1; k || q(this, g); this.symbolName && -1 !==
                    Q.indexOf(g) && (c || (this.symbolAttr(b), c = !0), r = !0); !this.rotation || "x" !== g && "y" !== g || (this.doTransform = !0); r || (a = this[g + "Setter"] || this._defaultSetter, a.call(this, d, g, z), !this.styledMode && this.shadows && /^(width|height|visibility|x|y|d|transform|cx|cy|r)$/.test(g) && this.updateShadows(g, d, a))
                }, this), this.afterSetters()); g && g.call(this); return I
            }; a.prototype.clip = function (b) { return this.attr("clip-path", b ? "url(" + this.renderer.url + "#" + b.id + ")" : "none") }; a.prototype.crisp = function (b, d) {
                d = d || b.strokeWidth ||
                0; var g = Math.round(d) % 2 / 2; b.x = Math.floor(b.x || this.x || 0) + g; b.y = Math.floor(b.y || this.y || 0) + g; b.width = Math.floor((b.width || this.width || 0) - 2 * g); b.height = Math.floor((b.height || this.height || 0) - 2 * g); K(b.strokeWidth) && (b.strokeWidth = d); return b
            }; a.prototype.complexColor = function (b, g, z) {
                var Q = this.renderer, c, r, a, I, l, e, f, p, n, C, h = [], t; d(this.renderer, "complexColor", { args: arguments }, function () {
                    b.radialGradient ? r = "radialGradient" : b.linearGradient && (r = "linearGradient"); if (r) {
                        a = b[r]; l = Q.gradients; e = b.stops; n = z.radialReference;
                        k(a) && (b[r] = a = { x1: a[0], y1: a[1], x2: a[2], y2: a[3], gradientUnits: "userSpaceOnUse" }); "radialGradient" === r && n && !K(a.gradientUnits) && (I = a, a = F(a, Q.getRadialAttr(n, I), { gradientUnits: "userSpaceOnUse" })); D(a, function (b, d) { "id" !== d && h.push(d, b) }); D(e, function (b) { h.push(b) }); h = h.join(","); if (l[h]) C = l[h].attr("id"); else {
                            a.id = C = S(); var d = l[h] = Q.createElement(r).attr(a).add(Q.defs); d.radAttr = I; d.stops = []; e.forEach(function (b) {
                                0 === b[1].indexOf("rgba") ? (c = E.parse(b[1]), f = c.get("rgb"), p = c.get("a")) : (f = b[1], p = 1); b = Q.createElement("stop").attr({
                                    offset: b[0],
                                    "stop-color": f, "stop-opacity": p
                                }).add(d); d.stops.push(b)
                            })
                        } t = "url(" + Q.url + "#" + C + ")"; z.setAttribute(g, t); z.gradient = h; b.toString = function () { return t }
                    }
                })
            }; a.prototype.css = function (b) {
                var d = this.styles, g = {}, k = this.element, c = ["textOutline", "textOverflow", "width"], r = "", a = !d; b && b.color && (b.fill = b.color); d && D(b, function (b, k) { d && d[k] !== b && (g[k] = b, a = !0) }); if (a) {
                    d && (b = p(d, g)); if (b) if (null === b.width || "auto" === b.width) delete this.textWidth; else if ("text" === k.nodeName.toLowerCase() && b.width) var I = this.textWidth =
                    C(b.width); this.styles = b; I && !e && this.renderer.forExport && delete b.width; if (k.namespaceURI === this.SVG_NS) { var l = function (b, d) { return "-" + d.toLowerCase() }; D(b, function (b, d) { -1 === c.indexOf(d) && (r += d.replace(/([A-Z])/g, l) + ":" + b + ";") }); r && n(k, "style", r) } else J(k, b); this.added && ("text" === this.element.nodeName && this.renderer.buildText(this), b && b.textOutline && this.applyTextOutline(b.textOutline))
                } return this
            }; a.prototype.dashstyleSetter = function (b) {
                var d = this["stroke-width"]; "inherit" === d && (d = 1); if (b = b && b.toLowerCase()) {
                    var g =
                    b.replace("shortdashdotdot", "3,1,1,1,1,1,").replace("shortdashdot", "3,1,1,1").replace("shortdot", "1,1,").replace("shortdash", "3,1,").replace("longdash", "8,3,").replace(/dot/g, "1,3,").replace("dash", "4,3,").replace(/,$/, "").split(","); for (b = g.length; b--;) g[b] = "" + C(g[b]) * M(d, NaN); b = g.join(",").replace(/NaN/g, "none"); this.element.setAttribute("stroke-dasharray", b)
                }
            }; a.prototype.destroy = function () {
                var b = this, d = b.element || {}, g = b.renderer, k = d.ownerSVGElement, c = g.isSVG && "SPAN" === d.nodeName && b.parentGroup ||
                void 0; d.onclick = d.onmouseout = d.onmouseover = d.onmousemove = d.point = null; q(b); if (b.clipPath && k) { var r = b.clipPath;[].forEach.call(k.querySelectorAll("[clip-path],[CLIP-PATH]"), function (b) { -1 < b.getAttribute("clip-path").indexOf(r.element.id) && b.removeAttribute("clip-path") }); b.clipPath = r.destroy() } if (b.stops) { for (k = 0; k < b.stops.length; k++) b.stops[k].destroy(); b.stops.length = 0; b.stops = void 0 } b.safeRemoveChild(d); for (g.styledMode || b.destroyShadows() ; c && c.div && 0 === c.div.childNodes.length;) d = c.parentGroup,
                b.safeRemoveChild(c.div), delete c.div, c = d; b.alignTo && A(g.alignedObjects, b); D(b, function (d, g) { b[g] && b[g].parentGroup === b && b[g].destroy && b[g].destroy(); delete b[g] })
            }; a.prototype.destroyShadows = function () { (this.shadows || []).forEach(function (b) { this.safeRemoveChild(b) }, this); this.shadows = void 0 }; a.prototype.destroyTextPath = function (b, d) {
                var g = b.getElementsByTagName("text")[0]; if (g) {
                    if (g.removeAttribute("dx"), g.removeAttribute("dy"), d.element.setAttribute("id", ""), this.textPathWrapper && g.getElementsByTagName("textPath").length) {
                        for (b =
                        this.textPathWrapper.element.childNodes; b.length;) g.appendChild(b[0]); g.removeChild(this.textPathWrapper.element)
                    }
                } else if (b.getAttribute("dx") || b.getAttribute("dy")) b.removeAttribute("dx"), b.removeAttribute("dy"); this.textPathWrapper && (this.textPathWrapper = this.textPathWrapper.destroy())
            }; a.prototype.dSetter = function (b, d, g) {
                k(b) && ("string" === typeof b[0] && (b = this.renderer.pathToSegments(b)), this.pathArray = b, b = b.reduce(function (b, d, g) { return d && d.join ? (g ? b + " " : "") + d.join(" ") : (d || "").toString() }, ""));
                /(NaN| {2}|^$)/.test(b) && (b = "M 0 0"); this[d] !== b && (g.setAttribute(d, b), this[d] = b)
            }; a.prototype.fadeOut = function (b) { var d = this; d.animate({ opacity: 0 }, { duration: M(b, 150), complete: function () { d.attr({ y: -9999 }).hide() } }) }; a.prototype.fillSetter = function (b, d, g) { "string" === typeof b ? g.setAttribute(d, b) : b && this.complexColor(b, d, g) }; a.prototype.getBBox = function (d, g) {
                var k = this.renderer, c = this.element, r = this.styles, l = this.textStr, e = k.cache, I = k.cacheKeys, F = c.namespaceURI === this.SVG_NS; g = M(g, this.rotation, 0); var P =
                k.styledMode ? c && a.prototype.getStyle.call(c, "font-size") : r && r.fontSize, f; if (K(l)) { var n = l.toString(); -1 === n.indexOf("<") && (n = n.replace(/[0-9]/g, "0")); n += ["", g, P, this.textWidth, r && r.textOverflow, r && r.fontWeight].join() } n && !d && (f = e[n]); if (!f) {
                    if (F || k.forExport) {
                        try { var D = this.fakeTS && function (b) { var d = c.querySelector(".highcharts-text-outline"); d && J(d, { display: b }) }; b(D) && D("none"); f = c.getBBox ? p({}, c.getBBox()) : { width: c.offsetWidth, height: c.offsetHeight }; b(D) && D("") } catch (V) { "" } if (!f || 0 > f.width) f = {
                            width: 0,
                            height: 0
                        }
                    } else f = this.htmlGetBBox(); k.isSVG && (d = f.width, k = f.height, F && (f.height = k = { "11px,17": 14, "13px,20": 16 }[r && r.fontSize + "," + Math.round(k)] || k), g && (r = g * h, f.width = Math.abs(k * Math.sin(r)) + Math.abs(d * Math.cos(r)), f.height = Math.abs(k * Math.cos(r)) + Math.abs(d * Math.sin(r)))); if (n && 0 < f.height) { for (; 250 < I.length;) delete e[I.shift()]; e[n] || I.push(n); e[n] = f }
                } return f
            }; a.prototype.getStyle = function (b) { return m.getComputedStyle(this.element || this, "").getPropertyValue(b) }; a.prototype.hasClass = function (b) {
                return -1 !==
                ("" + this.attr("class")).split(" ").indexOf(b)
            }; a.prototype.hide = function (b) { b ? this.attr({ y: -9999 }) : this.attr({ visibility: "hidden" }); return this }; a.prototype.htmlGetBBox = function () { return { height: 0, width: 0, x: 0, y: 0 } }; a.prototype.init = function (b, g) { this.element = "span" === g ? l(g) : f.createElementNS(this.SVG_NS, g); this.renderer = b; d(this, "afterInit") }; a.prototype.invert = function (b) { this.inverted = b; this.updateTransform(); return this }; a.prototype.on = function (b, d) {
                var g = this.onEvents; if (g[b]) g[b](); g[b] = w(this.element,
                b, d); return this
            }; a.prototype.opacitySetter = function (b, d, g) { this.opacity = b = Number(Number(b).toFixed(3)); g.setAttribute(d, b) }; a.prototype.removeClass = function (b) { return this.attr("class", ("" + this.attr("class")).replace(r(b) ? new RegExp("(^| )" + b + "( |$)") : b, " ").replace(/ +/g, " ").trim()) }; a.prototype.removeTextOutline = function () { var b = this.element.querySelector("tspan.highcharts-text-outline"); b && this.safeRemoveChild(b) }; a.prototype.safeRemoveChild = function (b) { var d = b.parentNode; d && d.removeChild(b) };
            a.prototype.setRadialReference = function (b) { var d = this.element.gradient && this.renderer.gradients[this.element.gradient]; this.element.radialReference = b; d && d.radAttr && d.animate(this.renderer.getRadialAttr(b, d.radAttr)); return this }; a.prototype.setTextPath = function (b, d) {
                var k = this.element, Q = this.text ? this.text.element : k, r = { textAnchor: "text-anchor" }, a = !1, l = this.textPathWrapper, I = !l; d = F(!0, { enabled: !0, attributes: { dy: -5, startOffset: "50%", textAnchor: "middle" } }, d); var e = u.filterUserAttributes(d.attributes);
                if (b && d && d.enabled) {
                    l && null === l.element.parentNode ? (I = !0, l = l.destroy()) : l && this.removeTextOutline.call(l.parentGroup); this.options && this.options.padding && (e.dx = -this.options.padding); l || (this.textPathWrapper = l = this.renderer.createElement("textPath"), a = !0); var P = l.element; (d = b.element.getAttribute("id")) || b.element.setAttribute("id", d = S()); if (I) for (Q.setAttribute("y", 0), g(e.dx) && Q.setAttribute("x", -e.dx), b = [].slice.call(Q.childNodes), I = 0; I < b.length; I++) {
                        var f = b[I]; f.nodeType !== Node.TEXT_NODE && "tspan" !==
                        f.nodeName || P.appendChild(f)
                    } a && l && l.add({ element: Q }); P.setAttributeNS("http://www.w3.org/1999/xlink", "href", this.renderer.url + "#" + d); K(e.dy) && (P.parentNode.setAttribute("dy", e.dy), delete e.dy); K(e.dx) && (P.parentNode.setAttribute("dx", e.dx), delete e.dx); D(e, function (b, d) { P.setAttribute(r[d] || d, b) }); k.removeAttribute("transform"); this.removeTextOutline.call(l); this.text && !this.renderer.styledMode && this.attr({ fill: "none", "stroke-width": 0 }); this.applyTextOutline = this.updateTransform = c
                } else l && (delete this.updateTransform,
                delete this.applyTextOutline, this.destroyTextPath(k, b), this.updateTransform(), this.options && this.options.rotation && this.applyTextOutline(this.options.style.textOutline)); return this
            }; a.prototype.shadow = function (b, d, g) {
                var k = [], c = this.element, z = this.oldShadowOptions, r = { color: x.neutralColor100, offsetX: this.parentInverted ? -1 : 1, offsetY: this.parentInverted ? -1 : 1, opacity: .15, width: 3 }, a = !1, l; !0 === b ? l = r : "object" === typeof b && (l = p(r, b)); l && (l && z && D(l, function (b, d) { b !== z[d] && (a = !0) }), a && this.destroyShadows(),
                this.oldShadowOptions = l); if (!l) this.destroyShadows(); else if (!this.shadows) {
                    var e = l.opacity / l.width; var F = this.parentInverted ? "translate(" + l.offsetY + ", " + l.offsetX + ")" : "translate(" + l.offsetX + ", " + l.offsetY + ")"; for (r = 1; r <= l.width; r++) {
                        var f = c.cloneNode(!1); var C = 2 * l.width + 1 - 2 * r; n(f, { stroke: b.color || x.neutralColor100, "stroke-opacity": e * r, "stroke-width": C, transform: F, fill: "none" }); f.setAttribute("class", (f.getAttribute("class") || "") + " highcharts-shadow"); g && (n(f, "height", Math.max(n(f, "height") - C, 0)),
                        f.cutHeight = C); d ? d.element.appendChild(f) : c.parentNode && c.parentNode.insertBefore(f, c); k.push(f)
                    } this.shadows = k
                } return this
            }; a.prototype.show = function (b) { return this.attr({ visibility: b ? "inherit" : "visible" }) }; a.prototype.strokeSetter = function (b, d, g) {
                this[d] = b; this.stroke && this["stroke-width"] ? (a.prototype.fillSetter.call(this, this.stroke, "stroke", g), g.setAttribute("stroke-width", this["stroke-width"]), this.hasStroke = !0) : "stroke-width" === d && 0 === b && this.hasStroke ? (g.removeAttribute("stroke"), this.hasStroke =
                !1) : this.renderer.styledMode && this["stroke-width"] && (g.setAttribute("stroke-width", this["stroke-width"]), this.hasStroke = !0)
            }; a.prototype.strokeWidth = function () { if (!this.renderer.styledMode) return this["stroke-width"] || 0; var b = this.getStyle("stroke-width"), d = 0; if (b.indexOf("px") === b.length - 2) d = C(b); else if ("" !== b) { var g = f.createElementNS(t, "rect"); n(g, { width: b, "stroke-width": 0 }); this.element.parentNode.appendChild(g); d = g.getBBox().width; g.parentNode.removeChild(g) } return d }; a.prototype.symbolAttr =
            function (b) { var d = this; "x y r start end width height innerR anchorX anchorY clockwise".split(" ").forEach(function (g) { d[g] = M(b[g], d[g]) }); d.attr({ d: d.renderer.symbols[d.symbolName](d.x, d.y, d.width, d.height, d) }) }; a.prototype.textSetter = function (b) { b !== this.textStr && (delete this.textPxLength, this.textStr = b, this.added && this.renderer.buildText(this)) }; a.prototype.titleSetter = function (b) {
                var d = this.element, g = d.getElementsByTagName("title")[0] || f.createElementNS(this.SVG_NS, "title"); d.insertBefore ? d.insertBefore(g,
                d.firstChild) : d.appendChild(g); g.textContent = String(M(b, "")).replace(/<[^>]*>/g, "").replace(/&lt;/g, "<").replace(/&gt;/g, ">")
            }; a.prototype.toFront = function () { var b = this.element; b.parentNode.appendChild(b); return this }; a.prototype.translate = function (b, d) { return this.attr({ translateX: b, translateY: d }) }; a.prototype.updateShadows = function (b, d, g) { var k = this.shadows; if (k) for (var c = k.length; c--;) g.call(k[c], "height" === b ? Math.max(d - (k[c].cutHeight || 0), 0) : "d" === b ? this.d : d, b, k[c]) }; a.prototype.updateTransform =
            function () {
                var b = this.scaleX, d = this.scaleY, g = this.inverted, k = this.rotation, c = this.matrix, r = this.element, a = this.translateX || 0, l = this.translateY || 0; g && (a += this.width, l += this.height); a = ["translate(" + a + "," + l + ")"]; K(c) && a.push("matrix(" + c.join(",") + ")"); g ? a.push("rotate(90) scale(-1,1)") : k && a.push("rotate(" + k + " " + M(this.rotationOriginX, r.getAttribute("x"), 0) + " " + M(this.rotationOriginY, r.getAttribute("y") || 0) + ")"); (K(b) || K(d)) && a.push("scale(" + M(b, 1) + " " + M(d, 1) + ")"); a.length && r.setAttribute("transform",
                a.join(" "))
            }; a.prototype.visibilitySetter = function (b, d, g) { "inherit" === b ? g.removeAttribute(d) : this[d] !== b && g.setAttribute(d, b); this[d] = b }; a.prototype.xGetter = function (b) { "circle" === this.element.nodeName && ("x" === b ? b = "cx" : "y" === b && (b = "cy")); return this._defaultGetter(b) }; a.prototype.zIndexSetter = function (b, d) {
                var g = this.renderer, k = this.parentGroup, c = (k || g).element || g.box, r = this.element; g = c === g.box; var a = !1; var l = this.added; var e; K(b) ? (r.setAttribute("data-z-index", b), b = +b, this[d] === b && (l = !1)) : K(this[d]) &&
                r.removeAttribute("data-z-index"); this[d] = b; if (l) { (b = this.zIndex) && k && (k.handleZ = !0); d = c.childNodes; for (e = d.length - 1; 0 <= e && !a; e--) { k = d[e]; l = k.getAttribute("data-z-index"); var f = !K(l); if (k !== r) if (0 > b && f && !g && !e) c.insertBefore(r, d[e]), a = !0; else if (C(l) <= b || f && (!K(b) || 0 <= b)) c.insertBefore(r, d[e + 1] || null), a = !0 } a || (c.insertBefore(r, d[g ? 3 : 0] || null), a = !0) } return a
            }; return a
        }(); a.prototype["stroke-widthSetter"] = a.prototype.strokeSetter; a.prototype.yGetter = a.prototype.xGetter; a.prototype.matrixSetter = a.prototype.rotationOriginXSetter =
        a.prototype.rotationOriginYSetter = a.prototype.rotationSetter = a.prototype.scaleXSetter = a.prototype.scaleYSetter = a.prototype.translateXSetter = a.prototype.translateYSetter = a.prototype.verticalAlignSetter = function (b, d) { this[d] = b; this.doTransform = !0 }; ""; return a
    }); L(a, "Core/Renderer/RendererRegistry.js", [a["Core/Globals.js"]], function (a) {
        var v; (function (v) {
            var u; v.rendererTypes = {}; v.getRendererType = function (a) { void 0 === a && (a = u); return v.rendererTypes[a] || v.rendererTypes[u] }; v.registerRendererType = function (x,
            y, G) { v.rendererTypes[x] = y; if (!u || G) u = x, a.Renderer = y }
        })(v || (v = {})); return v
    }); L(a, "Core/Renderer/SVG/SVGLabel.js", [a["Core/Renderer/SVG/SVGElement.js"], a["Core/Utilities.js"]], function (a, u) {
        var v = this && this.__extends || function () {
            var a = function (f, c) { a = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (c, a) { c.__proto__ = a } || function (c, a) { for (var e in a) a.hasOwnProperty(e) && (c[e] = a[e]) }; return a(f, c) }; return function (f, c) {
                function e() { this.constructor = f } a(f, c); f.prototype = null === c ? Object.create(c) :
                (e.prototype = c.prototype, new e)
            }
        }(), H = u.defined, x = u.extend, y = u.isNumber, G = u.merge, B = u.pick, q = u.removeEvent; return function (h) {
            function f(c, a, t, m, w, n, l, J, K, A) {
                var e = h.call(this) || this; e.paddingLeftSetter = e.paddingSetter; e.paddingRightSetter = e.paddingSetter; e.init(c, "g"); e.textStr = a; e.x = t; e.y = m; e.anchorX = n; e.anchorY = l; e.baseline = K; e.className = A; e.addClass("button" === A ? "highcharts-no-tooltip" : "highcharts-label"); A && e.addClass("highcharts-" + A); e.text = c.text("", 0, 0, J).attr({ zIndex: 1 }); var d; "string" ===
                typeof w && ((d = /^url\((.*?)\)$/.test(w)) || e.renderer.symbols[w]) && (e.symbolKey = w); e.bBox = f.emptyBBox; e.padding = 3; e.baselineOffset = 0; e.needsBox = c.styledMode || d; e.deferredAttr = {}; e.alignFactor = 0; return e
            } v(f, h); f.prototype.alignSetter = function (c) { c = { left: 0, center: .5, right: 1 }[c]; c !== this.alignFactor && (this.alignFactor = c, this.bBox && y(this.xSetting) && this.attr({ x: this.xSetting })) }; f.prototype.anchorXSetter = function (c, a) { this.anchorX = c; this.boxAttr(a, Math.round(c) - this.getCrispAdjust() - this.xSetting) }; f.prototype.anchorYSetter =
            function (c, a) { this.anchorY = c; this.boxAttr(a, c - this.ySetting) }; f.prototype.boxAttr = function (c, a) { this.box ? this.box.attr(c, a) : this.deferredAttr[c] = a }; f.prototype.css = function (c) { if (c) { var e = {}; c = G(c); f.textProps.forEach(function (a) { "undefined" !== typeof c[a] && (e[a] = c[a], delete c[a]) }); this.text.css(e); var h = "width" in e; "fontSize" in e || "fontWeight" in e ? this.updateTextPadding() : h && this.updateBoxSize() } return a.prototype.css.call(this, c) }; f.prototype.destroy = function () {
                q(this.element, "mouseenter"); q(this.element,
                "mouseleave"); this.text && this.text.destroy(); this.box && (this.box = this.box.destroy()); a.prototype.destroy.call(this)
            }; f.prototype.fillSetter = function (c, a) { c && (this.needsBox = !0); this.fill = c; this.boxAttr(a, c) }; f.prototype.getBBox = function () { this.textStr && 0 === this.bBox.width && 0 === this.bBox.height && this.updateBoxSize(); var c = this.padding, a = B(this.paddingLeft, c); return { width: this.width, height: this.height, x: this.bBox.x - a, y: this.bBox.y - c } }; f.prototype.getCrispAdjust = function () {
                return this.renderer.styledMode &&
                this.box ? this.box.strokeWidth() % 2 / 2 : (this["stroke-width"] ? parseInt(this["stroke-width"], 10) : 0) % 2 / 2
            }; f.prototype.heightSetter = function (c) { this.heightSetting = c }; f.prototype.on = function (c, e) {
                var f = this, h = f.text, w = h && "SPAN" === h.element.tagName ? h : void 0; if (w) {
                    var n = function (a) {
                        ("mouseenter" === c || "mouseleave" === c) && a.relatedTarget instanceof Element && (f.element.compareDocumentPosition(a.relatedTarget) & Node.DOCUMENT_POSITION_CONTAINED_BY || w.element.compareDocumentPosition(a.relatedTarget) & Node.DOCUMENT_POSITION_CONTAINED_BY) ||
                        e.call(f.element, a)
                    }; w.on(c, n)
                } a.prototype.on.call(f, c, n || e); return f
            }; f.prototype.onAdd = function () { var c = this.textStr; this.text.add(this); this.attr({ text: H(c) ? c : "", x: this.x, y: this.y }); this.box && H(this.anchorX) && this.attr({ anchorX: this.anchorX, anchorY: this.anchorY }) }; f.prototype.paddingSetter = function (c, a) { y(c) ? c !== this[a] && (this[a] = c, this.updateTextPadding()) : this[a] = void 0 }; f.prototype.rSetter = function (c, a) { this.boxAttr(a, c) }; f.prototype.shadow = function (c) {
                c && !this.renderer.styledMode && (this.updateBoxSize(),
                this.box && this.box.shadow(c)); return this
            }; f.prototype.strokeSetter = function (c, a) { this.stroke = c; this.boxAttr(a, c) }; f.prototype["stroke-widthSetter"] = function (a, e) { a && (this.needsBox = !0); this["stroke-width"] = a; this.boxAttr(e, a) }; f.prototype["text-alignSetter"] = function (a) { this.textAlign = a }; f.prototype.textSetter = function (a) { "undefined" !== typeof a && this.text.attr({ text: a }); this.updateTextPadding() }; f.prototype.updateBoxSize = function () {
                var a = this.text.element.style, e = {}, h = this.padding, m = this.bBox = y(this.widthSetting) &&
                y(this.heightSetting) && !this.textAlign || !H(this.text.textStr) ? f.emptyBBox : this.text.getBBox(); this.width = this.getPaddedWidth(); this.height = (this.heightSetting || m.height || 0) + 2 * h; a = this.renderer.fontMetrics(a && a.fontSize, this.text); this.baselineOffset = h + Math.min((this.text.firstLineMetrics || a).b, m.height || Infinity); this.heightSetting && (this.baselineOffset += (this.heightSetting - a.h) / 2); this.needsBox && (this.box || (h = this.box = this.symbolKey ? this.renderer.symbol(this.symbolKey) : this.renderer.rect(), h.addClass(("button" ===
                this.className ? "" : "highcharts-label-box") + (this.className ? " highcharts-" + this.className + "-box" : "")), h.add(this)), h = this.getCrispAdjust(), e.x = h, e.y = (this.baseline ? -this.baselineOffset : 0) + h, e.width = Math.round(this.width), e.height = Math.round(this.height), this.box.attr(x(e, this.deferredAttr)), this.deferredAttr = {})
            }; f.prototype.updateTextPadding = function () {
                var a = this.text; this.updateBoxSize(); var e = this.baseline ? 0 : this.baselineOffset, f = B(this.paddingLeft, this.padding); H(this.widthSetting) && this.bBox &&
                ("center" === this.textAlign || "right" === this.textAlign) && (f += { center: .5, right: 1 }[this.textAlign] * (this.widthSetting - this.bBox.width)); if (f !== a.x || e !== a.y) a.attr("x", f), a.hasBoxWidthChanged && (this.bBox = a.getBBox(!0)), "undefined" !== typeof e && a.attr("y", e); a.x = f; a.y = e
            }; f.prototype.widthSetter = function (a) { this.widthSetting = y(a) ? a : void 0 }; f.prototype.getPaddedWidth = function () { var a = this.padding, e = B(this.paddingLeft, a); a = B(this.paddingRight, a); return (this.widthSetting || this.bBox.width || 0) + e + a }; f.prototype.xSetter =
            function (a) { this.x = a; this.alignFactor && (a -= this.alignFactor * this.getPaddedWidth(), this["forceAnimate:x"] = !0); this.xSetting = Math.round(a); this.attr("translateX", this.xSetting) }; f.prototype.ySetter = function (a) { this.ySetting = this.y = Math.round(a); this.attr("translateY", this.ySetting) }; f.emptyBBox = { width: 0, height: 0, x: 0, y: 0 }; f.textProps = "color direction fontFamily fontSize fontStyle fontWeight lineHeight textAlign textDecoration textOutline textOverflow width".split(" "); return f
        }(a)
    }); L(a, "Core/Renderer/SVG/Symbols.js",
    [a["Core/Utilities.js"]], function (a) {
        function v(a, q, h, f, c) { var e = []; if (c) { var t = c.start || 0, m = G(c.r, h); h = G(c.r, f || h); var w = (c.end || 0) - .001; f = c.innerR; var n = G(c.open, .001 > Math.abs((c.end || 0) - t - 2 * Math.PI)), l = Math.cos(t), J = Math.sin(t), K = Math.cos(w), A = Math.sin(w); t = G(c.longArc, .001 > w - t - Math.PI ? 0 : 1); e.push(["M", a + m * l, q + h * J], ["A", m, h, 0, t, G(c.clockwise, 1), a + m * K, q + h * A]); x(f) && e.push(n ? ["M", a + f * K, q + f * A] : ["L", a + f * K, q + f * A], ["A", f, f, 0, t, x(c.clockwise) ? 1 - c.clockwise : 0, a + f * l, q + f * J]); n || e.push(["Z"]) } return e } function E(a,
        q, h, f, c) { return c && c.r ? H(a, q, h, f, c) : [["M", a, q], ["L", a + h, q], ["L", a + h, q + f], ["L", a, q + f], ["Z"]] } function H(a, q, h, f, c) { c = c && c.r || 0; return [["M", a + c, q], ["L", a + h - c, q], ["C", a + h, q, a + h, q, a + h, q + c], ["L", a + h, q + f - c], ["C", a + h, q + f, a + h, q + f, a + h - c, q + f], ["L", a + c, q + f], ["C", a, q + f, a, q + f, a, q + f - c], ["L", a, q + c], ["C", a, q, a, q, a + c, q]] } var x = a.defined, y = a.isNumber, G = a.pick; return {
            arc: v, callout: function (a, q, h, f, c) {
                var e = Math.min(c && c.r || 0, h, f), t = e + 6, m = c && c.anchorX; c = c && c.anchorY || 0; var w = H(a, q, h, f, { r: e }); if (!y(m)) return w; a + m >= h ?
                c > q + t && c < q + f - t ? w.splice(3, 1, ["L", a + h, c - 6], ["L", a + h + 6, c], ["L", a + h, c + 6], ["L", a + h, q + f - e]) : w.splice(3, 1, ["L", a + h, f / 2], ["L", m, c], ["L", a + h, f / 2], ["L", a + h, q + f - e]) : 0 >= a + m ? c > q + t && c < q + f - t ? w.splice(7, 1, ["L", a, c + 6], ["L", a - 6, c], ["L", a, c - 6], ["L", a, q + e]) : w.splice(7, 1, ["L", a, f / 2], ["L", m, c], ["L", a, f / 2], ["L", a, q + e]) : c && c > f && m > a + t && m < a + h - t ? w.splice(5, 1, ["L", m + 6, q + f], ["L", m, q + f + 6], ["L", m - 6, q + f], ["L", a + e, q + f]) : c && 0 > c && m > a + t && m < a + h - t && w.splice(1, 1, ["L", m - 6, q], ["L", m, q - 6], ["L", m + 6, q], ["L", h - e, q]); return w
            }, circle: function (a,
            q, h, f) { return v(a + h / 2, q + f / 2, h / 2, f / 2, { start: .5 * Math.PI, end: 2.5 * Math.PI, open: !1 }) }, diamond: function (a, q, h, f) { return [["M", a + h / 2, q], ["L", a + h, q + f / 2], ["L", a + h / 2, q + f], ["L", a, q + f / 2], ["Z"]] }, rect: E, roundedRect: H, square: E, triangle: function (a, q, h, f) { return [["M", a + h / 2, q], ["L", a + h, q + f], ["L", a, q + f], ["Z"]] }, "triangle-down": function (a, q, h, f) { return [["M", a, q], ["L", a + h, q], ["L", a + h / 2, q + f], ["Z"]] }
        }
    }); L(a, "Core/Renderer/SVG/TextBuilder.js", [a["Core/Renderer/HTML/AST.js"], a["Core/Globals.js"], a["Core/Utilities.js"]], function (a,
    u, E) {
        var v = u.doc, x = u.SVG_NS, y = E.attr, G = E.isString, B = E.objectEach, q = E.pick; return function () {
            function h(a) { var c = a.styles; this.renderer = a.renderer; this.svgElement = a; this.width = a.textWidth; this.textLineHeight = c && c.lineHeight; this.textOutline = c && c.textOutline; this.ellipsis = !(!c || "ellipsis" !== c.textOverflow); this.noWrap = !(!c || "nowrap" !== c.whiteSpace); this.fontSize = c && c.fontSize } h.prototype.buildSVG = function () {
                var f = this.svgElement, c = f.element, e = f.renderer, h = q(f.textStr, "").toString(), m = -1 !== h.indexOf("<"),
                w = c.childNodes, n = w.length; e = this.width && !f.added && e.box; var l = /<br.*?>/g; var J = [h, this.ellipsis, this.noWrap, this.textLineHeight, this.textOutline, this.fontSize, this.width].join(); if (J !== f.textCache) {
                    f.textCache = J; for (delete f.actualWidth; n--;) c.removeChild(w[n]); m || this.ellipsis || this.width || -1 !== h.indexOf(" ") && (!this.noWrap || l.test(h)) ? "" !== h && (e && e.appendChild(c), h = new a(h), this.modifyTree(h.nodes), h.addToDOM(f.element), this.modifyDOM(), this.ellipsis && -1 !== (c.textContent || "").indexOf("\u2026") &&
                    f.attr("title", this.unescapeEntities(f.textStr || "", ["&lt;", "&gt;"])), e && e.removeChild(c)) : c.appendChild(v.createTextNode(this.unescapeEntities(h))); G(this.textOutline) && f.applyTextOutline && f.applyTextOutline(this.textOutline)
                }
            }; h.prototype.modifyDOM = function () {
                var a = this, c = this.svgElement, e = y(c.element, "x"); c.firstLineMetrics = void 0;[].forEach.call(c.element.querySelectorAll("tspan.highcharts-br"), function (f, l) {
                    f.nextSibling && f.previousSibling && (0 === l && 1 === f.previousSibling.nodeType && (c.firstLineMetrics =
                    c.renderer.fontMetrics(void 0, f.previousSibling)), y(f, { dy: a.getLineHeight(f.nextSibling), x: e }))
                }); var h = this.width || 0; if (h) {
                    var m = function (f, l) {
                        var n = f.textContent || "", w = n.replace(/([^\^])-/g, "$1- ").split(" "), m = !a.noWrap && (1 < w.length || 1 < c.element.childNodes.length), p = a.getLineHeight(l), d = 0, k = c.actualWidth; if (a.ellipsis) n && a.truncate(f, n, void 0, 0, Math.max(0, h - parseInt(a.fontSize || 12, 10)), function (b, d) { return b.substring(0, d) + "\u2026" }); else if (m) {
                            n = []; for (m = []; l.firstChild && l.firstChild !== f;) m.push(l.firstChild),
                            l.removeChild(l.firstChild); for (; w.length;) w.length && !a.noWrap && 0 < d && (n.push(f.textContent || ""), f.textContent = w.join(" ").replace(/- /g, "-")), a.truncate(f, void 0, w, 0 === d ? k || 0 : 0, h, function (b, d) { return w.slice(0, d).join(" ").replace(/- /g, "-") }), k = c.actualWidth, d++; m.forEach(function (b) { l.insertBefore(b, f) }); n.forEach(function (b) { l.insertBefore(v.createTextNode(b), f); b = v.createElementNS(x, "tspan"); b.textContent = "\u200b"; y(b, { dy: p, x: e }); l.insertBefore(b, f) })
                        }
                    }, w = function (a) {
                        [].slice.call(a.childNodes).forEach(function (l) {
                            l.nodeType ===
                            Node.TEXT_NODE ? m(l, a) : (-1 !== l.className.baseVal.indexOf("highcharts-br") && (c.actualWidth = 0), w(l))
                        })
                    }; w(c.element)
                }
            }; h.prototype.getLineHeight = function (a) { var c; a = a.nodeType === Node.TEXT_NODE ? a.parentElement : a; this.renderer.styledMode || (c = a && /(px|em)$/.test(a.style.fontSize) ? a.style.fontSize : this.fontSize || this.renderer.style.fontSize || 12); return this.textLineHeight ? parseInt(this.textLineHeight.toString(), 10) : this.renderer.fontMetrics(c, a || this.svgElement.element).h }; h.prototype.modifyTree = function (a) {
                var c =
                this, e = function (f, h) {
                    var w = f.tagName, n = c.renderer.styledMode, l = f.attributes || {}; if ("b" === w || "strong" === w) n ? l["class"] = "highcharts-strong" : l.style = "font-weight:bold;" + (l.style || ""); else if ("i" === w || "em" === w) n ? l["class"] = "highcharts-emphasized" : l.style = "font-style:italic;" + (l.style || ""); G(l.style) && (l.style = l.style.replace(/(;| |^)color([ :])/, "$1fill$2")); "br" === w && (l["class"] = "highcharts-br", f.textContent = "\u200b", (h = a[h + 1]) && h.textContent && (h.textContent = h.textContent.replace(/^ +/gm, ""))); "#text" !==
                    w && "a" !== w && (f.tagName = "tspan"); f.attributes = l; f.children && f.children.filter(function (a) { return "#text" !== a.tagName }).forEach(e)
                }; for (a.forEach(e) ; a[0] && "tspan" === a[0].tagName && !a[0].children;) a.splice(0, 1)
            }; h.prototype.truncate = function (a, c, e, h, m, w) {
                var f = this.svgElement, l = f.renderer, t = f.rotation, K = [], A = e ? 1 : 0, p = (c || e || "").length, d = p, k, b = function (b, d) {
                    d = d || b; var g = a.parentNode; if (g && "undefined" === typeof K[d]) if (g.getSubStringLength) try { K[d] = h + g.getSubStringLength(0, e ? d + 1 : d) } catch (M) { "" } else l.getSpanWidth &&
                    (a.textContent = w(c || e, b), K[d] = h + l.getSpanWidth(f, a)); return K[d]
                }; f.rotation = 0; var g = b(a.textContent.length); if (h + g > m) { for (; A <= p;) d = Math.ceil((A + p) / 2), e && (k = w(e, d)), g = b(d, k && k.length - 1), A === p ? A = p + 1 : g > m ? p = d - 1 : A = d; 0 === p ? a.textContent = "" : c && p === c.length - 1 || (a.textContent = k || w(c || e, d)) } e && e.splice(0, d); f.actualWidth = g; f.rotation = t
            }; h.prototype.unescapeEntities = function (a, c) { B(this.renderer.escapes, function (e, f) { c && -1 !== c.indexOf(e) || (a = a.toString().replace(new RegExp(e, "g"), f)) }); return a }; return h
        }()
    });
    L(a, "Core/Renderer/SVG/SVGRenderer.js", [a["Core/Renderer/HTML/AST.js"], a["Core/Color/Color.js"], a["Core/Globals.js"], a["Core/Color/Palette.js"], a["Core/Renderer/RendererRegistry.js"], a["Core/Renderer/SVG/SVGElement.js"], a["Core/Renderer/SVG/SVGLabel.js"], a["Core/Renderer/SVG/Symbols.js"], a["Core/Renderer/SVG/TextBuilder.js"], a["Core/Utilities.js"]], function (a, u, E, H, x, y, G, B, q, h) {
        var f = E.charts, c = E.deg2rad, e = E.doc, t = E.isFirefox, m = E.isMS, w = E.isWebKit, n = E.noop, l = E.SVG_NS, J = E.symbolSizes, K = E.win, A = h.addEvent,
        p = h.attr, d = h.createElement, k = h.css, b = h.defined, g = h.destroyObjectProperties, r = h.extend, F = h.isArray, D = h.isNumber, M = h.isObject, C = h.isString, O = h.merge, v = h.pick, W = h.pInt, Y = h.uniqueKey, ba; E = function () {
            function z(b, d, g, a, k, r, c) { this.width = this.url = this.style = this.isSVG = this.imgCount = this.height = this.gradients = this.globalAnimation = this.defs = this.chartIndex = this.cacheKeys = this.cache = this.boxWrapper = this.box = this.alignedObjects = void 0; this.init(b, d, g, a, k, r, c) } z.prototype.init = function (b, d, g, a, r, c, z) {
                var l =
                this.createElement("svg").attr({ version: "1.1", "class": "highcharts-root" }), Q = l.element; z || l.css(this.getStyle(a)); b.appendChild(Q); p(b, "dir", "ltr"); -1 === b.innerHTML.indexOf("xmlns") && p(Q, "xmlns", this.SVG_NS); this.isSVG = !0; this.box = Q; this.boxWrapper = l; this.alignedObjects = []; this.url = this.getReferenceURL(); this.createElement("desc").add().element.appendChild(e.createTextNode("Created with Highcharts 9.1.2")); this.defs = this.createElement("defs").add(); this.allowHTML = c; this.forExport = r; this.styledMode =
                z; this.gradients = {}; this.cache = {}; this.cacheKeys = []; this.imgCount = 0; this.setSize(d, g, !1); var I; t && b.getBoundingClientRect && (d = function () { k(b, { left: 0, top: 0 }); I = b.getBoundingClientRect(); k(b, { left: Math.ceil(I.left) - I.left + "px", top: Math.ceil(I.top) - I.top + "px" }) }, d(), this.unSubPixelFix = A(K, "resize", d))
            }; z.prototype.definition = function (b) { return (new a([b])).addToDOM(this.defs.element) }; z.prototype.getReferenceURL = function () {
                if ((t || w) && e.getElementsByTagName("base").length) {
                    if (!b(ba)) {
                        var d = Y(); d = (new a([{
                            tagName: "svg",
                            attributes: { width: 8, height: 8 }, children: [{ tagName: "defs", children: [{ tagName: "clipPath", attributes: { id: d }, children: [{ tagName: "rect", attributes: { width: 4, height: 4 } }] }] }, { tagName: "rect", attributes: { id: "hitme", width: 8, height: 8, "clip-path": "url(#" + d + ")", fill: "rgba(0,0,0,0.001)" } }]
                        }])).addToDOM(e.body); k(d, { position: "fixed", top: 0, left: 0, zIndex: 9E5 }); var g = e.elementFromPoint(6, 6); ba = "hitme" === (g && g.id); e.body.removeChild(d)
                    } if (ba) return K.location.href.split("#")[0].replace(/<[^>]*>/g, "").replace(/([\('\)])/g,
                    "\\$1").replace(/ /g, "%20")
                } return ""
            }; z.prototype.getStyle = function (b) { return this.style = r({ fontFamily: '"Lucida Grande", "Lucida Sans Unicode", Arial, Helvetica, sans-serif', fontSize: "12px" }, b) }; z.prototype.setStyle = function (b) { this.boxWrapper.css(this.getStyle(b)) }; z.prototype.isHidden = function () { return !this.boxWrapper.getBBox().width }; z.prototype.destroy = function () {
                var b = this.defs; this.box = null; this.boxWrapper = this.boxWrapper.destroy(); g(this.gradients || {}); this.gradients = null; b && (this.defs = b.destroy());
                this.unSubPixelFix && this.unSubPixelFix(); return this.alignedObjects = null
            }; z.prototype.createElement = function (b) { var d = new this.Element; d.init(this, b); return d }; z.prototype.getRadialAttr = function (b, d) { return { cx: b[0] - b[2] / 2 + (d.cx || 0) * b[2], cy: b[1] - b[2] / 2 + (d.cy || 0) * b[2], r: (d.r || 0) * b[2] } }; z.prototype.buildText = function (b) { (new q(b)).buildSVG() }; z.prototype.getContrast = function (b) { b = u.parse(b).rgba; b[0] *= 1; b[1] *= 1.2; b[2] *= .5; return 459 < b[0] + b[1] + b[2] ? "#000000" : "#FFFFFF" }; z.prototype.button = function (b, d,
            g, k, c, z, l, e, f, F) {
                var I = this.label(b, d, g, f, void 0, void 0, F, void 0, "button"), Q = this.styledMode, P = 0, h = c ? O(c) : {}; b = h && h.style || {}; h = a.filterUserAttributes(h); I.attr(O({ padding: 8, r: 2 }, h)); if (!Q) {
                    h = O({ fill: H.neutralColor3, stroke: H.neutralColor20, "stroke-width": 1, style: { color: H.neutralColor80, cursor: "pointer", fontWeight: "normal" } }, { style: b }, h); var D = h.style; delete h.style; z = O(h, { fill: H.neutralColor10 }, a.filterUserAttributes(z || {})); var n = z.style; delete z.style; l = O(h, {
                        fill: H.highlightColor10, style: {
                            color: H.neutralColor100,
                            fontWeight: "bold"
                        }
                    }, a.filterUserAttributes(l || {})); var C = l.style; delete l.style; e = O(h, { style: { color: H.neutralColor20 } }, a.filterUserAttributes(e || {})); var N = e.style; delete e.style
                } A(I.element, m ? "mouseover" : "mouseenter", function () { 3 !== P && I.setState(1) }); A(I.element, m ? "mouseout" : "mouseleave", function () { 3 !== P && I.setState(P) }); I.setState = function (b) {
                    1 !== b && (I.state = P = b); I.removeClass(/highcharts-button-(normal|hover|pressed|disabled)/).addClass("highcharts-button-" + ["normal", "hover", "pressed", "disabled"][b ||
                    0]); Q || I.attr([h, z, l, e][b || 0]).css([D, n, C, N][b || 0])
                }; Q || I.attr(h).css(r({ cursor: "default" }, D)); return I.on("touchstart", function (b) { return b.stopPropagation() }).on("click", function (b) { 3 !== P && k.call(I, b) })
            }; z.prototype.crispLine = function (d, g, a) { void 0 === a && (a = "round"); var k = d[0], r = d[1]; b(k[1]) && k[1] === r[1] && (k[1] = r[1] = Math[a](k[1]) - g % 2 / 2); b(k[2]) && k[2] === r[2] && (k[2] = r[2] = Math[a](k[2]) + g % 2 / 2); return d }; z.prototype.path = function (b) { var d = this.styledMode ? {} : { fill: "none" }; F(b) ? d.d = b : M(b) && r(d, b); return this.createElement("path").attr(d) };
            z.prototype.circle = function (b, d, g) { b = M(b) ? b : "undefined" === typeof b ? {} : { x: b, y: d, r: g }; d = this.createElement("circle"); d.xSetter = d.ySetter = function (b, d, g) { g.setAttribute("c" + d, b) }; return d.attr(b) }; z.prototype.arc = function (b, d, g, a, k, r) { M(b) ? (a = b, d = a.y, g = a.r, b = a.x) : a = { innerR: a, start: k, end: r }; b = this.symbol("arc", b, d, g, g, a); b.r = g; return b }; z.prototype.rect = function (b, d, g, a, k, r) {
                k = M(b) ? b.r : k; var c = this.createElement("rect"); b = M(b) ? b : "undefined" === typeof b ? {} : {
                    x: b, y: d, width: Math.max(g, 0), height: Math.max(a,
                    0)
                }; this.styledMode || ("undefined" !== typeof r && (b["stroke-width"] = r, b = c.crisp(b)), b.fill = "none"); k && (b.r = k); c.rSetter = function (b, d, g) { c.r = b; p(g, { rx: b, ry: b }) }; c.rGetter = function () { return c.r || 0 }; return c.attr(b)
            }; z.prototype.setSize = function (b, d, g) { this.width = b; this.height = d; this.boxWrapper.animate({ width: b, height: d }, { step: function () { this.attr({ viewBox: "0 0 " + this.attr("width") + " " + this.attr("height") }) }, duration: v(g, !0) ? void 0 : 0 }); this.alignElements() }; z.prototype.g = function (b) {
                var d = this.createElement("g");
                return b ? d.attr({ "class": "highcharts-" + b }) : d
            }; z.prototype.image = function (b, d, g, a, k, c) {
                var z = { preserveAspectRatio: "none" }, l = function (b, d) { b.setAttributeNS ? b.setAttributeNS("http://www.w3.org/1999/xlink", "href", d) : b.setAttribute("hc-svg-href", d) }; 1 < arguments.length && r(z, { x: d, y: g, width: a, height: k }); var I = this.createElement("image").attr(z); z = function (d) { l(I.element, b); c.call(I, d) }; if (c) {
                    l(I.element, "data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw=="); var e = new K.Image; A(e, "load",
                    z); e.src = b; e.complete && z({})
                } else l(I.element, b); return I
            }; z.prototype.symbol = function (g, a, c, z, l, F) {
                var I = this, Q = /^url\((.*?)\)$/, h = Q.test(g), D = !h && (this.symbols[g] ? g : "circle"), n = D && this.symbols[D], C; if (n) { "number" === typeof a && (C = n.call(this.symbols, Math.round(a || 0), Math.round(c || 0), z || 0, l || 0, F)); var p = this.path(C); I.styledMode || p.attr("fill", "none"); r(p, { symbolName: D || void 0, x: a, y: c, width: z, height: l }); F && r(p, F) } else if (h) {
                    var N = g.match(Q)[1]; var w = p = this.image(N); w.imgwidth = v(J[N] && J[N].width, F &&
                    F.width); w.imgheight = v(J[N] && J[N].height, F && F.height); var m = function (b) { return b.attr({ width: b.width, height: b.height }) };["width", "height"].forEach(function (d) { w[d + "Setter"] = function (d, g) { var a = this["img" + g]; this[g] = d; b(a) && (F && "within" === F.backgroundSize && this.width && this.height && (a = Math.round(a * Math.min(this.width / this.imgwidth, this.height / this.imgheight))), this.element && this.element.setAttribute(g, a), this.alignByTranslate || (d = ((this[g] || 0) - a) / 2, this.attr("width" === g ? { translateX: d } : { translateY: d }))) } });
                    b(a) && w.attr({ x: a, y: c }); w.isImg = !0; b(w.imgwidth) && b(w.imgheight) ? m(w) : (w.attr({ width: 0, height: 0 }), d("img", { onload: function () { var b = f[I.chartIndex]; 0 === this.width && (k(this, { position: "absolute", top: "-999em" }), e.body.appendChild(this)); J[N] = { width: this.width, height: this.height }; w.imgwidth = this.width; w.imgheight = this.height; w.element && m(w); this.parentNode && this.parentNode.removeChild(this); I.imgCount--; if (!I.imgCount && b && !b.hasLoaded) b.onload() }, src: N }), this.imgCount++)
                } return p
            }; z.prototype.clipRect =
            function (b, d, g, a) { var k = Y() + "-", r = this.createElement("clipPath").attr({ id: k }).add(this.defs); b = this.rect(b, d, g, a, 0).add(r); b.id = k; b.clipPath = r; b.count = 0; return b }; z.prototype.text = function (d, g, a, k) {
                var r = {}; if (k && (this.allowHTML || !this.forExport)) return this.html(d, g, a); r.x = Math.round(g || 0); a && (r.y = Math.round(a)); b(d) && (r.text = d); d = this.createElement("text").attr(r); k || (d.xSetter = function (b, d, g) {
                    for (var a = g.getElementsByTagName("tspan"), k = g.getAttribute(d), r = 0, c; r < a.length; r++) c = a[r], c.getAttribute(d) ===
                    k && c.setAttribute(d, b); g.setAttribute(d, b)
                }); return d
            }; z.prototype.fontMetrics = function (b, d) { b = !this.styledMode && /px/.test(b) || !K.getComputedStyle ? b || d && d.style && d.style.fontSize || this.style && this.style.fontSize : d && y.prototype.getStyle.call(d, "font-size"); b = /px/.test(b) ? W(b) : 12; d = 24 > b ? b + 3 : Math.round(1.2 * b); return { h: d, b: Math.round(.8 * d), f: b } }; z.prototype.rotCorr = function (b, d, g) { var a = b; d && g && (a = Math.max(a * Math.cos(d * c), 4)); return { x: -b / 3 * Math.sin(d * c), y: a } }; z.prototype.pathToSegments = function (b) {
                for (var d =
                [], g = [], a = { A: 8, C: 7, H: 2, L: 3, M: 3, Q: 5, S: 5, T: 3, V: 2 }, k = 0; k < b.length; k++) C(g[0]) && D(b[k]) && g.length === a[g[0].toUpperCase()] && b.splice(k, 0, g[0].replace("M", "L").replace("m", "l")), "string" === typeof b[k] && (g.length && d.push(g.slice(0)), g.length = 0), g.push(b[k]); d.push(g.slice(0)); return d
            }; z.prototype.label = function (b, d, g, a, k, r, c, z, l) { return new G(this, b, d, g, a, k, r, c, z, l) }; z.prototype.alignElements = function () { this.alignedObjects.forEach(function (b) { return b.align() }) }; return z
        }(); r(E.prototype, {
            Element: y, SVG_NS: l,
            escapes: { "&": "&amp;", "<": "&lt;", ">": "&gt;", "'": "&#39;", '"': "&quot;" }, symbols: B, draw: n
        }); x.registerRendererType("svg", E, !0); ""; return E
    }); L(a, "Core/Renderer/HTML/HTMLElement.js", [a["Core/Globals.js"], a["Core/Renderer/SVG/SVGElement.js"], a["Core/Utilities.js"]], function (a, u, E) {
        var v = this && this.__extends || function () {
            var a = function (c, e) { a = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, c) { a.__proto__ = c } || function (a, c) { for (var l in c) c.hasOwnProperty(l) && (a[l] = c[l]) }; return a(c, e) }; return function (c,
            e) { function f() { this.constructor = c } a(c, e); c.prototype = null === e ? Object.create(e) : (f.prototype = e.prototype, new f) }
        }(), x = a.isFirefox, y = a.isMS, G = a.isWebKit, B = a.win, q = E.css, h = E.defined, f = E.extend, c = E.pick, e = E.pInt; return function (a) {
            function m() { return null !== a && a.apply(this, arguments) || this } v(m, a); m.compose = function (a) { a = a.prototype; var c = m.prototype; a.getSpanCorrection = c.getSpanCorrection; a.htmlCss = c.htmlCss; a.htmlGetBBox = c.htmlGetBBox; a.htmlUpdateTransform = c.htmlUpdateTransform; a.setSpanRotation = c.setSpanRotation };
            m.prototype.getSpanCorrection = function (a, c, l) { this.xCorr = -a * l; this.yCorr = -c }; m.prototype.htmlCss = function (a) { var e = "SPAN" === this.element.tagName && a && "width" in a, l = c(e && a.width, void 0); if (e) { delete a.width; this.textWidth = l; var h = !0 } a && "ellipsis" === a.textOverflow && (a.whiteSpace = "nowrap", a.overflow = "hidden"); this.styles = f(this.styles, a); q(this.element, a); h && this.htmlUpdateTransform(); return this }; m.prototype.htmlGetBBox = function () {
                var a = this.element; return {
                    x: a.offsetLeft, y: a.offsetTop, width: a.offsetWidth,
                    height: a.offsetHeight
                }
            }; m.prototype.htmlUpdateTransform = function () {
                if (this.added) {
                    var a = this.renderer, c = this.element, l = this.translateX || 0, f = this.translateY || 0, m = this.x || 0, t = this.y || 0, p = this.textAlign || "left", d = { left: 0, center: .5, right: 1 }[p], k = this.styles; k = k && k.whiteSpace; q(c, { marginLeft: l, marginTop: f }); !a.styledMode && this.shadows && this.shadows.forEach(function (b) { q(b, { marginLeft: l + 1, marginTop: f + 1 }) }); this.inverted && [].forEach.call(c.childNodes, function (b) { a.invertChild(b, c) }); if ("SPAN" === c.tagName) {
                        var b =
                        this.rotation, g = this.textWidth && e(this.textWidth), r = [b, p, c.innerHTML, this.textWidth, this.textAlign].join(), F = void 0; (F = g !== this.oldTextWidth) && !(F = g > this.oldTextWidth) && ((F = this.textPxLength) || (q(c, { width: "", whiteSpace: k || "nowrap" }), F = c.offsetWidth), F = F > g); F && (/[ \-]/.test(c.textContent || c.innerText) || "ellipsis" === c.style.textOverflow) ? (q(c, { width: g + "px", display: "block", whiteSpace: k || "normal" }), this.oldTextWidth = g, this.hasBoxWidthChanged = !0) : this.hasBoxWidthChanged = !1; r !== this.cTT && (F = a.fontMetrics(c.style.fontSize,
                        c).b, !h(b) || b === (this.oldRotation || 0) && p === this.oldAlign || this.setSpanRotation(b, d, F), this.getSpanCorrection(!h(b) && this.textPxLength || c.offsetWidth, F, d, b, p)); q(c, { left: m + (this.xCorr || 0) + "px", top: t + (this.yCorr || 0) + "px" }); this.cTT = r; this.oldRotation = b; this.oldAlign = p
                    }
                } else this.alignOnAdd = !0
            }; m.prototype.setSpanRotation = function (a, c, l) {
                var e = {}, f = y && !/Edge/.test(B.navigator.userAgent) ? "-ms-transform" : G ? "-webkit-transform" : x ? "MozTransform" : B.opera ? "-o-transform" : void 0; f && (e[f] = e.transform = "rotate(" +
                a + "deg)", e[f + (x ? "Origin" : "-origin")] = e.transformOrigin = 100 * c + "% " + l + "px", q(this.element, e))
            }; return m
        }(u)
    }); L(a, "Core/Renderer/HTML/HTMLRenderer.js", [a["Core/Renderer/HTML/AST.js"], a["Core/Renderer/SVG/SVGElement.js"], a["Core/Renderer/SVG/SVGRenderer.js"], a["Core/Utilities.js"]], function (a, u, E, H) {
        var v = this && this.__extends || function () {
            var a = function (f, c) {
                a = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, c) { a.__proto__ = c } || function (a, c) { for (var e in c) c.hasOwnProperty(e) && (a[e] = c[e]) };
                return a(f, c)
            }; return function (f, c) { function e() { this.constructor = f } a(f, c); f.prototype = null === c ? Object.create(c) : (e.prototype = c.prototype, new e) }
        }(), y = H.attr, G = H.createElement, B = H.extend, q = H.pick; return function (h) {
            function f() { return null !== h && h.apply(this, arguments) || this } v(f, h); f.compose = function (a) { a.prototype.html = f.prototype.html }; f.prototype.html = function (c, e, f) {
                var h = this.createElement("span"), w = h.element, n = h.renderer, l = n.isSVG, t = function (a, c) {
                    ["opacity", "visibility"].forEach(function (l) {
                        a[l +
                        "Setter"] = function (d, k, b) { var g = a.div ? a.div.style : c; u.prototype[l + "Setter"].call(this, d, k, b); g && (g[k] = d) }
                    }); a.addedSetters = !0
                }; h.textSetter = function (c) { c !== this.textStr && (delete this.bBox, delete this.oldTextWidth, a.setElementHTML(this.element, q(c, "")), this.textStr = c, h.doTransform = !0) }; l && t(h, h.element.style); h.xSetter = h.ySetter = h.alignSetter = h.rotationSetter = function (a, c) { "align" === c ? h.alignValue = h.textAlign = a : h[c] = a; h.doTransform = !0 }; h.afterSetters = function () {
                    this.doTransform && (this.htmlUpdateTransform(),
                    this.doTransform = !1)
                }; h.attr({ text: c, x: Math.round(e), y: Math.round(f) }).css({ position: "absolute" }); n.styledMode || h.css({ fontFamily: this.style.fontFamily, fontSize: this.style.fontSize }); w.style.whiteSpace = "nowrap"; h.css = h.htmlCss; l && (h.add = function (a) {
                    var c = n.box.parentNode, l = []; if (this.parentGroup = a) {
                        var d = a.div; if (!d) {
                            for (; a;) l.push(a), a = a.parentGroup; l.reverse().forEach(function (a) {
                                function b(b, d) { a[d] = b; "translateX" === d ? e.left = b + "px" : e.top = b + "px"; a.doTransform = !0 } var g = y(a.element, "class"), k = a.styles ||
                                {}; d = a.div = a.div || G("div", g ? { className: g } : void 0, { position: "absolute", left: (a.translateX || 0) + "px", top: (a.translateY || 0) + "px", display: a.display, opacity: a.opacity, cursor: k.cursor, pointerEvents: k.pointerEvents }, d || c); var e = d.style; B(a, { classSetter: function (b) { return function (d) { this.element.setAttribute("class", d); b.className = d } }(d), on: function () { l[0].div && h.on.apply({ element: l[0].div, onEvents: h.onEvents }, arguments); return a }, translateXSetter: b, translateYSetter: b }); a.addedSetters || t(a)
                            })
                        }
                    } else d = c; d.appendChild(w);
                    h.added = !0; h.alignOnAdd && h.htmlUpdateTransform(); return h
                }); return h
            }; return f
        }(E)
    }); L(a, "Core/Foundation.js", [a["Core/Utilities.js"]], function (a) { var v = a.addEvent, E = a.isFunction, H = a.objectEach, x = a.removeEvent; return { registerEventOptions: function (a, u) { a.eventOptions = a.eventOptions || {}; H(u.events, function (u, q) { E(u) && a.eventOptions[q] !== u && (E(a.eventOptions[q]) && x(a, q, a.eventOptions[q]), a.eventOptions[q] = u, v(a, q, u)) }) } } }); L(a, "Core/Axis/AxisDefaults.js", [a["Core/Color/Palette.js"]], function (a) {
        var v;
        (function (v) {
            v.defaultXAxisOptions = {
                alignTicks: !0, allowDecimals: void 0, panningEnabled: !0, zIndex: 2, zoomEnabled: !0, dateTimeLabelFormats: { millisecond: { main: "%H:%M:%S.%L", range: !1 }, second: { main: "%H:%M:%S", range: !1 }, minute: { main: "%H:%M", range: !1 }, hour: { main: "%H:%M", range: !1 }, day: { main: "%e. %b" }, week: { main: "%e. %b" }, month: { main: "%b '%y" }, year: { main: "%Y" } }, endOnTick: !1, gridLineDashStyle: "Solid", gridZIndex: 1, labels: {
                    autoRotation: void 0, autoRotationLimit: 80, distance: void 0, enabled: !0, indentation: 10, overflow: "justify",
                    padding: 5, reserveSpace: void 0, rotation: void 0, staggerLines: 0, step: 0, useHTML: !1, x: 0, zIndex: 7, style: { color: a.neutralColor60, cursor: "default", fontSize: "11px" }
                }, maxPadding: .01, minorGridLineDashStyle: "Solid", minorTickLength: 2, minorTickPosition: "outside", minPadding: .01, offset: void 0, opposite: !1, reversed: void 0, reversedStacks: !1, showEmpty: !0, showFirstLabel: !0, showLastLabel: !0, startOfWeek: 1, startOnTick: !1, tickLength: 10, tickPixelInterval: 100, tickmarkPlacement: "between", tickPosition: "outside", title: {
                    align: "middle",
                    rotation: 0, useHTML: !1, x: 0, y: 0, style: { color: a.neutralColor60 }
                }, type: "linear", uniqueNames: !0, visible: !0, minorGridLineColor: a.neutralColor5, minorGridLineWidth: 1, minorTickColor: a.neutralColor40, lineColor: a.highlightColor20, lineWidth: 1, gridLineColor: a.neutralColor10, gridLineWidth: void 0, tickColor: a.highlightColor20
            }; v.defaultYAxisOptions = {
                reversedStacks: !0, endOnTick: !0, maxPadding: .05, minPadding: .05, tickPixelInterval: 72, showLastLabel: !0, labels: { x: -8 }, startOnTick: !0, title: { rotation: 270, text: "Values" }, stackLabels: {
                    animation: {},
                    allowOverlap: !1, enabled: !1, crop: !0, overflow: "justify", formatter: function () { var a = this.axis.chart.numberFormatter; return a(this.total, -1) }, style: { color: a.neutralColor100, fontSize: "11px", fontWeight: "bold", textOutline: "1px contrast" }
                }, gridLineWidth: 1, lineWidth: 0
            }; v.defaultLeftAxisOptions = { labels: { x: -15 }, title: { rotation: 270 } }; v.defaultRightAxisOptions = { labels: { x: 15 }, title: { rotation: 90 } }; v.defaultBottomAxisOptions = { labels: { autoRotation: [-45], x: 0 }, margin: 15, title: { rotation: 0 } }; v.defaultTopAxisOptions = {
                labels: {
                    autoRotation: [-45],
                    x: 0
                }, margin: 15, title: { rotation: 0 }
            }
        })(v || (v = {})); return v
    }); L(a, "Core/Axis/Tick.js", [a["Core/FormatUtilities.js"], a["Core/Globals.js"], a["Core/Utilities.js"]], function (a, u, E) {
        var v = u.deg2rad, x = E.clamp, y = E.correctFloat, G = E.defined, B = E.destroyObjectProperties, q = E.extend, h = E.fireEvent, f = E.isNumber, c = E.merge, e = E.objectEach, t = E.pick; u = function () {
            function m(a, c, l, e, f) {
                this.isNewLabel = this.isNew = !0; this.axis = a; this.pos = c; this.type = l || ""; this.parameters = f || {}; this.tickmarkOffset = this.parameters.tickmarkOffset;
                this.options = this.parameters.options; h(this, "init"); l || e || this.addLabel()
            } m.prototype.addLabel = function () {
                var c = this, e = c.axis, l = e.options, m = e.chart, K = e.categories, A = e.logarithmic, p = e.names, d = c.pos, k = t(c.options && c.options.labels, l.labels), b = e.tickPositions, g = d === b[0], r = d === b[b.length - 1], F = (!k.step || 1 === k.step) && 1 === e.tickInterval; b = b.info; var D = c.label, M; K = this.parameters.category || (K ? t(K[d], p[d], d) : d); A && f(K) && (K = y(A.lin2log(K))); if (e.dateTime && b) {
                    var C = m.time.resolveDTLFormat(l.dateTimeLabelFormats[!l.grid &&
                    b.higherRanks[d] || b.unitName]); var O = C.main
                } c.isFirst = g; c.isLast = r; var v = { axis: e, chart: m, dateTimeLabelFormat: O, isFirst: g, isLast: r, pos: d, tick: c, tickPositionInfo: b, value: K }; h(this, "labelFormat", v); var W = function (b) { return k.formatter ? k.formatter.call(b, b) : k.format ? (b.text = e.defaultLabelFormatter.call(b), a.format(k.format, b, m)) : e.defaultLabelFormatter.call(b, b) }; l = W.call(v, v); var u = C && C.list; c.shortenLabel = u ? function () {
                    for (M = 0; M < u.length; M++) if (q(v, { dateTimeLabelFormat: u[M] }), D.attr({ text: W.call(v, v) }),
                    D.getBBox().width < e.getSlotWidth(c) - 2 * k.padding) return; D.attr({ text: "" })
                } : void 0; F && e._addedPlotLB && c.moveLabel(l, k); G(D) || c.movedLabel ? D && D.textStr !== l && !F && (!D.textWidth || k.style.width || D.styles.width || D.css({ width: null }), D.attr({ text: l }), D.textPxLength = D.getBBox().width) : (c.label = D = c.createLabel({ x: 0, y: 0 }, l, k), c.rotation = 0)
            }; m.prototype.createLabel = function (a, e, l) {
                var f = this.axis, h = f.chart; if (a = G(e) && l.enabled ? h.renderer.text(e, a.x, a.y, l.useHTML).add(f.labelGroup) : null) h.styledMode || a.css(c(l.style)),
                a.textPxLength = a.getBBox().width; return a
            }; m.prototype.destroy = function () { B(this, this.axis) }; m.prototype.getPosition = function (a, c, l, e) { var f = this.axis, n = f.chart, p = e && n.oldChartHeight || n.chartHeight; a = { x: a ? y(f.translate(c + l, null, null, e) + f.transB) : f.left + f.offset + (f.opposite ? (e && n.oldChartWidth || n.chartWidth) - f.right - f.left : 0), y: a ? p - f.bottom + f.offset - (f.opposite ? f.height : 0) : y(p - f.translate(c + l, null, null, e) - f.transB) }; a.y = x(a.y, -1E5, 1E5); h(this, "afterGetPosition", { pos: a }); return a }; m.prototype.getLabelPosition =
            function (a, c, l, e, f, m, p, d) {
                var k = this.axis, b = k.transA, g = k.isLinked && k.linkedParent ? k.linkedParent.reversed : k.reversed, r = k.staggerLines, F = k.tickRotCorr || { x: 0, y: 0 }, D = e || k.reserveSpaceDefault ? 0 : -k.labelOffset * ("center" === k.labelAlign ? .5 : 1), n = {}, C = f.y; G(C) || (C = 0 === k.side ? l.rotation ? -8 : -l.getBBox().height : 2 === k.side ? F.y + 8 : Math.cos(l.rotation * v) * (F.y - l.getBBox(!1, 0).height / 2)); a = a + f.x + D + F.x - (m && e ? m * b * (g ? -1 : 1) : 0); c = c + C - (m && !e ? m * b * (g ? 1 : -1) : 0); r && (l = p / (d || 1) % r, k.opposite && (l = r - l - 1), c += k.labelOffset / r * l); n.x =
                a; n.y = Math.round(c); h(this, "afterGetLabelPosition", { pos: n, tickmarkOffset: m, index: p }); return n
            }; m.prototype.getLabelSize = function () { return this.label ? this.label.getBBox()[this.axis.horiz ? "height" : "width"] : 0 }; m.prototype.getMarkPath = function (a, c, l, e, f, h) { return h.crispLine([["M", a, c], ["L", a + (f ? 0 : -l), c + (f ? l : 0)]], e) }; m.prototype.handleOverflow = function (a) {
                var c = this.axis, l = c.options.labels, e = a.x, f = c.chart.chartWidth, h = c.chart.spacing, p = t(c.labelLeft, Math.min(c.pos, h[3])); h = t(c.labelRight, Math.max(c.isRadial ?
                0 : c.pos + c.len, f - h[1])); var d = this.label, k = this.rotation, b = { left: 0, center: .5, right: 1 }[c.labelAlign || d.attr("align")], g = d.getBBox().width, r = c.getSlotWidth(this), F = {}, D = r, M = 1, C; if (k || "justify" !== l.overflow) 0 > k && e - b * g < p ? C = Math.round(e / Math.cos(k * v) - p) : 0 < k && e + b * g > h && (C = Math.round((f - e) / Math.cos(k * v))); else if (f = e + (1 - b) * g, e - b * g < p ? D = a.x + D * (1 - b) - p : f > h && (D = h - a.x + D * b, M = -1), D = Math.min(r, D), D < r && "center" === c.labelAlign && (a.x += M * (r - D - b * (r - Math.min(g, D)))), g > D || c.autoRotation && (d.styles || {}).width) C = D; C && (this.shortenLabel ?
                this.shortenLabel() : (F.width = Math.floor(C) + "px", (l.style || {}).textOverflow || (F.textOverflow = "ellipsis"), d.css(F)))
            }; m.prototype.moveLabel = function (a, c) {
                var l = this, f = l.label, h = l.axis, n = h.reversed, p = !1; f && f.textStr === a ? (l.movedLabel = f, p = !0, delete l.label) : e(h.ticks, function (d) { p || d.isNew || d === l || !d.label || d.label.textStr !== a || (l.movedLabel = d.label, p = !0, d.labelPos = l.movedLabel.xy, delete d.label) }); if (!p && (l.labelPos || f)) {
                    var d = l.labelPos || f.xy; f = h.horiz ? n ? 0 : h.width + h.left : d.x; h = h.horiz ? d.y : n ? h.width +
                    h.left : 0; l.movedLabel = l.createLabel({ x: f, y: h }, a, c); l.movedLabel && l.movedLabel.attr({ opacity: 0 })
                }
            }; m.prototype.render = function (a, c, l) { var e = this.axis, f = e.horiz, n = this.pos, p = t(this.tickmarkOffset, e.tickmarkOffset); n = this.getPosition(f, n, p, c); p = n.x; var d = n.y; e = f && p === e.pos + e.len || !f && d === e.pos ? -1 : 1; f = t(l, this.label && this.label.newOpacity, 1); l = t(l, 1); this.isActive = !0; this.renderGridLine(c, l, e); this.renderMark(n, l, e); this.renderLabel(n, c, f, a); this.isNew = !1; h(this, "afterRender") }; m.prototype.renderGridLine =
            function (a, c, e) {
                var l = this.axis, f = l.options, h = {}, p = this.pos, d = this.type, k = t(this.tickmarkOffset, l.tickmarkOffset), b = l.chart.renderer, g = this.gridLine, r = f.gridLineWidth, F = f.gridLineColor, D = f.gridLineDashStyle; "minor" === this.type && (r = f.minorGridLineWidth, F = f.minorGridLineColor, D = f.minorGridLineDashStyle); g || (l.chart.styledMode || (h.stroke = F, h["stroke-width"] = r || 0, h.dashstyle = D), d || (h.zIndex = 1), a && (c = 0), this.gridLine = g = b.path().attr(h).addClass("highcharts-" + (d ? d + "-" : "") + "grid-line").add(l.gridGroup));
                if (g && (e = l.getPlotLinePath({ value: p + k, lineWidth: g.strokeWidth() * e, force: "pass", old: a }))) g[a || this.isNew ? "attr" : "animate"]({ d: e, opacity: c })
            }; m.prototype.renderMark = function (a, c, e) {
                var l = this.axis, f = l.options, h = l.chart.renderer, p = this.type, d = l.tickSize(p ? p + "Tick" : "tick"), k = a.x; a = a.y; var b = t(f["minor" !== p ? "tickWidth" : "minorTickWidth"], !p && l.isXAxis ? 1 : 0); f = f["minor" !== p ? "tickColor" : "minorTickColor"]; var g = this.mark, r = !g; d && (l.opposite && (d[0] = -d[0]), g || (this.mark = g = h.path().addClass("highcharts-" + (p ? p +
                "-" : "") + "tick").add(l.axisGroup), l.chart.styledMode || g.attr({ stroke: f, "stroke-width": b })), g[r ? "attr" : "animate"]({ d: this.getMarkPath(k, a, d[0], g.strokeWidth() * e, l.horiz, h), opacity: c }))
            }; m.prototype.renderLabel = function (a, c, e, h) {
                var l = this.axis, n = l.horiz, p = l.options, d = this.label, k = p.labels, b = k.step; l = t(this.tickmarkOffset, l.tickmarkOffset); var g = a.x; a = a.y; var r = !0; d && f(g) && (d.xy = a = this.getLabelPosition(g, a, d, n, k, l, h, b), this.isFirst && !this.isLast && !p.showFirstLabel || this.isLast && !this.isFirst && !p.showLastLabel ?
                r = !1 : !n || k.step || k.rotation || c || 0 === e || this.handleOverflow(a), b && h % b && (r = !1), r && f(a.y) ? (a.opacity = e, d[this.isNewLabel ? "attr" : "animate"](a), this.isNewLabel = !1) : (d.attr("y", -9999), this.isNewLabel = !0))
            }; m.prototype.replaceMovedLabel = function () { var a = this.label, c = this.axis, e = c.reversed; if (a && !this.isNew) { var f = c.horiz ? e ? c.left : c.width + c.left : a.xy.x; e = c.horiz ? a.xy.y : e ? c.width + c.top : c.top; a.animate({ x: f, y: e, opacity: 0 }, void 0, a.destroy); delete this.label } c.isDirty = !0; this.label = this.movedLabel; delete this.movedLabel };
            return m
        }(); ""; return u
    }); L(a, "Core/Axis/Axis.js", [a["Core/Animation/AnimationUtilities.js"], a["Core/Color/Color.js"], a["Core/Color/Palette.js"], a["Core/DefaultOptions.js"], a["Core/Foundation.js"], a["Core/Globals.js"], a["Core/Utilities.js"], a["Core/Axis/AxisDefaults.js"], a["Core/Axis/Tick.js"]], function (a, u, E, H, x, y, G, B, q) {
        var h = a.animObject, f = x.registerEventOptions, c = y.deg2rad, e = H.defaultOptions, t = G.arrayMax, m = G.arrayMin, w = G.clamp, n = G.correctFloat, l = G.defined, J = G.destroyObjectProperties, v = G.erase,
        A = G.error, p = G.extend, d = G.fireEvent, k = G.getMagnitude, b = G.isArray, g = G.isNumber, r = G.isString, F = G.merge, D = G.normalizeTickInterval, M = G.objectEach, C = G.pick, O = G.relativeLength, S = G.removeEvent, W = G.splat, Y = G.syncTimeout; a = function () {
            function a(b, a) {
                this.zoomEnabled = this.width = this.visible = this.userOptions = this.translationSlope = this.transB = this.transA = this.top = this.ticks = this.tickRotCorr = this.tickPositions = this.tickmarkOffset = this.tickInterval = this.tickAmount = this.side = this.series = this.right = this.positiveValuesOnly =
                this.pos = this.pointRangePadding = this.pointRange = this.plotLinesAndBandsGroups = this.plotLinesAndBands = this.paddedTicks = this.overlap = this.options = this.offset = this.names = this.minPixelPadding = this.minorTicks = this.minorTickInterval = this.min = this.maxLabelLength = this.max = this.len = this.left = this.labelFormatter = this.labelEdge = this.isLinked = this.height = this.hasVisibleSeries = this.hasNames = this.eventOptions = this.coll = this.closestPointRange = this.chart = this.categories = this.bottom = this.alternateBands = void 0; this.init(b,
                a)
            } a.prototype.init = function (b, a) {
                var c = a.isX; this.chart = b; this.horiz = b.inverted && !this.isZAxis ? !c : c; this.isXAxis = c; this.coll = this.coll || (c ? "xAxis" : "yAxis"); d(this, "init", { userOptions: a }); this.opposite = C(a.opposite, this.opposite); this.side = C(a.side, this.side, this.horiz ? this.opposite ? 0 : 2 : this.opposite ? 1 : 3); this.setOptions(a); var k = this.options, r = k.labels, e = k.type; this.userOptions = a; this.minPixelPadding = 0; this.reversed = C(k.reversed, this.reversed); this.visible = k.visible; this.zoomEnabled = k.zoomEnabled;
                this.hasNames = "category" === e || !0 === k.categories; this.categories = k.categories || this.hasNames; this.names || (this.names = [], this.names.keys = {}); this.plotLinesAndBandsGroups = {}; this.positiveValuesOnly = !!this.logarithmic; this.isLinked = l(k.linkedTo); this.ticks = {}; this.labelEdge = []; this.minorTicks = {}; this.plotLinesAndBands = []; this.alternateBands = {}; this.len = 0; this.minRange = this.userMinRange = k.minRange || k.maxZoom; this.range = k.range; this.offset = k.offset || 0; this.min = this.max = null; a = C(k.crosshair, W(b.options.tooltip.crosshairs)[c ?
                0 : 1]); this.crosshair = !0 === a ? {} : a; -1 === b.axes.indexOf(this) && (c ? b.axes.splice(b.xAxis.length, 0, this) : b.axes.push(this), b[this.coll].push(this)); this.series = this.series || []; b.inverted && !this.isZAxis && c && "undefined" === typeof this.reversed && (this.reversed = !0); this.labelRotation = g(r.rotation) ? r.rotation : void 0; f(this, k); d(this, "afterInit")
            }; a.prototype.setOptions = function (b) {
                this.options = F(B.defaultXAxisOptions, "yAxis" === this.coll && B.defaultYAxisOptions, [B.defaultTopAxisOptions, B.defaultRightAxisOptions,
                B.defaultBottomAxisOptions, B.defaultLeftAxisOptions][this.side], F(e[this.coll], b)); d(this, "afterSetOptions", { userOptions: b })
            }; a.prototype.defaultLabelFormatter = function (b) {
                var a = this.axis; b = this.chart.numberFormatter; var d = g(this.value) ? this.value : NaN, c = a.chart.time, k = this.dateTimeLabelFormat, r = e.lang, l = r.numericSymbols; r = r.numericSymbolMagnitude || 1E3; var z = a.logarithmic ? Math.abs(d) : a.tickInterval, f = l && l.length; if (a.categories) var h = "" + this.value; else if (k) h = c.dateFormat(k, d); else if (f && 1E3 <= z) for (; f-- &&
                "undefined" === typeof h;) a = Math.pow(r, f + 1), z >= a && 0 === 10 * d % a && null !== l[f] && 0 !== d && (h = b(d / a, -1) + l[f]); "undefined" === typeof h && (h = 1E4 <= Math.abs(d) ? b(d, -1) : b(d, -1, void 0, "")); return h
            }; a.prototype.getSeriesExtremes = function () {
                var b = this, a = b.chart, k; d(this, "getSeriesExtremes", null, function () {
                    b.hasVisibleSeries = !1; b.dataMin = b.dataMax = b.threshold = null; b.softThreshold = !b.isXAxis; b.stacking && b.stacking.buildStacks(); b.series.forEach(function (d) {
                        if (d.visible || !a.options.chart.ignoreHiddenSeries) {
                            var c = d.options,
                            r = c.threshold; b.hasVisibleSeries = !0; b.positiveValuesOnly && 0 >= r && (r = null); if (b.isXAxis) { if (c = d.xData, c.length) { c = b.logarithmic ? c.filter(b.validatePositiveValue) : c; k = d.getXExtremes(c); var e = k.min; var z = k.max; g(e) || e instanceof Date || (c = c.filter(g), k = d.getXExtremes(c), e = k.min, z = k.max); c.length && (b.dataMin = Math.min(C(b.dataMin, e), e), b.dataMax = Math.max(C(b.dataMax, z), z)) } } else if (d = d.applyExtremes(), g(d.dataMin) && (e = d.dataMin, b.dataMin = Math.min(C(b.dataMin, e), e)), g(d.dataMax) && (z = d.dataMax, b.dataMax = Math.max(C(b.dataMax,
                            z), z)), l(r) && (b.threshold = r), !c.softThreshold || b.positiveValuesOnly) b.softThreshold = !1
                        }
                    })
                }); d(this, "afterGetSeriesExtremes")
            }; a.prototype.translate = function (b, a, d, c, k, r) {
                var e = this.linkedParent || this, l = c && e.old ? e.old.min : e.min, z = e.minPixelPadding; k = (e.isOrdinal || e.brokenAxis && e.brokenAxis.hasBreaks || e.logarithmic && k) && e.lin2val; var f = 1, I = 0; c = c && e.old ? e.old.transA : e.transA; c || (c = e.transA); d && (f *= -1, I = e.len); e.reversed && (f *= -1, I -= f * (e.sector || e.len)); a ? (b = (b * f + I - z) / c + l, k && (b = e.lin2val(b))) : (k && (b = e.val2lin(b)),
                b = g(l) ? f * (b - l) * c + I + f * z + (g(r) ? c * r : 0) : void 0); return b
            }; a.prototype.toPixels = function (b, a) { return this.translate(b, !1, !this.horiz, null, !0) + (a ? 0 : this.pos) }; a.prototype.toValue = function (b, a) { return this.translate(b - (a ? 0 : this.pos), !0, !this.horiz, null, !0) }; a.prototype.getPlotLinePath = function (b) {
                function a(b, a, d) { if ("pass" !== M && b < a || b > d) M ? b = w(b, a, d) : Z = !0; return b } var c = this, k = c.chart, r = c.left, e = c.top, l = b.old, f = b.value, z = b.lineWidth, h = l && k.oldChartHeight || k.chartHeight, F = l && k.oldChartWidth || k.chartWidth,
                D = c.transB, p = b.translatedValue, M = b.force, n, m, t, q, Z; b = { value: f, lineWidth: z, old: l, force: M, acrossPanes: b.acrossPanes, translatedValue: p }; d(this, "getPlotLinePath", b, function (b) { p = C(p, c.translate(f, null, null, l)); p = w(p, -1E5, 1E5); n = t = Math.round(p + D); m = q = Math.round(h - p - D); g(p) ? c.horiz ? (m = e, q = h - c.bottom, n = t = a(n, r, r + c.width)) : (n = r, t = F - c.right, m = q = a(m, e, e + c.height)) : (Z = !0, M = !1); b.path = Z && !M ? null : k.renderer.crispLine([["M", n, m], ["L", t, q]], z || 1) }); return b.path
            }; a.prototype.getLinearTickPositions = function (b, a,
            d) { var g = n(Math.floor(a / b) * b); d = n(Math.ceil(d / b) * b); var c = [], k; n(g + b) === g && (k = 20); if (this.single) return [a]; for (a = g; a <= d;) { c.push(a); a = n(a + b, k); if (a === r) break; var r = a } return c }; a.prototype.getMinorTickInterval = function () { var b = this.options; return !0 === b.minorTicks ? C(b.minorTickInterval, "auto") : !1 === b.minorTicks ? null : b.minorTickInterval }; a.prototype.getMinorTickPositions = function () {
                var b = this.options, a = this.tickPositions, d = this.minorTickInterval, g = this.pointRangePadding || 0, c = this.min - g; g = this.max + g;
                var k = g - c, r = []; if (k && k / d < this.len / 3) { var e = this.logarithmic; if (e) this.paddedTicks.forEach(function (b, a, g) { a && r.push.apply(r, e.getLogTickPositions(d, g[a - 1], g[a], !0)) }); else if (this.dateTime && "auto" === this.getMinorTickInterval()) r = r.concat(this.getTimeTicks(this.dateTime.normalizeTimeTickInterval(d), c, g, b.startOfWeek)); else for (b = c + (a[0] - c) % d; b <= g && b !== r[0]; b += d) r.push(b) } 0 !== r.length && this.trimTicks(r); return r
            }; a.prototype.adjustForMinRange = function () {
                var b = this.options, a = this.logarithmic, d = this.min,
                g = this.max, c = 0, k, r, e, f; this.isXAxis && "undefined" === typeof this.minRange && !a && (l(b.min) || l(b.max) ? this.minRange = null : (this.series.forEach(function (b) { e = b.xData; f = b.xIncrement ? 1 : e.length - 1; if (1 < e.length) for (k = f; 0 < k; k--) if (r = e[k] - e[k - 1], !c || r < c) c = r }), this.minRange = Math.min(5 * c, this.dataMax - this.dataMin))); if (g - d < this.minRange) {
                    var h = this.dataMax - this.dataMin >= this.minRange; var F = this.minRange; var D = (F - g + d) / 2; D = [d - D, C(b.min, d - D)]; h && (D[2] = this.logarithmic ? this.logarithmic.log2lin(this.dataMin) : this.dataMin);
                    d = t(D); g = [d + F, C(b.max, d + F)]; h && (g[2] = a ? a.log2lin(this.dataMax) : this.dataMax); g = m(g); g - d < F && (D[0] = g - F, D[1] = C(b.min, g - F), d = t(D))
                } this.min = d; this.max = g
            }; a.prototype.getClosest = function () { var b; this.categories ? b = 1 : this.series.forEach(function (a) { var d = a.closestPointRange, g = a.visible || !a.chart.options.chart.ignoreHiddenSeries; !a.noSharedTooltip && l(d) && g && (b = l(b) ? Math.min(b, d) : d) }); return b }; a.prototype.nameToX = function (a) {
                var d = b(this.categories), g = d ? this.categories : this.names, c = a.options.x; a.series.requireSorting =
                !1; l(c) || (c = this.options.uniqueNames ? d ? g.indexOf(a.name) : C(g.keys[a.name], -1) : a.series.autoIncrement()); if (-1 === c) { if (!d) var k = g.length } else k = c; "undefined" !== typeof k && (this.names[k] = a.name, this.names.keys[a.name] = k); return k
            }; a.prototype.updateNames = function () {
                var b = this, a = this.names; 0 < a.length && (Object.keys(a.keys).forEach(function (b) { delete a.keys[b] }), a.length = 0, this.minRange = this.userMinRange, (this.series || []).forEach(function (a) {
                    a.xIncrement = null; if (!a.points || a.isDirtyData) b.max = Math.max(b.max,
                    a.xData.length - 1), a.processData(), a.generatePoints(); a.data.forEach(function (d, g) { if (d && d.options && "undefined" !== typeof d.name) { var c = b.nameToX(d); "undefined" !== typeof c && c !== d.x && (d.x = c, a.xData[g] = c) } })
                }))
            }; a.prototype.setAxisTranslation = function () {
                var b = this, a = b.max - b.min, g = b.linkedParent, c = !!b.categories, k = b.isXAxis, e = b.axisPointRange || 0, l = 0, f = 0, h = b.transA; if (k || c || e) {
                    var F = b.getClosest(); g ? (l = g.minPointOffset, f = g.pointRangePadding) : b.series.forEach(function (a) {
                        var d = c ? 1 : k ? C(a.options.pointRange,
                        F, 0) : b.axisPointRange || 0, g = a.options.pointPlacement; e = Math.max(e, d); if (!b.single || c) a = a.is("xrange") ? !k : k, l = Math.max(l, a && r(g) ? 0 : d / 2), f = Math.max(f, a && "on" === g ? 0 : d)
                    }); g = b.ordinal && b.ordinal.slope && F ? b.ordinal.slope / F : 1; b.minPointOffset = l *= g; b.pointRangePadding = f *= g; b.pointRange = Math.min(e, b.single && c ? 1 : a); k && (b.closestPointRange = F)
                } b.translationSlope = b.transA = h = b.staticScale || b.len / (a + f || 1); b.transB = b.horiz ? b.left : b.bottom; b.minPixelPadding = h * l; d(this, "afterSetAxisTranslation")
            }; a.prototype.minFromRange =
            function () { return this.max - this.range }; a.prototype.setTickInterval = function (b) {
                var a = this, c = a.chart, r = a.logarithmic, e = a.options, f = a.isXAxis, h = a.isLinked, F = e.tickPixelInterval, z = a.categories, p = a.softThreshold, M = e.maxPadding, m = e.minPadding, t = e.tickInterval, q = g(a.threshold) ? a.threshold : null; a.dateTime || z || h || this.getTickAmount(); var w = C(a.userMin, e.min); var O = C(a.userMax, e.max); if (h) {
                    a.linkedParent = c[a.coll][e.linkedTo]; var J = a.linkedParent.getExtremes(); a.min = C(J.min, J.dataMin); a.max = C(J.max, J.dataMax);
                    e.type !== a.linkedParent.options.type && A(11, 1, c)
                } else { if (p && l(q)) if (a.dataMin >= q) J = q, m = 0; else if (a.dataMax <= q) { var v = q; M = 0 } a.min = C(w, J, a.dataMin); a.max = C(O, v, a.dataMax) } r && (a.positiveValuesOnly && !b && 0 >= Math.min(a.min, C(a.dataMin, a.min)) && A(10, 1, c), a.min = n(r.log2lin(a.min), 16), a.max = n(r.log2lin(a.max), 16)); a.range && l(a.max) && (a.userMin = a.min = w = Math.max(a.dataMin, a.minFromRange()), a.userMax = O = a.max, a.range = null); d(a, "foundExtremes"); a.beforePadding && a.beforePadding(); a.adjustForMinRange(); !(z || a.axisPointRange ||
                a.stacking && a.stacking.usePercentage || h) && l(a.min) && l(a.max) && (c = a.max - a.min) && (!l(w) && m && (a.min -= c * m), !l(O) && M && (a.max += c * M)); g(a.userMin) || (g(e.softMin) && e.softMin < a.min && (a.min = w = e.softMin), g(e.floor) && (a.min = Math.max(a.min, e.floor))); g(a.userMax) || (g(e.softMax) && e.softMax > a.max && (a.max = O = e.softMax), g(e.ceiling) && (a.max = Math.min(a.max, e.ceiling))); p && l(a.dataMin) && (q = q || 0, !l(w) && a.min < q && a.dataMin >= q ? a.min = a.options.minRange ? Math.min(q, a.max - a.minRange) : q : !l(O) && a.max > q && a.dataMax <= q && (a.max = a.options.minRange ?
                Math.max(q, a.min + a.minRange) : q)); g(a.min) && g(a.max) && !this.chart.polar && a.min > a.max && (l(a.options.min) ? a.max = a.min : l(a.options.max) && (a.min = a.max)); a.tickInterval = a.min === a.max || "undefined" === typeof a.min || "undefined" === typeof a.max ? 1 : h && a.linkedParent && !t && F === a.linkedParent.options.tickPixelInterval ? t = a.linkedParent.tickInterval : C(t, this.tickAmount ? (a.max - a.min) / Math.max(this.tickAmount - 1, 1) : void 0, z ? 1 : (a.max - a.min) * F / Math.max(a.len, F)); f && !b && a.series.forEach(function (b) {
                    b.processData(a.min !==
                    (a.old && a.old.min) || a.max !== (a.old && a.old.max))
                }); a.setAxisTranslation(); d(this, "initialAxisTranslation"); a.pointRange && !t && (a.tickInterval = Math.max(a.pointRange, a.tickInterval)); b = C(e.minTickInterval, a.dateTime && !a.series.some(function (b) { return b.noSharedTooltip }) ? a.closestPointRange : 0); !t && a.tickInterval < b && (a.tickInterval = b); a.dateTime || a.logarithmic || t || (a.tickInterval = D(a.tickInterval, void 0, k(a.tickInterval), C(e.allowDecimals, .5 > a.tickInterval || void 0 !== this.tickAmount), !!this.tickAmount));
                this.tickAmount || (a.tickInterval = a.unsquish()); this.setTickPositions()
            }; a.prototype.setTickPositions = function () {
                var b = this.options, a = b.tickPositions, g = this.getMinorTickInterval(), c = this.hasVerticalPanning(), k = "colorAxis" === this.coll, e = (k || !c) && b.startOnTick; c = (k || !c) && b.endOnTick; k = b.tickPositioner; this.tickmarkOffset = this.categories && "between" === b.tickmarkPlacement && 1 === this.tickInterval ? .5 : 0; this.minorTickInterval = "auto" === g && this.tickInterval ? this.tickInterval / 5 : g; this.single = this.min === this.max &&
                l(this.min) && !this.tickAmount && (parseInt(this.min, 10) === this.min || !1 !== b.allowDecimals); this.tickPositions = g = a && a.slice(); !g && (this.ordinal && this.ordinal.positions || !((this.max - this.min) / this.tickInterval > Math.max(2 * this.len, 200)) ? g = this.dateTime ? this.getTimeTicks(this.dateTime.normalizeTimeTickInterval(this.tickInterval, b.units), this.min, this.max, b.startOfWeek, this.ordinal && this.ordinal.positions, this.closestPointRange, !0) : this.logarithmic ? this.logarithmic.getLogTickPositions(this.tickInterval, this.min,
                this.max) : this.getLinearTickPositions(this.tickInterval, this.min, this.max) : (g = [this.min, this.max], A(19, !1, this.chart)), g.length > this.len && (g = [g[0], g.pop()], g[0] === g[1] && (g.length = 1)), this.tickPositions = g, k && (k = k.apply(this, [this.min, this.max]))) && (this.tickPositions = g = k); this.paddedTicks = g.slice(0); this.trimTicks(g, e, c); this.isLinked || (this.single && 2 > g.length && !this.categories && !this.series.some(function (b) { return b.is("heatmap") && "between" === b.options.pointPlacement }) && (this.min -= .5, this.max += .5),
                a || k || this.adjustTickAmount()); d(this, "afterSetTickPositions")
            }; a.prototype.trimTicks = function (b, a, g) { var c = b[0], k = b[b.length - 1], e = !this.isOrdinal && this.minPointOffset || 0; d(this, "trimTicks"); if (!this.isLinked) { if (a && -Infinity !== c) this.min = c; else for (; this.min - e > b[0];) b.shift(); if (g) this.max = k; else for (; this.max + e < b[b.length - 1];) b.pop(); 0 === b.length && l(c) && !this.options.tickPositions && b.push((k + c) / 2) } }; a.prototype.alignToOthers = function () {
                var b = {}, a = this.options, d; !1 !== this.chart.options.chart.alignTicks &&
                a.alignTicks && !1 !== a.startOnTick && !1 !== a.endOnTick && !this.logarithmic && this.chart[this.coll].forEach(function (a) { var g = a.options; g = [a.horiz ? g.left : g.top, g.width, g.height, g.pane].join(); a.series.length && (b[g] ? d = !0 : b[g] = 1) }); return d
            }; a.prototype.getTickAmount = function () {
                var b = this.options, a = b.tickPixelInterval, d = b.tickAmount; !l(b.tickInterval) && !d && this.len < a && !this.isRadial && !this.logarithmic && b.startOnTick && b.endOnTick && (d = 2); !d && this.alignToOthers() && (d = Math.ceil(this.len / a) + 1); 4 > d && (this.finalTickAmt =
                d, d = 5); this.tickAmount = d
            }; a.prototype.adjustTickAmount = function () {
                var b = this.options, a = this.tickInterval, d = this.tickPositions, c = this.tickAmount, k = this.finalTickAmt, e = d && d.length, r = C(this.threshold, this.softThreshold ? 0 : null); if (this.hasData() && g(this.min) && g(this.max)) {
                    if (e < c) {
                        for (; d.length < c;) d.length % 2 || this.min === r ? d.push(n(d[d.length - 1] + a)) : d.unshift(n(d[0] - a)); this.transA *= (e - 1) / (c - 1); this.min = b.startOnTick ? d[0] : Math.min(this.min, d[0]); this.max = b.endOnTick ? d[d.length - 1] : Math.max(this.max, d[d.length -
                        1])
                    } else e > c && (this.tickInterval *= 2, this.setTickPositions()); if (l(k)) { for (a = b = d.length; a--;) (3 === k && 1 === a % 2 || 2 >= k && 0 < a && a < b - 1) && d.splice(a, 1); this.finalTickAmt = void 0 }
                }
            }; a.prototype.setScale = function () {
                var b = !1, a = !1; this.series.forEach(function (d) { b = b || d.isDirtyData || d.isDirty; a = a || d.xAxis && d.xAxis.isDirty || !1 }); this.setAxisSize(); var g = this.len !== (this.old && this.old.len); g || b || a || this.isLinked || this.forceRedraw || this.userMin !== (this.old && this.old.userMin) || this.userMax !== (this.old && this.old.userMax) ||
                this.alignToOthers() ? (this.stacking && this.stacking.resetStacks(), this.forceRedraw = !1, this.getSeriesExtremes(), this.setTickInterval(), this.isDirty || (this.isDirty = g || this.min !== (this.old && this.old.min) || this.max !== (this.old && this.old.max))) : this.stacking && this.stacking.cleanStacks(); b && this.panningState && (this.panningState.isDirty = !0); d(this, "afterSetScale")
            }; a.prototype.setExtremes = function (b, a, g, c, k) {
                var e = this, r = e.chart; g = C(g, !0); e.series.forEach(function (b) { delete b.kdTree }); k = p(k, { min: b, max: a });
                d(e, "setExtremes", k, function () { e.userMin = b; e.userMax = a; e.eventArgs = k; g && r.redraw(c) })
            }; a.prototype.zoom = function (b, a) {
                var g = this, c = this.dataMin, k = this.dataMax, e = this.options, r = Math.min(c, C(e.min, c)), f = Math.max(k, C(e.max, k)); b = { newMin: b, newMax: a }; d(this, "zoom", b, function (b) {
                    var a = b.newMin, d = b.newMax; if (a !== g.min || d !== g.max) g.allowZoomOutside || (l(c) && (a < r && (a = r), a > f && (a = f)), l(k) && (d < r && (d = r), d > f && (d = f))), g.displayBtn = "undefined" !== typeof a || "undefined" !== typeof d, g.setExtremes(a, d, !1, void 0, { trigger: "zoom" });
                    b.zoomed = !0
                }); return b.zoomed
            }; a.prototype.setAxisSize = function () {
                var b = this.chart, a = this.options, d = a.offsets || [0, 0, 0, 0], g = this.horiz, c = this.width = Math.round(O(C(a.width, b.plotWidth - d[3] + d[1]), b.plotWidth)), k = this.height = Math.round(O(C(a.height, b.plotHeight - d[0] + d[2]), b.plotHeight)), e = this.top = Math.round(O(C(a.top, b.plotTop + d[0]), b.plotHeight, b.plotTop)); a = this.left = Math.round(O(C(a.left, b.plotLeft + d[3]), b.plotWidth, b.plotLeft)); this.bottom = b.chartHeight - k - e; this.right = b.chartWidth - c - a; this.len = Math.max(g ?
                    c : k, 0); this.pos = g ? a : e
            }; a.prototype.getExtremes = function () { var b = this.logarithmic; return { min: b ? n(b.lin2log(this.min)) : this.min, max: b ? n(b.lin2log(this.max)) : this.max, dataMin: this.dataMin, dataMax: this.dataMax, userMin: this.userMin, userMax: this.userMax } }; a.prototype.getThreshold = function (b) { var a = this.logarithmic, d = a ? a.lin2log(this.min) : this.min; a = a ? a.lin2log(this.max) : this.max; null === b || -Infinity === b ? b = d : Infinity === b ? b = a : d > b ? b = d : a < b && (b = a); return this.translate(b, 0, 1, 0, 1) }; a.prototype.autoLabelAlign =
            function (b) { var a = (C(b, 0) - 90 * this.side + 720) % 360; b = { align: "center" }; d(this, "autoLabelAlign", b, function (b) { 15 < a && 165 > a ? b.align = "right" : 195 < a && 345 > a && (b.align = "left") }); return b.align }; a.prototype.tickSize = function (b) { var a = this.options, g = C(a["tick" === b ? "tickWidth" : "minorTickWidth"], "tick" === b && this.isXAxis && !this.categories ? 1 : 0), c = a["tick" === b ? "tickLength" : "minorTickLength"]; if (g && c) { "inside" === a[b + "Position"] && (c = -c); var k = [c, g] } b = { tickSize: k }; d(this, "afterTickSize", b); return b.tickSize }; a.prototype.labelMetrics =
            function () { var b = this.tickPositions && this.tickPositions[0] || 0; return this.chart.renderer.fontMetrics(this.options.labels.style.fontSize, this.ticks[b] && this.ticks[b].label) }; a.prototype.unsquish = function () {
                var b = this.options.labels, a = this.horiz, d = this.tickInterval, k = this.len / (((this.categories ? 1 : 0) + this.max - this.min) / d), e = b.rotation, r = this.labelMetrics(), l = Math.max(this.max - this.min, 0), f = function (b) {
                    var a = b / (k || 1); a = 1 < a ? Math.ceil(a) : 1; a * d > l && Infinity !== b && Infinity !== k && l && (a = Math.ceil(l / d)); return n(a *
                    d)
                }, h = d, F, D, p = Number.MAX_VALUE; if (a) { if (!b.staggerLines && !b.step) if (g(e)) var M = [e]; else k < b.autoRotationLimit && (M = b.autoRotation); M && M.forEach(function (b) { if (b === e || b && -90 <= b && 90 >= b) { D = f(Math.abs(r.h / Math.sin(c * b))); var a = D + Math.abs(b / 360); a < p && (p = a, F = b, h = D) } }) } else b.step || (h = f(r.h)); this.autoRotation = M; this.labelRotation = C(F, g(e) ? e : 0); return h
            }; a.prototype.getSlotWidth = function (b) {
                var a = this.chart, d = this.horiz, c = this.options.labels, k = Math.max(this.tickPositions.length - (this.categories ? 0 : 1), 1), e =
                a.margin[3]; if (b && g(b.slotWidth)) return b.slotWidth; if (d && 2 > c.step) return c.rotation ? 0 : (this.staggerLines || 1) * this.len / k; if (!d) { b = c.style.width; if (void 0 !== b) return parseInt(String(b), 10); if (e) return e - a.spacing[3] } return .33 * a.chartWidth
            }; a.prototype.renderUnsquish = function () {
                var b = this.chart, a = b.renderer, d = this.tickPositions, g = this.ticks, c = this.options.labels, k = c.style, e = this.horiz, l = this.getSlotWidth(), f = Math.max(1, Math.round(l - 2 * c.padding)), h = {}, F = this.labelMetrics(), D = k.textOverflow, C = 0; r(c.rotation) ||
                (h.rotation = c.rotation || 0); d.forEach(function (b) { b = g[b]; b.movedLabel && b.replaceMovedLabel(); b && b.label && b.label.textPxLength > C && (C = b.label.textPxLength) }); this.maxLabelLength = C; if (this.autoRotation) C > f && C > F.h ? h.rotation = this.labelRotation : this.labelRotation = 0; else if (l) {
                    var p = f; if (!D) {
                        var M = "clip"; for (f = d.length; !e && f--;) {
                            var n = d[f]; if (n = g[n].label) n.styles && "ellipsis" === n.styles.textOverflow ? n.css({ textOverflow: "clip" }) : n.textPxLength > l && n.css({ width: l + "px" }), n.getBBox().height > this.len / d.length -
                            (F.h - F.f) && (n.specificTextOverflow = "ellipsis")
                        }
                    }
                } h.rotation && (p = C > .5 * b.chartHeight ? .33 * b.chartHeight : C, D || (M = "ellipsis")); if (this.labelAlign = c.align || this.autoLabelAlign(this.labelRotation)) h.align = this.labelAlign; d.forEach(function (b) {
                    var a = (b = g[b]) && b.label, d = k.width, c = {}; a && (a.attr(h), b.shortenLabel ? b.shortenLabel() : p && !d && "nowrap" !== k.whiteSpace && (p < a.textPxLength || "SPAN" === a.element.tagName) ? (c.width = p + "px", D || (c.textOverflow = a.specificTextOverflow || M), a.css(c)) : a.styles && a.styles.width && !c.width &&
                    !d && a.css({ width: null }), delete a.specificTextOverflow, b.rotation = h.rotation)
                }, this); this.tickRotCorr = a.rotCorr(F.b, this.labelRotation || 0, 0 !== this.side)
            }; a.prototype.hasData = function () { return this.series.some(function (b) { return b.hasData() }) || this.options.showEmpty && l(this.min) && l(this.max) }; a.prototype.addTitle = function (b) {
                var a = this.chart.renderer, d = this.horiz, g = this.opposite, c = this.options.title, k = this.chart.styledMode, e; this.axisTitle || ((e = c.textAlign) || (e = (d ? { low: "left", middle: "center", high: "right" } :
{ low: g ? "right" : "left", middle: "center", high: g ? "left" : "right" })[c.align]), this.axisTitle = a.text(c.text || "", 0, 0, c.useHTML).attr({ zIndex: 7, rotation: c.rotation, align: e }).addClass("highcharts-axis-title"), k || this.axisTitle.css(F(c.style)), this.axisTitle.add(this.axisGroup), this.axisTitle.isNew = !0); k || c.style.width || this.isRadial || this.axisTitle.css({ width: this.len + "px" }); this.axisTitle[b ? "show" : "hide"](b)
            }; a.prototype.generateTick = function (b) { var a = this.ticks; a[b] ? a[b].addLabel() : a[b] = new q(this, b) }; a.prototype.getOffset =
            function () {
                var b = this, a = this, g = a.chart, c = g.renderer, k = a.options, e = a.tickPositions, r = a.ticks, f = a.horiz, h = a.side, F = g.inverted && !a.isZAxis ? [1, 0, 3, 2][h] : h, D = a.hasData(), p = k.title, n = k.labels, m = g.axisOffset; g = g.clipOffset; var t = [-1, 1, 1, -1][h], q = k.className, w = a.axisParent, O, Z = 0, ha = 0, A = 0; a.showAxis = O = D || k.showEmpty; a.staggerLines = a.horiz && n.staggerLines || void 0; if (!a.axisGroup) {
                    var J = function (a, d, g) {
                        return c.g(a).attr({ zIndex: g }).addClass("highcharts-" + b.coll.toLowerCase() + d + " " + (b.isRadial ? "highcharts-radial-axis" +
                        d + " " : "") + (q || "")).add(w)
                    }; a.gridGroup = J("grid", "-grid", k.gridZIndex); a.axisGroup = J("axis", "", k.zIndex); a.labelGroup = J("axis-labels", "-labels", n.zIndex)
                } D || a.isLinked ? (e.forEach(function (b) { a.generateTick(b) }), a.renderUnsquish(), a.reserveSpaceDefault = 0 === h || 2 === h || { 1: "left", 3: "right" }[h] === a.labelAlign, C(n.reserveSpace, "center" === a.labelAlign ? !0 : null, a.reserveSpaceDefault) && e.forEach(function (b) { A = Math.max(r[b].getLabelSize(), A) }), a.staggerLines && (A *= a.staggerLines), a.labelOffset = A * (a.opposite ? -1 :
                1)) : M(r, function (b, a) { b.destroy(); delete r[a] }); if (p && p.text && !1 !== p.enabled && (a.addTitle(O), O && !1 !== p.reserveSpace)) { a.titleOffset = Z = a.axisTitle.getBBox()[f ? "height" : "width"]; var v = p.offset; ha = l(v) ? 0 : C(p.margin, f ? 5 : 10) } a.renderLine(); a.offset = t * C(k.offset, m[h] ? m[h] + (k.margin || 0) : 0); a.tickRotCorr = a.tickRotCorr || { x: 0, y: 0 }; p = 0 === h ? -a.labelMetrics().h : 2 === h ? a.tickRotCorr.y : 0; D = Math.abs(A) + ha; A && (D = D - p + t * (f ? C(n.y, a.tickRotCorr.y + 8 * t) : n.x)); a.axisTitleMargin = C(v, D); a.getMaxLabelDimensions && (a.maxLabelDimensions =
                a.getMaxLabelDimensions(r, e)); f = this.tickSize("tick"); m[h] = Math.max(m[h], (a.axisTitleMargin || 0) + Z + t * a.offset, D, e && e.length && f ? f[0] + t * a.offset : 0); k = k.offset ? 0 : 2 * Math.floor(a.axisLine.strokeWidth() / 2); g[F] = Math.max(g[F], k); d(this, "afterGetOffset")
            }; a.prototype.getLinePath = function (b) {
                var a = this.chart, d = this.opposite, g = this.offset, c = this.horiz, k = this.left + (d ? this.width : 0) + g; g = a.chartHeight - this.bottom - (d ? this.height : 0) + g; d && (b *= -1); return a.renderer.crispLine([["M", c ? this.left : k, c ? g : this.top], ["L", c ?
                a.chartWidth - this.right : k, c ? g : a.chartHeight - this.bottom]], b)
            }; a.prototype.renderLine = function () { this.axisLine || (this.axisLine = this.chart.renderer.path().addClass("highcharts-axis-line").add(this.axisGroup), this.chart.styledMode || this.axisLine.attr({ stroke: this.options.lineColor, "stroke-width": this.options.lineWidth, zIndex: 7 })) }; a.prototype.getTitlePosition = function () {
                var b = this.horiz, a = this.left, g = this.top, c = this.len, k = this.options.title, e = b ? a : g, r = this.opposite, l = this.offset, f = k.x, h = k.y, F = this.axisTitle,
                D = this.chart.renderer.fontMetrics(k.style.fontSize, F); F = Math.max(F.getBBox(null, 0).height - D.h - 1, 0); c = { low: e + (b ? 0 : c), middle: e + c / 2, high: e + (b ? c : 0) }[k.align]; a = (b ? g + this.height : a) + (b ? 1 : -1) * (r ? -1 : 1) * this.axisTitleMargin + [-F, F, D.f, -F][this.side]; b = { x: b ? c + f : a + (r ? this.width : 0) + l + f, y: b ? a + h - (r ? this.height : 0) + l : c + h }; d(this, "afterGetTitlePosition", { titlePosition: b }); return b
            }; a.prototype.renderMinorTick = function (b, a) {
                var d = this.minorTicks; d[b] || (d[b] = new q(this, b, "minor")); a && d[b].isNew && d[b].render(null, !0);
                d[b].render(null, !1, 1)
            }; a.prototype.renderTick = function (b, a, d) { var g = this.ticks; if (!this.isLinked || b >= this.min && b <= this.max || this.grid && this.grid.isColumn) g[b] || (g[b] = new q(this, b)), d && g[b].isNew && g[b].render(a, !0, -1), g[b].render(a) }; a.prototype.render = function () {
                var b = this, a = b.chart, c = b.logarithmic, k = b.options, e = b.isLinked, r = b.tickPositions, l = b.axisTitle, f = b.ticks, F = b.minorTicks, D = b.alternateBands, p = k.stackLabels, C = k.alternateGridColor, n = b.tickmarkOffset, m = b.axisLine, t = b.showAxis, w = h(a.renderer.globalAnimation),
                O, A; b.labelEdge.length = 0; b.overlap = !1;[f, F, D].forEach(function (b) { M(b, function (b) { b.isActive = !1 }) }); if (b.hasData() || e) {
                    var Z = b.chart.hasRendered && b.old && g(b.old.min); b.minorTickInterval && !b.categories && b.getMinorTickPositions().forEach(function (a) { b.renderMinorTick(a, Z) }); r.length && (r.forEach(function (a, d) { b.renderTick(a, d, Z) }), n && (0 === b.min || b.single) && (f[-1] || (f[-1] = new q(b, -1, null, !0)), f[-1].render(-1))); C && r.forEach(function (d, g) {
                        A = "undefined" !== typeof r[g + 1] ? r[g + 1] + n : b.max - n; 0 === g % 2 && d < b.max &&
                        A <= b.max + (a.polar ? -n : n) && (D[d] || (D[d] = new y.PlotLineOrBand(b)), O = d + n, D[d].options = { from: c ? c.lin2log(O) : O, to: c ? c.lin2log(A) : A, color: C, className: "highcharts-alternate-grid" }, D[d].render(), D[d].isActive = !0)
                    }); b._addedPlotLB || (b._addedPlotLB = !0, (k.plotLines || []).concat(k.plotBands || []).forEach(function (a) { b.addPlotBandOrLine(a) }))
                } [f, F, D].forEach(function (b) {
                    var d = [], g = w.duration; M(b, function (b, a) { b.isActive || (b.render(a, !1, 0), b.isActive = !1, d.push(a)) }); Y(function () {
                        for (var a = d.length; a--;) b[d[a]] && !b[d[a]].isActive &&
                        (b[d[a]].destroy(), delete b[d[a]])
                    }, b !== D && a.hasRendered && g ? g : 0)
                }); m && (m[m.isPlaced ? "animate" : "attr"]({ d: this.getLinePath(m.strokeWidth()) }), m.isPlaced = !0, m[t ? "show" : "hide"](t)); l && t && (k = b.getTitlePosition(), g(k.y) ? (l[l.isNew ? "attr" : "animate"](k), l.isNew = !1) : (l.attr("y", -9999), l.isNew = !0)); p && p.enabled && b.stacking && b.stacking.renderStackTotals(); b.old = { len: b.len, max: b.max, min: b.min, transA: b.transA, userMax: b.userMax, userMin: b.userMin }; b.isDirty = !1; d(this, "afterRender")
            }; a.prototype.redraw = function () {
                this.visible &&
                (this.render(), this.plotLinesAndBands.forEach(function (b) { b.render() })); this.series.forEach(function (b) { b.isDirty = !0 })
            }; a.prototype.getKeepProps = function () { return this.keepProps || a.keepProps }; a.prototype.destroy = function (b) {
                var a = this, g = a.plotLinesAndBands, c = this.eventOptions; d(this, "destroy", { keepEvents: b }); b || S(a);[a.ticks, a.minorTicks, a.alternateBands].forEach(function (b) { J(b) }); if (g) for (b = g.length; b--;) g[b].destroy(); "axisLine axisTitle axisGroup gridGroup labelGroup cross scrollbar".split(" ").forEach(function (b) {
                    a[b] &&
                    (a[b] = a[b].destroy())
                }); for (var k in a.plotLinesAndBandsGroups) a.plotLinesAndBandsGroups[k] = a.plotLinesAndBandsGroups[k].destroy(); M(a, function (b, d) { -1 === a.getKeepProps().indexOf(d) && delete a[d] }); this.eventOptions = c
            }; a.prototype.drawCrosshair = function (b, a) {
                var g = this.crosshair, c = C(g && g.snap, !0), k = this.chart, e, r = this.cross; d(this, "drawCrosshair", { e: b, point: a }); b || (b = this.cross && this.cross.e); if (g && !1 !== (l(a) || !c)) {
                    c ? l(a) && (e = C("colorAxis" !== this.coll ? a.crosshairPos : null, this.isXAxis ? a.plotX : this.len -
                    a.plotY)) : e = b && (this.horiz ? b.chartX - this.pos : this.len - b.chartY + this.pos); if (l(e)) { var f = { value: a && (this.isXAxis ? a.x : C(a.stackY, a.y)), translatedValue: e }; k.polar && p(f, { isCrosshair: !0, chartX: b && b.chartX, chartY: b && b.chartY, point: a }); f = this.getPlotLinePath(f) || null } if (!l(f)) { this.hideCrosshair(); return } c = this.categories && !this.isRadial; r || (this.cross = r = k.renderer.path().addClass("highcharts-crosshair highcharts-crosshair-" + (c ? "category " : "thin ") + (g.className || "")).attr({ zIndex: C(g.zIndex, 2) }).add(), k.styledMode ||
                    (r.attr({ stroke: g.color || (c ? u.parse(E.highlightColor20).setOpacity(.25).get() : E.neutralColor20), "stroke-width": C(g.width, 1) }).css({ "pointer-events": "none" }), g.dashStyle && r.attr({ dashstyle: g.dashStyle }))); r.show().attr({ d: f }); c && !g.width && r.attr({ "stroke-width": this.transA }); this.cross.e = b
                } else this.hideCrosshair(); d(this, "afterDrawCrosshair", { e: b, point: a })
            }; a.prototype.hideCrosshair = function () { this.cross && this.cross.hide(); d(this, "afterHideCrosshair") }; a.prototype.hasVerticalPanning = function () {
                var b =
                this.chart.options.chart.panning; return !!(b && b.enabled && /y/.test(b.type))
            }; a.prototype.validatePositiveValue = function (b) { return g(b) && 0 < b }; a.prototype.update = function (b, a) { var d = this.chart; b = F(this.userOptions, b); this.destroy(!0); this.init(d, b); d.isDirtyBox = !0; C(a, !0) && d.redraw() }; a.prototype.remove = function (b) {
                for (var a = this.chart, d = this.coll, g = this.series, c = g.length; c--;) g[c] && g[c].remove(!1); v(a.axes, this); v(a[d], this); a[d].forEach(function (b, a) { b.options.index = b.userOptions.index = a }); this.destroy();
                a.isDirtyBox = !0; C(b, !0) && a.redraw()
            }; a.prototype.setTitle = function (b, a) { this.update({ title: b }, a) }; a.prototype.setCategories = function (b, a) { this.update({ categories: b }, a) }; a.defaultOptions = B.defaultXAxisOptions; a.keepProps = "extKey hcEvents names series userMax userMin".split(" "); return a
        }(); ""; return a
    }); L(a, "Core/Axis/DateTimeAxis.js", [a["Core/Axis/Axis.js"], a["Core/Utilities.js"]], function (a, u) {
        var v = u.addEvent, H = u.getMagnitude, x = u.normalizeTickInterval, y = u.timeUnits, G = function () {
            function a(a) {
                this.axis =
                a
            } a.prototype.normalizeTimeTickInterval = function (a, h) { var f = h || [["millisecond", [1, 2, 5, 10, 20, 25, 50, 100, 200, 500]], ["second", [1, 2, 5, 10, 15, 30]], ["minute", [1, 2, 5, 10, 15, 30]], ["hour", [1, 2, 3, 4, 6, 8, 12]], ["day", [1, 2]], ["week", [1, 2]], ["month", [1, 2, 3, 4, 6]], ["year", null]]; h = f[f.length - 1]; var c = y[h[0]], e = h[1], t; for (t = 0; t < f.length && !(h = f[t], c = y[h[0]], e = h[1], f[t + 1] && a <= (c * e[e.length - 1] + y[f[t + 1][0]]) / 2) ; t++); c === y.year && a < 5 * c && (e = [1, 2, 5]); a = x(a / c, e, "year" === h[0] ? Math.max(H(a / c), 1) : 1); return { unitRange: c, count: a, unitName: h[0] } };
            return a
        }(); u = function () { function a() { } a.compose = function (a) { a.keepProps.push("dateTime"); a.prototype.getTimeTicks = function () { return this.chart.time.getTimeTicks.apply(this.chart.time, arguments) }; v(a, "init", function (a) { "datetime" !== a.userOptions.type ? this.dateTime = void 0 : this.dateTime || (this.dateTime = new G(this)) }) }; a.AdditionsClass = G; return a }(); u.compose(a); return u
    }); L(a, "Core/Axis/LogarithmicAxis.js", [a["Core/Axis/Axis.js"], a["Core/Utilities.js"]], function (a, u) {
        var v = u.addEvent, H = u.getMagnitude,
        x = u.normalizeTickInterval, y = u.pick, G = function () {
            function a(a) { this.axis = a } a.prototype.getLogTickPositions = function (a, h, f, c) {
                var e = this.axis, t = e.len, m = e.options, w = []; c || (this.minorAutoInterval = void 0); if (.5 <= a) a = Math.round(a), w = e.getLinearTickPositions(a, h, f); else if (.08 <= a) {
                    var n = Math.floor(h), l, q = m = void 0; for (t = .3 < a ? [1, 2, 4] : .15 < a ? [1, 2, 4, 6, 8] : [1, 2, 3, 4, 5, 6, 7, 8, 9]; n < f + 1 && !q; n++) {
                        var v = t.length; for (l = 0; l < v && !q; l++) {
                            var A = this.log2lin(this.lin2log(n) * t[l]); A > h && (!c || m <= f) && "undefined" !== typeof m && w.push(m);
                            m > f && (q = !0); m = A
                        }
                    }
                } else h = this.lin2log(h), f = this.lin2log(f), a = c ? e.getMinorTickInterval() : m.tickInterval, a = y("auto" === a ? null : a, this.minorAutoInterval, m.tickPixelInterval / (c ? 5 : 1) * (f - h) / ((c ? t / e.tickPositions.length : t) || 1)), a = x(a, void 0, H(a)), w = e.getLinearTickPositions(a, h, f).map(this.log2lin), c || (this.minorAutoInterval = a / 5); c || (e.tickInterval = a); return w
            }; a.prototype.lin2log = function (a) { return Math.pow(10, a) }; a.prototype.log2lin = function (a) { return Math.log(a) / Math.LN10 }; return a
        }(); u = function () {
            function a() { }
            a.compose = function (a) { a.keepProps.push("logarithmic"); v(a, "init", function (a) { var f = this.logarithmic; "logarithmic" !== a.userOptions.type ? this.logarithmic = void 0 : f || (this.logarithmic = new G(this)) }); v(a, "afterInit", function () { var a = this.logarithmic; a && (this.lin2val = function (f) { return a.lin2log(f) }, this.val2lin = function (f) { return a.log2lin(f) }) }) }; return a
        }(); u.compose(a); return u
    }); L(a, "Core/Axis/PlotLineOrBand.js", [a["Core/Axis/Axis.js"], a["Core/Color/Palette.js"], a["Core/Utilities.js"]], function (a,
    u, E) {
        var v = E.arrayMax, x = E.arrayMin, y = E.defined, G = E.destroyObjectProperties, B = E.erase, q = E.extend, h = E.fireEvent, f = E.isNumber, c = E.merge, e = E.objectEach, t = E.pick, m = function () {
            function a(a, c) { this.axis = a; c && (this.options = c, this.id = c.id) } a.prototype.render = function () {
                h(this, "render"); var a = this, l = a.axis, f = l.horiz, m = l.logarithmic, w = a.options, p = w.label, d = a.label, k = w.to, b = w.from, g = w.value, r = y(b) && y(k), F = y(g), D = a.svgElem, M = !D, C = [], q = w.color, v = t(w.zIndex, 0), W = w.events; C = {
                    "class": "highcharts-plot-" + (r ? "band " :
                    "line ") + (w.className || "")
                }; var B = {}, x = l.chart.renderer, z = r ? "bands" : "lines"; m && (b = m.log2lin(b), k = m.log2lin(k), g = m.log2lin(g)); l.chart.styledMode || (F ? (C.stroke = q || u.neutralColor40, C["stroke-width"] = t(w.width, 1), w.dashStyle && (C.dashstyle = w.dashStyle)) : r && (C.fill = q || u.highlightColor10, w.borderWidth && (C.stroke = w.borderColor, C["stroke-width"] = w.borderWidth))); B.zIndex = v; z += "-" + v; (m = l.plotLinesAndBandsGroups[z]) || (l.plotLinesAndBandsGroups[z] = m = x.g("plot-" + z).attr(B).add()); M && (a.svgElem = D = x.path().attr(C).add(m));
                if (F) C = l.getPlotLinePath({ value: g, lineWidth: D.strokeWidth(), acrossPanes: w.acrossPanes }); else if (r) C = l.getPlotBandPath(b, k, w); else return; !a.eventsAdded && W && (e(W, function (b, d) { D.on(d, function (b) { W[d].apply(a, [b]) }) }), a.eventsAdded = !0); (M || !D.d) && C && C.length ? D.attr({ d: C }) : D && (C ? (D.show(!0), D.animate({ d: C })) : D.d && (D.hide(), d && (a.label = d = d.destroy()))); p && (y(p.text) || y(p.formatter)) && C && C.length && 0 < l.width && 0 < l.height && !C.isFlat ? (p = c({
                    align: f && r && "center", x: f ? !r && 4 : 10, verticalAlign: !f && r && "middle", y: f ?
                    r ? 16 : 10 : r ? 6 : -4, rotation: f && !r && 90
                }, p), this.renderLabel(p, C, r, v)) : d && d.hide(); return a
            }; a.prototype.renderLabel = function (a, c, e, f) {
                var l = this.label, h = this.axis.chart.renderer; l || (l = { align: a.textAlign || a.align, rotation: a.rotation, "class": "highcharts-plot-" + (e ? "band" : "line") + "-label " + (a.className || "") }, l.zIndex = f, f = this.getLabelText(a), this.label = l = h.text(f, 0, 0, a.useHTML).attr(l).add(), this.axis.chart.styledMode || l.css(a.style)); h = c.xBounds || [c[0][1], c[1][1], e ? c[2][1] : c[0][1]]; c = c.yBounds || [c[0][2],
                c[1][2], e ? c[2][2] : c[0][2]]; e = x(h); f = x(c); l.align(a, !1, { x: e, y: f, width: v(h) - e, height: v(c) - f }); l.show(!0)
            }; a.prototype.getLabelText = function (a) { return y(a.formatter) ? a.formatter.call(this) : a.text }; a.prototype.destroy = function () { B(this.axis.plotLinesAndBands, this); delete this.axis; G(this) }; return a
        }(); q(a.prototype, {
            getPlotBandPath: function (a, c, e) {
                void 0 === e && (e = this.options); var l = this.getPlotLinePath({ value: c, force: !0, acrossPanes: e.acrossPanes }); e = this.getPlotLinePath({ value: a, force: !0, acrossPanes: e.acrossPanes });
                var h = [], m = this.horiz, p = 1; a = !f(this.min) || !f(this.max) || a < this.min && c < this.min || a > this.max && c > this.max; if (e && l) { if (a) { var d = e.toString() === l.toString(); p = 0 } for (a = 0; a < e.length; a += 2) { c = e[a]; var k = e[a + 1], b = l[a], g = l[a + 1]; "M" !== c[0] && "L" !== c[0] || "M" !== k[0] && "L" !== k[0] || "M" !== b[0] && "L" !== b[0] || "M" !== g[0] && "L" !== g[0] || (m && b[1] === c[1] ? (b[1] += p, g[1] += p) : m || b[2] !== c[2] || (b[2] += p, g[2] += p), h.push(["M", c[1], c[2]], ["L", k[1], k[2]], ["L", g[1], g[2]], ["L", b[1], b[2]], ["Z"])); h.isFlat = d } } return h
            }, addPlotBand: function (a) {
                return this.addPlotBandOrLine(a,
                "plotBands")
            }, addPlotLine: function (a) { return this.addPlotBandOrLine(a, "plotLines") }, addPlotBandOrLine: function (a, c) { var e = this, f = new m(this, a), h = this.userOptions; this.visible && (f = f.render()); if (f) { this._addedPlotLB || (this._addedPlotLB = !0, (h.plotLines || []).concat(h.plotBands || []).forEach(function (a) { e.addPlotBandOrLine(a) })); if (c) { var t = h[c] || []; t.push(a); h[c] = t } this.plotLinesAndBands.push(f) } return f }, removePlotBandOrLine: function (a) {
                var c = this.plotLinesAndBands, e = this.options, f = this.userOptions;
                if (c) { for (var h = c.length; h--;) c[h].id === a && c[h].destroy();[e.plotLines || [], f.plotLines || [], e.plotBands || [], f.plotBands || []].forEach(function (c) { for (h = c.length; h--;) (c[h] || {}).id === a && B(c, c[h]) }) }
            }, removePlotBand: function (a) { this.removePlotBandOrLine(a) }, removePlotLine: function (a) { this.removePlotBandOrLine(a) }
        }); return m
    }); L(a, "Core/Tooltip.js", [a["Core/FormatUtilities.js"], a["Core/Globals.js"], a["Core/Color/Palette.js"], a["Core/Renderer/RendererRegistry.js"], a["Core/Utilities.js"]], function (a, u, E,
    H, x) {
        var v = a.format, G = u.doc, B = x.clamp, q = x.css, h = x.defined, f = x.discardElement, c = x.extend, e = x.fireEvent, t = x.isArray, m = x.isNumber, w = x.isString, n = x.merge, l = x.pick, J = x.splat, K = x.syncTimeout, A = x.timeUnits; ""; a = function () {
            function a(a, c) { this.container = void 0; this.crosshairs = []; this.distance = 0; this.isHidden = !0; this.isSticky = !1; this.now = {}; this.options = {}; this.outside = !1; this.chart = a; this.init(a, c) } a.prototype.applyFilter = function () {
                var a = this.chart; a.renderer.definition({
                    tagName: "filter", attributes: {
                        id: "drop-shadow-" +
                        a.index, opacity: .5
                    }, children: [{ tagName: "feGaussianBlur", attributes: { "in": "SourceAlpha", stdDeviation: 1 } }, { tagName: "feOffset", attributes: { dx: 1, dy: 1 } }, { tagName: "feComponentTransfer", children: [{ tagName: "feFuncA", attributes: { type: "linear", slope: .3 } }] }, { tagName: "feMerge", children: [{ tagName: "feMergeNode" }, { tagName: "feMergeNode", attributes: { "in": "SourceGraphic" } }] }]
                })
            }; a.prototype.bodyFormatter = function (a) {
                return a.map(function (a) {
                    var b = a.series.tooltipOptions; return (b[(a.point.formatPrefix || "point") + "Formatter"] ||
                    a.point.tooltipFormatter).call(a.point, b[(a.point.formatPrefix || "point") + "Format"] || "")
                })
            }; a.prototype.cleanSplit = function (a) { this.chart.series.forEach(function (d) { var b = d && d.tt; b && (!b.isActive || a ? d.tt = b.destroy() : b.isActive = !1) }) }; a.prototype.defaultFormatter = function (a) { var d = this.points || J(this); var b = [a.tooltipFooterHeaderFormatter(d[0])]; b = b.concat(a.bodyFormatter(d)); b.push(a.tooltipFooterHeaderFormatter(d[0], !0)); return b }; a.prototype.destroy = function () {
                this.label && (this.label = this.label.destroy());
                this.split && this.tt && (this.cleanSplit(this.chart, !0), this.tt = this.tt.destroy()); this.renderer && (this.renderer = this.renderer.destroy(), f(this.container)); x.clearTimeout(this.hideTimer); x.clearTimeout(this.tooltipTimeout)
            }; a.prototype.getAnchor = function (a, c) {
                var b = this.chart; var d = b.pointer; var k = b.inverted, e = b.plotTop, f = b.plotLeft, l = 0, h = 0, p, m; a = J(a); this.followPointer && c ? ("undefined" === typeof c.chartX && (c = d.normalize(c)), d = [c.chartX - f, c.chartY - e]) : a[0].tooltipPos ? d = a[0].tooltipPos : (a.forEach(function (a) {
                    p =
                    a.series.yAxis; m = a.series.xAxis; l += a.plotX || 0; h += a.plotLow ? (a.plotLow + (a.plotHigh || 0)) / 2 : a.plotY || 0; m && p && (k ? (l += e + b.plotHeight - m.len - m.pos, h += f + b.plotWidth - p.len - p.pos) : (l += m.pos - f, h += p.pos - e))
                }), l /= a.length, h /= a.length, d = [k ? b.plotWidth - h : l, k ? b.plotHeight - l : h], this.shared && 1 < a.length && c && (k ? d[0] = c.chartX - f : d[1] = c.chartY - e)); return d.map(Math.round)
            }; a.prototype.getDateFormat = function (a, c, b, g) {
                var d = this.chart.time, k = d.dateFormat("%m-%d %H:%M:%S.%L", c), e = {
                    millisecond: 15, second: 12, minute: 9, hour: 6,
                    day: 3
                }, l = "millisecond"; for (f in A) { if (a === A.week && +d.dateFormat("%w", c) === b && "00:00:00.000" === k.substr(6)) { var f = "week"; break } if (A[f] > a) { f = l; break } if (e[f] && k.substr(e[f]) !== "01-01 00:00:00.000".substr(e[f])) break; "week" !== f && (l = f) } if (f) var h = d.resolveDTLFormat(g[f]).main; return h
            }; a.prototype.getLabel = function () {
                var a = this, c = this.chart.renderer, b = this.chart.styledMode, g = this.options, e = "tooltip" + (h(g.className) ? " " + g.className : ""), f = g.style && g.style.pointerEvents || (!this.followPointer && g.stickOnContact ?
                "auto" : "none"), l, p = function () { a.inContact = !0 }, C = function () { var b = a.chart.hoverSeries; a.inContact = !1; if (b && b.onMouseOut) b.onMouseOut() }; if (!this.label) {
                    if (this.outside) {
                        var m = this.chart.options.chart.style, t = H.getRendererType(); this.container = l = u.doc.createElement("div"); l.className = "highcharts-tooltip-container"; q(l, { position: "absolute", top: "1px", pointerEvents: f, zIndex: Math.max(this.options.style && this.options.style.zIndex || 0, (m && m.zIndex || 0) + 3) }); u.doc.body.appendChild(l); this.renderer = c = new t(l,
                        0, 0, m, void 0, void 0, c.styledMode)
                    } this.split ? this.label = c.g(e) : (this.label = c.label("", 0, 0, g.shape || "callout", null, null, g.useHTML, null, e).attr({ padding: g.padding, r: g.borderRadius }), b || this.label.attr({ fill: g.backgroundColor, "stroke-width": g.borderWidth }).css(g.style).css({ pointerEvents: f }).shadow(g.shadow)); b && g.shadow && (this.applyFilter(), this.label.attr({ filter: "url(#drop-shadow-" + this.chart.index + ")" })); if (a.outside && !a.split) {
                        var n = this.label, w = n.xSetter, v = n.ySetter; n.xSetter = function (b) {
                            w.call(n,
                            a.distance); l.style.left = b + "px"
                        }; n.ySetter = function (b) { v.call(n, a.distance); l.style.top = b + "px" }
                    } this.label.on("mouseenter", p).on("mouseleave", C).attr({ zIndex: 8 }).add()
                } return this.label
            }; a.prototype.getPosition = function (a, c, b) {
                var d = this.chart, k = this.distance, e = {}, f = d.inverted && b.h || 0, h, C = this.outside, p = C ? G.documentElement.clientWidth - 2 * k : d.chartWidth, m = C ? Math.max(G.body.scrollHeight, G.documentElement.scrollHeight, G.body.offsetHeight, G.documentElement.offsetHeight, G.documentElement.clientHeight) :
                d.chartHeight, t = d.pointer.getChartPosition(), n = function (g) { var e = "x" === g; return [g, e ? p : m, e ? a : c].concat(C ? [e ? a * t.scaleX : c * t.scaleY, e ? t.left - k + (b.plotX + d.plotLeft) * t.scaleX : t.top - k + (b.plotY + d.plotTop) * t.scaleY, 0, e ? p : m] : [e ? a : c, e ? b.plotX + d.plotLeft : b.plotY + d.plotTop, e ? d.plotLeft : d.plotTop, e ? d.plotLeft + d.plotWidth : d.plotTop + d.plotHeight]) }, q = n("y"), w = n("x"), v = !this.followPointer && l(b.ttBelow, !d.inverted === !!b.negative), A = function (b, a, d, g, c, r, l) {
                    var h = C ? "y" === b ? k * t.scaleY : k * t.scaleX : k, F = (d - g) / 2, I = g < c - k,
                    D = c + k + g < a, p = c - h - d + F; c = c + h - F; if (v && D) e[b] = c; else if (!v && I) e[b] = p; else if (I) e[b] = Math.min(l - g, 0 > p - f ? p : p - f); else if (D) e[b] = Math.max(r, c + f + d > a ? c : c + f); else return !1
                }, J = function (b, a, d, g, c) { var r; c < k || c > a - k ? r = !1 : e[b] = c < d / 2 ? 1 : c > a - g / 2 ? a - g - 2 : c - d / 2; return r }, u = function (b) { var a = q; q = w; w = a; h = b }, I = function () { !1 !== A.apply(0, q) ? !1 !== J.apply(0, w) || h || (u(!0), I()) : h ? e.x = e.y = 0 : (u(!0), I()) }; (d.inverted || 1 < this.len) && u(); I(); return e
            }; a.prototype.getXDateFormat = function (a, c, b) {
                c = c.dateTimeLabelFormats; var d = b && b.closestPointRange;
                return (d ? this.getDateFormat(d, a.x, b.options.startOfWeek, c) : c.day) || c.year
            }; a.prototype.hide = function (a) { var d = this; x.clearTimeout(this.hideTimer); a = l(a, this.options.hideDelay, 500); this.isHidden || (this.hideTimer = K(function () { d.getLabel().fadeOut(a ? void 0 : a); d.isHidden = !0 }, a)) }; a.prototype.init = function (a, c) {
                this.chart = a; this.options = c; this.crosshairs = []; this.now = { x: 0, y: 0 }; this.isHidden = !0; this.split = c.split && !a.inverted && !a.polar; this.shared = c.shared || this.split; this.outside = l(c.outside, !(!a.scrollablePixelsX &&
                !a.scrollablePixelsY))
            }; a.prototype.isStickyOnContact = function () { return !(this.followPointer || !this.options.stickOnContact || !this.inContact) }; a.prototype.move = function (a, k, b, g) {
                var d = this, e = d.now, f = !1 !== d.options.animation && !d.isHidden && (1 < Math.abs(a - e.x) || 1 < Math.abs(k - e.y)), l = d.followPointer || 1 < d.len; c(e, { x: f ? (2 * e.x + a) / 3 : a, y: f ? (e.y + k) / 2 : k, anchorX: l ? void 0 : f ? (2 * e.anchorX + b) / 3 : b, anchorY: l ? void 0 : f ? (e.anchorY + g) / 2 : g }); d.getLabel().attr(e); d.drawTracker(); f && (x.clearTimeout(this.tooltipTimeout), this.tooltipTimeout =
                setTimeout(function () { d && d.move(a, k, b, g) }, 32))
            }; a.prototype.refresh = function (a, c) {
                var b = this.chart, d = this.options, k = J(a), f = k[0], h = {}, p = [], C = d.formatter || this.defaultFormatter; h = this.shared; var m = b.styledMode; if (d.enabled) {
                    x.clearTimeout(this.hideTimer); this.followPointer = !this.split && f.series.tooltipOptions.followPointer; var n = this.getAnchor(a, c); var q = n[0]; var w = n[1]; !h || !t(a) && a.series && a.series.noSharedTooltip ? h = f.getLabelConfig() : (b.pointer.applyInactiveState(k), k.forEach(function (b) {
                        b.setState("hover");
                        p.push(b.getLabelConfig())
                    }), h = { x: f.category, y: f.y }, h.points = p); this.len = p.length; a = C.call(h, this); C = f.series; this.distance = l(C.tooltipOptions.distance, 16); if (!1 === a) this.hide(); else {
                        if (this.split) this.renderSplit(a, k); else if (k = q, h = w, c && b.pointer.isDirectTouch && (k = c.chartX - b.plotLeft, h = c.chartY - b.plotTop), b.polar || !1 === C.options.clip || C.shouldShowTooltip(k, h)) c = this.getLabel(), d.style.width && !m || c.css({ width: this.chart.spacingBox.width + "px" }), c.attr({ text: a && a.join ? a.join("") : a }), c.removeClass(/highcharts-color-[\d]+/g).addClass("highcharts-color-" +
                        l(f.colorIndex, C.colorIndex)), m || c.attr({ stroke: d.borderColor || f.color || C.color || E.neutralColor60 }), this.updatePosition({ plotX: q, plotY: w, negative: f.negative, ttBelow: f.ttBelow, h: n[2] || 0 }); else { this.hide(); return } this.isHidden && this.label && this.label.attr({ opacity: 1 }).show(); this.isHidden = !1
                    } e(this, "refresh")
                }
            }; a.prototype.renderSplit = function (a, k) {
                function b(b, a, g, c, k) {
                    void 0 === k && (k = !0); g ? (a = T ? 0 : da, b = B(b - c / 2, N.left, N.right - c - (d.outside ? R : 0))) : (a -= V, b = k ? b - c - x : b + x, b = B(b, k ? b : N.left, N.right)); return {
                        x: b,
                        y: a
                    }
                } var d = this, e = d.chart, f = d.chart, h = f.chartWidth, p = f.chartHeight, C = f.plotHeight, m = f.plotLeft, t = f.plotTop, n = f.pointer, q = f.scrollablePixelsY; q = void 0 === q ? 0 : q; var v = f.scrollablePixelsX, A = f.scrollingContainer; A = void 0 === A ? { scrollLeft: 0, scrollTop: 0 } : A; var J = A.scrollLeft; A = A.scrollTop; var K = f.styledMode, x = d.distance, y = d.options, I = d.options.positioner, N = d.outside && "number" !== typeof v ? G.documentElement.getBoundingClientRect() : { left: J, right: J + h, top: A, bottom: A + p }, P = d.getLabel(), U = this.renderer || e.renderer,
                T = !(!e.xAxis[0] || !e.xAxis[0].opposite); e = n.getChartPosition(); var R = e.left; e = e.top; var V = t + A, ca = 0, da = C - q; w(a) && (a = [!1, a]); a = a.slice(0, k.length + 1).reduce(function (a, g, c) {
                    if (!1 !== g && "" !== g) {
                        c = k[c - 1] || { isHeader: !0, plotX: k[0].plotX, plotY: C, series: {} }; var e = c.isHeader, f = e ? d : c.series; g = g.toString(); var r = f.tt, h = c.isHeader; var F = c.series; var p = "highcharts-color-" + l(c.colorIndex, F.colorIndex, "none"); r || (r = { padding: y.padding, r: y.borderRadius }, K || (r.fill = y.backgroundColor, r["stroke-width"] = y.borderWidth), r =
                        U.label("", 0, 0, y[h ? "headerShape" : "shape"] || "callout", void 0, void 0, y.useHTML).addClass((h ? "highcharts-tooltip-header " : "") + "highcharts-tooltip-box " + p).attr(r).add(P)); r.isActive = !0; r.attr({ text: g }); K || r.css(y.style).shadow(y.shadow).attr({ stroke: y.borderColor || c.color || F.color || E.neutralColor80 }); f = f.tt = r; h = f.getBBox(); g = h.width + f.strokeWidth(); e && (ca = h.height, da += ca, T && (V -= ca)); F = c.plotX; F = void 0 === F ? 0 : F; p = c.plotY; p = void 0 === p ? 0 : p; r = c.series; if (c.isHeader) { F = m + F; var D = t + C / 2 } else {
                            var n = r.xAxis, M =
                            r.yAxis; F = n.pos + B(F, -x, n.len + x); r.shouldShowTooltip(0, M.pos - t + p, { ignoreX: !0 }) && (D = M.pos + p)
                        } F = B(F, N.left - x, N.right + x); "number" === typeof D ? (h = h.height + 1, p = I ? I.call(d, g, h, c) : b(F, D, e, g), a.push({ align: I ? 0 : void 0, anchorX: F, anchorY: D, boxWidth: g, point: c, rank: l(p.rank, e ? 1 : 0), size: h, target: p.y, tt: f, x: p.x })) : f.isActive = !1
                    } return a
                }, []); !I && a.some(function (b) { var a = (d.outside ? R : 0) + b.anchorX; return a < N.left && a + b.boxWidth < N.right ? !0 : a < R - N.left + b.boxWidth && N.right - a > a }) && (a = a.map(function (a) {
                    var d = b(a.anchorX, a.anchorY,
                    a.point.isHeader, a.boxWidth, !1); return c(a, { target: d.y, x: d.x })
                })); d.cleanSplit(); u.distribute(a, da); var H = R, ea = R; a.forEach(function (b) { var a = b.x, g = b.boxWidth; b = b.isHeader; b || (d.outside && R + a < H && (H = R + a), !b && d.outside && H + g > ea && (ea = R + a)) }); a.forEach(function (b) { var a = b.x, g = b.anchorX, c = b.pos, k = b.point.isHeader; c = { visibility: "undefined" === typeof c ? "hidden" : "inherit", x: a, y: c + V, anchorX: g, anchorY: b.anchorY }; if (d.outside && a < g) { var e = R - H; 0 < e && (k || (c.x = a + e, c.anchorX = g + e), k && (c.x = (ea - H) / 2, c.anchorX = g + e)) } b.tt.attr(c) });
                a = d.container; q = d.renderer; d.outside && a && q && (f = P.getBBox(), q.setSize(f.width + f.x, f.height + f.y, !1), a.style.left = H + "px", a.style.top = e + "px")
            }; a.prototype.drawTracker = function () {
                if (this.followPointer || !this.options.stickOnContact) this.tracker && this.tracker.destroy(); else {
                    var a = this.chart, c = this.label, b = this.shared ? a.hoverPoints : a.hoverPoint; if (c && b) {
                        var g = { x: 0, y: 0, width: 0, height: 0 }; b = this.getAnchor(b); var e = c.getBBox(); b[0] += a.plotLeft - c.translateX; b[1] += a.plotTop - c.translateY; g.x = Math.min(0, b[0]); g.y =
                        Math.min(0, b[1]); g.width = 0 > b[0] ? Math.max(Math.abs(b[0]), e.width - b[0]) : Math.max(Math.abs(b[0]), e.width); g.height = 0 > b[1] ? Math.max(Math.abs(b[1]), e.height - Math.abs(b[1])) : Math.max(Math.abs(b[1]), e.height); this.tracker ? this.tracker.attr(g) : (this.tracker = c.renderer.rect(g).addClass("highcharts-tracker").add(c), a.styledMode || this.tracker.attr({ fill: "rgba(0,0,0,0)" }))
                    }
                }
            }; a.prototype.styledModeFormat = function (a) {
                return a.replace('style="font-size: 10px"', 'class="highcharts-header"').replace(/style="color:{(point|series)\.color}"/g,
                'class="highcharts-color-{$1.colorIndex}"')
            }; a.prototype.tooltipFooterHeaderFormatter = function (a, c) {
                var b = c ? "footer" : "header", d = a.series, k = d.tooltipOptions, f = k.xDateFormat, l = d.xAxis, h = l && "datetime" === l.options.type && m(a.key), p = k[b + "Format"]; c = { isFooter: c, labelConfig: a }; e(this, "headerFormatter", c, function (b) {
                    h && !f && (f = this.getXDateFormat(a, k, l)); h && f && (a.point && a.point.tooltipDateKeys || ["key"]).forEach(function (b) { p = p.replace("{point." + b + "}", "{point." + b + ":" + f + "}") }); d.chart.styledMode && (p = this.styledModeFormat(p));
                    b.text = v(p, { point: a, series: d }, this.chart)
                }); return c.text
            }; a.prototype.update = function (a) { this.destroy(); n(!0, this.chart.options.tooltip.userOptions, a); this.init(this.chart, n(!0, this.options, a)) }; a.prototype.updatePosition = function (a) {
                var d = this.chart, b = d.pointer, c = this.getLabel(), e = a.plotX + d.plotLeft; d = a.plotY + d.plotTop; b = b.getChartPosition(); a = (this.options.positioner || this.getPosition).call(this, c.width, c.height, a); if (this.outside) {
                    var f = (this.options.borderWidth || 0) + 2 * this.distance; this.renderer.setSize(c.width +
                    f, c.height + f, !1); if (1 !== b.scaleX || 1 !== b.scaleY) q(this.container, { transform: "scale(" + b.scaleX + ", " + b.scaleY + ")" }), e *= b.scaleX, d *= b.scaleY; e += b.left - a.x; d += b.top - a.y
                } this.move(Math.round(a.x), Math.round(a.y || 0), e, d)
            }; return a
        }(); u.Tooltip = a; return u.Tooltip
    }); L(a, "Core/Pointer.js", [a["Core/Color/Color.js"], a["Core/Globals.js"], a["Core/Color/Palette.js"], a["Core/Tooltip.js"], a["Core/Utilities.js"]], function (a, u, E, H, x) {
        var v = a.parse, G = u.charts, B = u.noop, q = x.addEvent, h = x.attr, f = x.css, c = x.defined, e = x.extend,
        t = x.find, m = x.fireEvent, w = x.isNumber, n = x.isObject, l = x.objectEach, J = x.offset, K = x.pick, A = x.splat; a = function () {
            function a(a, c) { this.lastValidTouch = {}; this.pinchDown = []; this.runChartClick = !1; this.eventsToUnbind = []; this.chart = a; this.hasDragged = !1; this.options = c; this.init(a, c) } a.prototype.applyInactiveState = function (a) {
                var d = [], b; (a || []).forEach(function (a) { b = a.series; d.push(b); b.linkedParent && d.push(b.linkedParent); b.linkedSeries && (d = d.concat(b.linkedSeries)); b.navigatorSeries && d.push(b.navigatorSeries) });
                this.chart.series.forEach(function (b) { -1 === d.indexOf(b) ? b.setState("inactive", !0) : b.options.inactiveOtherPoints && b.setAllPointsToState("inactive") })
            }; a.prototype.destroy = function () { var d = this; this.eventsToUnbind.forEach(function (a) { return a() }); this.eventsToUnbind = []; u.chartCount || (a.unbindDocumentMouseUp && (a.unbindDocumentMouseUp = a.unbindDocumentMouseUp()), a.unbindDocumentTouchEnd && (a.unbindDocumentTouchEnd = a.unbindDocumentTouchEnd())); clearInterval(d.tooltipTimeout); l(d, function (a, b) { d[b] = void 0 }) };
            a.prototype.drag = function (a) {
                var d = this.chart, b = d.options.chart, c = this.zoomHor, e = this.zoomVert, f = d.plotLeft, l = d.plotTop, h = d.plotWidth, C = d.plotHeight, p = this.mouseDownX || 0, m = this.mouseDownY || 0, t = n(b.panning) ? b.panning && b.panning.enabled : b.panning, q = b.panKey && a[b.panKey + "Key"], w = a.chartX, A = a.chartY, J = this.selectionMarker; if (!J || !J.touch) if (w < f ? w = f : w > f + h && (w = f + h), A < l ? A = l : A > l + C && (A = l + C), this.hasDragged = Math.sqrt(Math.pow(p - w, 2) + Math.pow(m - A, 2)), 10 < this.hasDragged) {
                    var u = d.isInsidePlot(p - f, m - l, { visiblePlotOnly: !0 });
                    d.hasCartesianSeries && (this.zoomX || this.zoomY) && u && !q && !J && (this.selectionMarker = J = d.renderer.rect(f, l, c ? 1 : h, e ? 1 : C, 0).attr({ "class": "highcharts-selection-marker", zIndex: 7 }).add(), d.styledMode || J.attr({ fill: b.selectionMarkerFill || v(E.highlightColor80).setOpacity(.25).get() })); J && c && (c = w - p, J.attr({ width: Math.abs(c), x: (0 < c ? 0 : c) + p })); J && e && (c = A - m, J.attr({ height: Math.abs(c), y: (0 < c ? 0 : c) + m })); u && !J && t && d.pan(a, b.panning)
                }
            }; a.prototype.dragStart = function (a) {
                var d = this.chart; d.mouseIsDown = a.type; d.cancelClick =
                !1; d.mouseDownX = this.mouseDownX = a.chartX; d.mouseDownY = this.mouseDownY = a.chartY
            }; a.prototype.drop = function (a) {
                var d = this, b = this.chart, g = this.hasPinched; if (this.selectionMarker) {
                    var r = { originalEvent: a, xAxis: [], yAxis: [] }, l = this.selectionMarker, h = l.attr ? l.attr("x") : l.x, p = l.attr ? l.attr("y") : l.y, C = l.attr ? l.attr("width") : l.width, t = l.attr ? l.attr("height") : l.height, n; if (this.hasDragged || g) b.axes.forEach(function (b) {
                        if (b.zoomEnabled && c(b.min) && (g || d[{ xAxis: "zoomX", yAxis: "zoomY" }[b.coll]]) && w(h) && w(p)) {
                            var e =
                            b.horiz, k = "touchend" === a.type ? b.minPixelPadding : 0, f = b.toValue((e ? h : p) + k); e = b.toValue((e ? h + C : p + t) - k); r[b.coll].push({ axis: b, min: Math.min(f, e), max: Math.max(f, e) }); n = !0
                        }
                    }), n && m(b, "selection", r, function (a) { b.zoom(e(a, g ? { animation: !1 } : null)) }); w(b.index) && (this.selectionMarker = this.selectionMarker.destroy()); g && this.scaleGroups()
                } b && w(b.index) && (f(b.container, { cursor: b._cursor }), b.cancelClick = 10 < this.hasDragged, b.mouseIsDown = this.hasDragged = this.hasPinched = !1, this.pinchDown = [])
            }; a.prototype.findNearestKDPoint =
            function (a, c, b) { var d = this.chart, e = d.hoverPoint; d = d.tooltip; if (e && d && d.isStickyOnContact()) return e; var k; a.forEach(function (a) { var d = !(a.noSharedTooltip && c) && 0 > a.options.findNearestPointBy.indexOf("y"); a = a.searchPoint(b, d); if ((d = n(a, !0) && a.series) && !(d = !n(k, !0))) { d = k.distX - a.distX; var g = k.dist - a.dist, e = (a.series.group && a.series.group.zIndex) - (k.series.group && k.series.group.zIndex); d = 0 < (0 !== d && c ? d : 0 !== g ? g : 0 !== e ? e : k.series.index > a.series.index ? -1 : 1) } d && (k = a) }); return k }; a.prototype.getChartCoordinatesFromPoint =
            function (a, c) { var b = a.series, d = b.xAxis; b = b.yAxis; var e = a.shapeArgs; if (d && b) { var k = K(a.clientX, a.plotX), f = a.plotY || 0; a.isNode && e && w(e.x) && w(e.y) && (k = e.x, f = e.y); return c ? { chartX: b.len + b.pos - f, chartY: d.len + d.pos - k } : { chartX: k + d.pos, chartY: f + b.pos } } if (e && e.x && e.y) return { chartX: e.x, chartY: e.y } }; a.prototype.getChartPosition = function () {
                if (this.chartPosition) return this.chartPosition; var a = this.chart.container, c = J(a); this.chartPosition = { left: c.left, top: c.top, scaleX: 1, scaleY: 1 }; var b = a.offsetWidth; a = a.offsetHeight;
                2 < b && 2 < a && (this.chartPosition.scaleX = c.width / b, this.chartPosition.scaleY = c.height / a); return this.chartPosition
            }; a.prototype.getCoordinates = function (a) { var d = { xAxis: [], yAxis: [] }; this.chart.axes.forEach(function (b) { d[b.isXAxis ? "xAxis" : "yAxis"].push({ axis: b, value: b.toValue(a[b.horiz ? "chartX" : "chartY"]) }) }); return d }; a.prototype.getHoverData = function (a, c, b, g, e, f) {
                var d = []; g = !(!g || !a); var k = { chartX: f ? f.chartX : void 0, chartY: f ? f.chartY : void 0, shared: e }; m(this, "beforeGetHoverData", k); var l = c && !c.stickyTracking ?
                [c] : b.filter(function (b) { return k.filter ? k.filter(b) : b.visible && !(!e && b.directTouch) && K(b.options.enableMouseTracking, !0) && b.stickyTracking }); var r = g || !f ? a : this.findNearestKDPoint(l, e, f); c = r && r.series; r && (e && !c.noSharedTooltip ? (l = b.filter(function (b) { return k.filter ? k.filter(b) : b.visible && !(!e && b.directTouch) && K(b.options.enableMouseTracking, !0) && !b.noSharedTooltip }), l.forEach(function (b) { var a = t(b.points, function (b) { return b.x === r.x && !b.isNull }); n(a) && (b.chart.isBoosting && (a = b.getPoint(a)), d.push(a)) })) :
                d.push(r)); k = { hoverPoint: r }; m(this, "afterGetHoverData", k); return { hoverPoint: k.hoverPoint, hoverSeries: c, hoverPoints: d }
            }; a.prototype.getPointFromEvent = function (a) { a = a.target; for (var d; a && !d;) d = a.point, a = a.parentNode; return d }; a.prototype.onTrackerMouseOut = function (a) { a = a.relatedTarget || a.toElement; var d = this.chart.hoverSeries; this.isDirectTouch = !1; if (!(!d || !a || d.stickyTracking || this.inClass(a, "highcharts-tooltip") || this.inClass(a, "highcharts-series-" + d.index) && this.inClass(a, "highcharts-tracker"))) d.onMouseOut() };
            a.prototype.inClass = function (a, c) { for (var b; a;) { if (b = h(a, "class")) { if (-1 !== b.indexOf(c)) return !0; if (-1 !== b.indexOf("highcharts-container")) return !1 } a = a.parentNode } }; a.prototype.init = function (a, c) { this.options = c; this.chart = a; this.runChartClick = !(!c.chart.events || !c.chart.events.click); this.pinchDown = []; this.lastValidTouch = {}; H && (a.tooltip = new H(a, c.tooltip), this.followTouchMove = K(c.tooltip.followTouchMove, !0)); this.setDOMEvents() }; a.prototype.normalize = function (a, c) {
                var b = a.touches, d = b ? b.length ? b.item(0) :
                K(b.changedTouches, a.changedTouches)[0] : a; c || (c = this.getChartPosition()); b = d.pageX - c.left; d = d.pageY - c.top; b /= c.scaleX; d /= c.scaleY; return e(a, { chartX: Math.round(b), chartY: Math.round(d) })
            }; a.prototype.onContainerClick = function (a) {
                var d = this.chart, b = d.hoverPoint; a = this.normalize(a); var c = d.plotLeft, f = d.plotTop; d.cancelClick || (b && this.inClass(a.target, "highcharts-tracker") ? (m(b.series, "click", e(a, { point: b })), d.hoverPoint && b.firePointEvent("click", a)) : (e(a, this.getCoordinates(a)), d.isInsidePlot(a.chartX -
                c, a.chartY - f, { visiblePlotOnly: !0 }) && m(d, "click", a)))
            }; a.prototype.onContainerMouseDown = function (a) { var d = 1 === ((a.buttons || a.button) & 1); a = this.normalize(a); if (u.isFirefox && 0 !== a.button) this.onContainerMouseMove(a); if ("undefined" === typeof a.button || d) this.zoomOption(a), d && a.preventDefault && a.preventDefault(), this.dragStart(a) }; a.prototype.onContainerMouseLeave = function (d) {
                var c = G[K(a.hoverChartIndex, -1)], b = this.chart.tooltip; d = this.normalize(d); c && (d.relatedTarget || d.toElement) && (c.pointer.reset(),
                c.pointer.chartPosition = void 0); b && !b.isHidden && this.reset()
            }; a.prototype.onContainerMouseEnter = function (a) { delete this.chartPosition }; a.prototype.onContainerMouseMove = function (a) {
                var d = this.chart; a = this.normalize(a); this.setHoverChartIndex(); a.preventDefault || (a.returnValue = !1); ("mousedown" === d.mouseIsDown || this.touchSelect(a)) && this.drag(a); d.openMenu || !this.inClass(a.target, "highcharts-tracker") && !d.isInsidePlot(a.chartX - d.plotLeft, a.chartY - d.plotTop, { visiblePlotOnly: !0 }) || (this.inClass(a.target,
                "highcharts-no-tooltip") ? this.reset(!1, 0) : this.runPointActions(a))
            }; a.prototype.onDocumentTouchEnd = function (d) { var c = G[K(a.hoverChartIndex, -1)]; c && c.pointer.drop(d) }; a.prototype.onContainerTouchMove = function (a) { if (this.touchSelect(a)) this.onContainerMouseMove(a); else this.touch(a) }; a.prototype.onContainerTouchStart = function (a) { if (this.touchSelect(a)) this.onContainerMouseDown(a); else this.zoomOption(a), this.touch(a, !0) }; a.prototype.onDocumentMouseMove = function (a) {
                var d = this.chart, b = this.chartPosition;
                a = this.normalize(a, b); var c = d.tooltip; !b || c && c.isStickyOnContact() || d.isInsidePlot(a.chartX - d.plotLeft, a.chartY - d.plotTop, { visiblePlotOnly: !0 }) || this.inClass(a.target, "highcharts-tracker") || this.reset()
            }; a.prototype.onDocumentMouseUp = function (d) { var c = G[K(a.hoverChartIndex, -1)]; c && c.pointer.drop(d) }; a.prototype.pinch = function (a) {
                var d = this, b = d.chart, c = d.pinchDown, f = a.touches || [], l = f.length, h = d.lastValidTouch, p = d.hasZoom, C = {}, m = 1 === l && (d.inClass(a.target, "highcharts-tracker") && b.runTrackerClick || d.runChartClick),
                t = {}, n = d.selectionMarker; 1 < l && (d.initiated = !0); p && d.initiated && !m && !1 !== a.cancelable && a.preventDefault();[].map.call(f, function (a) { return d.normalize(a) }); "touchstart" === a.type ? ([].forEach.call(f, function (a, b) { c[b] = { chartX: a.chartX, chartY: a.chartY } }), h.x = [c[0].chartX, c[1] && c[1].chartX], h.y = [c[0].chartY, c[1] && c[1].chartY], b.axes.forEach(function (a) {
                    if (a.zoomEnabled) {
                        var d = b.bounds[a.horiz ? "h" : "v"], c = a.minPixelPadding, g = a.toPixels(Math.min(K(a.options.min, a.dataMin), a.dataMin)), e = a.toPixels(Math.max(K(a.options.max,
                        a.dataMax), a.dataMax)), k = Math.max(g, e); d.min = Math.min(a.pos, Math.min(g, e) - c); d.max = Math.max(a.pos + a.len, k + c)
                    }
                }), d.res = !0) : d.followTouchMove && 1 === l ? this.runPointActions(d.normalize(a)) : c.length && (n || (d.selectionMarker = n = e({ destroy: B, touch: !0 }, b.plotBox)), d.pinchTranslate(c, f, C, n, t, h), d.hasPinched = p, d.scaleGroups(C, t), d.res && (d.res = !1, this.reset(!1, 0)))
            }; a.prototype.pinchTranslate = function (a, c, b, g, e, f) {
                this.zoomHor && this.pinchTranslateDirection(!0, a, c, b, g, e, f); this.zoomVert && this.pinchTranslateDirection(!1,
                a, c, b, g, e, f)
            }; a.prototype.pinchTranslateDirection = function (a, c, b, g, e, f, l, h) {
                var d = this.chart, k = a ? "x" : "y", r = a ? "X" : "Y", F = "chart" + r, p = a ? "width" : "height", m = d["plot" + (a ? "Left" : "Top")], D = d.inverted, t = d.bounds[a ? "h" : "v"], n = 1 === c.length, M = c[0][F], q = !n && c[1][F]; c = function () { "number" === typeof v && 20 < Math.abs(M - q) && (P = h || Math.abs(A - v) / Math.abs(M - q)); w = (m - A) / P + M; I = d["plot" + (a ? "Width" : "Height")] / P }; var I, w, P = h || 1, A = b[0][F], v = !n && b[1][F]; c(); b = w; if (b < t.min) { b = t.min; var J = !0 } else b + I > t.max && (b = t.max - I, J = !0); J ? (A -=
                .8 * (A - l[k][0]), "number" === typeof v && (v -= .8 * (v - l[k][1])), c()) : l[k] = [A, v]; D || (f[k] = w - m, f[p] = I); f = D ? 1 / P : P; e[p] = I; e[k] = b; g[D ? a ? "scaleY" : "scaleX" : "scale" + r] = P; g["translate" + r] = f * m + (A - f * M)
            }; a.prototype.reset = function (a, c) {
                var b = this.chart, d = b.hoverSeries, e = b.hoverPoint, k = b.hoverPoints, f = b.tooltip, l = f && f.shared ? k : e; a && l && A(l).forEach(function (b) { b.series.isCartesian && "undefined" === typeof b.plotX && (a = !1) }); if (a) f && l && A(l).length && (f.refresh(l), f.shared && k ? k.forEach(function (a) {
                    a.setState(a.state, !0); a.series.isCartesian &&
                    (a.series.xAxis.crosshair && a.series.xAxis.drawCrosshair(null, a), a.series.yAxis.crosshair && a.series.yAxis.drawCrosshair(null, a))
                }) : e && (e.setState(e.state, !0), b.axes.forEach(function (a) { a.crosshair && e.series[a.coll] === a && a.drawCrosshair(null, e) }))); else { if (e) e.onMouseOut(); k && k.forEach(function (a) { a.setState() }); if (d) d.onMouseOut(); f && f.hide(c); this.unDocMouseMove && (this.unDocMouseMove = this.unDocMouseMove()); b.axes.forEach(function (a) { a.hideCrosshair() }); this.hoverX = b.hoverPoints = b.hoverPoint = null }
            };
            a.prototype.runPointActions = function (d, c) {
                var b = this.chart, g = b.tooltip && b.tooltip.options.enabled ? b.tooltip : void 0, e = g ? g.shared : !1, f = c || b.hoverPoint, k = f && f.series || b.hoverSeries; c = this.getHoverData(f, k, b.series, (!d || "touchmove" !== d.type) && (!!c || k && k.directTouch && this.isDirectTouch), e, d); f = c.hoverPoint; k = c.hoverSeries; var l = c.hoverPoints; c = k && k.tooltipOptions.followPointer && !k.tooltipOptions.split; e = e && k && !k.noSharedTooltip; if (f && (f !== b.hoverPoint || g && g.isHidden)) {
                    (b.hoverPoints || []).forEach(function (a) {
                        -1 ===
                        l.indexOf(a) && a.setState()
                    }); if (b.hoverSeries !== k) k.onMouseOver(); this.applyInactiveState(l); (l || []).forEach(function (a) { a.setState("hover") }); b.hoverPoint && b.hoverPoint.firePointEvent("mouseOut"); if (!f.series) return; b.hoverPoints = l; b.hoverPoint = f; f.firePointEvent("mouseOver"); g && g.refresh(e ? l : f, d)
                } else c && g && !g.isHidden && (f = g.getAnchor([{}], d), b.isInsidePlot(f[0], f[1], { visiblePlotOnly: !0 }) && g.updatePosition({ plotX: f[0], plotY: f[1] })); this.unDocMouseMove || (this.unDocMouseMove = q(b.container.ownerDocument,
                "mousemove", function (b) { var d = G[a.hoverChartIndex]; if (d) d.pointer.onDocumentMouseMove(b) }), this.eventsToUnbind.push(this.unDocMouseMove)); b.axes.forEach(function (a) { var c = K((a.crosshair || {}).snap, !0), g; c && ((g = b.hoverPoint) && g.series[a.coll] === a || (g = t(l, function (b) { return b.series[a.coll] === a }))); g || !c ? a.drawCrosshair(d, g) : a.hideCrosshair() })
            }; a.prototype.scaleGroups = function (a, c) {
                var b = this.chart; b.series.forEach(function (d) {
                    var g = a || d.getPlotBox(); d.xAxis && d.xAxis.zoomEnabled && d.group && (d.group.attr(g),
                    d.markerGroup && (d.markerGroup.attr(g), d.markerGroup.clip(c ? b.clipRect : null)), d.dataLabelsGroup && d.dataLabelsGroup.attr(g))
                }); b.clipRect.attr(c || b.clipBox)
            }; a.prototype.setDOMEvents = function () {
                var d = this, c = this.chart.container, b = c.ownerDocument; c.onmousedown = this.onContainerMouseDown.bind(this); c.onmousemove = this.onContainerMouseMove.bind(this); c.onclick = this.onContainerClick.bind(this); this.eventsToUnbind.push(q(c, "mouseenter", this.onContainerMouseEnter.bind(this))); this.eventsToUnbind.push(q(c, "mouseleave",
                this.onContainerMouseLeave.bind(this))); a.unbindDocumentMouseUp || (a.unbindDocumentMouseUp = q(b, "mouseup", this.onDocumentMouseUp.bind(this))); for (var g = this.chart.renderTo.parentElement; g && "BODY" !== g.tagName;) this.eventsToUnbind.push(q(g, "scroll", function () { delete d.chartPosition })), g = g.parentElement; u.hasTouch && (this.eventsToUnbind.push(q(c, "touchstart", this.onContainerTouchStart.bind(this), { passive: !1 })), this.eventsToUnbind.push(q(c, "touchmove", this.onContainerTouchMove.bind(this), { passive: !1 })),
                a.unbindDocumentTouchEnd || (a.unbindDocumentTouchEnd = q(b, "touchend", this.onDocumentTouchEnd.bind(this), { passive: !1 })))
            }; a.prototype.setHoverChartIndex = function () { var d = this.chart, c = u.charts[K(a.hoverChartIndex, -1)]; if (c && c !== d) c.pointer.onContainerMouseLeave({ relatedTarget: !0 }); c && c.mouseIsDown || (a.hoverChartIndex = d.index) }; a.prototype.touch = function (a, c) {
                var b = this.chart, d; this.setHoverChartIndex(); if (1 === a.touches.length) if (a = this.normalize(a), (d = b.isInsidePlot(a.chartX - b.plotLeft, a.chartY - b.plotTop,
                { visiblePlotOnly: !0 })) && !b.openMenu) { c && this.runPointActions(a); if ("touchmove" === a.type) { c = this.pinchDown; var e = c[0] ? 4 <= Math.sqrt(Math.pow(c[0].chartX - a.chartX, 2) + Math.pow(c[0].chartY - a.chartY, 2)) : !1 } K(e, !0) && this.pinch(a) } else c && this.reset(); else 2 === a.touches.length && this.pinch(a)
            }; a.prototype.touchSelect = function (a) { return !(!this.chart.options.chart.zoomBySingleTouch || !a.touches || 1 !== a.touches.length) }; a.prototype.zoomOption = function (a) {
                var d = this.chart, b = d.options.chart; d = d.inverted; var c = b.zoomType ||
                ""; /touch/.test(a.type) && (c = K(b.pinchType, c)); this.zoomX = a = /x/.test(c); this.zoomY = b = /y/.test(c); this.zoomHor = a && !d || b && d; this.zoomVert = b && !d || a && d; this.hasZoom = a || b
            }; return a
        }(); ""; return a
    }); L(a, "Core/MSPointer.js", [a["Core/Globals.js"], a["Core/Pointer.js"], a["Core/Utilities.js"]], function (a, u, E) {
        function v() { var a = []; a.item = function (a) { return this[a] }; e(m, function (c) { a.push({ pageX: c.pageX, pageY: c.pageY, target: c.target }) }); return a } function x(a, c, e, f) {
            var l = G[u.hoverChartIndex || NaN]; "touch" !== a.pointerType &&
            a.pointerType !== a.MSPOINTER_TYPE_TOUCH || !l || (l = l.pointer, f(a), l[c]({ type: e, target: a.currentTarget, preventDefault: q, touches: v() }))
        } var y = this && this.__extends || function () { var a = function (c, e) { a = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, c) { a.__proto__ = c } || function (a, c) { for (var e in c) c.hasOwnProperty(e) && (a[e] = c[e]) }; return a(c, e) }; return function (c, e) { function f() { this.constructor = c } a(c, e); c.prototype = null === e ? Object.create(e) : (f.prototype = e.prototype, new f) } }(), G = a.charts, B =
        a.doc, q = a.noop, h = a.win, f = E.addEvent, c = E.css, e = E.objectEach, t = E.removeEvent, m = {}, w = !!h.PointerEvent; return function (e) {
            function l() { return null !== e && e.apply(this, arguments) || this } y(l, e); l.isRequired = function () { return !(a.hasTouch || !h.PointerEvent && !h.MSPointerEvent) }; l.prototype.batchMSEvents = function (a) {
                a(this.chart.container, w ? "pointerdown" : "MSPointerDown", this.onContainerPointerDown); a(this.chart.container, w ? "pointermove" : "MSPointerMove", this.onContainerPointerMove); a(B, w ? "pointerup" : "MSPointerUp",
                this.onDocumentPointerUp)
            }; l.prototype.destroy = function () { this.batchMSEvents(t); e.prototype.destroy.call(this) }; l.prototype.init = function (a, f) { e.prototype.init.call(this, a, f); this.hasZoom && c(a.container, { "-ms-touch-action": "none", "touch-action": "none" }) }; l.prototype.onContainerPointerDown = function (a) { x(a, "onContainerTouchStart", "touchstart", function (a) { m[a.pointerId] = { pageX: a.pageX, pageY: a.pageY, target: a.currentTarget } }) }; l.prototype.onContainerPointerMove = function (a) {
                x(a, "onContainerTouchMove", "touchmove",
                function (a) { m[a.pointerId] = { pageX: a.pageX, pageY: a.pageY }; m[a.pointerId].target || (m[a.pointerId].target = a.currentTarget) })
            }; l.prototype.onDocumentPointerUp = function (a) { x(a, "onDocumentTouchEnd", "touchend", function (a) { delete m[a.pointerId] }) }; l.prototype.setDOMEvents = function () { e.prototype.setDOMEvents.call(this); (this.hasZoom || this.followTouchMove) && this.batchMSEvents(f) }; return l
        }(u)
    }); L(a, "Core/Series/Point.js", [a["Core/Renderer/HTML/AST.js"], a["Core/Animation/AnimationUtilities.js"], a["Core/FormatUtilities.js"],
    a["Core/Globals.js"], a["Core/DefaultOptions.js"], a["Core/Utilities.js"]], function (a, u, E, H, x, y) {
        var v = u.animObject, B = E.format, q = x.defaultOptions, h = y.addEvent, f = y.defined, c = y.erase, e = y.extend, t = y.fireEvent, m = y.getNestedProperty, w = y.isArray, n = y.isFunction, l = y.isNumber, J = y.isObject, K = y.merge, A = y.objectEach, p = y.pick, d = y.syncTimeout, k = y.removeEvent, b = y.uniqueKey; ""; u = function () {
            function g() {
                this.colorIndex = this.category = void 0; this.formatPrefix = "point"; this.id = void 0; this.isNull = !1; this.percentage = this.options =
                this.name = void 0; this.selected = !1; this.total = this.series = void 0; this.visible = !0; this.x = void 0
            } g.prototype.animateBeforeDestroy = function () { var a = this, b = { x: a.startXPos, opacity: 0 }, d, c = a.getGraphicalProps(); c.singular.forEach(function (c) { d = "dataLabel" === c; a[c] = a[c].animate(d ? { x: a[c].startXPos, y: a[c].startYPos, opacity: 0 } : b) }); c.plural.forEach(function (b) { a[b].forEach(function (b) { b.element && b.animate(e({ x: a.startXPos }, b.startYPos ? { x: b.startXPos, y: b.startYPos } : {})) }) }) }; g.prototype.applyOptions = function (a,
            b) {
                var d = this.series, c = d.options.pointValKey || d.pointValKey; a = g.prototype.optionsToObject.call(this, a); e(this, a); this.options = this.options ? e(this.options, a) : a; a.group && delete this.group; a.dataLabels && delete this.dataLabels; c && (this.y = g.prototype.getNestedProperty.call(this, c)); this.formatPrefix = (this.isNull = p(this.isValid && !this.isValid(), null === this.x || !l(this.y))) ? "null" : "point"; this.selected && (this.state = "select"); "name" in this && "undefined" === typeof b && d.xAxis && d.xAxis.hasNames && (this.x = d.xAxis.nameToX(this));
                "undefined" === typeof this.x && d && (this.x = "undefined" === typeof b ? d.autoIncrement(this) : b); return this
            }; g.prototype.destroy = function () {
                function a() { if (b.graphic || b.dataLabel || b.dataLabels) k(b), b.destroyElements(); for (h in b) b[h] = null } var b = this, g = b.series, e = g.chart; g = g.options.dataSorting; var f = e.hoverPoints, l = v(b.series.chart.renderer.globalAnimation), h; b.legendItem && e.legend.destroyItem(b); f && (b.setState(), c(f, b), f.length || (e.hoverPoints = null)); if (b === e.hoverPoint) b.onMouseOut(); g && g.enabled ? (this.animateBeforeDestroy(),
                d(a, l.duration)) : a(); e.pointCount--
            }; g.prototype.destroyElements = function (a) { var b = this; a = b.getGraphicalProps(a); a.singular.forEach(function (a) { b[a] = b[a].destroy() }); a.plural.forEach(function (a) { b[a].forEach(function (a) { a.element && a.destroy() }); delete b[a] }) }; g.prototype.firePointEvent = function (a, b, d) {
                var c = this, g = this.series.options; (g.point.events[a] || c.options && c.options.events && c.options.events[a]) && c.importEvents(); "click" === a && g.allowPointSelect && (d = function (a) {
                    c.select && c.select(null, a.ctrlKey ||
                    a.metaKey || a.shiftKey)
                }); t(c, a, b, d)
            }; g.prototype.getClassName = function () { return "highcharts-point" + (this.selected ? " highcharts-point-select" : "") + (this.negative ? " highcharts-negative" : "") + (this.isNull ? " highcharts-null-point" : "") + ("undefined" !== typeof this.colorIndex ? " highcharts-color-" + this.colorIndex : "") + (this.options.className ? " " + this.options.className : "") + (this.zone && this.zone.className ? " " + this.zone.className.replace("highcharts-negative", "") : "") }; g.prototype.getGraphicalProps = function (a) {
                var b =
                this, d = [], c, g = { singular: [], plural: [] }; a = a || { graphic: 1, dataLabel: 1 }; a.graphic && d.push("graphic", "upperGraphic", "shadowGroup"); a.dataLabel && d.push("dataLabel", "dataLabelUpper", "connector"); for (c = d.length; c--;) { var e = d[c]; b[e] && g.singular.push(e) } ["dataLabel", "connector"].forEach(function (d) { var c = d + "s"; a[d] && b[c] && g.plural.push(c) }); return g
            }; g.prototype.getLabelConfig = function () {
                return {
                    x: this.category, y: this.y, color: this.color, colorIndex: this.colorIndex, key: this.name || this.category, series: this.series,
                    point: this, percentage: this.percentage, total: this.total || this.stackTotal
                }
            }; g.prototype.getNestedProperty = function (a) { if (a) return 0 === a.indexOf("custom.") ? m(a, this.options) : this[a] }; g.prototype.getZone = function () { var a = this.series, b = a.zones; a = a.zoneAxis || "y"; var d = 0, c; for (c = b[d]; this[a] >= c.value;) c = b[++d]; this.nonZonedColor || (this.nonZonedColor = this.color); this.color = c && c.color && !this.options.color ? c.color : this.nonZonedColor; return c }; g.prototype.hasNewShapeType = function () {
                return (this.graphic && (this.graphic.symbolName ||
                this.graphic.element.nodeName)) !== this.shapeType
            }; g.prototype.init = function (a, d, c) { this.series = a; this.applyOptions(d, c); this.id = f(this.id) ? this.id : b(); this.resolveColor(); a.chart.pointCount++; t(this, "afterInit"); return this }; g.prototype.optionsToObject = function (a) {
                var b = {}, d = this.series, c = d.options.keys, e = c || d.pointArrayMap || ["y"], f = e.length, k = 0, h = 0; if (l(a) || null === a) b[e[0]] = a; else if (w(a)) for (!c && a.length > f && (d = typeof a[0], "string" === d ? b.name = a[0] : "number" === d && (b.x = a[0]), k++) ; h < f;) c && "undefined" ===
                typeof a[k] || (0 < e[h].indexOf(".") ? g.prototype.setNestedProperty(b, a[k], e[h]) : b[e[h]] = a[k]), k++, h++; else "object" === typeof a && (b = a, a.dataLabels && (d._hasPointLabels = !0), a.marker && (d._hasPointMarkers = !0)); return b
            }; g.prototype.resolveColor = function () {
                var a = this.series; var b = a.chart.options.chart.colorCount; var d = a.chart.styledMode; delete this.nonZonedColor; if (a.options.colorByPoint) {
                    if (!d) { b = a.options.colors || a.chart.options.colors; var c = b[a.colorCounter]; b = b.length } d = a.colorCounter; a.colorCounter++;
                    a.colorCounter === b && (a.colorCounter = 0)
                } else d || (c = a.color), d = a.colorIndex; this.colorIndex = p(this.options.colorIndex, d); this.color = p(this.options.color, c)
            }; g.prototype.setNestedProperty = function (a, b, d) { d.split(".").reduce(function (a, d, c, g) { a[d] = g.length - 1 === c ? b : J(a[d], !0) ? a[d] : {}; return a[d] }, a); return a }; g.prototype.tooltipFormatter = function (a) {
                var b = this.series, d = b.tooltipOptions, c = p(d.valueDecimals, ""), g = d.valuePrefix || "", e = d.valueSuffix || ""; b.chart.styledMode && (a = b.chart.tooltip.styledModeFormat(a));
                (b.pointArrayMap || ["y"]).forEach(function (b) { b = "{point." + b; if (g || e) a = a.replace(RegExp(b + "}", "g"), g + b + "}" + e); a = a.replace(RegExp(b + "}", "g"), b + ":,." + c + "f}") }); return B(a, { point: this, series: this.series }, b.chart)
            }; g.prototype.update = function (a, b, d, c) {
                function g() {
                    e.applyOptions(a); var c = k && e.hasDummyGraphic; c = null === e.y ? !c : c; k && c && (e.graphic = k.destroy(), delete e.hasDummyGraphic); J(a, !0) && (k && k.element && a && a.marker && "undefined" !== typeof a.marker.symbol && (e.graphic = k.destroy()), a && a.dataLabels && e.dataLabel &&
                    (e.dataLabel = e.dataLabel.destroy()), e.connector && (e.connector = e.connector.destroy())); l = e.index; f.updateParallelArrays(e, l); r.data[l] = J(r.data[l], !0) || J(a, !0) ? e.options : p(a, r.data[l]); f.isDirty = f.isDirtyData = !0; !f.fixedBox && f.hasCartesianSeries && (h.isDirtyBox = !0); "point" === r.legendType && (h.isDirtyLegend = !0); b && h.redraw(d)
                } var e = this, f = e.series, k = e.graphic, l, h = f.chart, r = f.options; b = p(b, !0); !1 === c ? g() : e.firePointEvent("update", { options: a }, g)
            }; g.prototype.remove = function (a, b) {
                this.series.removePoint(this.series.data.indexOf(this),
                a, b)
            }; g.prototype.select = function (a, b) {
                var d = this, c = d.series, g = c.chart; this.selectedStaging = a = p(a, !d.selected); d.firePointEvent(a ? "select" : "unselect", { accumulate: b }, function () {
                    d.selected = d.options.selected = a; c.options.data[c.data.indexOf(d)] = d.options; d.setState(a && "select"); b || g.getSelectedPoints().forEach(function (a) {
                        var b = a.series; a.selected && a !== d && (a.selected = a.options.selected = !1, b.options.data[b.data.indexOf(a)] = a.options, a.setState(g.hoverPoints && b.options.inactiveOtherPoints ? "inactive" : ""),
                        a.firePointEvent("unselect"))
                    })
                }); delete this.selectedStaging
            }; g.prototype.onMouseOver = function (a) { var b = this.series.chart, d = b.pointer; a = a ? d.normalize(a) : d.getChartCoordinatesFromPoint(this, b.inverted); d.runPointActions(a, this) }; g.prototype.onMouseOut = function () { var a = this.series.chart; this.firePointEvent("mouseOut"); this.series.options.inactiveOtherPoints || (a.hoverPoints || []).forEach(function (a) { a.setState() }); a.hoverPoints = a.hoverPoint = null }; g.prototype.importEvents = function () {
                if (!this.hasImportedEvents) {
                    var a =
                    this, b = K(a.series.options.point, a.options).events; a.events = b; A(b, function (b, d) { n(b) && h(a, d, b) }); this.hasImportedEvents = !0
                }
            }; g.prototype.setState = function (b, d) {
                var c = this.series, g = this.state, f = c.options.states[b || "normal"] || {}, k = q.plotOptions[c.type].marker && c.options.marker, h = k && !1 === k.enabled, r = k && k.states && k.states[b || "normal"] || {}, m = !1 === r.enabled, n = c.stateMarkerGraphic, F = this.marker || {}, w = c.chart, v = c.halo, A, J = k && c.markerAttribs; b = b || ""; if (!(b === this.state && !d || this.selected && "select" !== b || !1 ===
                f.enabled || b && (m || h && !1 === r.enabled) || b && F.states && F.states[b] && !1 === F.states[b].enabled)) {
                    this.state = b; J && (A = c.markerAttribs(this, b)); if (this.graphic && !this.hasDummyGraphic) {
                        g && this.graphic.removeClass("highcharts-point-" + g); b && this.graphic.addClass("highcharts-point-" + b); if (!w.styledMode) {
                            var I = c.pointAttribs(this, b); var N = p(w.options.chart.animation, f.animation); c.options.inactiveOtherPoints && l(I.opacity) && ((this.dataLabels || []).forEach(function (a) { a && a.animate({ opacity: I.opacity }, N) }), this.connector &&
                            this.connector.animate({ opacity: I.opacity }, N)); this.graphic.animate(I, N)
                        } A && this.graphic.animate(A, p(w.options.chart.animation, r.animation, k.animation)); n && n.hide()
                    } else {
                        if (b && r) { g = F.symbol || c.symbol; n && n.currentSymbol !== g && (n = n.destroy()); if (A) if (n) n[d ? "animate" : "attr"]({ x: A.x, y: A.y }); else g && (c.stateMarkerGraphic = n = w.renderer.symbol(g, A.x, A.y, A.width, A.height).add(c.markerGroup), n.currentSymbol = g); !w.styledMode && n && n.attr(c.pointAttribs(this, b)) } n && (n[b && this.isInside ? "show" : "hide"](), n.element.point =
                        this)
                    } f = f.halo; A = (n = this.graphic || n) && n.visibility || "inherit"; f && f.size && n && "hidden" !== A && !this.isCluster ? (v || (c.halo = v = w.renderer.path().add(n.parentGroup)), v.show()[d ? "animate" : "attr"]({ d: this.haloPath(f.size) }), v.attr({ "class": "highcharts-halo highcharts-color-" + p(this.colorIndex, c.colorIndex) + (this.className ? " " + this.className : ""), visibility: A, zIndex: -1 }), v.point = this, w.styledMode || v.attr(e({ fill: this.color || c.color, "fill-opacity": f.opacity }, a.filterUserAttributes(f.attributes || {})))) : v && v.point &&
                    v.point.haloPath && v.animate({ d: v.point.haloPath(0) }, null, v.hide); t(this, "afterSetState", { state: b })
                }
            }; g.prototype.haloPath = function (a) { return this.series.chart.renderer.symbols.circle(Math.floor(this.plotX) - a, this.plotY - a, 2 * a, 2 * a) }; return g
        }(); return H.Point = u
    }); L(a, "Core/Legend.js", [a["Core/Animation/AnimationUtilities.js"], a["Core/FormatUtilities.js"], a["Core/Globals.js"], a["Core/Series/Point.js"], a["Core/Utilities.js"]], function (a, u, E, H, x) {
        var v = a.animObject, G = a.setAnimation, B = u.format; a = E.isFirefox;
        var q = E.marginNames; u = E.win; var h = x.addEvent, f = x.createElement, c = x.css, e = x.defined, t = x.discardElement, m = x.find, w = x.fireEvent, n = x.isNumber, l = x.merge, J = x.pick, K = x.relativeLength, A = x.stableSort, p = x.syncTimeout; x = x.wrap; var d = function () {
            function a(a, d) {
                this.allItems = []; this.contentGroup = this.box = void 0; this.display = !1; this.group = void 0; this.offsetWidth = this.maxLegendWidth = this.maxItemWidth = this.legendWidth = this.legendHeight = this.lastLineHeight = this.lastItemY = this.itemY = this.itemX = this.itemMarginTop = this.itemMarginBottom =
                this.itemHeight = this.initialItemY = 0; this.options = {}; this.padding = 0; this.pages = []; this.proximate = !1; this.scrollGroup = void 0; this.widthOption = this.totalItemWidth = this.titleHeight = this.symbolWidth = this.symbolHeight = 0; this.chart = a; this.init(a, d)
            } a.prototype.init = function (a, d) {
                this.chart = a; this.setOptions(d); d.enabled && (this.render(), h(this.chart, "endResize", function () { this.legend.positionCheckboxes() }), this.proximate ? this.unchartrender = h(this.chart, "render", function () {
                    this.legend.proximatePositions();
                    this.legend.positionItems()
                }) : this.unchartrender && this.unchartrender())
            }; a.prototype.setOptions = function (a) { var b = J(a.padding, 8); this.options = a; this.chart.styledMode || (this.itemStyle = a.itemStyle, this.itemHiddenStyle = l(this.itemStyle, a.itemHiddenStyle)); this.itemMarginTop = a.itemMarginTop || 0; this.itemMarginBottom = a.itemMarginBottom || 0; this.padding = b; this.initialItemY = b - 5; this.symbolWidth = J(a.symbolWidth, 16); this.pages = []; this.proximate = "proximate" === a.layout && !this.chart.inverted; this.baseline = void 0 };
            a.prototype.update = function (a, d) { var b = this.chart; this.setOptions(l(!0, this.options, a)); this.destroy(); b.isDirtyLegend = b.isDirtyBox = !0; J(d, !0) && b.redraw(); w(this, "afterUpdate") }; a.prototype.colorizeItem = function (a, d) {
                a.legendGroup[d ? "removeClass" : "addClass"]("highcharts-legend-item-hidden"); if (!this.chart.styledMode) {
                    var b = this.options, c = a.legendItem, g = a.legendLine, e = a.legendSymbol, f = this.itemHiddenStyle.color; b = d ? b.itemStyle.color : f; var k = d ? a.color || f : f, l = a.options && a.options.marker, h = { fill: k }; c &&
                    c.css({ fill: b, color: b }); g && g.attr({ stroke: k }); e && (l && e.isMarker && (h = a.pointAttribs(), d || (h.stroke = h.fill = f)), e.attr(h))
                } w(this, "afterColorizeItem", { item: a, visible: d })
            }; a.prototype.positionItems = function () { this.allItems.forEach(this.positionItem, this); this.chart.isResizing || this.positionCheckboxes() }; a.prototype.positionItem = function (a) {
                var b = this, d = this.options, c = d.symbolPadding, f = !d.rtl, k = a._legendItemPos; d = k[0]; k = k[1]; var l = a.checkbox, h = a.legendGroup; h && h.element && (c = {
                    translateX: f ? d : this.legendWidth -
                    d - 2 * c - 4, translateY: k
                }, f = function () { w(b, "afterPositionItem", { item: a }) }, e(h.translateY) ? h.animate(c, void 0, f) : (h.attr(c), f())); l && (l.x = d, l.y = k)
            }; a.prototype.destroyItem = function (a) { var b = a.checkbox;["legendItem", "legendLine", "legendSymbol", "legendGroup"].forEach(function (b) { a[b] && (a[b] = a[b].destroy()) }); b && t(a.checkbox) }; a.prototype.destroy = function () {
                function a(a) { this[a] && (this[a] = this[a].destroy()) } this.getAllItems().forEach(function (b) { ["legendItem", "legendGroup"].forEach(a, b) }); "clipRect up down pager nav box title group".split(" ").forEach(a,
                this); this.display = null
            }; a.prototype.positionCheckboxes = function () { var a = this.group && this.group.alignAttr, d = this.clipHeight || this.legendHeight, e = this.titleHeight; if (a) { var f = a.translateY; this.allItems.forEach(function (b) { var g = b.checkbox; if (g) { var k = f + e + g.y + (this.scrollOffset || 0) + 3; c(g, { left: a.translateX + b.checkboxOffset + g.x - 20 + "px", top: k + "px", display: this.proximate || k > f - 6 && k < f + d - 6 ? "" : "none" }) } }, this) } }; a.prototype.renderTitle = function () {
                var a = this.options, d = this.padding, c = a.title, e = 0; c.text && (this.title ||
                (this.title = this.chart.renderer.label(c.text, d - 3, d - 4, null, null, null, a.useHTML, null, "legend-title").attr({ zIndex: 1 }), this.chart.styledMode || this.title.css(c.style), this.title.add(this.group)), c.width || this.title.css({ width: this.maxLegendWidth + "px" }), a = this.title.getBBox(), e = a.height, this.offsetWidth = a.width, this.contentGroup.attr({ translateY: e })); this.titleHeight = e
            }; a.prototype.setText = function (a) { var b = this.options; a.legendItem.attr({ text: b.labelFormat ? B(b.labelFormat, a, this.chart) : b.labelFormatter.call(a) }) };
            a.prototype.renderItem = function (a) {
                var b = this.chart, d = b.renderer, c = this.options, e = this.symbolWidth, f = c.symbolPadding || 0, k = this.itemStyle, h = this.itemHiddenStyle, p = "horizontal" === c.layout ? J(c.itemDistance, 20) : 0, m = !c.rtl, t = a.legendItem, n = !a.series, q = !n && a.series.drawLegendSymbol ? a.series : a, w = q.options, v = this.createCheckboxForItem && w && w.showCheckbox; w = e + f + p + (v ? 20 : 0); var A = c.useHTML, u = a.options.className; t || (a.legendGroup = d.g("legend-item").addClass("highcharts-" + q.type + "-series highcharts-color-" + a.colorIndex +
                (u ? " " + u : "") + (n ? " highcharts-series-" + a.index : "")).attr({ zIndex: 1 }).add(this.scrollGroup), a.legendItem = t = d.text("", m ? e + f : -f, this.baseline || 0, A), b.styledMode || t.css(l(a.visible ? k : h)), t.attr({ align: m ? "left" : "right", zIndex: 2 }).add(a.legendGroup), this.baseline || (this.fontMetrics = d.fontMetrics(b.styledMode ? 12 : k.fontSize, t), this.baseline = this.fontMetrics.f + 3 + this.itemMarginTop, t.attr("y", this.baseline), this.symbolHeight = c.symbolHeight || this.fontMetrics.f, c.squareSymbol && (this.symbolWidth = J(c.symbolWidth,
                Math.max(this.symbolHeight, 16)), w = this.symbolWidth + f + p + (v ? 20 : 0), m && t.attr("x", this.symbolWidth + f))), q.drawLegendSymbol(this, a), this.setItemEvents && this.setItemEvents(a, t, A)); v && !a.checkbox && this.createCheckboxForItem && this.createCheckboxForItem(a); this.colorizeItem(a, a.visible); !b.styledMode && k.width || t.css({ width: (c.itemWidth || this.widthOption || b.spacingBox.width) - w + "px" }); this.setText(a); b = t.getBBox(); a.itemWidth = a.checkboxOffset = c.itemWidth || a.legendItemWidth || b.width + w; this.maxItemWidth = Math.max(this.maxItemWidth,
                a.itemWidth); this.totalItemWidth += a.itemWidth; this.itemHeight = a.itemHeight = Math.round(a.legendItemHeight || b.height || this.symbolHeight)
            }; a.prototype.layoutItem = function (a) {
                var b = this.options, d = this.padding, c = "horizontal" === b.layout, e = a.itemHeight, f = this.itemMarginBottom, k = this.itemMarginTop, l = c ? J(b.itemDistance, 20) : 0, h = this.maxLegendWidth; b = b.alignColumns && this.totalItemWidth > h ? this.maxItemWidth : a.itemWidth; c && this.itemX - d + b > h && (this.itemX = d, this.lastLineHeight && (this.itemY += k + this.lastLineHeight +
                f), this.lastLineHeight = 0); this.lastItemY = k + this.itemY + f; this.lastLineHeight = Math.max(e, this.lastLineHeight); a._legendItemPos = [this.itemX, this.itemY]; c ? this.itemX += b : (this.itemY += k + e + f, this.lastLineHeight = e); this.offsetWidth = this.widthOption || Math.max((c ? this.itemX - d - (a.checkbox ? 0 : l) : b) + d, this.offsetWidth)
            }; a.prototype.getAllItems = function () {
                var a = []; this.chart.series.forEach(function (b) {
                    var d = b && b.options; b && J(d.showInLegend, e(d.linkedTo) ? !1 : void 0, !0) && (a = a.concat(b.legendItems || ("point" === d.legendType ?
                    b.data : b)))
                }); w(this, "afterGetAllItems", { allItems: a }); return a
            }; a.prototype.getAlignment = function () { var a = this.options; return this.proximate ? a.align.charAt(0) + "tv" : a.floating ? "" : a.align.charAt(0) + a.verticalAlign.charAt(0) + a.layout.charAt(0) }; a.prototype.adjustMargins = function (a, d) {
                var b = this.chart, c = this.options, g = this.getAlignment(); g && [/(lth|ct|rth)/, /(rtv|rm|rbv)/, /(rbh|cb|lbh)/, /(lbv|lm|ltv)/].forEach(function (f, k) {
                    f.test(g) && !e(a[k]) && (b[q[k]] = Math.max(b[q[k]], b.legend[(k + 1) % 2 ? "legendHeight" :
                    "legendWidth"] + [1, -1, -1, 1][k] * c[k % 2 ? "x" : "y"] + J(c.margin, 12) + d[k] + (b.titleOffset[k] || 0)))
                })
            }; a.prototype.proximatePositions = function () {
                var a = this.chart, d = [], c = "left" === this.options.align; this.allItems.forEach(function (b) {
                    var e; var g = c; if (b.yAxis) {
                        b.xAxis.options.reversed && (g = !g); b.points && (e = m(g ? b.points : b.points.slice(0).reverse(), function (a) { return n(a.plotY) })); g = this.itemMarginTop + b.legendItem.getBBox().height + this.itemMarginBottom; var f = b.yAxis.top - a.plotTop; b.visible ? (e = e ? e.plotY : b.yAxis.height,
                        e += f - .3 * g) : e = f + b.yAxis.height; d.push({ target: e, size: g, item: b })
                    }
                }, this); E.distribute(d, a.plotHeight); d.forEach(function (b) { b.item._legendItemPos[1] = a.plotTop - a.spacing[0] + b.pos })
            }; a.prototype.render = function () {
                var a = this.chart, d = a.renderer, c = this.group, e = this.box, f = this.options, k = this.padding; this.itemX = k; this.itemY = this.initialItemY; this.lastItemY = this.offsetWidth = 0; this.widthOption = K(f.width, a.spacingBox.width - k); var l = a.spacingBox.width - 2 * k - f.x; -1 < ["rm", "lm"].indexOf(this.getAlignment().substring(0,
                2)) && (l /= 2); this.maxLegendWidth = this.widthOption || l; c || (this.group = c = d.g("legend").addClass(f.className || "").attr({ zIndex: 7 }).add(), this.contentGroup = d.g().attr({ zIndex: 1 }).add(c), this.scrollGroup = d.g().add(this.contentGroup)); this.renderTitle(); var h = this.getAllItems(); A(h, function (a, b) { return (a.options && a.options.legendIndex || 0) - (b.options && b.options.legendIndex || 0) }); f.reversed && h.reverse(); this.allItems = h; this.display = l = !!h.length; this.itemHeight = this.totalItemWidth = this.maxItemWidth = this.lastLineHeight =
                0; h.forEach(this.renderItem, this); h.forEach(this.layoutItem, this); h = (this.widthOption || this.offsetWidth) + k; var m = this.lastItemY + this.lastLineHeight + this.titleHeight; m = this.handleOverflow(m); m += k; e || (this.box = e = d.rect().addClass("highcharts-legend-box").attr({ r: f.borderRadius }).add(c), e.isNew = !0); a.styledMode || e.attr({ stroke: f.borderColor, "stroke-width": f.borderWidth || 0, fill: f.backgroundColor || "none" }).shadow(f.shadow); 0 < h && 0 < m && (e[e.isNew ? "attr" : "animate"](e.crisp.call({}, { x: 0, y: 0, width: h, height: m },
                e.strokeWidth())), e.isNew = !1); e[l ? "show" : "hide"](); a.styledMode && "none" === c.getStyle("display") && (h = m = 0); this.legendWidth = h; this.legendHeight = m; l && this.align(); this.proximate || this.positionItems(); w(this, "afterRender")
            }; a.prototype.align = function (a) {
                void 0 === a && (a = this.chart.spacingBox); var b = this.chart, d = this.options, c = a.y; /(lth|ct|rth)/.test(this.getAlignment()) && 0 < b.titleOffset[0] ? c += b.titleOffset[0] : /(lbh|cb|rbh)/.test(this.getAlignment()) && 0 < b.titleOffset[2] && (c -= b.titleOffset[2]); c !== a.y && (a =
                l(a, { y: c })); this.group.align(l(d, { width: this.legendWidth, height: this.legendHeight, verticalAlign: this.proximate ? "top" : d.verticalAlign }), !0, a)
            }; a.prototype.handleOverflow = function (a) {
                var b = this, d = this.chart, c = d.renderer, e = this.options, f = e.y, k = this.padding; f = d.spacingBox.height + ("top" === e.verticalAlign ? -f : f) - k; var l = e.maxHeight, h, m = this.clipRect, p = e.navigation, t = J(p.animation, !0), n = p.arrowSize || 12, w = this.nav, q = this.pages, v, A = this.allItems, I = function (a) {
                    "number" === typeof a ? m.attr({ height: a }) : m && (b.clipRect =
                    m.destroy(), b.contentGroup.clip()); b.contentGroup.div && (b.contentGroup.div.style.clip = a ? "rect(" + k + "px,9999px," + (k + a) + "px,0)" : "auto")
                }, N = function (a) { b[a] = c.circle(0, 0, 1.3 * n).translate(n / 2, n / 2).add(w); d.styledMode || b[a].attr("fill", "rgba(0,0,0,0.0001)"); return b[a] }; "horizontal" !== e.layout || "middle" === e.verticalAlign || e.floating || (f /= 2); l && (f = Math.min(f, l)); q.length = 0; a && 0 < f && a > f && !1 !== p.enabled ? (this.clipHeight = h = Math.max(f - 20 - this.titleHeight - k, 0), this.currentPage = J(this.currentPage, 1), this.fullHeight =
                a, A.forEach(function (a, b) { var d = a._legendItemPos[1], c = Math.round(a.legendItem.getBBox().height), e = q.length; if (!e || d - q[e - 1] > h && (v || d) !== q[e - 1]) q.push(v || d), e++; a.pageIx = e - 1; v && (A[b - 1].pageIx = e - 1); b === A.length - 1 && d + c - q[e - 1] > h && d !== v && (q.push(d), a.pageIx = e); d !== v && (v = d) }), m || (m = b.clipRect = c.clipRect(0, k, 9999, 0), b.contentGroup.clip(m)), I(h), w || (this.nav = w = c.g().attr({ zIndex: 1 }).add(this.group), this.up = c.symbol("triangle", 0, 0, n, n).add(w), N("upTracker").on("click", function () { b.scroll(-1, t) }), this.pager =
                c.text("", 15, 10).addClass("highcharts-legend-navigation"), d.styledMode || this.pager.css(p.style), this.pager.add(w), this.down = c.symbol("triangle-down", 0, 0, n, n).add(w), N("downTracker").on("click", function () { b.scroll(1, t) })), b.scroll(0), a = f) : w && (I(), this.nav = w.destroy(), this.scrollGroup.attr({ translateY: 1 }), this.clipHeight = 0); return a
            }; a.prototype.scroll = function (a, d) {
                var b = this, c = this.chart, e = this.pages, g = e.length, f = this.currentPage + a; a = this.clipHeight; var k = this.options.navigation, l = this.pager, h = this.padding;
                f > g && (f = g); 0 < f && ("undefined" !== typeof d && G(d, c), this.nav.attr({ translateX: h, translateY: a + this.padding + 7 + this.titleHeight, visibility: "visible" }), [this.up, this.upTracker].forEach(function (a) { a.attr({ "class": 1 === f ? "highcharts-legend-nav-inactive" : "highcharts-legend-nav-active" }) }), l.attr({ text: f + "/" + g }), [this.down, this.downTracker].forEach(function (a) { a.attr({ x: 18 + this.pager.getBBox().width, "class": f === g ? "highcharts-legend-nav-inactive" : "highcharts-legend-nav-active" }) }, this), c.styledMode || (this.up.attr({
                    fill: 1 ===
                    f ? k.inactiveColor : k.activeColor
                }), this.upTracker.css({ cursor: 1 === f ? "default" : "pointer" }), this.down.attr({ fill: f === g ? k.inactiveColor : k.activeColor }), this.downTracker.css({ cursor: f === g ? "default" : "pointer" })), this.scrollOffset = -e[f - 1] + this.initialItemY, this.scrollGroup.animate({ translateY: this.scrollOffset }), this.currentPage = f, this.positionCheckboxes(), d = v(J(d, c.renderer.globalAnimation, !0)), p(function () { w(b, "afterScroll", { currentPage: f }) }, d.duration))
            }; a.prototype.setItemEvents = function (a, d, c) {
                var b =
                this, e = b.chart.renderer.boxWrapper, g = a instanceof H, f = "highcharts-legend-" + (g ? "point" : "series") + "-active", k = b.chart.styledMode; (c ? [d, a.legendSymbol] : [a.legendGroup]).forEach(function (c) {
                    if (c) c.on("mouseover", function () { a.visible && b.allItems.forEach(function (b) { a !== b && b.setState("inactive", !g) }); a.setState("hover"); a.visible && e.addClass(f); k || d.css(b.options.itemHoverStyle) }).on("mouseout", function () {
                        b.chart.styledMode || d.css(l(a.visible ? b.itemStyle : b.itemHiddenStyle)); b.allItems.forEach(function (b) {
                            a !==
                            b && b.setState("", !g)
                        }); e.removeClass(f); a.setState()
                    }).on("click", function (d) { var c = function () { a.setVisible && a.setVisible(); b.allItems.forEach(function (b) { a !== b && b.setState(a.visible ? "inactive" : "", !g) }) }; e.removeClass(f); d = { browserEvent: d }; a.firePointEvent ? a.firePointEvent("legendItemClick", d, c) : w(a, "legendItemClick", d, c) })
                })
            }; a.prototype.createCheckboxForItem = function (a) {
                a.checkbox = f("input", { type: "checkbox", className: "highcharts-legend-checkbox", checked: a.selected, defaultChecked: a.selected }, this.options.itemCheckboxStyle,
                this.chart.container); h(a.checkbox, "click", function (b) { w(a.series || a, "checkboxClick", { checked: b.target.checked, item: a }, function () { a.select() }) })
            }; return a
        }(); (/Trident\/7\.0/.test(u.navigator && u.navigator.userAgent) || a) && x(d.prototype, "positionItem", function (a, b) { var d = this, c = function () { b._legendItemPos && a.call(d, b) }; c(); d.bubbleLegend || setTimeout(c) }); E.Legend = d; return E.Legend
    }); L(a, "Core/Series/SeriesRegistry.js", [a["Core/Globals.js"], a["Core/DefaultOptions.js"], a["Core/Series/Point.js"], a["Core/Utilities.js"]],
    function (a, u, E, H) {
        var v = u.defaultOptions, y = H.error, G = H.extendClass, B = H.merge, q; (function (h) {
            function f(a, e) { var c = v.plotOptions || {}, f = e.defaultOptions; e.prototype.pointClass || (e.prototype.pointClass = E); e.prototype.type = a; f && (c[a] = f); h.seriesTypes[a] = e } h.seriesTypes = a.seriesTypes; h.getSeries = function (a, e) { void 0 === e && (e = {}); var c = a.options.chart; c = e.type || c.type || c.defaultSeriesType || ""; var f = h.seriesTypes[c]; h || y(17, !0, a, { missingModuleFor: c }); c = new f; "function" === typeof c.init && c.init(a, e); return c };
            h.registerSeriesType = f; h.seriesType = function (a, e, t, m, w) { var c = v.plotOptions || {}; e = e || ""; c[a] = B(c[e], t); f(a, G(h.seriesTypes[e] || function () { }, m)); h.seriesTypes[a].prototype.type = a; w && (h.seriesTypes[a].prototype.pointClass = G(E, w)); return h.seriesTypes[a] }
        })(q || (q = {})); a.seriesType = q.seriesType; return q
    }); L(a, "Core/Chart/Chart.js", [a["Core/Animation/AnimationUtilities.js"], a["Core/Axis/Axis.js"], a["Core/FormatUtilities.js"], a["Core/Foundation.js"], a["Core/Globals.js"], a["Core/Legend.js"], a["Core/MSPointer.js"],
    a["Core/DefaultOptions.js"], a["Core/Color/Palette.js"], a["Core/Pointer.js"], a["Core/Renderer/RendererRegistry.js"], a["Core/Series/SeriesRegistry.js"], a["Core/Time.js"], a["Core/Utilities.js"], a["Core/Renderer/HTML/AST.js"]], function (a, u, E, H, x, y, G, B, q, h, f, c, e, t, m) {
        var w = a.animate, n = a.animObject, l = a.setAnimation, v = E.numberFormat, K = H.registerEventOptions, A = x.charts, p = x.doc, d = x.marginNames, k = x.win, b = B.defaultOptions, g = B.defaultTime, r = c.seriesTypes, F = t.addEvent, D = t.attr, M = t.cleanRecursively, C = t.createElement,
        O = t.css, S = t.defined, W = t.discardElement, Y = t.erase, ba = t.error, z = t.extend, Q = t.find, L = t.fireEvent, aa = t.getStyle, ja = t.isArray, I = t.isNumber, N = t.isObject, P = t.isString, U = t.merge, T = t.objectEach, R = t.pick, V = t.pInt, ca = t.relativeLength, da = t.removeEvent, fa = t.splat, ea = t.syncTimeout, ka = t.uniqueKey; a = function () {
            function a(a, b, d) {
                this.series = this.renderTo = this.renderer = this.pointer = this.pointCount = this.plotWidth = this.plotTop = this.plotLeft = this.plotHeight = this.plotBox = this.options = this.numberFormatter = this.margin = this.legend =
                this.labelCollectors = this.isResizing = this.index = this.eventOptions = this.container = this.colorCounter = this.clipBox = this.chartWidth = this.chartHeight = this.bounds = this.axisOffset = this.axes = void 0; this.sharedClips = {}; this.yAxis = this.xAxis = this.userOptions = this.titleOffset = this.time = this.symbolCounter = this.spacingBox = this.spacing = void 0; this.getArgs(a, b, d)
            } a.chart = function (b, d, c) { return new a(b, d, c) }; a.prototype.getArgs = function (a, b, d) { P(a) || a.nodeName ? (this.renderTo = a, this.init(b, d)) : this.init(a, b) }; a.prototype.init =
            function (a, d) {
                var c = a.plotOptions || {}; L(this, "init", { args: arguments }, function () {
                    var g = U(b, a), f = g.chart; T(g.plotOptions, function (a, b) { N(a) && (a.tooltip = c[b] && U(c[b].tooltip) || void 0) }); g.tooltip.userOptions = a.chart && a.chart.forExport && a.tooltip.userOptions || a.tooltip; this.userOptions = a; this.margin = []; this.spacing = []; this.bounds = { h: {}, v: {} }; this.labelCollectors = []; this.callback = d; this.isResizing = 0; this.options = g; this.axes = []; this.series = []; this.time = a.time && Object.keys(a.time).length ? new e(a.time) : x.time;
                    this.numberFormatter = f.numberFormatter || v; this.styledMode = f.styledMode; this.hasCartesianSeries = f.showAxes; this.index = A.length; A.push(this); x.chartCount++; K(this, f); this.xAxis = []; this.yAxis = []; this.pointCount = this.colorCounter = this.symbolCounter = 0; L(this, "afterInit"); this.firstRender()
                })
            }; a.prototype.initSeries = function (a) { var b = this.options.chart; b = a.type || b.type || b.defaultSeriesType; var d = r[b]; d || ba(17, !0, this, { missingModuleFor: b }); b = new d; "function" === typeof b.init && b.init(this, a); return b }; a.prototype.setSeriesData =
            function () { this.getSeriesOrderByLinks().forEach(function (a) { a.points || a.data || !a.enabledDataSorting || a.setData(a.options.data, !1) }) }; a.prototype.getSeriesOrderByLinks = function () { return this.series.concat().sort(function (a, b) { return a.linkedSeries.length || b.linkedSeries.length ? b.linkedSeries.length - a.linkedSeries.length : 0 }) }; a.prototype.orderSeries = function (a) { var b = this.series; a = a || 0; for (var d = b.length; a < d; ++a) b[a] && (b[a].index = a, b[a].name = b[a].getName()) }; a.prototype.isInsidePlot = function (a, b, d) {
                void 0 ===
                d && (d = {}); var c = this.inverted, e = this.plotBox, g = this.plotLeft, f = this.plotTop, k = this.scrollablePlotBox, l = 0; var h = 0; d.visiblePlotOnly && this.scrollingContainer && (h = this.scrollingContainer, l = h.scrollLeft, h = h.scrollTop); var I = d.series; e = d.visiblePlotOnly && k || e; k = d.inverted ? b : a; b = d.inverted ? a : b; a = { x: k, y: b, isInsidePlot: !0 }; if (!d.ignoreX) { var r = I && (c ? I.yAxis : I.xAxis) || { pos: g, len: Infinity }; k = d.paneCoordinates ? r.pos + k : g + k; k >= Math.max(l + g, r.pos) && k <= Math.min(l + g + e.width, r.pos + r.len) || (a.isInsidePlot = !1) } !d.ignoreY &&
                a.isInsidePlot && (c = I && (c ? I.xAxis : I.yAxis) || { pos: f, len: Infinity }, d = d.paneCoordinates ? c.pos + b : f + b, d >= Math.max(h + f, c.pos) && d <= Math.min(h + f + e.height, c.pos + c.len) || (a.isInsidePlot = !1)); L(this, "afterIsInsidePlot", a); return a.isInsidePlot
            }; a.prototype.redraw = function (a) {
                L(this, "beforeRedraw"); var b = this.hasCartesianSeries ? this.axes : this.colorAxis || [], d = this.series, c = this.pointer, e = this.legend, g = this.userOptions.legend, f = this.renderer, k = f.isHidden(), h = [], I = this.isDirtyBox, r = this.isDirtyLegend; this.setResponsive &&
                this.setResponsive(!1); l(this.hasRendered ? a : !1, this); k && this.temporaryDisplay(); this.layOutTitles(); for (a = d.length; a--;) { var m = d[a]; if (m.options.stacking || m.options.centerInCategory) { var p = !0; if (m.isDirty) { var t = !0; break } } } if (t) for (a = d.length; a--;) m = d[a], m.options.stacking && (m.isDirty = !0); d.forEach(function (a) { a.isDirty && ("point" === a.options.legendType ? ("function" === typeof a.updateTotals && a.updateTotals(), r = !0) : g && (g.labelFormatter || g.labelFormat) && (r = !0)); a.isDirtyData && L(a, "updatedData") }); r && e &&
                e.options.enabled && (e.render(), this.isDirtyLegend = !1); p && this.getStacks(); b.forEach(function (a) { a.updateNames(); a.setScale() }); this.getMargins(); b.forEach(function (a) { a.isDirty && (I = !0) }); b.forEach(function (a) { var b = a.min + "," + a.max; a.extKey !== b && (a.extKey = b, h.push(function () { L(a, "afterSetExtremes", z(a.eventArgs, a.getExtremes())); delete a.eventArgs })); (I || p) && a.redraw() }); I && this.drawChartBox(); L(this, "predraw"); d.forEach(function (a) { (I || a.isDirty) && a.visible && a.redraw(); a.isDirtyData = !1 }); c && c.reset(!0);
                f.draw(); L(this, "redraw"); L(this, "render"); k && this.temporaryDisplay(!0); h.forEach(function (a) { a.call() })
            }; a.prototype.get = function (a) { function b(b) { return b.id === a || b.options && b.options.id === a } for (var d = this.series, c = Q(this.axes, b) || Q(this.series, b), e = 0; !c && e < d.length; e++) c = Q(d[e].points || [], b); return c }; a.prototype.getAxes = function () {
                var a = this, b = this.options, d = b.xAxis = fa(b.xAxis || {}); b = b.yAxis = fa(b.yAxis || {}); L(this, "getAxes"); d.forEach(function (a, b) { a.index = b; a.isX = !0 }); b.forEach(function (a, b) {
                    a.index =
                    b
                }); d.concat(b).forEach(function (b) { new u(a, b) }); L(this, "afterGetAxes")
            }; a.prototype.getSelectedPoints = function () { var a = []; this.series.forEach(function (b) { a = a.concat(b.getPointsCollection().filter(function (a) { return R(a.selectedStaging, a.selected) })) }); return a }; a.prototype.getSelectedSeries = function () { return this.series.filter(function (a) { return a.selected }) }; a.prototype.setTitle = function (a, b, d) {
                this.applyDescription("title", a); this.applyDescription("subtitle", b); this.applyDescription("caption",
                void 0); this.layOutTitles(d)
            }; a.prototype.applyDescription = function (a, b) {
                var d = this, c = "title" === a ? { color: q.neutralColor80, fontSize: this.options.isStock ? "16px" : "18px" } : { color: q.neutralColor60 }; c = this.options[a] = U(!this.styledMode && { style: c }, this.options[a], b); var e = this[a]; e && b && (this[a] = e = e.destroy()); c && !e && (e = this.renderer.text(c.text, 0, 0, c.useHTML).attr({ align: c.align, "class": "highcharts-" + a, zIndex: c.zIndex || 4 }).add(), e.update = function (b) { d[{ title: "setTitle", subtitle: "setSubtitle", caption: "setCaption" }[a]](b) },
                this.styledMode || e.css(c.style), this[a] = e)
            }; a.prototype.layOutTitles = function (a) {
                var b = [0, 0, 0], d = this.renderer, c = this.spacingBox;["title", "subtitle", "caption"].forEach(function (a) {
                    var e = this[a], g = this.options[a], f = g.verticalAlign || "top"; a = "title" === a ? "top" === f ? -3 : 0 : "top" === f ? b[0] + 2 : 0; var k; if (e) {
                        this.styledMode || (k = g.style && g.style.fontSize); k = d.fontMetrics(k, e).b; e.css({ width: (g.width || c.width + (g.widthAdjust || 0)) + "px" }); var l = Math.round(e.getBBox(g.useHTML).height); e.align(z({
                            y: "bottom" === f ? k : a +
                            k, height: l
                        }, g), !1, "spacingBox"); g.floating || ("top" === f ? b[0] = Math.ceil(b[0] + l) : "bottom" === f && (b[2] = Math.ceil(b[2] + l)))
                    }
                }, this); b[0] && "top" === (this.options.title.verticalAlign || "top") && (b[0] += this.options.title.margin); b[2] && "bottom" === this.options.caption.verticalAlign && (b[2] += this.options.caption.margin); var e = !this.titleOffset || this.titleOffset.join(",") !== b.join(","); this.titleOffset = b; L(this, "afterLayOutTitles"); !this.isDirtyBox && e && (this.isDirtyBox = this.isDirtyLegend = e, this.hasRendered && R(a, !0) &&
                this.isDirtyBox && this.redraw())
            }; a.prototype.getChartSize = function () { var a = this.options.chart, b = a.width; a = a.height; var d = this.renderTo; S(b) || (this.containerWidth = aa(d, "width")); S(a) || (this.containerHeight = aa(d, "height")); this.chartWidth = Math.max(0, b || this.containerWidth || 600); this.chartHeight = Math.max(0, ca(a, this.chartWidth) || (1 < this.containerHeight ? this.containerHeight : 400)) }; a.prototype.temporaryDisplay = function (a) {
                var b = this.renderTo; if (a) for (; b && b.style;) b.hcOrigStyle && (O(b, b.hcOrigStyle), delete b.hcOrigStyle),
                b.hcOrigDetached && (p.body.removeChild(b), b.hcOrigDetached = !1), b = b.parentNode; else for (; b && b.style;) { p.body.contains(b) || b.parentNode || (b.hcOrigDetached = !0, p.body.appendChild(b)); if ("none" === aa(b, "display", !1) || b.hcOricDetached) b.hcOrigStyle = { display: b.style.display, height: b.style.height, overflow: b.style.overflow }, a = { display: "block", overflow: "hidden" }, b !== this.renderTo && (a.height = 0), O(b, a), b.offsetWidth || b.style.setProperty("display", "block", "important"); b = b.parentNode; if (b === p.body) break }
            }; a.prototype.setClassName =
            function (a) { this.container.className = "highcharts-container " + (a || "") }; a.prototype.getContainer = function () {
                var a = this.options, b = a.chart, d = ka(), c, e = this.renderTo; e || (this.renderTo = e = b.renderTo); P(e) && (this.renderTo = e = p.getElementById(e)); e || ba(13, !0, this); var g = V(D(e, "data-highcharts-chart")); I(g) && A[g] && A[g].hasRendered && A[g].destroy(); D(e, "data-highcharts-chart", this.index); e.innerHTML = ""; b.skipClone || e.offsetWidth || this.temporaryDisplay(); this.getChartSize(); g = this.chartWidth; var k = this.chartHeight;
                O(e, { overflow: "hidden" }); this.styledMode || (c = z({ position: "relative", overflow: "hidden", width: g + "px", height: k + "px", textAlign: "left", lineHeight: "normal", zIndex: 0, "-webkit-tap-highlight-color": "rgba(0,0,0,0)", userSelect: "none", "touch-action": "manipulation", outline: "none" }, b.style || {})); this.container = d = C("div", { id: d }, c, e); this._cursor = d.style.cursor; this.renderer = new (f.getRendererType(b.renderer))(d, g, k, void 0, b.forExport, a.exporting && a.exporting.allowHTML, this.styledMode); l(void 0, this); this.setClassName(b.className);
                if (this.styledMode) for (var h in a.defs) this.renderer.definition(a.defs[h]); else this.renderer.setStyle(b.style); this.renderer.chartIndex = this.index; L(this, "afterGetContainer")
            }; a.prototype.getMargins = function (a) {
                var b = this.spacing, d = this.margin, c = this.titleOffset; this.resetMargins(); c[0] && !S(d[0]) && (this.plotTop = Math.max(this.plotTop, c[0] + b[0])); c[2] && !S(d[2]) && (this.marginBottom = Math.max(this.marginBottom, c[2] + b[2])); this.legend && this.legend.display && this.legend.adjustMargins(d, b); L(this, "getMargins");
                a || this.getAxisMargins()
            }; a.prototype.getAxisMargins = function () { var a = this, b = a.axisOffset = [0, 0, 0, 0], c = a.colorAxis, e = a.margin, g = function (a) { a.forEach(function (a) { a.visible && a.getOffset() }) }; a.hasCartesianSeries ? g(a.axes) : c && c.length && g(c); d.forEach(function (d, c) { S(e[c]) || (a[d] += b[c]) }); a.setChartSize() }; a.prototype.reflow = function (a) {
                var b = this, d = b.options.chart, c = b.renderTo, e = S(d.width) && S(d.height), g = d.width || aa(c, "width"); d = d.height || aa(c, "height"); c = a ? a.target : k; delete b.pointer.chartPosition;
                if (!e && !b.isPrinting && g && d && (c === k || c === p)) { if (g !== b.containerWidth || d !== b.containerHeight) t.clearTimeout(b.reflowTimeout), b.reflowTimeout = ea(function () { b.container && b.setSize(void 0, void 0, !1) }, a ? 100 : 0); b.containerWidth = g; b.containerHeight = d }
            }; a.prototype.setReflow = function (a) { var b = this; !1 === a || this.unbindReflow ? !1 === a && this.unbindReflow && (this.unbindReflow = this.unbindReflow()) : (this.unbindReflow = F(k, "resize", function (a) { b.options && b.reflow(a) }), F(this, "destroy", this.unbindReflow)) }; a.prototype.setSize =
            function (a, b, d) {
                var c = this, e = c.renderer; c.isResizing += 1; l(d, c); d = e.globalAnimation; c.oldChartHeight = c.chartHeight; c.oldChartWidth = c.chartWidth; "undefined" !== typeof a && (c.options.chart.width = a); "undefined" !== typeof b && (c.options.chart.height = b); c.getChartSize(); c.styledMode || (d ? w : O)(c.container, { width: c.chartWidth + "px", height: c.chartHeight + "px" }, d); c.setChartSize(!0); e.setSize(c.chartWidth, c.chartHeight, d); c.axes.forEach(function (a) { a.isDirty = !0; a.setScale() }); c.isDirtyLegend = !0; c.isDirtyBox = !0; c.layOutTitles();
                c.getMargins(); c.redraw(d); c.oldChartHeight = null; L(c, "resize"); ea(function () { c && L(c, "endResize", null, function () { --c.isResizing }) }, n(d).duration)
            }; a.prototype.setChartSize = function (a) {
                var b = this.inverted, d = this.renderer, c = this.chartWidth, e = this.chartHeight, g = this.options.chart, f = this.spacing, k = this.clipOffset, l, h, I, r; this.plotLeft = l = Math.round(this.plotLeft); this.plotTop = h = Math.round(this.plotTop); this.plotWidth = I = Math.max(0, Math.round(c - l - this.marginRight)); this.plotHeight = r = Math.max(0, Math.round(e -
                h - this.marginBottom)); this.plotSizeX = b ? r : I; this.plotSizeY = b ? I : r; this.plotBorderWidth = g.plotBorderWidth || 0; this.spacingBox = d.spacingBox = { x: f[3], y: f[0], width: c - f[3] - f[1], height: e - f[0] - f[2] }; this.plotBox = d.plotBox = { x: l, y: h, width: I, height: r }; b = 2 * Math.floor(this.plotBorderWidth / 2); c = Math.ceil(Math.max(b, k[3]) / 2); e = Math.ceil(Math.max(b, k[0]) / 2); this.clipBox = { x: c, y: e, width: Math.floor(this.plotSizeX - Math.max(b, k[1]) / 2 - c), height: Math.max(0, Math.floor(this.plotSizeY - Math.max(b, k[2]) / 2 - e)) }; a || (this.axes.forEach(function (a) {
                    a.setAxisSize();
                    a.setAxisTranslation()
                }), d.alignElements()); L(this, "afterSetChartSize", { skipAxes: a })
            }; a.prototype.resetMargins = function () { L(this, "resetMargins"); var a = this, b = a.options.chart;["margin", "spacing"].forEach(function (d) { var c = b[d], e = N(c) ? c : [c, c, c, c];["Top", "Right", "Bottom", "Left"].forEach(function (c, g) { a[d][g] = R(b[d + c], e[g]) }) }); d.forEach(function (b, d) { a[b] = R(a.margin[d], a.spacing[d]) }); a.axisOffset = [0, 0, 0, 0]; a.clipOffset = [0, 0, 0, 0] }; a.prototype.drawChartBox = function () {
                var a = this.options.chart, b = this.renderer,
                d = this.chartWidth, c = this.chartHeight, e = this.styledMode, g = this.plotBGImage, f = a.backgroundColor, k = a.plotBackgroundColor, l = a.plotBackgroundImage, h = this.plotLeft, I = this.plotTop, r = this.plotWidth, m = this.plotHeight, p = this.plotBox, t = this.clipRect, n = this.clipBox, P = this.chartBackground, w = this.plotBackground, q = this.plotBorder, A, v = "animate"; P || (this.chartBackground = P = b.rect().addClass("highcharts-background").add(), v = "attr"); if (e) var N = A = P.strokeWidth(); else {
                    N = a.borderWidth || 0; A = N + (a.shadow ? 8 : 0); f = { fill: f || "none" };
                    if (N || P["stroke-width"]) f.stroke = a.borderColor, f["stroke-width"] = N; P.attr(f).shadow(a.shadow)
                } P[v]({ x: A / 2, y: A / 2, width: d - A - N % 2, height: c - A - N % 2, r: a.borderRadius }); v = "animate"; w || (v = "attr", this.plotBackground = w = b.rect().addClass("highcharts-plot-background").add()); w[v](p); e || (w.attr({ fill: k || "none" }).shadow(a.plotShadow), l && (g ? (l !== g.attr("href") && g.attr("href", l), g.animate(p)) : this.plotBGImage = b.image(l, h, I, r, m).add())); t ? t.animate({ width: n.width, height: n.height }) : this.clipRect = b.clipRect(n); v = "animate";
                q || (v = "attr", this.plotBorder = q = b.rect().addClass("highcharts-plot-border").attr({ zIndex: 1 }).add()); e || q.attr({ stroke: a.plotBorderColor, "stroke-width": a.plotBorderWidth || 0, fill: "none" }); q[v](q.crisp({ x: h, y: I, width: r, height: m }, -q.strokeWidth())); this.isDirtyBox = !1; L(this, "afterDrawChartBox")
            }; a.prototype.propFromSeries = function () {
                var a = this, b = a.options.chart, d = a.options.series, c, e, g;["inverted", "angular", "polar"].forEach(function (f) {
                    e = r[b.type || b.defaultSeriesType]; g = b[f] || e && e.prototype[f]; for (c = d &&
                    d.length; !g && c--;) (e = r[d[c].type]) && e.prototype[f] && (g = !0); a[f] = g
                })
            }; a.prototype.linkSeries = function () { var a = this, b = a.series; b.forEach(function (a) { a.linkedSeries.length = 0 }); b.forEach(function (b) { var d = b.options.linkedTo; P(d) && (d = ":previous" === d ? a.series[b.index - 1] : a.get(d)) && d.linkedParent !== b && (d.linkedSeries.push(b), b.linkedParent = d, d.enabledDataSorting && b.setDataSortingOptions(), b.visible = R(b.options.visible, d.options.visible, b.visible)) }); L(this, "afterLinkSeries") }; a.prototype.renderSeries = function () {
                this.series.forEach(function (a) {
                    a.translate();
                    a.render()
                })
            }; a.prototype.renderLabels = function () { var a = this, b = a.options.labels; b.items && b.items.forEach(function (d) { var c = z(b.style, d.style), e = V(c.left) + a.plotLeft, g = V(c.top) + a.plotTop + 12; delete c.left; delete c.top; a.renderer.text(d.html, e, g).attr({ zIndex: 2 }).css(c).add() }) }; a.prototype.render = function () {
                var a = this.axes, b = this.colorAxis, d = this.renderer, c = this.options, e = function (a) { a.forEach(function (a) { a.visible && a.render() }) }, g = 0; this.setTitle(); this.legend = new y(this, c.legend); this.getStacks &&
                this.getStacks(); this.getMargins(!0); this.setChartSize(); c = this.plotWidth; a.some(function (a) { if (a.horiz && a.visible && a.options.labels.enabled && a.series.length) return g = 21, !0 }); var f = this.plotHeight = Math.max(this.plotHeight - g, 0); a.forEach(function (a) { a.setScale() }); this.getAxisMargins(); var k = 1.1 < c / this.plotWidth, l = 1.05 < f / this.plotHeight; if (k || l) a.forEach(function (a) { (a.horiz && k || !a.horiz && l) && a.setTickInterval(!0) }), this.getMargins(); this.drawChartBox(); this.hasCartesianSeries ? e(a) : b && b.length && e(b);
                this.seriesGroup || (this.seriesGroup = d.g("series-group").attr({ zIndex: 3 }).add()); this.renderSeries(); this.renderLabels(); this.addCredits(); this.setResponsive && this.setResponsive(); this.hasRendered = !0
            }; a.prototype.addCredits = function (a) {
                var b = this, d = U(!0, this.options.credits, a); d.enabled && !this.credits && (this.credits = this.renderer.text(d.text + (this.mapCredits || ""), 0, 0).addClass("highcharts-credits").on("click", function () { d.href && (k.location.href = d.href) }).attr({ align: d.position.align, zIndex: 8 }), b.styledMode ||
                this.credits.css(d.style), this.credits.add().align(d.position), this.credits.update = function (a) { b.credits = b.credits.destroy(); b.addCredits(a) })
            }; a.prototype.destroy = function () {
                var a = this, b = a.axes, d = a.series, c = a.container, e = c && c.parentNode, g; L(a, "destroy"); a.renderer.forExport ? Y(A, a) : A[a.index] = void 0; x.chartCount--; a.renderTo.removeAttribute("data-highcharts-chart"); da(a); for (g = b.length; g--;) b[g] = b[g].destroy(); this.scroller && this.scroller.destroy && this.scroller.destroy(); for (g = d.length; g--;) d[g] =
                d[g].destroy(); "title subtitle chartBackground plotBackground plotBGImage plotBorder seriesGroup clipRect credits pointer rangeSelector legend resetZoomButton tooltip renderer".split(" ").forEach(function (b) { var d = a[b]; d && d.destroy && (a[b] = d.destroy()) }); c && (c.innerHTML = "", da(c), e && W(c)); T(a, function (b, d) { delete a[d] })
            }; a.prototype.firstRender = function () {
                var a = this, b = a.options; if (!a.isReadyToRender || a.isReadyToRender()) {
                    a.getContainer(); a.resetMargins(); a.setChartSize(); a.propFromSeries(); a.getAxes();
                    (ja(b.series) ? b.series : []).forEach(function (b) { a.initSeries(b) }); a.linkSeries(); a.setSeriesData(); L(a, "beforeRender"); h && (G.isRequired() ? a.pointer = new G(a, b) : a.pointer = new h(a, b)); a.render(); a.pointer.getChartPosition(); if (!a.renderer.imgCount && !a.hasLoaded) a.onload(); a.temporaryDisplay(!0)
                }
            }; a.prototype.onload = function () {
                this.callbacks.concat([this.callback]).forEach(function (a) { a && "undefined" !== typeof this.index && a.apply(this, [this]) }, this); L(this, "load"); L(this, "render"); S(this.index) && this.setReflow(this.options.chart.reflow);
                this.hasLoaded = !0
            }; a.prototype.addSeries = function (a, b, d) { var c = this, e; a && (b = R(b, !0), L(c, "addSeries", { options: a }, function () { e = c.initSeries(a); c.isDirtyLegend = !0; c.linkSeries(); e.enabledDataSorting && e.setData(a.data, !1); L(c, "afterAddSeries", { series: e }); b && c.redraw(d) })); return e }; a.prototype.addAxis = function (a, b, d, c) { return this.createAxis(b ? "xAxis" : "yAxis", { axis: a, redraw: d, animation: c }) }; a.prototype.addColorAxis = function (a, b, d) { return this.createAxis("colorAxis", { axis: a, redraw: b, animation: d }) }; a.prototype.createAxis =
            function (a, b) { var d = "colorAxis" === a, c = b.redraw, e = b.animation; a = U(b.axis, { index: this[a].length, isX: "xAxis" === a }); a = d ? new x.ColorAxis(this, a) : new u(this, a); d && (this.isDirtyLegend = !0, this.axes.forEach(function (a) { a.series = [] }), this.series.forEach(function (a) { a.bindAxes(); a.isDirtyData = !0 })); R(c, !0) && this.redraw(e); return a }; a.prototype.showLoading = function (a) {
                var b = this, d = b.options, c = d.loading, e = function () { g && O(g, { left: b.plotLeft + "px", top: b.plotTop + "px", width: b.plotWidth + "px", height: b.plotHeight + "px" }) },
                g = b.loadingDiv, f = b.loadingSpan; g || (b.loadingDiv = g = C("div", { className: "highcharts-loading highcharts-loading-hidden" }, null, b.container)); f || (b.loadingSpan = f = C("span", { className: "highcharts-loading-inner" }, null, g), F(b, "redraw", e)); g.className = "highcharts-loading"; m.setElementHTML(f, R(a, d.lang.loading, "")); b.styledMode || (O(g, z(c.style, { zIndex: 10 })), O(f, c.labelStyle), b.loadingShown || (O(g, { opacity: 0, display: "" }), w(g, { opacity: c.style.opacity || .5 }, { duration: c.showDuration || 0 }))); b.loadingShown = !0; e()
            }; a.prototype.hideLoading =
            function () { var a = this.options, b = this.loadingDiv; b && (b.className = "highcharts-loading highcharts-loading-hidden", this.styledMode || w(b, { opacity: 0 }, { duration: a.loading.hideDuration || 100, complete: function () { O(b, { display: "none" }) } })); this.loadingShown = !1 }; a.prototype.update = function (a, b, d, c) {
                var f = this, k = { credits: "addCredits", title: "setTitle", subtitle: "setSubtitle", caption: "setCaption" }, l = a.isResponsiveOptions, h = [], r, m; L(f, "update", { options: a }); l || f.setResponsive(!1, !0); a = M(a, f.options); f.userOptions = U(f.userOptions,
                a); var p = a.chart; if (p) { U(!0, f.options.chart, p); "className" in p && f.setClassName(p.className); "reflow" in p && f.setReflow(p.reflow); if ("inverted" in p || "polar" in p || "type" in p) { f.propFromSeries(); var t = !0 } "alignTicks" in p && (t = !0); "events" in p && K(this, p); T(p, function (a, b) { -1 !== f.propsRequireUpdateSeries.indexOf("chart." + b) && (r = !0); -1 !== f.propsRequireDirtyBox.indexOf(b) && (f.isDirtyBox = !0); -1 !== f.propsRequireReflow.indexOf(b) && (l ? f.isDirtyBox = !0 : m = !0) }); !f.styledMode && "style" in p && f.renderer.setStyle(p.style) } !f.styledMode &&
                a.colors && (this.options.colors = a.colors); a.time && (this.time === g && (this.time = new e(a.time)), U(!0, f.options.time, a.time)); T(a, function (b, d) { if (f[d] && "function" === typeof f[d].update) f[d].update(b, !1); else if ("function" === typeof f[k[d]]) f[k[d]](b); else "colors" !== d && -1 === f.collectionsWithUpdate.indexOf(d) && U(!0, f.options[d], a[d]); "chart" !== d && -1 !== f.propsRequireUpdateSeries.indexOf(d) && (r = !0) }); this.collectionsWithUpdate.forEach(function (b) {
                    if (a[b]) {
                        var c = []; f[b].forEach(function (a, b) {
                            a.options.isInternal ||
                            c.push(R(a.options.index, b))
                        }); fa(a[b]).forEach(function (a, e) { var g = S(a.id), k; g && (k = f.get(a.id)); !k && f[b] && (k = f[b][c ? c[e] : e]) && g && S(k.options.id) && (k = void 0); k && k.coll === b && (k.update(a, !1), d && (k.touched = !0)); !k && d && f.collectionsWithInit[b] && (f.collectionsWithInit[b][0].apply(f, [a].concat(f.collectionsWithInit[b][1] || []).concat([!1])).touched = !0) }); d && f[b].forEach(function (a) { a.touched || a.options.isInternal ? delete a.touched : h.push(a) })
                    }
                }); h.forEach(function (a) { a.chart && a.remove(!1) }); t && f.axes.forEach(function (a) {
                    a.update({},
                    !1)
                }); r && f.getSeriesOrderByLinks().forEach(function (a) { a.chart && a.update({}, !1) }, this); t = p && p.width; p = p && (P(p.height) ? ca(p.height, t || f.chartWidth) : p.height); m || I(t) && t !== f.chartWidth || I(p) && p !== f.chartHeight ? f.setSize(t, p, c) : R(b, !0) && f.redraw(c); L(f, "afterUpdate", { options: a, redraw: b, animation: c })
            }; a.prototype.setSubtitle = function (a, b) { this.applyDescription("subtitle", a); this.layOutTitles(b) }; a.prototype.setCaption = function (a, b) { this.applyDescription("caption", a); this.layOutTitles(b) }; a.prototype.showResetZoom =
            function () { function a() { d.zoomOut() } var d = this, c = b.lang, e = d.options.chart.resetZoomButton, g = e.theme, f = g.states, k = "chart" === e.relativeTo || "spacingBox" === e.relativeTo ? null : "scrollablePlotBox"; L(this, "beforeShowResetZoom", null, function () { d.resetZoomButton = d.renderer.button(c.resetZoom, null, null, a, g, f && f.hover).attr({ align: e.position.align, title: c.resetZoomTitle }).addClass("highcharts-reset-zoom").add().align(e.position, !1, k) }); L(this, "afterShowResetZoom") }; a.prototype.zoomOut = function () {
                L(this, "selection",
                { resetSelection: !0 }, this.zoom)
            }; a.prototype.zoom = function (a) {
                var b = this, d = b.pointer, c = b.inverted ? d.mouseDownX : d.mouseDownY, e = !1, g; !a || a.resetSelection ? (b.axes.forEach(function (a) { g = a.zoom() }), d.initiated = !1) : a.xAxis.concat(a.yAxis).forEach(function (a) { var f = a.axis, k = b.inverted ? f.left : f.top, l = b.inverted ? k + f.width : k + f.height, h = f.isXAxis, I = !1; if (!h && c >= k && c <= l || h || !S(c)) I = !0; d[h ? "zoomX" : "zoomY"] && I && (g = f.zoom(a.min, a.max), f.displayBtn && (e = !0)) }); var f = b.resetZoomButton; e && !f ? b.showResetZoom() : !e && N(f) &&
                (b.resetZoomButton = f.destroy()); g && b.redraw(R(b.options.chart.animation, a && a.animation, 100 > b.pointCount))
            }; a.prototype.pan = function (a, b) {
                var d = this, c = d.hoverPoints; b = "object" === typeof b ? b : { enabled: b, type: "x" }; var e = d.options.chart, g = d.options.mapNavigation && d.options.mapNavigation.enabled; e && e.panning && (e.panning = b); var f = b.type, k; L(this, "pan", { originalEvent: a }, function () {
                    c && c.forEach(function (a) { a.setState() }); var b = d.xAxis; "xy" === f ? b = b.concat(d.yAxis) : "y" === f && (b = d.yAxis); var e = {}; b.forEach(function (b) {
                        if (b.options.panningEnabled &&
                        !b.options.isInternal) {
                            var c = b.horiz, l = a[c ? "chartX" : "chartY"]; c = c ? "mouseDownX" : "mouseDownY"; var h = d[c], r = b.minPointOffset || 0, p = b.reversed && !d.inverted || !b.reversed && d.inverted ? -1 : 1, m = b.getExtremes(), t = b.toValue(h - l, !0) + r * p, n = b.toValue(h + b.len - l, !0) - (r * p || b.isXAxis && b.pointRangePadding || 0), P = n < t; p = b.hasVerticalPanning(); h = P ? n : t; t = P ? t : n; var q = b.panningState; !p || b.isXAxis || q && !q.isDirty || b.series.forEach(function (a) {
                                var b = a.getProcessedData(!0); b = a.getExtremes(b.yData, !0); q || (q = {
                                    startMin: Number.MAX_VALUE,
                                    startMax: -Number.MAX_VALUE
                                }); I(b.dataMin) && I(b.dataMax) && (q.startMin = Math.min(R(a.options.threshold, Infinity), b.dataMin, q.startMin), q.startMax = Math.max(R(a.options.threshold, -Infinity), b.dataMax, q.startMax))
                            }); p = Math.min(R(q && q.startMin, m.dataMin), r ? m.min : b.toValue(b.toPixels(m.min) - b.minPixelPadding)); n = Math.max(R(q && q.startMax, m.dataMax), r ? m.max : b.toValue(b.toPixels(m.max) + b.minPixelPadding)); b.panningState = q; b.isOrdinal || (r = p - h, 0 < r && (t += r, h = p), r = t - n, 0 < r && (t = n, h -= r), b.series.length && h !== m.min &&
                            t !== m.max && h >= p && t <= n && (b.setExtremes(h, t, !1, !1, { trigger: "pan" }), d.resetZoomButton || g || h === p || t === n || !f.match("y") || (d.showResetZoom(), b.displayBtn = !1), k = !0), e[c] = l)
                        }
                    }); T(e, function (a, b) { d[b] = a }); k && d.redraw(!1); O(d.container, { cursor: "move" })
                })
            }; return a
        }(); z(a.prototype, {
            callbacks: [], collectionsWithInit: { xAxis: [a.prototype.addAxis, [!0]], yAxis: [a.prototype.addAxis, [!1]], series: [a.prototype.addSeries] }, collectionsWithUpdate: ["xAxis", "yAxis", "zAxis", "series"], propsRequireDirtyBox: "backgroundColor borderColor borderWidth borderRadius plotBackgroundColor plotBackgroundImage plotBorderColor plotBorderWidth plotShadow shadow".split(" "),
            propsRequireReflow: "margin marginTop marginRight marginBottom marginLeft spacing spacingTop spacingRight spacingBottom spacingLeft".split(" "), propsRequireUpdateSeries: "chart.inverted chart.polar chart.ignoreHiddenSeries chart.type colors plotOptions time tooltip".split(" ")
        }); ""; return a
    }); L(a, "Mixins/LegendSymbol.js", [a["Core/Globals.js"], a["Core/Utilities.js"]], function (a, u) {
        var v = u.merge, H = u.pick; return a.LegendSymbolMixin = {
            drawRectangle: function (a, v) {
                var u = a.symbolHeight, B = a.options.squareSymbol;
                v.legendSymbol = this.chart.renderer.rect(B ? (a.symbolWidth - u) / 2 : 0, a.baseline - u + 1, B ? u : a.symbolWidth, u, H(a.options.symbolRadius, u / 2)).addClass("highcharts-point").attr({ zIndex: 3 }).add(v.legendGroup)
            }, drawLineMarker: function (a) {
                var u = this.options, x = u.marker, B = a.symbolWidth, q = a.symbolHeight, h = q / 2, f = this.chart.renderer, c = this.legendGroup; a = a.baseline - Math.round(.3 * a.fontMetrics.b); var e = {}; this.chart.styledMode || (e = { "stroke-width": u.lineWidth || 0 }, u.dashStyle && (e.dashstyle = u.dashStyle)); this.legendLine = f.path([["M",
                0, a], ["L", B, a]]).addClass("highcharts-graph").attr(e).add(c); x && !1 !== x.enabled && B && (u = Math.min(H(x.radius, h), h), 0 === this.symbol.indexOf("url") && (x = v(x, { width: q, height: q }), u = 0), this.legendSymbol = x = f.symbol(this.symbol, B / 2 - u, a - u, 2 * u, 2 * u, x).addClass("highcharts-point").add(c), x.isMarker = !0)
            }
        }
    }); L(a, "Core/Series/Series.js", [a["Core/Animation/AnimationUtilities.js"], a["Core/Foundation.js"], a["Core/Globals.js"], a["Mixins/LegendSymbol.js"], a["Core/DefaultOptions.js"], a["Core/Color/Palette.js"], a["Core/Series/Point.js"],
    a["Core/Series/SeriesRegistry.js"], a["Core/Renderer/SVG/SVGElement.js"], a["Core/Utilities.js"]], function (a, u, E, H, x, y, G, B, q, h) {
        var f = a.animObject, c = a.setAnimation, e = u.registerEventOptions, t = E.hasTouch, m = E.svg, w = E.win, n = x.defaultOptions, l = B.seriesTypes, v = h.addEvent, K = h.arrayMax, A = h.arrayMin, p = h.clamp, d = h.cleanRecursively, k = h.correctFloat, b = h.defined, g = h.erase, r = h.error, F = h.extend, D = h.find, M = h.fireEvent, C = h.getNestedProperty, O = h.isArray, S = h.isNumber, W = h.isString, Y = h.merge, L = h.objectEach, z = h.pick, Q = h.removeEvent,
        ia = h.splat, aa = h.syncTimeout; a = function () {
            function a() { this.zones = this.yAxis = this.xAxis = this.userOptions = this.tooltipOptions = this.processedYData = this.processedXData = this.points = this.options = this.linkedSeries = this.index = this.eventsToUnbind = this.eventOptions = this.data = this.chart = this._i = void 0 } a.prototype.init = function (a, b) {
                M(this, "init", { options: b }); var d = this, c = a.series; this.eventsToUnbind = []; d.chart = a; d.options = d.setOptions(b); b = d.options; d.linkedSeries = []; d.bindAxes(); F(d, {
                    name: b.name, state: "", visible: !1 !==
                    b.visible, selected: !0 === b.selected
                }); e(this, b); var g = b.events; if (g && g.click || b.point && b.point.events && b.point.events.click || b.allowPointSelect) a.runTrackerClick = !0; d.getColor(); d.getSymbol(); d.parallelArrays.forEach(function (a) { d[a + "Data"] || (d[a + "Data"] = []) }); d.isCartesian && (a.hasCartesianSeries = !0); var f; c.length && (f = c[c.length - 1]); d._i = z(f && f._i, -1) + 1; d.opacity = d.options.opacity; a.orderSeries(this.insert(c)); b.dataSorting && b.dataSorting.enabled ? d.setDataSortingOptions() : d.points || d.data || d.setData(b.data,
                !1); M(this, "afterInit")
            }; a.prototype.is = function (a) { return l[a] && this instanceof l[a] }; a.prototype.insert = function (a) { var b = this.options.index, d; if (S(b)) { for (d = a.length; d--;) if (b >= z(a[d].options.index, a[d]._i)) { a.splice(d + 1, 0, this); break } -1 === d && a.unshift(this); d += 1 } else a.push(this); return z(d, a.length - 1) }; a.prototype.bindAxes = function () {
                var a = this, b = a.options, d = a.chart, c; M(this, "bindAxes", null, function () {
                    (a.axisTypes || []).forEach(function (e) {
                        var g = 0; d[e].forEach(function (d) {
                            c = d.options; if (b[e] ===
                            g && !c.isInternal || "undefined" !== typeof b[e] && b[e] === c.id || "undefined" === typeof b[e] && 0 === c.index) a.insert(d.series), a[e] = d, d.isDirty = !0; c.isInternal || g++
                        }); a[e] || a.optionalAxis === e || r(18, !0, d)
                    })
                }); M(this, "afterBindAxes")
            }; a.prototype.updateParallelArrays = function (a, b) { var d = a.series, c = arguments, e = S(b) ? function (c) { var e = "y" === c && d.toYData ? d.toYData(a) : a[c]; d[c + "Data"][b] = e } : function (a) { Array.prototype[b].apply(d[a + "Data"], Array.prototype.slice.call(c, 2)) }; d.parallelArrays.forEach(e) }; a.prototype.hasData =
            function () { return this.visible && "undefined" !== typeof this.dataMax && "undefined" !== typeof this.dataMin || this.visible && this.yData && 0 < this.yData.length }; a.prototype.autoIncrement = function () {
                var a = this.options, b = this.xIncrement, d, c = a.pointIntervalUnit, e = this.chart.time; b = z(b, a.pointStart, 0); this.pointInterval = d = z(this.pointInterval, a.pointInterval, 1); c && (a = new e.Date(b), "day" === c ? e.set("Date", a, e.get("Date", a) + d) : "month" === c ? e.set("Month", a, e.get("Month", a) + d) : "year" === c && e.set("FullYear", a, e.get("FullYear",
                a) + d), d = a.getTime() - b); this.xIncrement = b + d; return b
            }; a.prototype.setDataSortingOptions = function () { var a = this.options; F(this, { requireSorting: !1, sorted: !1, enabledDataSorting: !0, allowDG: !1 }); b(a.pointRange) || (a.pointRange = 1) }; a.prototype.setOptions = function (a) {
                var d = this.chart, c = d.options, e = c.plotOptions, g = d.userOptions || {}; a = Y(a); d = d.styledMode; var f = { plotOptions: e, userOptions: a }; M(this, "setOptions", f); var k = f.plotOptions[this.type], l = g.plotOptions || {}; this.userOptions = f.userOptions; g = Y(k, e.series,
                g.plotOptions && g.plotOptions[this.type], a); this.tooltipOptions = Y(n.tooltip, n.plotOptions.series && n.plotOptions.series.tooltip, n.plotOptions[this.type].tooltip, c.tooltip.userOptions, e.series && e.series.tooltip, e[this.type].tooltip, a.tooltip); this.stickyTracking = z(a.stickyTracking, l[this.type] && l[this.type].stickyTracking, l.series && l.series.stickyTracking, this.tooltipOptions.shared && !this.noSharedTooltip ? !0 : g.stickyTracking); null === k.marker && delete g.marker; this.zoneAxis = g.zoneAxis; c = this.zones = (g.zones ||
                []).slice(); !g.negativeColor && !g.negativeFillColor || g.zones || (e = { value: g[this.zoneAxis + "Threshold"] || g.threshold || 0, className: "highcharts-negative" }, d || (e.color = g.negativeColor, e.fillColor = g.negativeFillColor), c.push(e)); c.length && b(c[c.length - 1].value) && c.push(d ? {} : { color: this.color, fillColor: this.fillColor }); M(this, "afterSetOptions", { options: g }); return g
            }; a.prototype.getName = function () { return z(this.options.name, "Series " + (this.index + 1)) }; a.prototype.getCyclic = function (a, d, c) {
                var e = this.chart, g =
                this.userOptions, f = a + "Index", k = a + "Counter", l = c ? c.length : z(e.options.chart[a + "Count"], e[a + "Count"]); if (!d) { var h = z(g[f], g["_" + f]); b(h) || (e.series.length || (e[k] = 0), g["_" + f] = h = e[k] % l, e[k] += 1); c && (d = c[h]) } "undefined" !== typeof h && (this[f] = h); this[a] = d
            }; a.prototype.getColor = function () { this.chart.styledMode ? this.getCyclic("color") : this.options.colorByPoint ? this.color = y.neutralColor20 : this.getCyclic("color", this.options.color || n.plotOptions[this.type].color, this.chart.options.colors) }; a.prototype.getPointsCollection =
            function () { return (this.hasGroupedData ? this.points : this.data) || [] }; a.prototype.getSymbol = function () { this.getCyclic("symbol", this.options.marker.symbol, this.chart.options.symbols) }; a.prototype.findPointIndex = function (a, b) {
                var d = a.id, c = a.x, e = this.points, g, f = this.options.dataSorting; if (d) var k = this.chart.get(d); else if (this.linkedParent || this.enabledDataSorting) { var l = f && f.matchByName ? "name" : "index"; k = D(e, function (b) { return !b.touched && b[l] === a[l] }); if (!k) return } if (k) {
                    var h = k && k.index; "undefined" !== typeof h &&
                    (g = !0)
                } "undefined" === typeof h && S(c) && (h = this.xData.indexOf(c, b)); -1 !== h && "undefined" !== typeof h && this.cropped && (h = h >= this.cropStart ? h - this.cropStart : h); !g && e[h] && e[h].touched && (h = void 0); return h
            }; a.prototype.updateData = function (a, d) {
                var c = this.options, e = c.dataSorting, g = this.points, f = [], k, l, h, r = this.requireSorting, p = a.length === g.length, m = !0; this.xIncrement = null; a.forEach(function (a, d) {
                    var l = b(a) && this.pointClass.prototype.optionsToObject.call({ series: this }, a) || {}; var m = l.x; if (l.id || S(m)) {
                        if (m = this.findPointIndex(l,
                        h), -1 === m || "undefined" === typeof m ? f.push(a) : g[m] && a !== c.data[m] ? (g[m].update(a, !1, null, !1), g[m].touched = !0, r && (h = m + 1)) : g[m] && (g[m].touched = !0), !p || d !== m || e && e.enabled || this.hasDerivedData) k = !0
                    } else f.push(a)
                }, this); if (k) for (a = g.length; a--;) (l = g[a]) && !l.touched && l.remove && l.remove(!1, d); else !p || e && e.enabled ? m = !1 : (a.forEach(function (a, b) { a !== g[b].y && g[b].update && g[b].update(a, !1, null, !1) }), f.length = 0); g.forEach(function (a) { a && (a.touched = !1) }); if (!m) return !1; f.forEach(function (a) {
                    this.addPoint(a, !1,
                    null, null, !1)
                }, this); null === this.xIncrement && this.xData && this.xData.length && (this.xIncrement = K(this.xData), this.autoIncrement()); return !0
            }; a.prototype.setData = function (a, b, d, c) {
                var e = this, g = e.points, f = g && g.length || 0, k, l = e.options, h = e.chart, p = l.dataSorting, m = null, t = e.xAxis; m = l.turboThreshold; var I = this.xData, n = this.yData, q = (k = e.pointArrayMap) && k.length, w = l.keys, A = 0, v = 1, C; a = a || []; k = a.length; b = z(b, !0); p && p.enabled && (a = this.sortData(a)); !1 !== c && k && f && !e.cropped && !e.hasGroupedData && e.visible && !e.isSeriesBoosting &&
                (C = this.updateData(a, d)); if (!C) {
                    e.xIncrement = null; e.colorCounter = 0; this.parallelArrays.forEach(function (a) { e[a + "Data"].length = 0 }); if (m && k > m) if (m = e.getFirstValidPoint(a), S(m)) for (d = 0; d < k; d++) I[d] = this.autoIncrement(), n[d] = a[d]; else if (O(m)) if (q) for (d = 0; d < k; d++) c = a[d], I[d] = c[0], n[d] = c.slice(1, q + 1); else for (w && (A = w.indexOf("x"), v = w.indexOf("y"), A = 0 <= A ? A : 0, v = 0 <= v ? v : 1), d = 0; d < k; d++) c = a[d], I[d] = c[A], n[d] = c[v]; else r(12, !1, h); else for (d = 0; d < k; d++) "undefined" !== typeof a[d] && (c = { series: e }, e.pointClass.prototype.applyOptions.apply(c,
                    [a[d]]), e.updateParallelArrays(c, d)); n && W(n[0]) && r(14, !0, h); e.data = []; e.options.data = e.userOptions.data = a; for (d = f; d--;) g[d] && g[d].destroy && g[d].destroy(); t && (t.minRange = t.userMinRange); e.isDirty = h.isDirtyBox = !0; e.isDirtyData = !!g; d = !1
                } "point" === l.legendType && (this.processData(), this.generatePoints()); b && h.redraw(d)
            }; a.prototype.sortData = function (a) {
                var d = this, c = d.options.dataSorting.sortKey || "y", e = function (a, d) { return b(d) && a.pointClass.prototype.optionsToObject.call({ series: a }, d) || {} }; a.forEach(function (b,
                c) { a[c] = e(d, b); a[c].index = c }, this); a.concat().sort(function (a, b) { a = C(c, a); b = C(c, b); return b < a ? -1 : b > a ? 1 : 0 }).forEach(function (a, b) { a.x = b }, this); d.linkedSeries && d.linkedSeries.forEach(function (b) { var d = b.options, c = d.data; d.dataSorting && d.dataSorting.enabled || !c || (c.forEach(function (d, g) { c[g] = e(b, d); a[g] && (c[g].x = a[g].x, c[g].index = g) }), b.setData(c, !1)) }); return a
            }; a.prototype.getProcessedData = function (a) {
                var b = this.xData, d = this.yData, c = b.length; var e = 0; var g = this.xAxis, f = this.options; var k = f.cropThreshold;
                var l = a || this.getExtremesFromAll || f.getExtremesFromAll, h = this.isCartesian; a = g && g.val2lin; f = !(!g || !g.logarithmic); var m = this.requireSorting; if (g) { g = g.getExtremes(); var p = g.min; var t = g.max } if (h && this.sorted && !l && (!k || c > k || this.forceCrop)) if (b[c - 1] < p || b[0] > t) b = [], d = []; else if (this.yData && (b[0] < p || b[c - 1] > t)) { e = this.cropData(this.xData, this.yData, p, t); b = e.xData; d = e.yData; e = e.start; var I = !0 } for (k = b.length || 1; --k;) if (c = f ? a(b[k]) - a(b[k - 1]) : b[k] - b[k - 1], 0 < c && ("undefined" === typeof n || c < n)) var n = c; else 0 > c &&
                m && (r(15, !1, this.chart), m = !1); return { xData: b, yData: d, cropped: I, cropStart: e, closestPointRange: n }
            }; a.prototype.processData = function (a) { var b = this.xAxis; if (this.isCartesian && !this.isDirty && !b.isDirty && !this.yAxis.isDirty && !a) return !1; a = this.getProcessedData(); this.cropped = a.cropped; this.cropStart = a.cropStart; this.processedXData = a.xData; this.processedYData = a.yData; this.closestPointRange = this.basePointRange = a.closestPointRange }; a.prototype.cropData = function (a, b, d, c, e) {
                var g = a.length, f = 0, k = g, l; e = z(e, this.cropShoulder);
                for (l = 0; l < g; l++) if (a[l] >= d) { f = Math.max(0, l - e); break } for (d = l; d < g; d++) if (a[d] > c) { k = d + e; break } return { xData: a.slice(f, k), yData: b.slice(f, k), start: f, end: k }
            }; a.prototype.generatePoints = function () {
                var a = this.options, b = a.data, d = this.data, c, e = this.processedXData, g = this.processedYData, f = this.pointClass, k = e.length, l = this.cropStart || 0, h = this.hasGroupedData, r = a.keys, m = [], p; a = a.dataGrouping && a.dataGrouping.groupAll ? l : 0; d || h || (d = [], d.length = b.length, d = this.data = d); r && h && (this.options.keys = !1); for (p = 0; p < k; p++) {
                    var t =
                    l + p; if (h) { var n = (new f).init(this, [e[p]].concat(ia(g[p]))); n.dataGroup = this.groupMap[a + p]; n.dataGroup.options && (n.options = n.dataGroup.options, F(n, n.dataGroup.options), delete n.dataLabels) } else (n = d[t]) || "undefined" === typeof b[t] || (d[t] = n = (new f).init(this, b[t], e[p])); n && (n.index = h ? a + p : t, m[p] = n)
                } this.options.keys = r; if (d && (k !== (c = d.length) || h)) for (p = 0; p < c; p++) p !== l || h || (p += k), d[p] && (d[p].destroyElements(), d[p].plotX = void 0); this.data = d; this.points = m; M(this, "afterGeneratePoints")
            }; a.prototype.getXExtremes =
            function (a) { return { min: A(a), max: K(a) } }; a.prototype.getExtremes = function (a, b) {
                var d = this.xAxis, c = this.yAxis, e = this.processedXData || this.xData, g = [], f = 0, k = 0; var l = 0; var h = this.requireSorting ? this.cropShoulder : 0, p = c ? c.positiveValuesOnly : !1, r; a = a || this.stackedYData || this.processedYData || []; c = a.length; d && (l = d.getExtremes(), k = l.min, l = l.max); for (r = 0; r < c; r++) {
                    var m = e[r]; var t = a[r]; var n = (S(t) || O(t)) && (t.length || 0 < t || !p); m = b || this.getExtremesFromAll || this.options.getExtremesFromAll || this.cropped || !d || (e[r +
                    h] || m) >= k && (e[r - h] || m) <= l; if (n && m) if (n = t.length) for (; n--;) S(t[n]) && (g[f++] = t[n]); else g[f++] = t
                } a = { dataMin: A(g), dataMax: K(g) }; M(this, "afterGetExtremes", { dataExtremes: a }); return a
            }; a.prototype.applyExtremes = function () { var a = this.getExtremes(); this.dataMin = a.dataMin; this.dataMax = a.dataMax; return a }; a.prototype.getFirstValidPoint = function (a) { for (var b = null, d = a.length, c = 0; null === b && c < d;) b = a[c], c++; return b }; a.prototype.translate = function () {
                this.processedXData || this.processData(); this.generatePoints();
                var a = this.options, d = a.stacking, c = this.xAxis, e = c.categories, g = this.enabledDataSorting, f = this.yAxis, l = this.points, h = l.length, r = !!this.modifyValue, m, t = this.pointPlacementToXValue(), n = !!t, q = a.threshold, w = a.startFromThreshold ? q : 0, A, v = this.zoneAxis || "y", C = Number.MAX_VALUE; for (m = 0; m < h; m++) {
                    var u = l[m], F = u.x, D = u.y, J = u.low, B = d && f.stacking && f.stacking.stacks[(this.negStacks && D < (w ? 0 : q) ? "-" : "") + this.stackKey], x = void 0, K = void 0; if (f.positiveValuesOnly && !f.validatePositiveValue(D) || c.positiveValuesOnly && !c.validatePositiveValue(F)) u.isNull =
                    !0; u.plotX = A = k(p(c.translate(F, 0, 0, 0, 1, t, "flags" === this.type), -1E5, 1E5)); if (d && this.visible && B && B[F]) { var y = this.getStackIndicator(y, F, this.index); u.isNull || (x = B[F], K = x.points[y.key]) } O(K) && (J = K[0], D = K[1], J === w && y.key === B[F].base && (J = z(S(q) && q, f.min)), f.positiveValuesOnly && 0 >= J && (J = null), u.total = u.stackTotal = x.total, u.percentage = x.total && u.y / x.total * 100, u.stackY = D, this.irregularWidths || x.setOffset(this.pointXOffset || 0, this.barW || 0)); u.yBottom = b(J) ? p(f.translate(J, 0, 1, 0, 1), -1E5, 1E5) : null; r && (D = this.modifyValue(D,
                    u)); u.plotY = void 0; S(D) && (D = f.translate(D, !1, !0, !1, !0), "undefined" !== typeof D && (u.plotY = p(D, -1E5, 1E5))); u.isInside = this.isPointInside(u); u.clientX = n ? k(c.translate(F, 0, 0, 0, 1, t)) : A; u.negative = u[v] < (a[v + "Threshold"] || q || 0); u.category = e && "undefined" !== typeof e[u.x] ? e[u.x] : u.x; if (!u.isNull && !1 !== u.visible) { "undefined" !== typeof E && (C = Math.min(C, Math.abs(A - E))); var E = A } u.zone = this.zones.length && u.getZone(); !u.graphic && this.group && g && (u.isNew = !0)
                } this.closestPointRangePx = C; M(this, "afterTranslate")
            }; a.prototype.getValidPoints =
            function (a, b, d) { var c = this.chart; return (a || this.points || []).filter(function (a) { return b && !c.isInsidePlot(a.plotX, a.plotY, { inverted: c.inverted }) ? !1 : !1 !== a.visible && (d || !a.isNull) }) }; a.prototype.getClipBox = function (a, b) {
                var d = this.options, c = this.chart, e = c.inverted, g = this.xAxis, f = g && this.yAxis, k = c.options.chart.scrollablePlotArea || {}; a && !1 === d.clip && f ? a = e ? { y: -c.chartWidth + f.len + f.pos, height: c.chartWidth, width: c.chartHeight, x: -c.chartHeight + g.len + g.pos } : {
                    y: -f.pos, height: c.chartHeight, width: c.chartWidth,
                    x: -g.pos
                } : (a = this.clipBox || c.clipBox, b && (a.width = c.plotSizeX, a.x = (c.scrollablePixelsX || 0) * (k.scrollPositionX || 0))); return b ? { width: a.width, x: a.x } : a
            }; a.prototype.getSharedClipKey = function (a) { if (this.sharedClipKey) return this.sharedClipKey; var b = [a && a.duration, a && a.easing, a && a.defer, this.getClipBox(a).height, this.options.xAxis, this.options.yAxis].join(); if (!1 !== this.options.clip || a) this.sharedClipKey = b; return b }; a.prototype.setClip = function (a) {
                var b = this.chart, d = this.options, c = b.renderer, e = b.inverted,
                g = this.clipBox, f = this.getClipBox(a), k = this.getSharedClipKey(a), l = b.sharedClips[k], h = b.sharedClips[k + "m"]; a && (f.width = 0, e && (f.x = b.plotHeight + (!1 !== d.clip ? 0 : b.plotTop))); l ? b.hasLoaded || l.attr(f) : (a && (b.sharedClips[k + "m"] = h = c.clipRect(e ? (b.plotSizeX || 0) + 99 : -99, e ? -b.plotLeft : -b.plotTop, 99, e ? b.chartWidth : b.chartHeight)), b.sharedClips[k] = l = c.clipRect(f), l.count = { length: 0 }); a && !l.count[this.index] && (l.count[this.index] = !0, l.count.length += 1); if (!1 !== d.clip || a) this.group.clip(a || g ? l : b.clipRect), this.markerGroup.clip(h);
                a || (l.count[this.index] && (delete l.count[this.index], --l.count.length), 0 === l.count.length && (g || (b.sharedClips[k] = l.destroy()), h && (b.sharedClips[k + "m"] = h.destroy())))
            }; a.prototype.animate = function (a) { var b = this.chart, d = f(this.options.animation), c = this.sharedClipKey; if (a) this.setClip(d); else if (c) { a = b.sharedClips[c]; c = b.sharedClips[c + "m"]; var e = this.getClipBox(d, !0); a && a.animate(e, d); c && c.animate({ width: e.width + 99, x: e.x - (b.inverted ? 0 : 99) }, d) } }; a.prototype.afterAnimate = function () {
                this.setClip(); M(this,
                "afterAnimate"); this.finishedAnimating = !0
            }; a.prototype.drawPoints = function () {
                var a = this.points, b = this.chart, d, c, e = this.options.marker, g = this[this.specialGroup] || this.markerGroup, f = this.xAxis, k = z(e.enabled, !f || f.isRadial ? !0 : null, this.closestPointRangePx >= e.enabledThreshold * e.radius); if (!1 !== e.enabled || this._hasPointMarkers) for (d = 0; d < a.length; d++) {
                    var l = a[d]; var h = (c = l.graphic) ? "animate" : "attr"; var p = l.marker || {}; var r = !!l.marker; if ((k && "undefined" === typeof p.enabled || p.enabled) && !l.isNull && !1 !== l.visible) {
                        var m =
                        z(p.symbol, this.symbol, "rect"); var t = this.markerAttribs(l, l.selected && "select"); this.enabledDataSorting && (l.startXPos = f.reversed ? -(t.width || 0) : f.width); var n = !1 !== l.isInside; c ? c[n ? "show" : "hide"](n).animate(t) : n && (0 < (t.width || 0) || l.hasImage) && (l.graphic = c = b.renderer.symbol(m, t.x, t.y, t.width, t.height, r ? p : e).add(g), this.enabledDataSorting && b.hasRendered && (c.attr({ x: l.startXPos }), h = "animate")); c && "animate" === h && c[n ? "show" : "hide"](n).animate(t); if (c && !b.styledMode) c[h](this.pointAttribs(l, l.selected &&
                        "select")); c && c.addClass(l.getClassName(), !0)
                    } else c && (l.graphic = c.destroy())
                }
            }; a.prototype.markerAttribs = function (a, b) { var d = this.options, c = d.marker, e = a.marker || {}, g = e.symbol || c.symbol, f = z(e.radius, c.radius); b && (c = c.states[b], b = e.states && e.states[b], f = z(b && b.radius, c && c.radius, f + (c && c.radiusPlus || 0))); a.hasImage = g && 0 === g.indexOf("url"); a.hasImage && (f = 0); a = { x: d.crisp ? Math.floor(a.plotX - f) : a.plotX - f, y: a.plotY - f }; f && (a.width = a.height = 2 * f); return a }; a.prototype.pointAttribs = function (a, b) {
                var d = this.options.marker,
                c = a && a.options, e = c && c.marker || {}, g = this.color, f = c && c.color, k = a && a.color; c = z(e.lineWidth, d.lineWidth); var l = a && a.zone && a.zone.color; a = 1; g = f || l || k || g; f = e.fillColor || d.fillColor || g; g = e.lineColor || d.lineColor || g; b = b || "normal"; d = d.states[b]; b = e.states && e.states[b] || {}; c = z(b.lineWidth, d.lineWidth, c + z(b.lineWidthPlus, d.lineWidthPlus, 0)); f = b.fillColor || d.fillColor || f; g = b.lineColor || d.lineColor || g; a = z(b.opacity, d.opacity, a); return { stroke: g, "stroke-width": c, fill: f, opacity: a }
            }; a.prototype.destroy = function (a) {
                var b =
                this, d = b.chart, c = /AppleWebKit\/533/.test(w.navigator.userAgent), e, f, k = b.data || [], l, p; M(b, "destroy"); this.removeEvents(a); (b.axisTypes || []).forEach(function (a) { (p = b[a]) && p.series && (g(p.series, b), p.isDirty = p.forceRedraw = !0) }); b.legendItem && b.chart.legend.destroyItem(b); for (f = k.length; f--;) (l = k[f]) && l.destroy && l.destroy(); b.clips && b.clips.forEach(function (a) { return a.destroy() }); h.clearTimeout(b.animationTimeout); L(b, function (a, b) { a instanceof q && !a.survive && (e = c && "group" === b ? "hide" : "destroy", a[e]()) });
                d.hoverSeries === b && (d.hoverSeries = void 0); g(d.series, b); d.orderSeries(); L(b, function (d, c) { a && "hcEvents" === c || delete b[c] })
            }; a.prototype.applyZones = function () {
                var a = this, b = this.chart, d = b.renderer, c = this.zones, e, g, f = this.clips || [], k, l = this.graph, h = this.area, r = Math.max(b.chartWidth, b.chartHeight), m = this[(this.zoneAxis || "y") + "Axis"], t = b.inverted, n, q, w, A = !1, v, u; if (c.length && (l || h) && m && "undefined" !== typeof m.min) {
                    var C = m.reversed; var F = m.horiz; l && !this.showLine && l.hide(); h && h.hide(); var D = m.getExtremes();
                    c.forEach(function (c, I) {
                        e = C ? F ? b.plotWidth : 0 : F ? 0 : m.toPixels(D.min) || 0; e = p(z(g, e), 0, r); g = p(Math.round(m.toPixels(z(c.value, D.max), !0) || 0), 0, r); A && (e = g = m.toPixels(D.max)); n = Math.abs(e - g); q = Math.min(e, g); w = Math.max(e, g); m.isXAxis ? (k = { x: t ? w : q, y: 0, width: n, height: r }, F || (k.x = b.plotHeight - k.x)) : (k = { x: 0, y: t ? w : q, width: r, height: n }, F && (k.y = b.plotWidth - k.y)); t && d.isVML && (k = m.isXAxis ? { x: 0, y: C ? q : w, height: k.width, width: b.chartWidth } : { x: k.y - b.plotLeft - b.spacingBox.x, y: 0, width: k.height, height: b.chartHeight }); f[I] ? f[I].animate(k) :
                        f[I] = d.clipRect(k); v = a["zone-area-" + I]; u = a["zone-graph-" + I]; l && u && u.clip(f[I]); h && v && v.clip(f[I]); A = c.value > D.max; a.resetZones && 0 === g && (g = void 0)
                    }); this.clips = f
                } else a.visible && (l && l.show(!0), h && h.show(!0))
            }; a.prototype.invertGroups = function (a) {
                function b() { ["group", "markerGroup"].forEach(function (b) { d[b] && (c.renderer.isVML && d[b].attr({ width: d.yAxis.len, height: d.xAxis.len }), d[b].width = d.yAxis.len, d[b].height = d.xAxis.len, d[b].invert(d.isRadialSeries ? !1 : a)) }) } var d = this, c = d.chart; d.xAxis && (d.eventsToUnbind.push(v(c,
                "resize", b)), b(), d.invertGroups = b)
            }; a.prototype.plotGroup = function (a, d, c, e, g) {
                var f = this[a], k = !f; c = { visibility: c, zIndex: e || .1 }; "undefined" === typeof this.opacity || this.chart.styledMode || "inactive" === this.state || (c.opacity = this.opacity); k && (this[a] = f = this.chart.renderer.g().add(g)); f.addClass("highcharts-" + d + " highcharts-series-" + this.index + " highcharts-" + this.type + "-series " + (b(this.colorIndex) ? "highcharts-color-" + this.colorIndex + " " : "") + (this.options.className || "") + (f.hasClass("highcharts-tracker") ?
                " highcharts-tracker" : ""), !0); f.attr(c)[k ? "attr" : "animate"](this.getPlotBox()); return f
            }; a.prototype.getPlotBox = function () { var a = this.chart, b = this.xAxis, d = this.yAxis; a.inverted && (b = d, d = this.xAxis); return { translateX: b ? b.left : a.plotLeft, translateY: d ? d.top : a.plotTop, scaleX: 1, scaleY: 1 } }; a.prototype.removeEvents = function (a) { a || Q(this); this.eventsToUnbind.length && (this.eventsToUnbind.forEach(function (a) { a() }), this.eventsToUnbind.length = 0) }; a.prototype.render = function () {
                var a = this, b = a.chart, d = a.options,
                c = f(d.animation), e = !a.finishedAnimating && b.renderer.isSVG && c.duration, g = a.visible ? "inherit" : "hidden", k = d.zIndex, l = a.hasRendered, h = b.seriesGroup, p = b.inverted; M(this, "render"); var m = a.plotGroup("group", "series", g, k, h); a.markerGroup = a.plotGroup("markerGroup", "markers", g, k, h); e && a.animate && a.animate(!0); m.inverted = z(a.invertible, a.isCartesian) ? p : !1; a.drawGraph && (a.drawGraph(), a.applyZones()); a.visible && a.drawPoints(); a.drawDataLabels && a.drawDataLabels(); a.redrawPoints && a.redrawPoints(); a.drawTracker &&
                !1 !== a.options.enableMouseTracking && a.drawTracker(); a.invertGroups(p); !1 === d.clip || a.sharedClipKey || l || m.clip(b.clipRect); e && a.animate && a.animate(); l || (e && c.defer && (e += c.defer), a.animationTimeout = aa(function () { a.afterAnimate() }, e || 0)); a.isDirty = !1; a.hasRendered = !0; M(a, "afterRender")
            }; a.prototype.redraw = function () {
                var a = this.chart, b = this.isDirty || this.isDirtyData, d = this.group, c = this.xAxis, e = this.yAxis; d && (a.inverted && d.attr({ width: a.plotWidth, height: a.plotHeight }), d.animate({
                    translateX: z(c && c.left,
                    a.plotLeft), translateY: z(e && e.top, a.plotTop)
                })); this.translate(); this.render(); b && delete this.kdTree
            }; a.prototype.searchPoint = function (a, b) { var d = this.xAxis, c = this.yAxis, e = this.chart.inverted; return this.searchKDTree({ clientX: e ? d.len - a.chartY + d.pos : a.chartX - d.pos, plotY: e ? c.len - a.chartX + c.pos : a.chartY - c.pos }, b, a) }; a.prototype.buildKDTree = function (a) {
                function b(a, c, e) {
                    var g; if (g = a && a.length) {
                        var f = d.kdAxisArray[c % e]; a.sort(function (a, b) { return a[f] - b[f] }); g = Math.floor(g / 2); return {
                            point: a[g], left: b(a.slice(0,
                            g), c + 1, e), right: b(a.slice(g + 1), c + 1, e)
                        }
                    }
                } this.buildingKdTree = !0; var d = this, c = -1 < d.options.findNearestPointBy.indexOf("y") ? 2 : 1; delete d.kdTree; aa(function () { d.kdTree = b(d.getValidPoints(null, !d.directTouch), c, c); d.buildingKdTree = !1 }, d.options.kdNow || a && "touchstart" === a.type ? 0 : 1)
            }; a.prototype.searchKDTree = function (a, d, c) {
                function e(a, d, c, h) {
                    var p = d.point, m = g.kdAxisArray[c % h], r = p; var t = b(a[f]) && b(p[f]) ? Math.pow(a[f] - p[f], 2) : null; var n = b(a[k]) && b(p[k]) ? Math.pow(a[k] - p[k], 2) : null; n = (t || 0) + (n || 0); p.dist =
                    b(n) ? Math.sqrt(n) : Number.MAX_VALUE; p.distX = b(t) ? Math.sqrt(t) : Number.MAX_VALUE; m = a[m] - p[m]; n = 0 > m ? "left" : "right"; t = 0 > m ? "right" : "left"; d[n] && (n = e(a, d[n], c + 1, h), r = n[l] < r[l] ? n : p); d[t] && Math.sqrt(m * m) < r[l] && (a = e(a, d[t], c + 1, h), r = a[l] < r[l] ? a : r); return r
                } var g = this, f = this.kdAxisArray[0], k = this.kdAxisArray[1], l = d ? "distX" : "dist"; d = -1 < g.options.findNearestPointBy.indexOf("y") ? 2 : 1; this.kdTree || this.buildingKdTree || this.buildKDTree(c); if (this.kdTree) return e(a, this.kdTree, d, d)
            }; a.prototype.pointPlacementToXValue =
            function () { var a = this.options, b = a.pointRange, d = this.xAxis; a = a.pointPlacement; "between" === a && (a = d.reversed ? -.5 : .5); return S(a) ? a * (b || d.pointRange) : 0 }; a.prototype.isPointInside = function (a) { return "undefined" !== typeof a.plotY && "undefined" !== typeof a.plotX && 0 <= a.plotY && a.plotY <= this.yAxis.len && 0 <= a.plotX && a.plotX <= this.xAxis.len }; a.prototype.drawTracker = function () {
                var a = this, b = a.options, d = b.trackByArea, c = [].concat(d ? a.areaPath : a.graphPath), e = a.chart, g = e.pointer, f = e.renderer, k = e.options.tooltip.snap, l =
                a.tracker, h = function (b) { if (e.hoverSeries !== a) a.onMouseOver() }, p = "rgba(192,192,192," + (m ? .0001 : .002) + ")"; l ? l.attr({ d: c }) : a.graph && (a.tracker = f.path(c).attr({ visibility: a.visible ? "visible" : "hidden", zIndex: 2 }).addClass(d ? "highcharts-tracker-area" : "highcharts-tracker-line").add(a.group), e.styledMode || a.tracker.attr({ "stroke-linecap": "round", "stroke-linejoin": "round", stroke: p, fill: d ? p : "none", "stroke-width": a.graph.strokeWidth() + (d ? 0 : 2 * k) }), [a.tracker, a.markerGroup, a.dataLabelsGroup].forEach(function (a) {
                    if (a &&
                    (a.addClass("highcharts-tracker").on("mouseover", h).on("mouseout", function (a) { g.onTrackerMouseOut(a) }), b.cursor && !e.styledMode && a.css({ cursor: b.cursor }), t)) a.on("touchstart", h)
                })); M(this, "afterDrawTracker")
            }; a.prototype.addPoint = function (a, b, d, c, e) {
                var g = this.options, f = this.data, k = this.chart, l = this.xAxis; l = l && l.hasNames && l.names; var h = g.data, p = this.xData, m; b = z(b, !0); var r = { series: this }; this.pointClass.prototype.applyOptions.apply(r, [a]); var t = r.x; var n = p.length; if (this.requireSorting && t < p[n - 1]) for (m =
                !0; n && p[n - 1] > t;) n--; this.updateParallelArrays(r, "splice", n, 0, 0); this.updateParallelArrays(r, n); l && r.name && (l[t] = r.name); h.splice(n, 0, a); m && (this.data.splice(n, 0, null), this.processData()); "point" === g.legendType && this.generatePoints(); d && (f[0] && f[0].remove ? f[0].remove(!1) : (f.shift(), this.updateParallelArrays(r, "shift"), h.shift())); !1 !== e && M(this, "addPoint", { point: r }); this.isDirtyData = this.isDirty = !0; b && k.redraw(c)
            }; a.prototype.removePoint = function (a, b, d) {
                var e = this, g = e.data, f = g[a], k = e.points, l = e.chart,
                h = function () { k && k.length === g.length && k.splice(a, 1); g.splice(a, 1); e.options.data.splice(a, 1); e.updateParallelArrays(f || { series: e }, "splice", a, 1); f && f.destroy(); e.isDirty = !0; e.isDirtyData = !0; b && l.redraw() }; c(d, l); b = z(b, !0); f ? f.firePointEvent("remove", null, h) : h()
            }; a.prototype.remove = function (a, b, d, c) { function e() { g.destroy(c); f.isDirtyLegend = f.isDirtyBox = !0; f.linkSeries(); z(a, !0) && f.redraw(b) } var g = this, f = g.chart; !1 !== d ? M(g, "remove", null, e) : e() }; a.prototype.update = function (a, b) {
                a = d(a, this.userOptions);
                M(this, "update", { options: a }); var c = this, e = c.chart, g = c.userOptions, f = c.initialType || c.type, k = e.options.plotOptions, h = a.type || g.type || e.options.chart.type, p = !(this.hasDerivedData || h && h !== this.type || "undefined" !== typeof a.pointStart || "undefined" !== typeof a.pointInterval || c.hasOptionChanged("dataGrouping") || c.hasOptionChanged("pointStart") || c.hasOptionChanged("pointInterval") || c.hasOptionChanged("pointIntervalUnit") || c.hasOptionChanged("keys")), m = l[f].prototype, t, n = ["eventOptions", "navigatorSeries", "baseSeries"],
                q = c.finishedAnimating && { animation: !1 }, w = {}; h = h || f; p && (n.push("data", "isDirtyData", "points", "processedXData", "processedYData", "xIncrement", "cropped", "_hasPointMarkers", "_hasPointLabels", "clips", "nodes", "layout", "mapMap", "mapData", "minY", "maxY", "minX", "maxX"), !1 !== a.visible && n.push("area", "graph"), c.parallelArrays.forEach(function (a) { n.push(a + "Data") }), a.data && (a.dataSorting && F(c.options.dataSorting, a.dataSorting), this.setData(a.data, !1))); a = Y(g, q, {
                    index: "undefined" === typeof g.index ? c.index : g.index,
                    pointStart: z(k && k.series && k.series.pointStart, g.pointStart, c.xData[0])
                }, !p && { data: c.options.data }, a); p && a.data && (a.data = c.options.data); n = ["group", "markerGroup", "dataLabelsGroup", "transformGroup"].concat(n); n.forEach(function (a) { n[a] = c[a]; delete c[a] }); g = !1; if (l[h]) { if (g = h !== c.type, c.remove(!1, !1, !1, !0), g) if (Object.setPrototypeOf) Object.setPrototypeOf(c, l[h].prototype); else { k = Object.hasOwnProperty.call(c, "hcEvents") && c.hcEvents; for (t in m) c[t] = void 0; F(c, l[h].prototype); k ? c.hcEvents = k : delete c.hcEvents } } else r(17,
                !0, e, { missingModuleFor: h }); n.forEach(function (a) { c[a] = n[a] }); c.init(e, a); if (p && this.points) { var A = c.options; !1 === A.visible ? (w.graphic = 1, w.dataLabel = 1) : c._hasPointLabels || (a = A.marker, h = A.dataLabels, a && (!1 === a.enabled || "symbol" in a) && (w.graphic = 1), h && !1 === h.enabled && (w.dataLabel = 1)); this.points.forEach(function (a) { a && a.series && (a.resolveColor(), Object.keys(w).length && a.destroyElements(w), !1 === A.showInLegend && a.legendItem && e.legend.destroyItem(a)) }, this) } c.initialType = f; e.linkSeries(); g && c.linkedSeries.length &&
                (c.isDirtyData = !0); M(this, "afterUpdate"); z(b, !0) && e.redraw(p ? void 0 : !1)
            }; a.prototype.setName = function (a) { this.name = this.options.name = this.userOptions.name = a; this.chart.isDirtyLegend = !0 }; a.prototype.hasOptionChanged = function (a) { var b = this.options[a], d = this.chart.options.plotOptions, c = this.userOptions[a]; return c ? b !== c : b !== z(d && d[this.type] && d[this.type][a], d && d.series && d.series[a], b) }; a.prototype.onMouseOver = function () {
                var a = this.chart, b = a.hoverSeries; a.pointer.setHoverChartIndex(); if (b && b !== this) b.onMouseOut();
                this.options.events.mouseOver && M(this, "mouseOver"); this.setState("hover"); a.hoverSeries = this
            }; a.prototype.onMouseOut = function () { var a = this.options, b = this.chart, d = b.tooltip, c = b.hoverPoint; b.hoverSeries = null; if (c) c.onMouseOut(); this && a.events.mouseOut && M(this, "mouseOut"); !d || this.stickyTracking || d.shared && !this.noSharedTooltip || d.hide(); b.series.forEach(function (a) { a.setState("", !0) }) }; a.prototype.setState = function (a, b) {
                var d = this, c = d.options, e = d.graph, g = c.inactiveOtherPoints, f = c.states, k = c.lineWidth,
                l = c.opacity, h = z(f[a || "normal"] && f[a || "normal"].animation, d.chart.options.chart.animation); c = 0; a = a || ""; if (d.state !== a && ([d.group, d.markerGroup, d.dataLabelsGroup].forEach(function (b) { b && (d.state && b.removeClass("highcharts-series-" + d.state), a && b.addClass("highcharts-series-" + a)) }), d.state = a, !d.chart.styledMode)) {
                    if (f[a] && !1 === f[a].enabled) return; a && (k = f[a].lineWidth || k + (f[a].lineWidthPlus || 0), l = z(f[a].opacity, l)); if (e && !e.dashstyle) for (f = { "stroke-width": k }, e.animate(f, h) ; d["zone-graph-" + c];) d["zone-graph-" +
                    c].animate(f, h), c += 1; g || [d.group, d.markerGroup, d.dataLabelsGroup, d.labelBySeries].forEach(function (a) { a && a.animate({ opacity: l }, h) })
                } b && g && d.points && d.setAllPointsToState(a || void 0)
            }; a.prototype.setAllPointsToState = function (a) { this.points.forEach(function (b) { b.setState && b.setState(a) }) }; a.prototype.setVisible = function (a, b) {
                var d = this, c = d.chart, e = d.legendItem, g = c.options.chart.ignoreHiddenSeries, f = d.visible; var k = (d.visible = a = d.options.visible = d.userOptions.visible = "undefined" === typeof a ? !f : a) ? "show" :
                "hide";["group", "dataLabelsGroup", "markerGroup", "tracker", "tt"].forEach(function (a) { if (d[a]) d[a][k]() }); if (c.hoverSeries === d || (c.hoverPoint && c.hoverPoint.series) === d) d.onMouseOut(); e && c.legend.colorizeItem(d, a); d.isDirty = !0; d.options.stacking && c.series.forEach(function (a) { a.options.stacking && a.visible && (a.isDirty = !0) }); d.linkedSeries.forEach(function (b) { b.setVisible(a, !1) }); g && (c.isDirtyBox = !0); M(d, k); !1 !== b && c.redraw()
            }; a.prototype.show = function () { this.setVisible(!0) }; a.prototype.hide = function () { this.setVisible(!1) };
            a.prototype.select = function (a) { this.selected = a = this.options.selected = "undefined" === typeof a ? !this.selected : a; this.checkbox && (this.checkbox.checked = a); M(this, a ? "select" : "unselect") }; a.prototype.shouldShowTooltip = function (a, b, d) { void 0 === d && (d = {}); d.series = this; d.visiblePlotOnly = !0; return this.chart.isInsidePlot(a, b, d) }; a.defaultOptions = {
                lineWidth: 2, allowPointSelect: !1, crisp: !0, showCheckbox: !1, animation: { duration: 1E3 }, events: {}, marker: {
                    enabledThreshold: 2, lineColor: y.backgroundColor, lineWidth: 0, radius: 4,
                    states: { normal: { animation: !0 }, hover: { animation: { duration: 50 }, enabled: !0, radiusPlus: 2, lineWidthPlus: 1 }, select: { fillColor: y.neutralColor20, lineColor: y.neutralColor100, lineWidth: 2 } }
                }, point: { events: {} }, dataLabels: { animation: {}, align: "center", defer: !0, formatter: function () { var a = this.series.chart.numberFormatter; return "number" !== typeof this.y ? "" : a(this.y, -1) }, padding: 5, style: { fontSize: "11px", fontWeight: "bold", color: "contrast", textOutline: "1px contrast" }, verticalAlign: "bottom", x: 0, y: 0 }, cropThreshold: 300,
                opacity: 1, pointRange: 0, softThreshold: !0, states: { normal: { animation: !0 }, hover: { animation: { duration: 50 }, lineWidthPlus: 1, marker: {}, halo: { size: 10, opacity: .25 } }, select: { animation: { duration: 0 } }, inactive: { animation: { duration: 50 }, opacity: .2 } }, stickyTracking: !0, turboThreshold: 1E3, findNearestPointBy: "x"
            }; return a
        }(); F(a.prototype, {
            axisTypes: ["xAxis", "yAxis"], coll: "series", colorCounter: 0, cropShoulder: 1, directTouch: !1, drawLegendSymbol: H.drawLineMarker, isCartesian: !0, kdAxisArray: ["clientX", "plotY"], parallelArrays: ["x",
            "y"], pointClass: G, requireSorting: !0, sorted: !0
        }); B.series = a; ""; ""; return a
    }); L(a, "Extensions/ScrollablePlotArea.js", [a["Core/Animation/AnimationUtilities.js"], a["Core/Axis/Axis.js"], a["Core/Chart/Chart.js"], a["Core/Series/Series.js"], a["Core/Renderer/RendererRegistry.js"], a["Core/Utilities.js"]], function (a, u, E, H, x, y) {
        var v = a.stop, B = y.addEvent, q = y.createElement, h = y.merge, f = y.pick; B(E, "afterSetChartSize", function (a) {
            var c = this.options.chart.scrollablePlotArea, f = c && c.minWidth; c = c && c.minHeight; if (!this.renderer.forExport) {
                if (f) {
                    if (this.scrollablePixelsX =
                    f = Math.max(0, f - this.chartWidth)) { this.scrollablePlotBox = this.renderer.scrollablePlotBox = h(this.plotBox); this.plotBox.width = this.plotWidth += f; this.inverted ? this.clipBox.height += f : this.clipBox.width += f; var m = { 1: { name: "right", value: f } } }
                } else c && (this.scrollablePixelsY = f = Math.max(0, c - this.chartHeight)) && (this.scrollablePlotBox = this.renderer.scrollablePlotBox = h(this.plotBox), this.plotBox.height = this.plotHeight += f, this.inverted ? this.clipBox.width += f : this.clipBox.height += f, m = { 2: { name: "bottom", value: f } });
                m && !a.skipAxes && this.axes.forEach(function (a) { m[a.side] ? a.getPlotLinePath = function () { var c = m[a.side].name, e = this[c]; this[c] = e - m[a.side].value; var f = u.prototype.getPlotLinePath.apply(this, arguments); this[c] = e; return f } : (a.setAxisSize(), a.setAxisTranslation()) })
            }
        }); B(E, "render", function () { this.scrollablePixelsX || this.scrollablePixelsY ? (this.setUpScrolling && this.setUpScrolling(), this.applyFixed()) : this.fixedDiv && this.applyFixed() }); E.prototype.setUpScrolling = function () {
            var a = this, e = {
                WebkitOverflowScrolling: "touch",
                overflowX: "hidden", overflowY: "hidden"
            }; this.scrollablePixelsX && (e.overflowX = "auto"); this.scrollablePixelsY && (e.overflowY = "auto"); this.scrollingParent = q("div", { className: "highcharts-scrolling-parent" }, { position: "relative" }, this.renderTo); this.scrollingContainer = q("div", { className: "highcharts-scrolling" }, e, this.scrollingParent); B(this.scrollingContainer, "scroll", function () { a.pointer && delete a.pointer.chartPosition }); this.innerContainer = q("div", { className: "highcharts-inner-container" }, null, this.scrollingContainer);
            this.innerContainer.appendChild(this.container); this.setUpScrolling = null
        }; E.prototype.moveFixedElements = function () {
            var a = this.container, e = this.fixedRenderer, f = ".highcharts-contextbutton .highcharts-credits .highcharts-legend .highcharts-legend-checkbox .highcharts-navigator-series .highcharts-navigator-xaxis .highcharts-navigator-yaxis .highcharts-navigator .highcharts-reset-zoom .highcharts-drillup-button .highcharts-scrollbar .highcharts-subtitle .highcharts-title".split(" "), h; this.scrollablePixelsX &&
            !this.inverted ? h = ".highcharts-yaxis" : this.scrollablePixelsX && this.inverted ? h = ".highcharts-xaxis" : this.scrollablePixelsY && !this.inverted ? h = ".highcharts-xaxis" : this.scrollablePixelsY && this.inverted && (h = ".highcharts-yaxis"); h && f.push(h + ":not(.highcharts-radial-axis)", h + "-labels:not(.highcharts-radial-axis-labels)"); f.forEach(function (c) { [].forEach.call(a.querySelectorAll(c), function (a) { (a.namespaceURI === e.SVG_NS ? e.box : e.box.parentNode).appendChild(a); a.style.pointerEvents = "auto" }) })
        }; E.prototype.applyFixed =
        function () {
            var a = !this.fixedDiv, e = this.options.chart, h = e.scrollablePlotArea, m = x.getRendererType(); a ? (this.fixedDiv = q("div", { className: "highcharts-fixed" }, { position: "absolute", overflow: "hidden", pointerEvents: "none", zIndex: (e.style && e.style.zIndex || 0) + 2, top: 0 }, null, !0), this.scrollingContainer && this.scrollingContainer.parentNode.insertBefore(this.fixedDiv, this.scrollingContainer), this.renderTo.style.overflow = "visible", this.fixedRenderer = e = new m(this.fixedDiv, this.chartWidth, this.chartHeight, this.options.chart.style),
            this.scrollableMask = e.path().attr({ fill: this.options.chart.backgroundColor || "#fff", "fill-opacity": f(h.opacity, .85), zIndex: -1 }).addClass("highcharts-scrollable-mask").add(), B(this, "afterShowResetZoom", this.moveFixedElements), B(this, "afterDrilldown", this.moveFixedElements), B(this, "afterLayOutTitles", this.moveFixedElements)) : this.fixedRenderer.setSize(this.chartWidth, this.chartHeight); if (this.scrollableDirty || a) this.scrollableDirty = !1, this.moveFixedElements(); e = this.chartWidth + (this.scrollablePixelsX ||
            0); m = this.chartHeight + (this.scrollablePixelsY || 0); v(this.container); this.container.style.width = e + "px"; this.container.style.height = m + "px"; this.renderer.boxWrapper.attr({ width: e, height: m, viewBox: [0, 0, e, m].join(" ") }); this.chartBackground.attr({ width: e, height: m }); this.scrollingContainer.style.height = this.chartHeight + "px"; a && (h.scrollPositionX && (this.scrollingContainer.scrollLeft = this.scrollablePixelsX * h.scrollPositionX), h.scrollPositionY && (this.scrollingContainer.scrollTop = this.scrollablePixelsY * h.scrollPositionY));
            m = this.axisOffset; a = this.plotTop - m[0] - 1; h = this.plotLeft - m[3] - 1; e = this.plotTop + this.plotHeight + m[2] + 1; m = this.plotLeft + this.plotWidth + m[1] + 1; var w = this.plotLeft + this.plotWidth - (this.scrollablePixelsX || 0), n = this.plotTop + this.plotHeight - (this.scrollablePixelsY || 0); a = this.scrollablePixelsX ? [["M", 0, a], ["L", this.plotLeft - 1, a], ["L", this.plotLeft - 1, e], ["L", 0, e], ["Z"], ["M", w, a], ["L", this.chartWidth, a], ["L", this.chartWidth, e], ["L", w, e], ["Z"]] : this.scrollablePixelsY ? [["M", h, 0], ["L", h, this.plotTop - 1], ["L", m, this.plotTop -
            1], ["L", m, 0], ["Z"], ["M", h, n], ["L", h, this.chartHeight], ["L", m, this.chartHeight], ["L", m, n], ["Z"]] : [["M", 0, 0]]; "adjustHeight" !== this.redrawTrigger && this.scrollableMask.attr({ d: a })
        }; B(u, "afterInit", function () { this.chart.scrollableDirty = !0 }); B(H, "show", function () { this.chart.scrollableDirty = !0 }); ""
    }); L(a, "Core/Axis/StackingAxis.js", [a["Core/Animation/AnimationUtilities.js"], a["Core/Utilities.js"]], function (a, u) {
        var v = a.getDeferredAnimation, H = u.addEvent, x = u.destroyObjectProperties, y = u.fireEvent, G = u.isNumber,
        B = u.objectEach, q = function () {
            function a(a) { this.oldStacks = {}; this.stacks = {}; this.stacksTouched = 0; this.axis = a } a.prototype.buildStacks = function () { var a = this.axis, c = a.series, e = a.options.reversedStacks, h = c.length, m; if (!a.isXAxis) { this.usePercentage = !1; for (m = h; m--;) { var q = c[e ? m : h - m - 1]; q.setStackedPoints(); q.setGroupedPoints() } for (m = 0; m < h; m++) c[m].modifyStacks(); y(a, "afterBuildStacks") } }; a.prototype.cleanStacks = function () {
                if (!this.axis.isXAxis) {
                    if (this.oldStacks) var a = this.stacks = this.oldStacks; B(a, function (a) {
                        B(a,
                        function (a) { a.cumulative = a.total })
                    })
                }
            }; a.prototype.resetStacks = function () { var a = this, c = this.stacks; this.axis.isXAxis || B(c, function (c) { B(c, function (e, f) { G(e.touched) && e.touched < a.stacksTouched ? (e.destroy(), delete c[f]) : (e.total = null, e.cumulative = null) }) }) }; a.prototype.renderStackTotals = function () {
                var a = this.axis, c = a.chart, e = c.renderer, h = this.stacks; a = v(c, a.options.stackLabels && a.options.stackLabels.animation || !1); var m = this.stackTotalGroup = this.stackTotalGroup || e.g("stack-labels").attr({
                    visibility: "visible",
                    zIndex: 6, opacity: 0
                }).add(); m.translate(c.plotLeft, c.plotTop); B(h, function (a) { B(a, function (a) { a.render(m) }) }); m.animate({ opacity: 1 }, a)
            }; return a
        }(); return function () { function a() { } a.compose = function (f) { H(f, "init", a.onInit); H(f, "destroy", a.onDestroy) }; a.onDestroy = function () { var a = this.stacking; if (a) { var c = a.stacks; B(c, function (a, f) { x(a); c[f] = null }); a && a.stackTotalGroup && a.stackTotalGroup.destroy() } }; a.onInit = function () { this.stacking || (this.stacking = new q(this)) }; return a }()
    }); L(a, "Extensions/Stacking.js",
    [a["Core/Axis/Axis.js"], a["Core/Chart/Chart.js"], a["Core/FormatUtilities.js"], a["Core/Globals.js"], a["Core/Series/Series.js"], a["Core/Axis/StackingAxis.js"], a["Core/Utilities.js"]], function (a, u, E, H, x, y, G) {
        var v = E.format, q = G.correctFloat, h = G.defined, f = G.destroyObjectProperties, c = G.isArray, e = G.isNumber, t = G.objectEach, m = G.pick, w = function () {
            function a(a, c, e, f, h) {
                var d = a.chart.inverted; this.axis = a; this.isNegative = e; this.options = c = c || {}; this.x = f; this.total = null; this.points = {}; this.hasValidPoints = !1; this.stack =
                h; this.rightCliff = this.leftCliff = 0; this.alignOptions = { align: c.align || (d ? e ? "left" : "right" : "center"), verticalAlign: c.verticalAlign || (d ? "middle" : e ? "bottom" : "top"), y: c.y, x: c.x }; this.textAlign = c.textAlign || (d ? e ? "right" : "left" : "center")
            } a.prototype.destroy = function () { f(this, this.axis) }; a.prototype.render = function (a) {
                var c = this.axis.chart, e = this.options, f = e.format; f = f ? v(f, this, c) : e.formatter.call(this); this.label ? this.label.attr({ text: f, visibility: "hidden" }) : (this.label = c.renderer.label(f, null, null, e.shape,
                null, null, e.useHTML, !1, "stack-labels"), f = { r: e.borderRadius || 0, text: f, rotation: e.rotation, padding: m(e.padding, 5), visibility: "hidden" }, c.styledMode || (f.fill = e.backgroundColor, f.stroke = e.borderColor, f["stroke-width"] = e.borderWidth, this.label.css(e.style)), this.label.attr(f), this.label.added || this.label.add(a)); this.label.labelrank = c.plotSizeY
            }; a.prototype.setOffset = function (a, c, f, n, p) {
                var d = this.axis, k = d.chart; n = d.translate(d.stacking.usePercentage ? 100 : n ? n : this.total, 0, 0, 0, 1); f = d.translate(f ? f : 0); f =
                h(n) && Math.abs(n - f); a = m(p, k.xAxis[0].translate(this.x)) + a; d = h(n) && this.getStackBox(k, this, a, n, c, f, d); c = this.label; f = this.isNegative; a = "justify" === m(this.options.overflow, "justify"); var b = this.textAlign; c && d && (p = c.getBBox(), n = c.padding, b = "left" === b ? k.inverted ? -n : n : "right" === b ? p.width : k.inverted && "center" === b ? p.width / 2 : k.inverted ? f ? p.width + n : -n : p.width / 2, f = k.inverted ? p.height / 2 : f ? -n : p.height, this.alignOptions.x = m(this.options.x, 0), this.alignOptions.y = m(this.options.y, 0), d.x -= b, d.y -= f, c.align(this.alignOptions,
                null, d), k.isInsidePlot(c.alignAttr.x + b - this.alignOptions.x, c.alignAttr.y + f - this.alignOptions.y) ? c.show() : (c.alignAttr.y = -9999, a = !1), a && x.prototype.justifyDataLabel.call(this.axis, c, this.alignOptions, c.alignAttr, p, d), c.attr({ x: c.alignAttr.x, y: c.alignAttr.y }), m(!a && this.options.crop, !0) && ((k = e(c.x) && e(c.y) && k.isInsidePlot(c.x - n + c.width, c.y) && k.isInsidePlot(c.x + n, c.y)) || c.hide()))
            }; a.prototype.getStackBox = function (a, c, e, f, h, d, k) {
                var b = c.axis.reversed, g = a.inverted, l = k.height + k.pos - (g ? a.plotLeft : a.plotTop);
                c = c.isNegative && !b || !c.isNegative && b; return { x: g ? c ? f - k.right : f - d + k.pos - a.plotLeft : e + a.xAxis[0].transB - a.plotLeft, y: g ? k.height - e - h : c ? l - f - d : l - f, width: g ? d : h, height: g ? h : d }
            }; return a
        }(); u.prototype.getStacks = function () {
            var a = this, c = a.inverted; a.yAxis.forEach(function (a) { a.stacking && a.stacking.stacks && a.hasVisibleSeries && (a.stacking.oldStacks = a.stacking.stacks) }); a.series.forEach(function (e) {
                var f = e.xAxis && e.xAxis.options || {}; !e.options.stacking || !0 !== e.visible && !1 !== a.options.chart.ignoreHiddenSeries ||
                (e.stackKey = [e.type, m(e.options.stack, ""), c ? f.top : f.left, c ? f.height : f.width].join())
            })
        }; y.compose(a); x.prototype.setGroupedPoints = function () { var a = this.yAxis.stacking; this.options.centerInCategory && (this.is("column") || this.is("columnrange")) && !this.options.stacking && 1 < this.chart.series.length ? x.prototype.setStackedPoints.call(this, "group") : a && t(a.stacks, function (c, e) { "group" === e.slice(-5) && (t(c, function (a) { return a.destroy() }), delete a.stacks[e]) }) }; x.prototype.setStackedPoints = function (a) {
            var e =
            a || this.options.stacking; if (e && (!0 === this.visible || !1 === this.chart.options.chart.ignoreHiddenSeries)) {
                var f = this.processedXData, n = this.processedYData, t = [], p = n.length, d = this.options, k = d.threshold, b = m(d.startFromThreshold && k, 0); d = d.stack; a = a ? this.type + "," + e : this.stackKey; var g = "-" + a, r = this.negStacks, v = this.yAxis, u = v.stacking.stacks, B = v.stacking.oldStacks, C, x; v.stacking.stacksTouched += 1; for (x = 0; x < p; x++) {
                    var y = f[x]; var E = n[x]; var G = this.getStackIndicator(G, y, this.index); var H = G.key; var z = (C = r && E < (b ? 0 :
                    k)) ? g : a; u[z] || (u[z] = {}); u[z][y] || (B[z] && B[z][y] ? (u[z][y] = B[z][y], u[z][y].total = null) : u[z][y] = new w(v, v.options.stackLabels, C, y, d)); z = u[z][y]; null !== E ? (z.points[H] = z.points[this.index] = [m(z.cumulative, b)], h(z.cumulative) || (z.base = H), z.touched = v.stacking.stacksTouched, 0 < G.index && !1 === this.singleStacks && (z.points[H][0] = z.points[this.index + "," + y + ",0"][0])) : z.points[H] = z.points[this.index] = null; "percent" === e ? (C = C ? a : g, r && u[C] && u[C][y] ? (C = u[C][y], z.total = C.total = Math.max(C.total, z.total) + Math.abs(E) || 0) :
                    z.total = q(z.total + (Math.abs(E) || 0))) : "group" === e ? (c(E) && (E = E[0]), null !== E && (z.total = (z.total || 0) + 1)) : z.total = q(z.total + (E || 0)); z.cumulative = "group" === e ? (z.total || 1) - 1 : m(z.cumulative, b) + (E || 0); null !== E && (z.points[H].push(z.cumulative), t[x] = z.cumulative, z.hasValidPoints = !0)
                } "percent" === e && (v.stacking.usePercentage = !0); "group" !== e && (this.stackedYData = t); v.stacking.oldStacks = {}
            }
        }; x.prototype.modifyStacks = function () {
            var a = this, c = a.stackKey, e = a.yAxis.stacking.stacks, f = a.processedXData, h, p = a.options.stacking;
            a[p + "Stacker"] && [c, "-" + c].forEach(function (d) { for (var c = f.length, b, g; c--;) if (b = f[c], h = a.getStackIndicator(h, b, a.index, d), g = (b = e[d] && e[d][b]) && b.points[h.key]) a[p + "Stacker"](g, b, c) })
        }; x.prototype.percentStacker = function (a, c, e) { c = c.total ? 100 / c.total : 0; a[0] = q(a[0] * c); a[1] = q(a[1] * c); this.stackedYData[e] = a[1] }; x.prototype.getStackIndicator = function (a, c, e, f) { !h(a) || a.x !== c || f && a.key !== f ? a = { x: c, index: 0, key: f } : a.index++; a.key = [e, c, a.index].join(); return a }; H.StackItem = w; ""; return H.StackItem
    }); L(a, "Series/Line/LineSeries.js",
    [a["Core/Color/Palette.js"], a["Core/Series/Series.js"], a["Core/Series/SeriesRegistry.js"], a["Core/Utilities.js"]], function (a, u, E, H) {
        var v = this && this.__extends || function () { var a = function (q, h) { a = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, c) { a.__proto__ = c } || function (a, c) { for (var e in c) c.hasOwnProperty(e) && (a[e] = c[e]) }; return a(q, h) }; return function (q, h) { function f() { this.constructor = q } a(q, h); q.prototype = null === h ? Object.create(h) : (f.prototype = h.prototype, new f) } }(), y = H.defined,
        G = H.merge; H = function (B) {
            function q() { var a = null !== B && B.apply(this, arguments) || this; a.data = void 0; a.options = void 0; a.points = void 0; return a } v(q, B); q.prototype.drawGraph = function () {
                var h = this, f = this.options, c = (this.gappedPath || this.getGraphPath).call(this), e = this.chart.styledMode, t = [["graph", "highcharts-graph"]]; e || t[0].push(f.lineColor || this.color || a.neutralColor20, f.dashStyle); t = h.getZonesGraphs(t); t.forEach(function (a, t) {
                    var m = a[0], l = h[m], q = l ? "animate" : "attr"; l ? (l.endX = h.preventGraphAnimation ? null :
                    c.xMap, l.animate({ d: c })) : c.length && (h[m] = l = h.chart.renderer.path(c).addClass(a[1]).attr({ zIndex: 1 }).add(h.group)); l && !e && (m = { stroke: a[2], "stroke-width": f.lineWidth, fill: h.fillGraph && h.color || "none" }, a[3] ? m.dashstyle = a[3] : "square" !== f.linecap && (m["stroke-linecap"] = m["stroke-linejoin"] = "round"), l[q](m).shadow(2 > t && f.shadow)); l && (l.startX = c.xMap, l.isArea = c.isArea)
                })
            }; q.prototype.getGraphPath = function (a, f, c) {
                var e = this, h = e.options, m = h.step, q, n = [], l = [], v; a = a || e.points; (q = a.reversed) && a.reverse(); (m = {
                    right: 1,
                    center: 2
                }[m] || m && 3) && q && (m = 4 - m); a = this.getValidPoints(a, !1, !(h.connectNulls && !f && !c)); a.forEach(function (t, q) {
                    var p = t.plotX, d = t.plotY, k = a[q - 1]; (t.leftCliff || k && k.rightCliff) && !c && (v = !0); t.isNull && !y(f) && 0 < q ? v = !h.connectNulls : t.isNull && !f ? v = !0 : (0 === q || v ? q = [["M", t.plotX, t.plotY]] : e.getPointSpline ? q = [e.getPointSpline(a, t, q)] : m ? (q = 1 === m ? [["L", k.plotX, d]] : 2 === m ? [["L", (k.plotX + p) / 2, k.plotY], ["L", (k.plotX + p) / 2, d]] : [["L", p, k.plotY]], q.push(["L", p, d])) : q = [["L", p, d]], l.push(t.x), m && (l.push(t.x), 2 === m && l.push(t.x)),
                    n.push.apply(n, q), v = !1)
                }); n.xMap = l; return e.graphPath = n
            }; q.prototype.getZonesGraphs = function (a) { this.zones.forEach(function (f, c) { c = ["zone-graph-" + c, "highcharts-graph highcharts-zone-graph-" + c + " " + (f.className || "")]; this.chart.styledMode || c.push(f.color || this.color, f.dashStyle || this.options.dashStyle); a.push(c) }, this); return a }; q.defaultOptions = G(u.defaultOptions, {}); return q
        }(u); E.registerSeriesType("line", H); ""; return H
    }); L(a, "Series/Area/AreaSeries.js", [a["Core/Color/Color.js"], a["Mixins/LegendSymbol.js"],
    a["Core/Series/SeriesRegistry.js"], a["Core/Utilities.js"]], function (a, u, E, H) {
        var v = this && this.__extends || function () { var a = function (c, e) { a = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, c) { a.__proto__ = c } || function (a, c) { for (var e in c) c.hasOwnProperty(e) && (a[e] = c[e]) }; return a(c, e) }; return function (c, e) { function f() { this.constructor = c } a(c, e); c.prototype = null === e ? Object.create(e) : (f.prototype = e.prototype, new f) } }(), y = a.parse, G = E.seriesTypes.line; a = H.extend; var B = H.merge, q = H.objectEach,
        h = H.pick; H = function (a) {
            function c() { var c = null !== a && a.apply(this, arguments) || this; c.data = void 0; c.options = void 0; c.points = void 0; return c } v(c, a); c.prototype.drawGraph = function () {
                this.areaPath = []; a.prototype.drawGraph.apply(this); var c = this, f = this.areaPath, m = this.options, q = [["area", "highcharts-area", this.color, m.fillColor]]; this.zones.forEach(function (a, e) { q.push(["zone-area-" + e, "highcharts-area highcharts-zone-area-" + e + " " + a.className, a.color || c.color, a.fillColor || m.fillColor]) }); q.forEach(function (a) {
                    var e =
                    a[0], t = c[e], n = t ? "animate" : "attr", q = {}; t ? (t.endX = c.preventGraphAnimation ? null : f.xMap, t.animate({ d: f })) : (q.zIndex = 0, t = c[e] = c.chart.renderer.path(f).addClass(a[1]).add(c.group), t.isArea = !0); c.chart.styledMode || (q.fill = h(a[3], y(a[2]).setOpacity(h(m.fillOpacity, .75)).get())); t[n](q); t.startX = f.xMap; t.shiftUnit = m.step ? 2 : 1
                })
            }; c.prototype.getGraphPath = function (a) {
                var c = G.prototype.getGraphPath, e = this.options, f = e.stacking, n = this.yAxis, l, q = [], v = [], u = this.index, p = n.stacking.stacks[this.stackKey], d = e.threshold,
                k = Math.round(n.getThreshold(e.threshold)); e = h(e.connectNulls, "percent" === f); var b = function (b, c, e) { var g = a[b]; b = f && p[g.x].points[u]; var h = g[e + "Null"] || 0; e = g[e + "Cliff"] || 0; g = !0; if (e || h) { var l = (h ? b[0] : b[1]) + e; var m = b[0] + e; g = !!h } else !f && a[c] && a[c].isNull && (l = m = d); "undefined" !== typeof l && (v.push({ plotX: r, plotY: null === l ? k : n.getThreshold(l), isNull: g, isCliff: !0 }), q.push({ plotX: r, plotY: null === m ? k : n.getThreshold(m), doCurve: !1 })) }; a = a || this.points; f && (a = this.getStackPoints(a)); for (l = 0; l < a.length; l++) {
                    f || (a[l].leftCliff =
                    a[l].rightCliff = a[l].leftNull = a[l].rightNull = void 0); var g = a[l].isNull; var r = h(a[l].rectPlotX, a[l].plotX); var F = f ? h(a[l].yBottom, k) : k; if (!g || e) e || b(l, l - 1, "left"), g && !f && e || (v.push(a[l]), q.push({ x: l, plotX: r, plotY: F })), e || b(l, l + 1, "right")
                } l = c.call(this, v, !0, !0); q.reversed = !0; g = c.call(this, q, !0, !0); (F = g[0]) && "M" === F[0] && (g[0] = ["L", F[1], F[2]]); g = l.concat(g); g.length && g.push(["Z"]); c = c.call(this, v, !1, e); g.xMap = l.xMap; this.areaPath = g; return c
            }; c.prototype.getStackPoints = function (a) {
                var c = this, e = [], f = [],
                n = this.xAxis, l = this.yAxis, v = l.stacking.stacks[this.stackKey], u = {}, A = l.series, p = A.length, d = l.options.reversedStacks ? 1 : -1, k = A.indexOf(c); a = a || this.points; if (this.options.stacking) {
                    for (var b = 0; b < a.length; b++) a[b].leftNull = a[b].rightNull = void 0, u[a[b].x] = a[b]; q(v, function (a, b) { null !== a.total && f.push(b) }); f.sort(function (a, b) { return a - b }); var g = A.map(function (a) { return a.visible }); f.forEach(function (a, b) {
                        var r = 0, m, t; if (u[a] && !u[a].isNull) e.push(u[a]), [-1, 1].forEach(function (e) {
                            var h = 1 === e ? "rightNull" :
                            "leftNull", l = 0, r = v[f[b + e]]; if (r) for (var n = k; 0 <= n && n < p;) { var q = A[n].index; m = r.points[q]; m || (q === c.index ? u[a][h] = !0 : g[n] && (t = v[a].points[q]) && (l -= t[1] - t[0])); n += d } u[a][1 === e ? "rightCliff" : "leftCliff"] = l
                        }); else { for (var q = k; 0 <= q && q < p;) { if (m = v[a].points[A[q].index]) { r = m[1]; break } q += d } r = h(r, 0); r = l.translate(r, 0, 1, 0, 1); e.push({ isNull: !0, plotX: n.translate(a, 0, 0, 0, 1), x: a, plotY: r, yBottom: r }) }
                    })
                } return e
            }; c.defaultOptions = B(G.defaultOptions, { threshold: 0 }); return c
        }(G); a(H.prototype, { singleStacks: !1, drawLegendSymbol: u.drawRectangle });
        E.registerSeriesType("area", H); ""; return H
    }); L(a, "Series/Spline/SplineSeries.js", [a["Core/Series/SeriesRegistry.js"], a["Core/Utilities.js"]], function (a, u) {
        var v = this && this.__extends || function () {
            var a = function (v, q) { a = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, f) { a.__proto__ = f } || function (a, f) { for (var c in f) f.hasOwnProperty(c) && (a[c] = f[c]) }; return a(v, q) }; return function (v, q) {
                function h() { this.constructor = v } a(v, q); v.prototype = null === q ? Object.create(q) : (h.prototype = q.prototype,
                new h)
            }
        }(), H = a.seriesTypes.line, x = u.merge, y = u.pick; u = function (a) {
            function u() { var q = null !== a && a.apply(this, arguments) || this; q.data = void 0; q.options = void 0; q.points = void 0; return q } v(u, a); u.prototype.getPointSpline = function (a, h, f) {
                var c = h.plotX || 0, e = h.plotY || 0, t = a[f - 1]; f = a[f + 1]; if (t && !t.isNull && !1 !== t.doCurve && !h.isCliff && f && !f.isNull && !1 !== f.doCurve && !h.isCliff) {
                    a = t.plotY || 0; var m = f.plotX || 0; f = f.plotY || 0; var q = 0; var n = (1.5 * c + (t.plotX || 0)) / 2.5; var l = (1.5 * e + a) / 2.5; m = (1.5 * c + m) / 2.5; var v = (1.5 * e + f) / 2.5;
                    m !== n && (q = (v - l) * (m - c) / (m - n) + e - v); l += q; v += q; l > a && l > e ? (l = Math.max(a, e), v = 2 * e - l) : l < a && l < e && (l = Math.min(a, e), v = 2 * e - l); v > f && v > e ? (v = Math.max(f, e), l = 2 * e - v) : v < f && v < e && (v = Math.min(f, e), l = 2 * e - v); h.rightContX = m; h.rightContY = v
                } h = ["C", y(t.rightContX, t.plotX, 0), y(t.rightContY, t.plotY, 0), y(n, c, 0), y(l, e, 0), c, e]; t.rightContX = t.rightContY = void 0; return h
            }; u.defaultOptions = x(H.defaultOptions); return u
        }(H); a.registerSeriesType("spline", u); ""; return u
    }); L(a, "Series/AreaSpline/AreaSplineSeries.js", [a["Series/Area/AreaSeries.js"],
    a["Series/Spline/SplineSeries.js"], a["Mixins/LegendSymbol.js"], a["Core/Series/SeriesRegistry.js"], a["Core/Utilities.js"]], function (a, u, E, H, x) {
        var v = this && this.__extends || function () { var a = function (f, c) { a = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, c) { a.__proto__ = c } || function (a, c) { for (var e in c) c.hasOwnProperty(e) && (a[e] = c[e]) }; return a(f, c) }; return function (f, c) { function e() { this.constructor = f } a(f, c); f.prototype = null === c ? Object.create(c) : (e.prototype = c.prototype, new e) } }(), G =
        a.prototype, B = x.extend, q = x.merge; x = function (h) { function f() { var a = null !== h && h.apply(this, arguments) || this; a.data = void 0; a.points = void 0; a.options = void 0; return a } v(f, h); f.defaultOptions = q(u.defaultOptions, a.defaultOptions); return f }(u); B(x.prototype, { getGraphPath: G.getGraphPath, getStackPoints: G.getStackPoints, drawGraph: G.drawGraph, drawLegendSymbol: E.drawRectangle }); H.registerSeriesType("areaspline", x); ""; return x
    }); L(a, "Series/Column/ColumnSeries.js", [a["Core/Animation/AnimationUtilities.js"], a["Core/Color/Color.js"],
    a["Core/Globals.js"], a["Mixins/LegendSymbol.js"], a["Core/Color/Palette.js"], a["Core/Series/Series.js"], a["Core/Series/SeriesRegistry.js"], a["Core/Utilities.js"]], function (a, u, E, H, x, y, G, B) {
        var q = this && this.__extends || function () {
            var a = function (d, b) { a = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, b) { a.__proto__ = b } || function (a, b) { for (var d in b) b.hasOwnProperty(d) && (a[d] = b[d]) }; return a(d, b) }; return function (d, b) {
                function c() { this.constructor = d } a(d, b); d.prototype = null === b ? Object.create(b) :
                (c.prototype = b.prototype, new c)
            }
        }(), h = a.animObject, f = u.parse, c = E.hasTouch; a = E.noop; var e = B.clamp, t = B.css, m = B.defined, v = B.extend, n = B.fireEvent, l = B.isArray, J = B.isNumber, K = B.merge, A = B.pick, p = B.objectEach; B = function (a) {
            function d() { var b = null !== a && a.apply(this, arguments) || this; b.borderWidth = void 0; b.data = void 0; b.group = void 0; b.options = void 0; b.points = void 0; return b } q(d, a); d.prototype.animate = function (a) {
                var b = this, d = this.yAxis, c = b.options, f = this.chart.inverted, k = {}, l = f ? "translateX" : "translateY"; if (a) k.scaleY =
                .001, a = e(d.toPixels(c.threshold), d.pos, d.pos + d.len), f ? k.translateX = a - d.len : k.translateY = a, b.clipBox && b.setClip(), b.group.attr(k); else { var p = Number(b.group.attr(l)); b.group.animate({ scaleY: 1 }, v(h(b.options.animation), { step: function (a, c) { b.group && (k[l] = p + c.pos * (d.pos - p), b.group.attr(k)) } })) }
            }; d.prototype.init = function (b, d) { a.prototype.init.apply(this, arguments); var c = this; b = c.chart; b.hasRendered && b.series.forEach(function (a) { a.type === c.type && (a.isDirty = !0) }) }; d.prototype.getColumnMetrics = function () {
                var a =
                this, d = a.options, c = a.xAxis, e = a.yAxis, f = c.options.reversedStacks; f = c.reversed && !f || !c.reversed && f; var k, l = {}, h = 0; !1 === d.grouping ? h = 1 : a.chart.series.forEach(function (b) { var d = b.yAxis, c = b.options; if (b.type === a.type && (b.visible || !a.chart.options.chart.ignoreHiddenSeries) && e.len === d.len && e.pos === d.pos) { if (c.stacking && "group" !== c.stacking) { k = b.stackKey; "undefined" === typeof l[k] && (l[k] = h++); var f = l[k] } else !1 !== c.grouping && (f = h++); b.columnIndex = f } }); var p = Math.min(Math.abs(c.transA) * (c.ordinal && c.ordinal.slope ||
                d.pointRange || c.closestPointRange || c.tickInterval || 1), c.len), m = p * d.groupPadding, t = (p - 2 * m) / (h || 1); d = Math.min(d.maxPointWidth || c.len, A(d.pointWidth, t * (1 - 2 * d.pointPadding))); a.columnMetrics = { width: d, offset: (t - d) / 2 + (m + ((a.columnIndex || 0) + (f ? 1 : 0)) * t - p / 2) * (f ? -1 : 1), paddedWidth: t, columnCount: h }; return a.columnMetrics
            }; d.prototype.crispCol = function (a, d, c, e) {
                var b = this.chart, f = this.borderWidth, g = -(f % 2 ? .5 : 0); f = f % 2 ? .5 : 1; b.inverted && b.renderer.isVML && (f += 1); this.options.crisp && (c = Math.round(a + c) + g, a = Math.round(a) +
                g, c -= a); e = Math.round(d + e) + f; g = .5 >= Math.abs(d) && .5 < e; d = Math.round(d) + f; e -= d; g && e && (--d, e += 1); return { x: a, y: d, width: c, height: e }
            }; d.prototype.adjustForMissingColumns = function (a, d, c, e) {
                var b = this, f = this.options.stacking; if (!c.isNull && 1 < e.columnCount) {
                    var g = 0, k = 0; p(this.yAxis.stacking && this.yAxis.stacking.stacks, function (a) { if ("number" === typeof c.x && (a = a[c.x.toString()])) { var d = a.points[b.index], e = a.total; f ? (d && (g = k), a.hasValidPoints && k++) : l(d) && (g = d[1], k = e || 0) } }); a = (c.plotX || 0) + ((k - 1) * e.paddedWidth + d) /
                    2 - d - g * e.paddedWidth
                } return a
            }; d.prototype.translate = function () {
                var a = this, d = a.chart, c = a.options, f = a.dense = 2 > a.closestPointRange * a.xAxis.transA; f = a.borderWidth = A(c.borderWidth, f ? 0 : 1); var k = a.xAxis, l = a.yAxis, h = c.threshold, p = a.translatedThreshold = l.getThreshold(h), t = A(c.minPointLength, 5), n = a.getColumnMetrics(), q = n.width, v = a.barW = Math.max(q, 1 + 2 * f), u = a.pointXOffset = n.offset, w = a.dataMin, x = a.dataMax; d.inverted && (p -= .5); c.pointPadding && (v = Math.ceil(v)); y.prototype.translate.apply(a); a.points.forEach(function (b) {
                    var f =
                    A(b.yBottom, p), g = 999 + Math.abs(f), r = q, C = b.plotX || 0; g = e(b.plotY, -g, l.len + g); C += u; var F = v, D = Math.min(g, f), z = Math.max(g, f) - D; if (t && Math.abs(z) < t) { z = t; var B = !l.reversed && !b.negative || l.reversed && b.negative; J(h) && J(x) && b.y === h && x <= h && (l.min || 0) < h && (w !== x || (l.max || 0) <= h) && (B = !B); D = Math.abs(D - p) > t ? f - t : p - (B ? t : 0) } m(b.options.pointWidth) && (r = F = Math.ceil(b.options.pointWidth), C -= Math.round((r - q) / 2)); c.centerInCategory && (C = a.adjustForMissingColumns(C, r, b, n)); b.barX = C; b.pointWidth = r; b.tooltipPos = d.inverted ? [e(l.len +
                    l.pos - d.plotLeft - g, l.pos - d.plotLeft, l.len + l.pos - d.plotLeft), k.len + k.pos - d.plotTop - C - F / 2, z] : [k.left - d.plotLeft + C + F / 2, e(g + l.pos - d.plotTop, l.pos - d.plotTop, l.len + l.pos - d.plotTop), z]; b.shapeType = a.pointClass.prototype.shapeType || "rect"; b.shapeArgs = a.crispCol.apply(a, b.isNull ? [C, p, F, 0] : [C, D, F, z])
                })
            }; d.prototype.drawGraph = function () { this.group[this.dense ? "addClass" : "removeClass"]("highcharts-dense-data") }; d.prototype.pointAttribs = function (a, d) {
                var b = this.options, c = this.pointAttrToOptions || {}; var e = c.stroke ||
                "borderColor"; var g = c["stroke-width"] || "borderWidth", k = a && a.color || this.color, l = a && a[e] || b[e] || k, h = a && a[g] || b[g] || this[g] || 0; c = a && a.options.dashStyle || b.dashStyle; var p = A(a && a.opacity, b.opacity, 1); if (a && this.zones.length) { var m = a.getZone(); k = a.options.color || m && (m.color || a.nonZonedColor) || this.color; m && (l = m.borderColor || l, c = m.dashStyle || c, h = m.borderWidth || h) } d && a && (a = K(b.states[d], a.options.states && a.options.states[d] || {}), d = a.brightness, k = a.color || "undefined" !== typeof d && f(k).brighten(a.brightness).get() ||
                k, l = a[e] || l, h = a[g] || h, c = a.dashStyle || c, p = A(a.opacity, p)); e = { fill: k, stroke: l, "stroke-width": h, opacity: p }; c && (e.dashstyle = c); return e
            }; d.prototype.drawPoints = function () {
                var a = this, d = this.chart, c = a.options, e = d.renderer, f = c.animationLimit || 250, k; a.points.forEach(function (b) {
                    var g = b.graphic, l = !!g, h = g && d.pointCount < f ? "animate" : "attr"; if (J(b.plotY) && null !== b.y) {
                        k = b.shapeArgs; g && b.hasNewShapeType() && (g = g.destroy()); a.enabledDataSorting && (b.startXPos = a.xAxis.reversed ? -(k ? k.width || 0 : 0) : a.xAxis.width); g || (b.graphic =
                        g = e[b.shapeType](k).add(b.group || a.group)) && a.enabledDataSorting && d.hasRendered && d.pointCount < f && (g.attr({ x: b.startXPos }), l = !0, h = "animate"); if (g && l) g[h](K(k)); if (c.borderRadius) g[h]({ r: c.borderRadius }); d.styledMode || g[h](a.pointAttribs(b, b.selected && "select")).shadow(!1 !== b.allowShadow && c.shadow, null, c.stacking && !c.borderRadius); g && (g.addClass(b.getClassName(), !0), g.attr({ visibility: b.visible ? "inherit" : "hidden" }))
                    } else g && (b.graphic = g.destroy())
                })
            }; d.prototype.drawTracker = function () {
                var a = this, d =
                a.chart, e = d.pointer, f = function (a) { var b = e.getPointFromEvent(a); "undefined" !== typeof b && (e.isDirectTouch = !0, b.onMouseOver(a)) }, k; a.points.forEach(function (a) { k = l(a.dataLabels) ? a.dataLabels : a.dataLabel ? [a.dataLabel] : []; a.graphic && (a.graphic.element.point = a); k.forEach(function (b) { b.div ? b.div.point = a : b.element.point = a }) }); a._hasTracking || (a.trackerGroups.forEach(function (b) {
                    if (a[b]) {
                        a[b].addClass("highcharts-tracker").on("mouseover", f).on("mouseout", function (a) { e.onTrackerMouseOut(a) }); if (c) a[b].on("touchstart",
                        f); !d.styledMode && a.options.cursor && a[b].css(t).css({ cursor: a.options.cursor })
                    }
                }), a._hasTracking = !0); n(this, "afterDrawTracker")
            }; d.prototype.remove = function () { var a = this, d = a.chart; d.hasRendered && d.series.forEach(function (b) { b.type === a.type && (b.isDirty = !0) }); y.prototype.remove.apply(a, arguments) }; d.defaultOptions = K(y.defaultOptions, {
                borderRadius: 0, centerInCategory: !1, groupPadding: .2, marker: null, pointPadding: .1, minPointLength: 0, cropThreshold: 50, pointRange: null, states: {
                    hover: { halo: !1, brightness: .1 },
                    select: { color: x.neutralColor20, borderColor: x.neutralColor100 }
                }, dataLabels: { align: void 0, verticalAlign: void 0, y: void 0 }, startFromThreshold: !0, stickyTracking: !1, tooltip: { distance: 6 }, threshold: 0, borderColor: x.backgroundColor
            }); return d
        }(y); v(B.prototype, { cropShoulder: 0, directTouch: !0, drawLegendSymbol: H.drawRectangle, getSymbol: a, negStacks: !0, trackerGroups: ["group", "dataLabelsGroup"] }); G.registerSeriesType("column", B); ""; ""; return B
    }); L(a, "Series/Bar/BarSeries.js", [a["Series/Column/ColumnSeries.js"], a["Core/Series/SeriesRegistry.js"],
    a["Core/Utilities.js"]], function (a, u, E) {
        var v = this && this.__extends || function () { var a = function (v, q) { a = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, f) { a.__proto__ = f } || function (a, f) { for (var c in f) f.hasOwnProperty(c) && (a[c] = f[c]) }; return a(v, q) }; return function (v, q) { function h() { this.constructor = v } a(v, q); v.prototype = null === q ? Object.create(q) : (h.prototype = q.prototype, new h) } }(), x = E.extend, y = E.merge; E = function (u) {
            function x() {
                var a = null !== u && u.apply(this, arguments) || this; a.data = void 0;
                a.options = void 0; a.points = void 0; return a
            } v(x, u); x.defaultOptions = y(a.defaultOptions, {}); return x
        }(a); x(E.prototype, { inverted: !0 }); u.registerSeriesType("bar", E); ""; return E
    }); L(a, "Series/Scatter/ScatterSeries.js", [a["Series/Column/ColumnSeries.js"], a["Series/Line/LineSeries.js"], a["Core/Series/SeriesRegistry.js"], a["Core/Utilities.js"]], function (a, u, E, H) {
        var v = this && this.__extends || function () {
            var a = function (h, f) {
                a = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, e) { a.__proto__ = e } ||
                function (a, e) { for (var c in e) e.hasOwnProperty(c) && (a[c] = e[c]) }; return a(h, f)
            }; return function (h, f) { function c() { this.constructor = h } a(h, f); h.prototype = null === f ? Object.create(f) : (c.prototype = f.prototype, new c) }
        }(), y = H.addEvent, G = H.extend, B = H.merge; H = function (a) {
            function h() { var f = null !== a && a.apply(this, arguments) || this; f.data = void 0; f.options = void 0; f.points = void 0; return f } v(h, a); h.prototype.applyJitter = function () {
                var a = this, c = this.options.jitter, e = this.points.length; c && this.points.forEach(function (f,
                h) { ["x", "y"].forEach(function (m, t) { var l = "plot" + m.toUpperCase(); if (c[m] && !f.isNull) { var n = a[m + "Axis"]; var q = c[m] * n.transA; if (n && !n.isLog) { var v = Math.max(0, f[l] - q); n = Math.min(n.len, f[l] + q); t = 1E4 * Math.sin(h + t * e); f[l] = v + (n - v) * (t - Math.floor(t)); "x" === m && (f.clientX = f.plotX) } } }) })
            }; h.prototype.drawGraph = function () { this.options.lineWidth ? a.prototype.drawGraph.call(this) : this.graph && (this.graph = this.graph.destroy()) }; h.defaultOptions = B(u.defaultOptions, {
                lineWidth: 0, findNearestPointBy: "xy", jitter: { x: 0, y: 0 },
                marker: { enabled: !0 }, tooltip: { headerFormat: '<span style="color:{point.color}">\u25cf</span> <span style="font-size: 10px"> {series.name}</span><br/>', pointFormat: "x: <b>{point.x}</b><br/>y: <b>{point.y}</b><br/>" }
            }); return h
        }(u); G(H.prototype, { drawTracker: a.prototype.drawTracker, sorted: !1, requireSorting: !1, noSharedTooltip: !0, trackerGroups: ["group", "markerGroup", "dataLabelsGroup"], takeOrdinalPosition: !1 }); y(H, "afterTranslate", function () { this.applyJitter() }); E.registerSeriesType("scatter", H); ""; return H
    });
    L(a, "Mixins/CenteredSeries.js", [a["Core/Globals.js"], a["Core/Series/Series.js"], a["Core/Utilities.js"]], function (a, u, E) {
        var v = E.isNumber, x = E.pick, y = E.relativeLength, G = a.deg2rad; return a.CenteredSeriesMixin = {
            getCenter: function () {
                var a = this.options, q = this.chart, h = 2 * (a.slicedOffset || 0), f = q.plotWidth - 2 * h, c = q.plotHeight - 2 * h, e = a.center, t = Math.min(f, c), m = a.size, v = a.innerSize || 0; "string" === typeof m && (m = parseFloat(m)); "string" === typeof v && (v = parseFloat(v)); a = [x(e[0], "50%"), x(e[1], "50%"), x(m && 0 > m ? void 0 : a.size,
                "100%"), x(v && 0 > v ? void 0 : a.innerSize || 0, "0%")]; !q.angular || this instanceof u || (a[3] = 0); for (e = 0; 4 > e; ++e) m = a[e], q = 2 > e || 2 === e && /%$/.test(m), a[e] = y(m, [f, c, t, a[2]][e]) + (q ? h : 0); a[3] > a[2] && (a[3] = a[2]); return a
            }, getStartAndEndRadians: function (a, q) { a = v(a) ? a : 0; q = v(q) && q > a && 360 > q - a ? q : a + 360; return { start: G * (a + -90), end: G * (q + -90) } }
        }
    }); L(a, "Series/Pie/PiePoint.js", [a["Core/Animation/AnimationUtilities.js"], a["Core/Series/Point.js"], a["Core/Utilities.js"]], function (a, u, E) {
        var v = this && this.__extends || function () {
            var a =
            function (c, e) { a = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, c) { a.__proto__ = c } || function (a, c) { for (var e in c) c.hasOwnProperty(e) && (a[e] = c[e]) }; return a(c, e) }; return function (c, e) { function f() { this.constructor = c } a(c, e); c.prototype = null === e ? Object.create(e) : (f.prototype = e.prototype, new f) }
        }(), x = a.setAnimation, y = E.addEvent, G = E.defined; a = E.extend; var B = E.isNumber, q = E.pick, h = E.relativeLength; E = function (a) {
            function c() {
                var c = null !== a && a.apply(this, arguments) || this; c.labelDistance = void 0;
                c.options = void 0; c.series = void 0; return c
            } v(c, a); c.prototype.getConnectorPath = function () { var a = this.labelPosition, c = this.series.options.dataLabels, f = c.connectorShape, h = this.connectorShapes; h[f] && (f = h[f]); return f.call(this, { x: a.final.x, y: a.final.y, alignment: a.alignment }, a.connectorPosition, c) }; c.prototype.getTranslate = function () { return this.sliced ? this.slicedTranslation : { translateX: 0, translateY: 0 } }; c.prototype.haloPath = function (a) {
                var c = this.shapeArgs; return this.sliced || !this.visible ? [] : this.series.chart.renderer.symbols.arc(c.x,
                c.y, c.r + a, c.r + a, { innerR: c.r - 1, start: c.start, end: c.end })
            }; c.prototype.init = function () { u.prototype.init.apply(this, arguments); var a = this; a.name = q(a.name, "Slice"); var c = function (c) { a.slice("select" === c.type) }; y(a, "select", c); y(a, "unselect", c); return a }; c.prototype.isValid = function () { return B(this.y) && 0 <= this.y }; c.prototype.setVisible = function (a, c) {
                var e = this, f = e.series, h = f.chart, l = f.options.ignoreHiddenPoint; c = q(c, l); a !== e.visible && (e.visible = e.options.visible = a = "undefined" === typeof a ? !e.visible : a, f.options.data[f.data.indexOf(e)] =
                e.options, ["graphic", "dataLabel", "connector", "shadowGroup"].forEach(function (c) { if (e[c]) e[c][a ? "show" : "hide"](a) }), e.legendItem && h.legend.colorizeItem(e, a), a || "hover" !== e.state || e.setState(""), l && (f.isDirty = !0), c && h.redraw())
            }; c.prototype.slice = function (a, c, f) { var e = this.series; x(f, e.chart); q(c, !0); this.sliced = this.options.sliced = G(a) ? a : !this.sliced; e.options.data[e.data.indexOf(this)] = this.options; this.graphic && this.graphic.animate(this.getTranslate()); this.shadowGroup && this.shadowGroup.animate(this.getTranslate()) };
            return c
        }(u); a(E.prototype, {
            connectorShapes: {
                fixedOffset: function (a, c, e) { var f = c.breakAt; c = c.touchingSliceAt; return [["M", a.x, a.y], e.softConnector ? ["C", a.x + ("left" === a.alignment ? -5 : 5), a.y, 2 * f.x - c.x, 2 * f.y - c.y, f.x, f.y] : ["L", f.x, f.y], ["L", c.x, c.y]] }, straight: function (a, c) { c = c.touchingSliceAt; return [["M", a.x, a.y], ["L", c.x, c.y]] }, crookedLine: function (a, c, e) {
                    c = c.touchingSliceAt; var f = this.series, m = f.center[0], q = f.chart.plotWidth, n = f.chart.plotLeft; f = a.alignment; var l = this.shapeArgs.r; e = h(e.crookDistance,
                    1); q = "left" === f ? m + l + (q + n - m - l) * (1 - e) : n + (m - l) * e; e = ["L", q, a.y]; m = !0; if ("left" === f ? q > a.x || q < c.x : q < a.x || q > c.x) m = !1; a = [["M", a.x, a.y]]; m && a.push(e); a.push(["L", c.x, c.y]); return a
                }
            }
        }); return E
    }); L(a, "Series/Pie/PieSeries.js", [a["Mixins/CenteredSeries.js"], a["Series/Column/ColumnSeries.js"], a["Core/Globals.js"], a["Mixins/LegendSymbol.js"], a["Core/Color/Palette.js"], a["Series/Pie/PiePoint.js"], a["Core/Series/Series.js"], a["Core/Series/SeriesRegistry.js"], a["Core/Renderer/SVG/Symbols.js"], a["Core/Utilities.js"]],
    function (a, u, E, H, x, y, G, B, q, h) {
        var f = this && this.__extends || function () { var a = function (c, e) { a = Object.setPrototypeOf || { __proto__: [] } instanceof Array && function (a, c) { a.__proto__ = c } || function (a, c) { for (var d in c) c.hasOwnProperty(d) && (a[d] = c[d]) }; return a(c, e) }; return function (c, e) { function f() { this.constructor = c } a(c, e); c.prototype = null === e ? Object.create(e) : (f.prototype = e.prototype, new f) } }(), c = a.getStartAndEndRadians; E = E.noop; var e = h.clamp, t = h.extend, m = h.fireEvent, v = h.merge, n = h.pick, l = h.relativeLength; h =
        function (a) {
            function h() { var c = null !== a && a.apply(this, arguments) || this; c.center = void 0; c.data = void 0; c.maxLabelDistance = void 0; c.options = void 0; c.points = void 0; return c } f(h, a); h.prototype.animate = function (a) { var c = this, d = c.points, e = c.startAngleRad; a || d.forEach(function (a) { var b = a.graphic, d = a.shapeArgs; b && d && (b.attr({ r: n(a.startR, c.center && c.center[3] / 2), start: e, end: e }), b.animate({ r: d.r, start: d.start, end: d.end }, c.options.animation)) }) }; h.prototype.drawEmpty = function () {
                var a = this.startAngleRad, c = this.endAngleRad,
                d = this.options; if (0 === this.total && this.center) { var e = this.center[0]; var b = this.center[1]; this.graph || (this.graph = this.chart.renderer.arc(e, b, this.center[1] / 2, 0, a, c).addClass("highcharts-empty-series").add(this.group)); this.graph.attr({ d: q.arc(e, b, this.center[2] / 2, 0, { start: a, end: c, innerR: this.center[3] / 2 }) }); this.chart.styledMode || this.graph.attr({ "stroke-width": d.borderWidth, fill: d.fillColor || "none", stroke: d.color || x.neutralColor20 }) } else this.graph && (this.graph = this.graph.destroy())
            }; h.prototype.drawPoints =
            function () { var a = this.chart.renderer; this.points.forEach(function (c) { c.graphic && c.hasNewShapeType() && (c.graphic = c.graphic.destroy()); c.graphic || (c.graphic = a[c.shapeType](c.shapeArgs).add(c.series.group), c.delayedRendering = !0) }) }; h.prototype.generatePoints = function () { a.prototype.generatePoints.call(this); this.updateTotals() }; h.prototype.getX = function (a, c, d) {
                var f = this.center, b = this.radii ? this.radii[d.index] || 0 : f[2] / 2; a = Math.asin(e((a - f[1]) / (b + d.labelDistance), -1, 1)); return f[0] + (c ? -1 : 1) * Math.cos(a) *
                (b + d.labelDistance) + (0 < d.labelDistance ? (c ? -1 : 1) * this.options.dataLabels.padding : 0)
            }; h.prototype.hasData = function () { return !!this.processedXData.length }; h.prototype.redrawPoints = function () {
                var a = this, c = a.chart, d = c.renderer, e, b, f, h, l = a.options.shadow; this.drawEmpty(); !l || a.shadowGroup || c.styledMode || (a.shadowGroup = d.g("shadow").attr({ zIndex: -1 }).add(a.group)); a.points.forEach(function (g) {
                    var k = {}; b = g.graphic; if (!g.isNull && b) {
                        var p = void 0; h = g.shapeArgs; e = g.getTranslate(); c.styledMode || (p = g.shadowGroup,
                        l && !p && (p = g.shadowGroup = d.g("shadow").add(a.shadowGroup)), p && p.attr(e), f = a.pointAttribs(g, g.selected && "select")); g.delayedRendering ? (b.setRadialReference(a.center).attr(h).attr(e), c.styledMode || b.attr(f).attr({ "stroke-linejoin": "round" }).shadow(l, p), g.delayedRendering = !1) : (b.setRadialReference(a.center), c.styledMode || v(!0, k, f), v(!0, k, h, e), b.animate(k)); b.attr({ visibility: g.visible ? "inherit" : "hidden" }); b.addClass(g.getClassName(), !0)
                    } else b && (g.graphic = b.destroy())
                })
            }; h.prototype.sortByAngle = function (a,
            c) { a.sort(function (a, e) { return "undefined" !== typeof a.angle && (e.angle - a.angle) * c }) }; h.prototype.translate = function (a) {
                this.generatePoints(); var e = 0, d = this.options, f = d.slicedOffset, b = f + (d.borderWidth || 0), g = c(d.startAngle, d.endAngle), h = this.startAngleRad = g.start; g = (this.endAngleRad = g.end) - h; var q = this.points, t = d.dataLabels.distance; d = d.ignoreHiddenPoint; var v, u = q.length; a || (this.center = a = this.getCenter()); for (v = 0; v < u; v++) {
                    var w = q[v]; var A = h + e * g; !w.isValid() || d && !w.visible || (e += w.percentage / 100); var x =
                    h + e * g; var y = { x: a[0], y: a[1], r: a[2] / 2, innerR: a[3] / 2, start: Math.round(1E3 * A) / 1E3, end: Math.round(1E3 * x) / 1E3 }; w.shapeType = "arc"; w.shapeArgs = y; w.labelDistance = n(w.options.dataLabels && w.options.dataLabels.distance, t); w.labelDistance = l(w.labelDistance, y.r); this.maxLabelDistance = Math.max(this.maxLabelDistance || 0, w.labelDistance); x = (x + A) / 2; x > 1.5 * Math.PI ? x -= 2 * Math.PI : x < -Math.PI / 2 && (x += 2 * Math.PI); w.slicedTranslation = { translateX: Math.round(Math.cos(x) * f), translateY: Math.round(Math.sin(x) * f) }; y = Math.cos(x) * a[2] /
                    2; var B = Math.sin(x) * a[2] / 2; w.tooltipPos = [a[0] + .7 * y, a[1] + .7 * B]; w.half = x < -Math.PI / 2 || x > Math.PI / 2 ? 1 : 0; w.angle = x; A = Math.min(b, w.labelDistance / 5); w.labelPosition = { natural: { x: a[0] + y + Math.cos(x) * w.labelDistance, y: a[1] + B + Math.sin(x) * w.labelDistance }, "final": {}, alignment: 0 > w.labelDistance ? "center" : w.half ? "right" : "left", connectorPosition: { breakAt: { x: a[0] + y + Math.cos(x) * A, y: a[1] + B + Math.sin(x) * A }, touchingSliceAt: { x: a[0] + y, y: a[1] + B } } }
                } m(this, "afterTranslate")
            }; h.prototype.updateTotals = function () {
                var a, c = 0, d = this.points,
                e = d.length, b = this.options.ignoreHiddenPoint; for (a = 0; a < e; a++) { var f = d[a]; !f.isValid() || b && !f.visible || (c += f.y) } this.total = c; for (a = 0; a < e; a++) f = d[a], f.percentage = 0 < c && (f.visible || !b) ? f.y / c * 100 : 0, f.total = c
            }; h.defaultOptions = v(G.defaultOptions, {
                center: [null, null], clip: !1, colorByPoint: !0, dataLabels: { allowOverlap: !0, connectorPadding: 5, connectorShape: "fixedOffset", crookDistance: "70%", distance: 30, enabled: !0, formatter: function () { return this.point.isNull ? void 0 : this.point.name }, softConnector: !0, x: 0 }, fillColor: void 0,
                ignoreHiddenPoint: !0, inactiveOtherPoints: !0, legendType: "point", marker: null, size: null, showInLegend: !1, slicedOffset: 10, stickyTracking: !1, tooltip: { followPointer: !0 }, borderColor: x.backgroundColor, borderWidth: 1, lineWidth: void 0, states: { hover: { brightness: .1 } }
            }); return h
        }(G); t(h.prototype, {
            axisTypes: [], directTouch: !0, drawGraph: void 0, drawLegendSymbol: H.drawRectangle, drawTracker: u.prototype.drawTracker, getCenter: a.getCenter, getSymbol: E, isCartesian: !1, noSharedTooltip: !0, pointAttribs: u.prototype.pointAttribs,
            pointClass: y, requireSorting: !1, searchPoint: E, trackerGroups: ["group", "dataLabelsGroup"]
        }); B.registerSeriesType("pie", h); ""; return h
    }); L(a, "Core/Series/DataLabels.js", [a["Core/Animation/AnimationUtilities.js"], a["Core/FormatUtilities.js"], a["Core/Globals.js"], a["Core/Color/Palette.js"], a["Core/Series/Series.js"], a["Core/Series/SeriesRegistry.js"], a["Core/Utilities.js"]], function (a, u, E, H, x, y, G) {
        var v = a.getDeferredAnimation, q = u.format; a = E.noop; y = y.seriesTypes; var h = G.arrayMax, f = G.clamp, c = G.defined, e =
        G.extend, t = G.fireEvent, m = G.isArray, w = G.merge, n = G.objectEach, l = G.pick, J = G.relativeLength, K = G.splat, A = G.stableSort; ""; E.distribute = function (a, c, e) {
            function b(a, b) { return a.target - b.target } var d, k = !0, h = a, m = []; var p = 0; var n = h.reducedLen || c; for (d = a.length; d--;) p += a[d].size; if (p > n) { A(a, function (a, b) { return (b.rank || 0) - (a.rank || 0) }); for (p = d = 0; p <= n;) p += a[d].size, d++; m = a.splice(d - 1, a.length) } A(a, b); for (a = a.map(function (a) { return { size: a.size, targets: [a.target], align: l(a.align, .5) } }) ; k;) {
                for (d = a.length; d--;) k =
                a[d], p = (Math.min.apply(0, k.targets) + Math.max.apply(0, k.targets)) / 2, k.pos = f(p - k.size * k.align, 0, c - k.size); d = a.length; for (k = !1; d--;) 0 < d && a[d - 1].pos + a[d - 1].size > a[d].pos && (a[d - 1].size += a[d].size, a[d - 1].targets = a[d - 1].targets.concat(a[d].targets), a[d - 1].align = .5, a[d - 1].pos + a[d - 1].size > c && (a[d - 1].pos = c - a[d - 1].size), a.splice(d, 1), k = !0)
            } h.push.apply(h, m); d = 0; a.some(function (a) {
                var b = 0; if (a.targets.some(function () {
                h[d].pos = a.pos + b; if ("undefined" !== typeof e && Math.abs(h[d].pos - h[d].target) > e) return h.slice(0,
                d + 1).forEach(function (a) { delete a.pos }), h.reducedLen = (h.reducedLen || c) - .1 * c, h.reducedLen > .1 * c && E.distribute(h, c, e), !0; b += h[d].size; d++
                })) return !0
            }); A(h, b)
        }; x.prototype.drawDataLabels = function () {
            function a(a, b) { var c = b.filter; return c ? (b = c.operator, a = a[c.property], c = c.value, ">" === b && a > c || "<" === b && a < c || ">=" === b && a >= c || "<=" === b && a <= c || "==" === b && a == c || "===" === b && a === c ? !0 : !1) : !0 } function d(a, b) {
                var c = [], d; if (m(a) && !m(b)) c = a.map(function (a) { return w(a, b) }); else if (m(b) && !m(a)) c = b.map(function (b) {
                    return w(a,
                    b)
                }); else if (m(a) || m(b)) for (d = Math.max(a.length, b.length) ; d--;) c[d] = w(a[d], b[d]); else c = w(a, b); return c
            } var e = this, b = e.chart, f = e.options, h = f.dataLabels, u = e.points, x, A = e.hasRendered || 0, C = h.animation; C = h.defer ? v(b, C, e) : { defer: 0, duration: 0 }; var y = b.renderer; h = d(d(b.options.plotOptions && b.options.plotOptions.series && b.options.plotOptions.series.dataLabels, b.options.plotOptions && b.options.plotOptions[e.type] && b.options.plotOptions[e.type].dataLabels), h); t(this, "drawDataLabels"); if (m(h) || h.enabled || e._hasPointLabels) {
                var B =
                e.plotGroup("dataLabelsGroup", "data-labels", A ? "inherit" : "hidden", h.zIndex || 6); B.attr({ opacity: +A }); !A && (A = e.dataLabelsGroup) && (e.visible && B.show(!0), A[f.animation ? "animate" : "attr"]({ opacity: 1 }, C)); u.forEach(function (g) {
                    x = K(d(h, g.dlOptions || g.options && g.options.dataLabels)); x.forEach(function (d, k) {
                        var h = d.enabled && (!g.isNull || g.dataLabelOnNull) && a(g, d), m = g.dataLabels ? g.dataLabels[k] : g.dataLabel, p = g.connectors ? g.connectors[k] : g.connector, r = l(d.distance, g.labelDistance), t = !m; if (h) {
                            var v = g.getLabelConfig();
                            var u = l(d[g.formatPrefix + "Format"], d.format); v = c(u) ? q(u, v, b) : (d[g.formatPrefix + "Formatter"] || d.formatter).call(v, d); u = d.style; var w = d.rotation; b.styledMode || (u.color = l(d.color, u.color, e.color, H.neutralColor100), "contrast" === u.color ? (g.contrastColor = y.getContrast(g.color || e.color), u.color = !c(r) && d.inside || 0 > r || f.stacking ? g.contrastColor : H.neutralColor100) : delete g.contrastColor, f.cursor && (u.cursor = f.cursor)); var x = { r: d.borderRadius || 0, rotation: w, padding: d.padding, zIndex: 1 }; b.styledMode || (x.fill = d.backgroundColor,
                            x.stroke = d.borderColor, x["stroke-width"] = d.borderWidth); n(x, function (a, b) { "undefined" === typeof a && delete x[b] })
                        } !m || h && c(v) ? h && c(v) && (m ? x.text = v : (g.dataLabels = g.dataLabels || [], m = g.dataLabels[k] = w ? y.text(v, 0, -9999, d.useHTML).addClass("highcharts-data-label") : y.label(v, 0, -9999, d.shape, null, null, d.useHTML, null, "data-label"), k || (g.dataLabel = m), m.addClass(" highcharts-data-label-color-" + g.colorIndex + " " + (d.className || "") + (d.useHTML ? " highcharts-tracker" : ""))), m.options = d, m.attr(x), b.styledMode || m.css(u).shadow(d.shadow),
                        m.added || m.add(B), d.textPath && !d.useHTML && (m.setTextPath(g.getDataLabelPath && g.getDataLabelPath(m) || g.graphic, d.textPath), g.dataLabelPath && !d.textPath.enabled && (g.dataLabelPath = g.dataLabelPath.destroy())), e.alignDataLabel(g, m, d, null, t)) : (g.dataLabel = g.dataLabel && g.dataLabel.destroy(), g.dataLabels && (1 === g.dataLabels.length ? delete g.dataLabels : delete g.dataLabels[k]), k || delete g.dataLabel, p && (g.connector = g.connector.destroy(), g.connectors && (1 === g.connectors.length ? delete g.connectors : delete g.connectors[k])))
                    })
                })
            } t(this,
            "afterDrawDataLabels")
        }; x.prototype.alignDataLabel = function (a, c, f, b, g) {
            var d = this, k = this.chart, h = this.isCartesian && k.inverted, m = this.enabledDataSorting, p = l(a.dlBox && a.dlBox.centerX, a.plotX, -9999), n = l(a.plotY, -9999), q = c.getBBox(), t = f.rotation, v = f.align, u = k.isInsidePlot(p, Math.round(n), { inverted: h, paneCoordinates: !0, series: d }), w = "justify" === l(f.overflow, m ? "none" : "justify"), x = this.visible && !1 !== a.visible && (a.series.forceDL || m && !w || u || l(f.inside, !!this.options.stacking) && b && k.isInsidePlot(p, h ? b.x + 1 : b.y +
            b.height - 1, { inverted: h, paneCoordinates: !0, series: d })); var A = function (b) { m && d.xAxis && !w && d.setDataLabelStartPos(a, c, g, u, b) }; if (x) {
                var y = k.renderer.fontMetrics(k.styledMode ? void 0 : f.style.fontSize, c).b; b = e({ x: h ? this.yAxis.len - n : p, y: Math.round(h ? this.xAxis.len - p : n), width: 0, height: 0 }, b); e(f, { width: q.width, height: q.height }); t ? (w = !1, p = k.renderer.rotCorr(y, t), p = { x: b.x + (f.x || 0) + b.width / 2 + p.x, y: b.y + (f.y || 0) + { top: 0, middle: .5, bottom: 1 }[f.verticalAlign] * b.height }, A(p), c[g ? "attr" : "animate"](p).attr({ align: v }),
                A = (t + 720) % 360, A = 180 < A && 360 > A, "left" === v ? p.y -= A ? q.height : 0 : "center" === v ? (p.x -= q.width / 2, p.y -= q.height / 2) : "right" === v && (p.x -= q.width, p.y -= A ? 0 : q.height), c.placed = !0, c.alignAttr = p) : (A(b), c.align(f, void 0, b), p = c.alignAttr); w && 0 <= b.height ? this.justifyDataLabel(c, f, p, q, b, g) : l(f.crop, !0) && (x = k.isInsidePlot(p.x, p.y, { paneCoordinates: !0, series: d }) && k.isInsidePlot(p.x + q.width, p.y + q.height, { paneCoordinates: !0, series: d })); if (f.shape && !t) c[g ? "attr" : "animate"]({
                    anchorX: h ? k.plotWidth - a.plotY : a.plotX, anchorY: h ? k.plotHeight -
                    a.plotX : a.plotY
                })
            } g && m && (c.placed = !1); x || m && !w || (c.hide(!0), c.placed = !1)
        }; x.prototype.setDataLabelStartPos = function (a, c, e, b, f) {
            var d = this.chart, g = d.inverted, k = this.xAxis, h = k.reversed, l = g ? c.height / 2 : c.width / 2; a = (a = a.pointWidth) ? a / 2 : 0; k = g ? f.x : h ? -l - a : k.width - l + a; f = g ? h ? this.yAxis.height - l + a : -l - a : f.y; c.startXPos = k; c.startYPos = f; b ? "hidden" === c.visibility && (c.show(), c.attr({ opacity: 0 }).animate({ opacity: 1 })) : c.attr({ opacity: 1 }).animate({ opacity: 0 }, void 0, c.hide); d.hasRendered && (e && c.attr({ x: c.startXPos, y: c.startYPos }),
            c.placed = !0)
        }; x.prototype.justifyDataLabel = function (a, c, e, b, f, h) {
            var d = this.chart, g = c.align, k = c.verticalAlign, l = a.box ? 0 : a.padding || 0, m = c.x; m = void 0 === m ? 0 : m; var p = c.y; var n = void 0 === p ? 0 : p; p = (e.x || 0) + l; if (0 > p) { "right" === g && 0 <= m ? (c.align = "left", c.inside = !0) : m -= p; var r = !0 } p = (e.x || 0) + b.width - l; p > d.plotWidth && ("left" === g && 0 >= m ? (c.align = "right", c.inside = !0) : m += d.plotWidth - p, r = !0); p = e.y + l; 0 > p && ("bottom" === k && 0 <= n ? (c.verticalAlign = "top", c.inside = !0) : n -= p, r = !0); p = (e.y || 0) + b.height - l; p > d.plotHeight && ("top" ===
            k && 0 >= n ? (c.verticalAlign = "bottom", c.inside = !0) : n += d.plotHeight - p, r = !0); r && (c.x = m, c.y = n, a.placed = !h, a.align(c, void 0, f)); return r
        }; y.pie && (y.pie.prototype.dataLabelPositioners = {
            radialDistributionY: function (a) { return a.top + a.distributeBox.pos }, radialDistributionX: function (a, c, e, b) { return a.getX(e < c.top + 2 || e > c.bottom - 2 ? b : e, c.half, c) }, justify: function (a, c, e) { return e[0] + (a.half ? -1 : 1) * (c + a.labelDistance) }, alignToPlotEdges: function (a, c, e, b) { a = a.getBBox().width; return c ? a + b : e - a - b }, alignToConnectors: function (a,
            c, e, b) { var d = 0, f; a.forEach(function (a) { f = a.dataLabel.getBBox().width; f > d && (d = f) }); return c ? d + b : e - d - b }
        }, y.pie.prototype.drawDataLabels = function () {
            var a = this, d = a.data, e, b = a.chart, f = a.options.dataLabels || {}, m = f.connectorPadding, n, q = b.plotWidth, t = b.plotHeight, v = b.plotLeft, u = Math.round(b.chartWidth / 3), A, y = a.center, B = y[2] / 2, G = y[1], z, J, K, L, X = [[], []], I, N, P, U, T = [0, 0, 0, 0], R = a.dataLabelPositioners, V; a.visible && (f.enabled || a._hasPointLabels) && (d.forEach(function (a) {
                a.dataLabel && a.visible && a.dataLabel.shortened &&
                (a.dataLabel.attr({ width: "auto" }).css({ width: "auto", textOverflow: "clip" }), a.dataLabel.shortened = !1)
            }), x.prototype.drawDataLabels.apply(a), d.forEach(function (a) {
                a.dataLabel && (a.visible ? (X[a.half].push(a), a.dataLabel._pos = null, !c(f.style.width) && !c(a.options.dataLabels && a.options.dataLabels.style && a.options.dataLabels.style.width) && a.dataLabel.getBBox().width > u && (a.dataLabel.css({ width: Math.round(.7 * u) + "px" }), a.dataLabel.shortened = !0)) : (a.dataLabel = a.dataLabel.destroy(), a.dataLabels && 1 === a.dataLabels.length &&
                delete a.dataLabels))
            }), X.forEach(function (d, g) {
                var h = d.length, k = [], p; if (h) {
                    a.sortByAngle(d, g - .5); if (0 < a.maxLabelDistance) {
                        var n = Math.max(0, G - B - a.maxLabelDistance); var r = Math.min(G + B + a.maxLabelDistance, b.plotHeight); d.forEach(function (a) { 0 < a.labelDistance && a.dataLabel && (a.top = Math.max(0, G - B - a.labelDistance), a.bottom = Math.min(G + B + a.labelDistance, b.plotHeight), p = a.dataLabel.getBBox().height || 21, a.distributeBox = { target: a.labelPosition.natural.y - a.top + p / 2, size: p, rank: a.y }, k.push(a.distributeBox)) }); n =
                        r + p - n; E.distribute(k, n, n / 5)
                    } for (U = 0; U < h; U++) {
                        e = d[U]; K = e.labelPosition; z = e.dataLabel; P = !1 === e.visible ? "hidden" : "inherit"; N = n = K.natural.y; k && c(e.distributeBox) && ("undefined" === typeof e.distributeBox.pos ? P = "hidden" : (L = e.distributeBox.size, N = R.radialDistributionY(e))); delete e.positionIndex; if (f.justify) I = R.justify(e, B, y); else switch (f.alignTo) { case "connectors": I = R.alignToConnectors(d, g, q, v); break; case "plotEdges": I = R.alignToPlotEdges(z, g, q, v); break; default: I = R.radialDistributionX(a, e, N, n) } z._attr = {
                            visibility: P,
                            align: K.alignment
                        }; V = e.options.dataLabels || {}; z._pos = { x: I + l(V.x, f.x) + ({ left: m, right: -m }[K.alignment] || 0), y: N + l(V.y, f.y) - 10 }; K.final.x = I; K.final.y = N; l(f.crop, !0) && (J = z.getBBox().width, n = null, I - J < m && 1 === g ? (n = Math.round(J - I + m), T[3] = Math.max(n, T[3])) : I + J > q - m && 0 === g && (n = Math.round(I + J - q + m), T[1] = Math.max(n, T[1])), 0 > N - L / 2 ? T[0] = Math.max(Math.round(-N + L / 2), T[0]) : N + L / 2 > t && (T[2] = Math.max(Math.round(N + L / 2 - t), T[2])), z.sideOverflow = n)
                    }
                }
            }), 0 === h(T) || this.verifyDataLabelOverflow(T)) && (this.placeDataLabels(), this.points.forEach(function (c) {
                V =
                w(f, c.options.dataLabels); if (n = l(V.connectorWidth, 1)) {
                    var d; A = c.connector; if ((z = c.dataLabel) && z._pos && c.visible && 0 < c.labelDistance) { P = z._attr.visibility; if (d = !A) c.connector = A = b.renderer.path().addClass("highcharts-data-label-connector  highcharts-color-" + c.colorIndex + (c.className ? " " + c.className : "")).add(a.dataLabelsGroup), b.styledMode || A.attr({ "stroke-width": n, stroke: V.connectorColor || c.color || H.neutralColor60 }); A[d ? "attr" : "animate"]({ d: c.getConnectorPath() }); A.attr("visibility", P) } else A && (c.connector =
                    A.destroy())
                }
            }))
        }, y.pie.prototype.placeDataLabels = function () { this.points.forEach(function (a) { var c = a.dataLabel, e; c && a.visible && ((e = c._pos) ? (c.sideOverflow && (c._attr.width = Math.max(c.getBBox().width - c.sideOverflow, 0), c.css({ width: c._attr.width + "px", textOverflow: (this.options.dataLabels.style || {}).textOverflow || "ellipsis" }), c.shortened = !0), c.attr(c._attr), c[c.moved ? "animate" : "attr"](e), c.moved = !0) : c && c.attr({ y: -9999 })); delete a.distributeBox }, this) }, y.pie.prototype.alignDataLabel = a, y.pie.prototype.verifyDataLabelOverflow =
        function (a) { var c = this.center, e = this.options, b = e.center, g = e.minSize || 80, h = null !== e.size; if (!h) { if (null !== b[0]) var l = Math.max(c[2] - Math.max(a[1], a[3]), g); else l = Math.max(c[2] - a[1] - a[3], g), c[0] += (a[3] - a[1]) / 2; null !== b[1] ? l = f(l, g, c[2] - Math.max(a[0], a[2])) : (l = f(l, g, c[2] - a[0] - a[2]), c[1] += (a[0] - a[2]) / 2); l < c[2] ? (c[2] = l, c[3] = Math.min(J(e.innerSize || 0, l), l), this.translate(c), this.drawDataLabels && this.drawDataLabels()) : h = !0 } return h }); y.column && (y.column.prototype.alignDataLabel = function (a, c, e, b, f) {
            var d = this.chart.inverted,
            g = a.series, h = a.dlBox || a.shapeArgs, k = l(a.below, a.plotY > l(this.translatedThreshold, g.yAxis.len)), m = l(e.inside, !!this.options.stacking); h && (b = w(h), 0 > b.y && (b.height += b.y, b.y = 0), h = b.y + b.height - g.yAxis.len, 0 < h && h < b.height && (b.height -= h), d && (b = { x: g.yAxis.len - b.y - b.height, y: g.xAxis.len - b.x - b.width, width: b.height, height: b.width }), m || (d ? (b.x += k ? 0 : b.width, b.width = 0) : (b.y += k ? b.height : 0, b.height = 0))); e.align = l(e.align, !d || m ? "center" : k ? "right" : "left"); e.verticalAlign = l(e.verticalAlign, d || m ? "middle" : k ? "top" : "bottom");
            x.prototype.alignDataLabel.call(this, a, c, e, b, f); e.inside && a.contrastColor && c.css({ color: a.contrastColor })
        })
    }); L(a, "Extensions/OverlappingDataLabels.js", [a["Core/Chart/Chart.js"], a["Core/Utilities.js"]], function (a, u) {
        function v(a, f) {
            var c = !1; if (a) {
                var e = a.newOpacity; a.oldOpacity !== e && (a.alignAttr && a.placed ? (a[e ? "removeClass" : "addClass"]("highcharts-data-label-hidden"), c = !0, a.alignAttr.opacity = e, a[a.isOld ? "animate" : "attr"](a.alignAttr, null, function () { f.styledMode || a.css({ pointerEvents: e ? "auto" : "none" }) }),
                x(f, "afterHideOverlappingLabel")) : a.attr({ opacity: e })); a.isOld = !0
            } return c
        } var H = u.addEvent, x = u.fireEvent, y = u.isArray, G = u.isNumber, B = u.objectEach, q = u.pick; H(a, "render", function () {
            var a = this, f = []; (this.labelCollectors || []).forEach(function (a) { f = f.concat(a()) }); (this.yAxis || []).forEach(function (a) { a.stacking && a.options.stackLabels && !a.options.stackLabels.allowOverlap && B(a.stacking.stacks, function (a) { B(a, function (a) { a.label && "hidden" !== a.label.visibility && f.push(a.label) }) }) }); (this.series || []).forEach(function (c) {
                var e =
                c.options.dataLabels; c.visible && (!1 !== e.enabled || c._hasPointLabels) && (e = function (c) { return c.forEach(function (c) { c.visible && (y(c.dataLabels) ? c.dataLabels : c.dataLabel ? [c.dataLabel] : []).forEach(function (e) { var h = e.options; e.labelrank = q(h.labelrank, c.labelrank, c.shapeArgs && c.shapeArgs.height); h.allowOverlap ? (e.oldOpacity = e.opacity, e.newOpacity = 1, v(e, a)) : f.push(e) }) }) }, e(c.nodes || []), e(c.points))
            }); this.hideOverlappingLabels(f)
        }); a.prototype.hideOverlappingLabels = function (a) {
            var f = this, c = a.length, e = f.renderer,
            h, m, q, n = !1; var l = function (a) {
                var c, d = a.box ? 0 : a.padding || 0, f = c = 0, b; if (a && (!a.alignAttr || a.placed)) {
                    var g = a.alignAttr || { x: a.attr("x"), y: a.attr("y") }; var h = a.parentGroup; a.width || (c = a.getBBox(), a.width = c.width, a.height = c.height, c = e.fontMetrics(null, a.element).h); var l = a.width - 2 * d; (b = { left: "0", center: "0.5", right: "1" }[a.alignValue]) ? f = +b * l : G(a.x) && Math.round(a.x) !== a.translateX && (f = a.x - a.translateX); return {
                        x: g.x + (h.translateX || 0) + d - (f || 0), y: g.y + (h.translateY || 0) + d - c, width: a.width - 2 * d, height: a.height - 2 *
                        d
                    }
                }
            }; for (m = 0; m < c; m++) if (h = a[m]) h.oldOpacity = h.opacity, h.newOpacity = 1, h.absoluteBox = l(h); a.sort(function (a, c) { return (c.labelrank || 0) - (a.labelrank || 0) }); for (m = 0; m < c; m++) { var u = (l = a[m]) && l.absoluteBox; for (h = m + 1; h < c; ++h) { var y = (q = a[h]) && q.absoluteBox; !u || !y || l === q || 0 === l.newOpacity || 0 === q.newOpacity || y.x >= u.x + u.width || y.x + y.width <= u.x || y.y >= u.y + u.height || y.y + y.height <= u.y || ((l.labelrank < q.labelrank ? l : q).newOpacity = 0) } } a.forEach(function (a) { v(a, f) && (n = !0) }); n && x(f, "afterHideAllOverlappingLabels")
        }
    }); L(a,
    "Core/Responsive.js", [a["Core/Utilities.js"]], function (a) {
        var v = a.extend, E = a.find, H = a.isArray, x = a.isObject, y = a.merge, G = a.objectEach, B = a.pick, q = a.splat, h = a.uniqueKey, f = function () {
            function a() { } a.prototype.currentOptions = function (a) {
                function c(a, f, h, m) {
                    var l; G(a, function (a, d) {
                        if (!m && -1 < e.collectionsWithUpdate.indexOf(d) && f[d]) for (a = q(a), h[d] = [], l = 0; l < Math.max(a.length, f[d].length) ; l++) f[d][l] && (void 0 === a[l] ? h[d][l] = f[d][l] : (h[d][l] = {}, c(a[l], f[d][l], h[d][l], m + 1))); else x(a) ? (h[d] = H(a) ? [] : {}, c(a, f[d] ||
                        {}, h[d], m + 1)) : h[d] = "undefined" === typeof f[d] ? null : f[d]
                    })
                } var e = this, f = {}; c(a, this.options, f, 0); return f
            }; a.prototype.matchResponsiveRule = function (a, c) { var e = a.condition; (e.callback || function () { return this.chartWidth <= B(e.maxWidth, Number.MAX_VALUE) && this.chartHeight <= B(e.maxHeight, Number.MAX_VALUE) && this.chartWidth >= B(e.minWidth, 0) && this.chartHeight >= B(e.minHeight, 0) }).call(this) && c.push(a._id) }; a.prototype.setResponsive = function (a, c) {
                var e = this.options.responsive, f = this.currentResponsive, n = []; !c &&
                e && e.rules && e.rules.forEach(function (a) { "undefined" === typeof a._id && (a._id = h()); this.matchResponsiveRule(a, n) }, this); c = y.apply(void 0, n.map(function (a) { return E((e || {}).rules || [], function (c) { return c._id === a }) }).map(function (a) { return a && a.chartOptions })); c.isResponsiveOptions = !0; n = n.toString() || void 0; n !== (f && f.ruleIds) && (f && this.update(f.undoOptions, a, !0), n ? (f = this.currentOptions(c), f.isResponsiveOptions = !0, this.currentResponsive = { ruleIds: n, mergedOptions: c, undoOptions: f }, this.update(c, a, !0)) : this.currentResponsive =
                void 0)
            }; return a
        }(); a = function () { function a() { } a.compose = function (a) { v(a.prototype, f.prototype); return a }; return a }(); ""; ""; return a
    }); L(a, "masters/highcharts.src.js", [a["Core/Globals.js"], a["Core/Utilities.js"], a["Core/DefaultOptions.js"], a["Core/Animation/Fx.js"], a["Core/Animation/AnimationUtilities.js"], a["Core/Renderer/HTML/AST.js"], a["Core/FormatUtilities.js"], a["Core/Renderer/SVG/SVGElement.js"], a["Core/Renderer/SVG/SVGRenderer.js"], a["Core/Renderer/HTML/HTMLElement.js"], a["Core/Renderer/HTML/HTMLRenderer.js"],
    a["Core/Axis/Axis.js"], a["Core/Axis/PlotLineOrBand.js"], a["Core/Axis/Tick.js"], a["Core/Pointer.js"], a["Core/MSPointer.js"], a["Core/Chart/Chart.js"], a["Core/Series/Series.js"], a["Core/Responsive.js"], a["Core/Color/Color.js"], a["Core/Time.js"]], function (a, u, E, H, x, y, G, B, q, h, f, c, e, t, m, w, n, l, J, K, A) {
        a.animate = x.animate; a.animObject = x.animObject; a.getDeferredAnimation = x.getDeferredAnimation; a.setAnimation = x.setAnimation; a.stop = x.stop; a.timers = H.timers; a.AST = y; a.Axis = c; a.Chart = n; a.chart = n.chart; a.Fx = H; a.PlotLineOrBand =
        e; a.Pointer = w.isRequired() ? w : m; a.Series = l; a.SVGElement = B; a.SVGRenderer = q; a.Tick = t; a.Time = A; a.Color = K; a.color = K.parse; f.compose(q); h.compose(B); a.defaultOptions = E.defaultOptions; a.getOptions = E.getOptions; a.time = E.defaultTime; a.setOptions = E.setOptions; a.dateFormat = G.dateFormat; a.format = G.format; a.numberFormat = G.numberFormat; a.addEvent = u.addEvent; a.arrayMax = u.arrayMax; a.arrayMin = u.arrayMin; a.attr = u.attr; a.clearTimeout = u.clearTimeout; a.correctFloat = u.correctFloat; a.createElement = u.createElement; a.css =
        u.css; a.defined = u.defined; a.destroyObjectProperties = u.destroyObjectProperties; a.discardElement = u.discardElement; a.erase = u.erase; a.error = u.error; a.extend = u.extend; a.extendClass = u.extendClass; a.find = u.find; a.fireEvent = u.fireEvent; a.getMagnitude = u.getMagnitude; a.getStyle = u.getStyle; a.inArray = u.inArray; a.isArray = u.isArray; a.isClass = u.isClass; a.isDOMElement = u.isDOMElement; a.isFunction = u.isFunction; a.isNumber = u.isNumber; a.isObject = u.isObject; a.isString = u.isString; a.keys = u.keys; a.merge = u.merge; a.normalizeTickInterval =
        u.normalizeTickInterval; a.objectEach = u.objectEach; a.offset = u.offset; a.pad = u.pad; a.pick = u.pick; a.pInt = u.pInt; a.relativeLength = u.relativeLength; a.removeEvent = u.removeEvent; a.splat = u.splat; a.stableSort = u.stableSort; a.syncTimeout = u.syncTimeout; a.timeUnits = u.timeUnits; a.uniqueKey = u.uniqueKey; a.useSerialIds = u.useSerialIds; a.wrap = u.wrap; J.compose(n); return a
    }); a["masters/highcharts.src.js"]._modules = a; return a["masters/highcharts.src.js"]
});
//# sourceMappingURL=highcharts.js.map