
'use strict';
const DEFAULT_COORDS = [{"id":1.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S11","controlId":"svg-reagent-s11","row":1.0,"col":1.0,"x":316.25,"y":16.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":2.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S21","controlId":"svg-reagent-s21","row":1.0,"col":2.0,"x":291.25,"y":16.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":3.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S31","controlId":"svg-reagent-s31","row":1.0,"col":3.0,"x":266.25,"y":16.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":4.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S41","controlId":"svg-reagent-s41","row":1.0,"col":4.0,"x":241.25,"y":16.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":5.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S51","controlId":"svg-reagent-s51","row":1.0,"col":5.0,"x":216.25,"y":16.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":6.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S12","controlId":"svg-reagent-s12","row":2.0,"col":1.0,"x":316.25,"y":41.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":7.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S22","controlId":"svg-reagent-s22","row":2.0,"col":2.0,"x":291.25,"y":41.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":8.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S32","controlId":"svg-reagent-s32","row":2.0,"col":3.0,"x":266.25,"y":41.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":9.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S42","controlId":"svg-reagent-s42","row":2.0,"col":4.0,"x":241.25,"y":41.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":10.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S52","controlId":"svg-reagent-s52","row":2.0,"col":5.0,"x":216.25,"y":41.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":11.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S13","controlId":"svg-reagent-s13","row":3.0,"col":1.0,"x":316.25,"y":66.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":12.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S23","controlId":"svg-reagent-s23","row":3.0,"col":2.0,"x":291.25,"y":66.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":13.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S33","controlId":"svg-reagent-s33","row":3.0,"col":3.0,"x":266.25,"y":66.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":14.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S43","controlId":"svg-reagent-s43","row":3.0,"col":4.0,"x":241.25,"y":66.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":15.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S53","controlId":"svg-reagent-s53","row":3.0,"col":5.0,"x":216.25,"y":66.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":16.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S14","controlId":"svg-reagent-s14","row":4.0,"col":1.0,"x":316.25,"y":91.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":17.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S24","controlId":"svg-reagent-s24","row":4.0,"col":2.0,"x":291.25,"y":91.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":18.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S34","controlId":"svg-reagent-s34","row":4.0,"col":3.0,"x":266.25,"y":91.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":19.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S44","controlId":"svg-reagent-s44","row":4.0,"col":4.0,"x":241.25,"y":91.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":20.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S54","controlId":"svg-reagent-s54","row":4.0,"col":5.0,"x":216.25,"y":91.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":21.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S15","controlId":"svg-reagent-s15","row":5.0,"col":1.0,"x":316.25,"y":116.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":22.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S25","controlId":"svg-reagent-s25","row":5.0,"col":2.0,"x":291.25,"y":116.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":23.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S35","controlId":"svg-reagent-s35","row":5.0,"col":3.0,"x":266.25,"y":116.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":24.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S45","controlId":"svg-reagent-s45","row":5.0,"col":4.0,"x":241.25,"y":116.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":25.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S55","controlId":"svg-reagent-s55","row":5.0,"col":5.0,"x":216.25,"y":116.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":26.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S16","controlId":"svg-reagent-s16","row":6.0,"col":1.0,"x":316.25,"y":141.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":27.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S26","controlId":"svg-reagent-s26","row":6.0,"col":2.0,"x":291.25,"y":141.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":28.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S36","controlId":"svg-reagent-s36","row":6.0,"col":3.0,"x":266.25,"y":141.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":29.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S46","controlId":"svg-reagent-s46","row":6.0,"col":4.0,"x":241.25,"y":141.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":30.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S56","controlId":"svg-reagent-s56","row":6.0,"col":5.0,"x":216.25,"y":141.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":31.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S17","controlId":"svg-reagent-s17","row":7.0,"col":1.0,"x":316.25,"y":166.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":32.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S27","controlId":"svg-reagent-s27","row":7.0,"col":2.0,"x":291.25,"y":166.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":33.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S37","controlId":"svg-reagent-s37","row":7.0,"col":3.0,"x":266.25,"y":166.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":34.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S47","controlId":"svg-reagent-s47","row":7.0,"col":4.0,"x":241.25,"y":166.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":35.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S57","controlId":"svg-reagent-s57","row":7.0,"col":5.0,"x":216.25,"y":166.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":36.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S18","controlId":"svg-reagent-s18","row":8.0,"col":1.0,"x":316.25,"y":191.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":37.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S28","controlId":"svg-reagent-s28","row":8.0,"col":2.0,"x":291.25,"y":191.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":38.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S38","controlId":"svg-reagent-s38","row":8.0,"col":3.0,"x":266.25,"y":191.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":39.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S48","controlId":"svg-reagent-s48","row":8.0,"col":4.0,"x":241.25,"y":191.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":40.0,"category":"璇曞墏鍖?,"name":"璇曞墏_S58","controlId":"svg-reagent-s58","row":8.0,"col":5.0,"x":216.25,"y":191.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"5鍒椕?琛岃瘯鍓傚渾涓績锛涘垪鍙?璇曞墏閫氶亾1~5锛岃鍙?浠庝笂鍒颁笅1~8锛涚紪鍙疯鍒欎负S<閫氶亾><鐡朵綅>锛屽宸︿笂S11銆佸乏涓婼18銆佸彸涓婼58"},{"id":41.0,"category":"娲楅拡澶?,"name":"娲楅拡澶確宸﹀垪_娲楀澹乢R1","controlId":"svg-wash-outer-row-1","row":1.0,"col":1.0,"x":184.75,"y":17.0,"shape":"circle","radius":2.0,"width":null,"height":null,"note":"2x2娲楅拡澶村渾涓績锛涘彸鍒?娲楀唴澹侊紝宸﹀垪=娲楀澹?},{"id":42.0,"category":"娲楅拡澶?,"name":"娲楅拡澶確鍙冲垪_娲楀唴澹乢R1","controlId":"svg-wash-inner-row-1","row":1.0,"col":2.0,"x":177.75,"y":17.0,"shape":"circle","radius":2.0,"width":null,"height":null,"note":"2x2娲楅拡澶村渾涓績锛涘彸鍒?娲楀唴澹侊紝宸﹀垪=娲楀澹?},{"id":43.0,"category":"娲楅拡澶?,"name":"娲楅拡澶確宸﹀垪_娲楀澹乢R2","controlId":"svg-wash-outer-row-2","row":2.0,"col":1.0,"x":184.75,"y":42.0,"shape":"circle","radius":2.0,"width":null,"height":null,"note":"2x2娲楅拡澶村渾涓績锛涘彸鍒?娲楀唴澹侊紝宸﹀垪=娲楀澹?},{"id":44.0,"category":"娲楅拡澶?,"name":"娲楅拡澶確鍙冲垪_娲楀唴澹乢R2","controlId":"svg-wash-inner-row-2","row":2.0,"col":2.0,"x":177.75,"y":42.0,"shape":"circle","radius":2.0,"width":null,"height":null,"note":"2x2娲楅拡澶村渾涓績锛涘彸鍒?娲楀唴澹侊紝宸﹀垪=娲楀澹?},{"id":45.0,"category":"娣峰悎娑蹭綋閰嶆恫鍖?,"name":"閰嶆恫_R1_C1","controlId":"svg-mix-p11","row":1.0,"col":1.0,"x":184.75,"y":66.5,"shape":"circle","radius":4.25,"width":null,"height":null,"note":"2x4閰嶆恫鍦嗕腑蹇冿紱鍒楀彿涓哄浘闈粠宸﹀埌鍙筹紝琛屽彿涓哄浘闈粠涓婂埌涓?},{"id":46.0,"category":"娣峰悎娑蹭綋閰嶆恫鍖?,"name":"閰嶆恫_R1_C2","controlId":"svg-mix-p12","row":1.0,"col":2.0,"x":170.75,"y":66.5,"shape":"circle","radius":4.25,"width":null,"height":null,"note":"2x4閰嶆恫鍦嗕腑蹇冿紱鍒楀彿涓哄浘闈粠宸﹀埌鍙筹紝琛屽彿涓哄浘闈粠涓婂埌涓?},{"id":47.0,"category":"娣峰悎娑蹭綋閰嶆恫鍖?,"name":"閰嶆恫_R2_C1","controlId":"svg-mix-p21","row":2.0,"col":1.0,"x":184.75,"y":91.5,"shape":"circle","radius":4.25,"width":null,"height":null,"note":"2x4閰嶆恫鍦嗕腑蹇冿紱鍒楀彿涓哄浘闈粠宸﹀埌鍙筹紝琛屽彿涓哄浘闈粠涓婂埌涓?},{"id":48.0,"category":"娣峰悎娑蹭綋閰嶆恫鍖?,"name":"閰嶆恫_R2_C2","controlId":"svg-mix-p22","row":2.0,"col":2.0,"x":170.75,"y":91.5,"shape":"circle","radius":4.25,"width":null,"height":null,"note":"2x4閰嶆恫鍦嗕腑蹇冿紱鍒楀彿涓哄浘闈粠宸﹀埌鍙筹紝琛屽彿涓哄浘闈粠涓婂埌涓?},{"id":49.0,"category":"娣峰悎娑蹭綋閰嶆恫鍖?,"name":"閰嶆恫_R3_C1","controlId":"svg-mix-p31","row":3.0,"col":1.0,"x":184.75,"y":116.5,"shape":"circle","radius":4.25,"width":null,"height":null,"note":"2x4閰嶆恫鍦嗕腑蹇冿紱鍒楀彿涓哄浘闈粠宸﹀埌鍙筹紝琛屽彿涓哄浘闈粠涓婂埌涓?},{"id":50.0,"category":"娣峰悎娑蹭綋閰嶆恫鍖?,"name":"閰嶆恫_R3_C2","controlId":"svg-mix-p32","row":3.0,"col":2.0,"x":170.75,"y":116.5,"shape":"circle","radius":4.25,"width":null,"height":null,"note":"2x4閰嶆恫鍦嗕腑蹇冿紱鍒楀彿涓哄浘闈粠宸﹀埌鍙筹紝琛屽彿涓哄浘闈粠涓婂埌涓?},{"id":51.0,"category":"娣峰悎娑蹭綋閰嶆恫鍖?,"name":"閰嶆恫_R4_C1","controlId":"svg-mix-p41","row":4.0,"col":1.0,"x":184.75,"y":141.5,"shape":"circle","radius":4.25,"width":null,"height":null,"note":"2x4閰嶆恫鍦嗕腑蹇冿紱鍒楀彿涓哄浘闈粠宸﹀埌鍙筹紝琛屽彿涓哄浘闈粠涓婂埌涓?},{"id":52.0,"category":"娣峰悎娑蹭綋閰嶆恫鍖?,"name":"閰嶆恫_R4_C2","controlId":"svg-mix-p42","row":4.0,"col":2.0,"x":170.75,"y":141.5,"shape":"circle","radius":4.25,"width":null,"height":null,"note":"2x4閰嶆恫鍦嗕腑蹇冿紱鍒楀彿涓哄浘闈粠宸﹀埌鍙筹紝琛屽彿涓哄浘闈粠涓婂埌涓?},{"id":53.0,"category":"A/B娑?,"name":"A娑?,"controlId":"svg-liquid-a","row":1.0,"col":1.0,"x":177.75,"y":166.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"1x2澶у渾锛屼笂鏂?},{"id":54.0,"category":"A/B娑?,"name":"B娑?,"controlId":"svg-liquid-b","row":2.0,"col":1.0,"x":177.75,"y":191.5,"shape":"circle","radius":10.0,"width":null,"height":null,"note":"1x2澶у渾锛屼笅鏂?},{"id":55.0,"category":"娣峰寑鐢垫満","name":"娣峰寑鐢垫満_1","controlId":"svg-motor-m1","row":null,"col":1.0,"x":135.25,"y":16.5,"shape":"square","radius":null,"width":null,"height":null,"note":"鍙充晶鍖哄煙涓婃柟鍥涗釜鏂瑰潡锛涜緭鍑轰负涓績鐐癸紝鍗婂緞涓虹┖"},{"id":56.0,"category":"娣峰寑鐢垫満","name":"娣峰寑鐢垫満_2","controlId":"svg-motor-m2","row":null,"col":2.0,"x":85.25,"y":16.5,"shape":"square","radius":null,"width":null,"height":null,"note":"鍙充晶鍖哄煙涓婃柟鍥涗釜鏂瑰潡锛涜緭鍑轰负涓績鐐癸紝鍗婂緞涓虹┖"},{"id":57.0,"category":"娣峰寑鐢垫満","name":"娣峰寑鐢垫満_3","controlId":"svg-motor-m3","row":null,"col":3.0,"x":35.25,"y":16.5,"shape":"square","radius":null,"width":null,"height":null,"note":"鍙充晶鍖哄煙涓婃柟鍥涗釜鏂瑰潡锛涜緭鍑轰负涓績鐐癸紝鍗婂緞涓虹┖"},{"id":58.0,"category":"娣峰寑鐢垫満","name":"娣峰寑鐢垫満_4","controlId":"svg-motor-m4","row":null,"col":4.0,"x":-14.75,"y":16.5,"shape":"square","radius":null,"width":null,"height":null,"note":"鍙充晶鍖哄煙涓婃柟鍥涗釜鏂瑰潡锛涜緭鍑轰负涓績鐐癸紝鍗婂緞涓虹┖"},{"id":59.0,"category":"鐜荤墖閫氶亾","name":"R11","controlId":"svg-slide-r11","row":1.0,"col":1.0,"x":135.25,"y":116.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":60.0,"category":"鐜荤墖閫氶亾","name":"R12","controlId":"svg-slide-r12","row":2.0,"col":1.0,"x":135.25,"y":141.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":61.0,"category":"鐜荤墖閫氶亾","name":"R13","controlId":"svg-slide-r13","row":3.0,"col":1.0,"x":135.25,"y":166.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":62.0,"category":"鐜荤墖閫氶亾","name":"R14","controlId":"svg-slide-r14","row":4.0,"col":1.0,"x":135.25,"y":191.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":63.0,"category":"鐜荤墖閫氶亾","name":"R21","controlId":"svg-slide-r21","row":1.0,"col":2.0,"x":85.25,"y":116.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":64.0,"category":"鐜荤墖閫氶亾","name":"R22","controlId":"svg-slide-r22","row":2.0,"col":2.0,"x":85.25,"y":141.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":65.0,"category":"鐜荤墖閫氶亾","name":"R23","controlId":"svg-slide-r23","row":3.0,"col":2.0,"x":85.25,"y":166.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":66.0,"category":"鐜荤墖閫氶亾","name":"R24","controlId":"svg-slide-r24","row":4.0,"col":2.0,"x":85.25,"y":191.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":67.0,"category":"鐜荤墖閫氶亾","name":"R31","controlId":"svg-slide-r31","row":1.0,"col":3.0,"x":35.25,"y":116.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":68.0,"category":"鐜荤墖閫氶亾","name":"R32","controlId":"svg-slide-r32","row":2.0,"col":3.0,"x":35.25,"y":141.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":69.0,"category":"鐜荤墖閫氶亾","name":"R33","controlId":"svg-slide-r33","row":3.0,"col":3.0,"x":35.25,"y":166.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":70.0,"category":"鐜荤墖閫氶亾","name":"R34","controlId":"svg-slide-r34","row":4.0,"col":3.0,"x":35.25,"y":191.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":71.0,"category":"鐜荤墖閫氶亾","name":"R41","controlId":"svg-slide-r41","row":1.0,"col":4.0,"x":-14.75,"y":116.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":72.0,"category":"鐜荤墖閫氶亾","name":"R42","controlId":"svg-slide-r42","row":2.0,"col":4.0,"x":-14.75,"y":141.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":73.0,"category":"鐜荤墖閫氶亾","name":"R43","controlId":"svg-slide-r43","row":3.0,"col":4.0,"x":-14.75,"y":166.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":74.0,"category":"鐜荤墖閫氶亾","name":"R44","controlId":"svg-slide-r44","row":4.0,"col":4.0,"x":-14.75,"y":191.5,"shape":"square","radius":null,"width":10.0,"height":10.0,"note":"鍙充晶鍖哄煙涓嬫柟闀挎潯鍐呯幓鐗囦腑蹇冿紱缂栧彿Rxy锛歺=閫氶亾鍙凤紝y=浠庝笂鍒颁笅绗瑈寮狅紱渚嬪R14涓洪€氶亾1鏈€涓嬮潰涓€寮犵幓鐗囷紱鍧愭爣涓烘鏂瑰舰涓績鐐?},{"id":75.0,"category":"搴熸恫瀛?,"name":"搴熸恫瀛擾M1","controlId":"svg-port-waste-m1","row":null,"col":1.0,"x":139.85,"y":3.0,"shape":"circle","radius":2.5,"width":null,"height":null,"note":"鍥哄畾瀛斾綅锛涗綅浜嶮1娣峰寑鐢垫満涓婃柟宸︿晶锛涗笌缃戦〉绔簾娑插瓟娓叉煋鍧愭爣涓€鑷?},{"id":76.0,"category":"搴熸恫瀛?,"name":"搴熸恫瀛擾M2","controlId":"svg-port-waste-m2","row":null,"col":2.0,"x":89.85,"y":3.0,"shape":"circle","radius":2.5,"width":null,"height":null,"note":"鍥哄畾瀛斾綅锛涗綅浜嶮2娣峰寑鐢垫満涓婃柟宸︿晶锛涗笌缃戦〉绔簾娑插瓟娓叉煋鍧愭爣涓€鑷?},{"id":77.0,"category":"搴熸恫瀛?,"name":"搴熸恫瀛擾M3","controlId":"svg-port-waste-m3","row":null,"col":3.0,"x":39.85,"y":3.0,"shape":"circle","radius":2.5,"width":null,"height":null,"note":"鍥哄畾瀛斾綅锛涗綅浜嶮3娣峰寑鐢垫満涓婃柟宸︿晶锛涗笌缃戦〉绔簾娑插瓟娓叉煋鍧愭爣涓€鑷?},{"id":78.0,"category":"搴熸恫瀛?,"name":"搴熸恫瀛擾M4","controlId":"svg-port-waste-m4","row":null,"col":4.0,"x":-10.15,"y":3.0,"shape":"circle","radius":2.5,"width":null,"height":null,"note":"鍥哄畾瀛斾綅锛涗綅浜嶮4娣峰寑鐢垫満涓婃柟宸︿晶锛涗笌缃戦〉绔簾娑插瓟娓叉煋鍧愭爣涓€鑷?},{"id":79.0,"category":"鎺掓瘨瀛?,"name":"鎺掓瘨瀛擾M1","controlId":"svg-port-toxic-m1","row":null,"col":1.0,"x":130.65,"y":3.0,"shape":"circle","radius":2.5,"width":null,"height":null,"note":"鍥哄畾瀛斾綅锛涗綅浜嶮1娣峰寑鐢垫満涓婃柟鍙充晶锛涗笌缃戦〉绔帓姣掑瓟娓叉煋鍧愭爣涓€鑷?},{"id":80.0,"category":"鎺掓瘨瀛?,"name":"鎺掓瘨瀛擾M2","controlId":"svg-port-toxic-m2","row":null,"col":2.0,"x":80.65,"y":3.0,"shape":"circle","radius":2.5,"width":null,"height":null,"note":"鍥哄畾瀛斾綅锛涗綅浜嶮2娣峰寑鐢垫満涓婃柟鍙充晶锛涗笌缃戦〉绔帓姣掑瓟娓叉煋鍧愭爣涓€鑷?},{"id":81.0,"category":"鎺掓瘨瀛?,"name":"鎺掓瘨瀛擾M3","controlId":"svg-port-toxic-m3","row":null,"col":3.0,"x":30.65,"y":3.0,"shape":"circle","radius":2.5,"width":null,"height":null,"note":"鍥哄畾瀛斾綅锛涗綅浜嶮3娣峰寑鐢垫満涓婃柟鍙充晶锛涗笌缃戦〉绔帓姣掑瓟娓叉煋鍧愭爣涓€鑷?},{"id":82.0,"category":"鎺掓瘨瀛?,"name":"鎺掓瘨瀛擾M4","controlId":"svg-port-toxic-m4","row":null,"col":4.0,"x":-19.35,"y":3.0,"shape":"circle","radius":2.5,"width":null,"height":null,"note":"鍥哄畾瀛斾綅锛涗綅浜嶮4娣峰寑鐢垫満涓婃柟鍙充晶锛涗笌缃戦〉绔帓姣掑瓟娓叉煋鍧愭爣涓€鑷?},{"id":83.0,"category":"娓呮礂瀛?,"name":"閫氶亾1_娓呮礂瀛?,"controlId":"svg-port-clean-channel-1","row":null,"col":1.0,"x":135.25,"y":217.5,"shape":"circle","radius":2.8,"width":null,"height":null,"note":"鍥哄畾瀛斾綅锛涗綅浜庨€氶亾1涓嬫柟锛涗笌缃戦〉绔竻娲楀瓟娓叉煋鍧愭爣涓€鑷?},{"id":84.0,"category":"娓呮礂瀛?,"name":"閫氶亾2_娓呮礂瀛?,"controlId":"svg-port-clean-channel-2","row":null,"col":2.0,"x":85.25,"y":217.5,"shape":"circle","radius":2.8,"width":null,"height":null,"note":"鍥哄畾瀛斾綅锛涗綅浜庨€氶亾2涓嬫柟锛涗笌缃戦〉绔竻娲楀瓟娓叉煋鍧愭爣涓€鑷?},{"id":85.0,"category":"娓呮礂瀛?,"name":"閫氶亾3_娓呮礂瀛?,"controlId":"svg-port-clean-channel-3","row":null,"col":3.0,"x":35.25,"y":217.5,"shape":"circle","radius":2.8,"width":null,"height":null,"note":"鍥哄畾瀛斾綅锛涗綅浜庨€氶亾3涓嬫柟锛涗笌缃戦〉绔竻娲楀瓟娓叉煋鍧愭爣涓€鑷?},{"id":86.0,"category":"娓呮礂瀛?,"name":"閫氶亾4_娓呮礂瀛?,"controlId":"svg-port-clean-channel-4","row":null,"col":4.0,"x":-14.75,"y":217.5,"shape":"circle","radius":2.8,"width":null,"height":null,"note":"鍥哄畾瀛斾綅锛涗綅浜庨€氶亾4涓嬫柟锛涗笌缃戦〉绔竻娲楀瓟娓叉煋鍧愭爣涓€鑷?},{"id":87.0,"category":"鐩告満","name":"璇曞墏鎵爜鐩告満","controlId":"svg-camera-reagent-scanner","row":null,"col":null,"x":342.0,"y":211.0,"shape":"camera","radius":null,"width":15.0,"height":9.5,"note":"鍥哄畾鍧愭爣锛涗綅浜庤瘯鍓傚尯宸︿笅瑙掑渚э紝姘村钩绉诲嚭璇曞墏鍖轰絾绱ц创鍖哄煙锛涚敤浜庤瘯鍓傛壂鐮?},{"id":88.0,"category":"鏈烘鑷傜浉瀵逛綅缃?,"name":"閽堝ご_Z1","controlId":"svg-arm-needle-z1","row":null,"col":null,"x":0.0,"y":0.0,"shape":"circle","radius":6.0,"width":null,"height":null,"note":"鏈烘鑷傜浉瀵瑰潗鏍囷紱浠ユ満姊拌噦绾靛悜杞翠笂鐨刏1閽堜腑蹇冧负(0,0)锛孹鍚戝乏涓烘锛孻鍚戜笅涓烘"},{"id":89.0,"category":"鏈烘鑷傜浉瀵逛綅缃?,"name":"閽堝ご_Z2","controlId":"svg-arm-needle-z2","row":null,"col":null,"x":0.0,"y":25.0,"shape":"circle","radius":6.0,"width":null,"height":null,"note":"鏈烘鑷傜浉瀵瑰潗鏍囷紱Z1涓嶼2绾靛悜闂磋窛25mm"},{"id":90.0,"category":"鏈烘鑷傜浉瀵逛綅缃?,"name":"鏈烘鑷傞殢鍔ㄧ浉鏈篲鏍锋湰鎵爜","controlId":"svg-arm-camera-follow","row":null,"col":null,"x":0.0,"y":43.0,"shape":"camera","radius":null,"width":9.3,"height":5.9,"note":"鏈烘鑷傜浉瀵瑰潗鏍囷紱浣嶄簬涓や釜閽堝ご涓嬫柟骞堕殢鏈烘鑷傛暣浣撶Щ鍔紱璇ョ浉鏈哄悓鏃朵綔涓烘牱鏈?鐜荤墖鏉＄爜鎵爜鐩告満锛岀偣鍑诲悗杩涘叆鏍锋湰鎵爜鍣ㄥ璞¤鎯呫€?},{"id":91.0,"category":"璇曞墏鍒颁綅鎰熷簲","name":"璇曞墏閫氶亾1_鍒颁綅鎰熷簲","controlId":"svg-reagent-lane-1-position-sensor","row":null,"col":1.0,"x":316.25,"y":3.5,"shape":"square","radius":null,"width":6.0,"height":6.0,"note":"姣忓垪璇曞墏閫氶亾椤堕儴鍒颁綅鎰熷簲鐏紱鍚庣鍙寜鎺т欢ID鏇存柊鐘舵€侀鑹?},{"id":92.0,"category":"璇曞墏鍒颁綅鎰熷簲","name":"璇曞墏閫氶亾2_鍒颁綅鎰熷簲","controlId":"svg-reagent-lane-2-position-sensor","row":null,"col":2.0,"x":291.25,"y":3.5,"shape":"square","radius":null,"width":6.0,"height":6.0,"note":"姣忓垪璇曞墏閫氶亾椤堕儴鍒颁綅鎰熷簲鐏紱鍚庣鍙寜鎺т欢ID鏇存柊鐘舵€侀鑹?},{"id":93.0,"category":"璇曞墏鍒颁綅鎰熷簲","name":"璇曞墏閫氶亾3_鍒颁綅鎰熷簲","controlId":"svg-reagent-lane-3-position-sensor","row":null,"col":3.0,"x":266.25,"y":3.5,"shape":"square","radius":null,"width":6.0,"height":6.0,"note":"姣忓垪璇曞墏閫氶亾椤堕儴鍒颁綅鎰熷簲鐏紱鍚庣鍙寜鎺т欢ID鏇存柊鐘舵€侀鑹?},{"id":94.0,"category":"璇曞墏鍒颁綅鎰熷簲","name":"璇曞墏閫氶亾4_鍒颁綅鎰熷簲","controlId":"svg-reagent-lane-4-position-sensor","row":null,"col":4.0,"x":241.25,"y":3.5,"shape":"square","radius":null,"width":6.0,"height":6.0,"note":"姣忓垪璇曞墏閫氶亾椤堕儴鍒颁綅鎰熷簲鐏紱鍚庣鍙寜鎺т欢ID鏇存柊鐘舵€侀鑹?},{"id":95.0,"category":"璇曞墏鍒颁綅鎰熷簲","name":"璇曞墏閫氶亾5_鍒颁綅鎰熷簲","controlId":"svg-reagent-lane-5-position-sensor","row":null,"col":5.0,"x":216.25,"y":3.5,"shape":"square","radius":null,"width":6.0,"height":6.0,"note":"姣忓垪璇曞墏閫氶亾椤堕儴鍒颁綅鎰熷簲鐏紱鍚庣鍙寜鎺т欢ID鏇存柊鐘舵€侀鑹?},{"id":96.0,"category":"璇曞墏鍖洪€氶亾鍒嗛殧绾?,"name":"璇曞墏閫氶亾1-2_鍒嗛殧铏氱嚎","controlId":"svg-reagent-lane-separator-1-2","row":null,"col":null,"x":303.75,"y":104.0,"shape":"line","radius":null,"width":null,"height":195.0,"note":"璇曞墏鍖哄垪闂磋櫄绾匡紝鐢ㄤ簬绀烘剰涓嶅悓璇曞墏閫氶亾杈圭晫"},{"id":97.0,"category":"璇曞墏鍖洪€氶亾鍒嗛殧绾?,"name":"璇曞墏閫氶亾2-3_鍒嗛殧铏氱嚎","controlId":"svg-reagent-lane-separator-2-3","row":null,"col":null,"x":278.75,"y":104.0,"shape":"line","radius":null,"width":null,"height":195.0,"note":"璇曞墏鍖哄垪闂磋櫄绾匡紝鐢ㄤ簬绀烘剰涓嶅悓璇曞墏閫氶亾杈圭晫"},{"id":98.0,"category":"璇曞墏鍖洪€氶亾鍒嗛殧绾?,"name":"璇曞墏閫氶亾3-4_鍒嗛殧铏氱嚎","controlId":"svg-reagent-lane-separator-3-4","row":null,"col":null,"x":253.75,"y":104.0,"shape":"line","radius":null,"width":null,"height":195.0,"note":"璇曞墏鍖哄垪闂磋櫄绾匡紝鐢ㄤ簬绀烘剰涓嶅悓璇曞墏閫氶亾杈圭晫"},{"id":99.0,"category":"璇曞墏鍖洪€氶亾鍒嗛殧绾?,"name":"璇曞墏閫氶亾4-5_鍒嗛殧铏氱嚎","controlId":"svg-reagent-lane-separator-4-5","row":null,"col":null,"x":228.75,"y":104.0,"shape":"line","radius":null,"width":null,"height":195.0,"note":"璇曞墏鍖哄垪闂磋櫄绾匡紝鐢ㄤ簬绀烘剰涓嶅悓璇曞墏閫氶亾杈圭晫"},{"id":100.0,"category":"璇曞墏鍏ュ彛鎰熷簲","name":"璇曞墏閫氶亾1_鍏ュ彛鎰熷簲","controlId":"svg-reagent-lane-1-entry-sensor","row":null,"col":1.0,"x":316.25,"y":202.3,"shape":"square","radius":null,"width":6.5,"height":6.5,"note":"姣忓垪璇曞墏閫氶亾搴曢儴鍏ュ彛鎰熷簲鐏紱鍚庣鍙寜鎺т欢ID鏇存柊鐘舵€侀鑹诧紱鍥鹃潰涓嶆樉绀烘枃瀛楁爣璇?},{"id":101.0,"category":"璇曞墏鍏ュ彛鎰熷簲","name":"璇曞墏閫氶亾2_鍏ュ彛鎰熷簲","controlId":"svg-reagent-lane-2-entry-sensor","row":null,"col":2.0,"x":291.25,"y":202.3,"shape":"square","radius":null,"width":6.5,"height":6.5,"note":"姣忓垪璇曞墏閫氶亾搴曢儴鍏ュ彛鎰熷簲鐏紱鍚庣鍙寜鎺т欢ID鏇存柊鐘舵€侀鑹诧紱鍥鹃潰涓嶆樉绀烘枃瀛楁爣璇?},{"id":102.0,"category":"璇曞墏鍏ュ彛鎰熷簲","name":"璇曞墏閫氶亾3_鍏ュ彛鎰熷簲","controlId":"svg-reagent-lane-3-entry-sensor","row":null,"col":3.0,"x":266.25,"y":202.3,"shape":"square","radius":null,"width":6.5,"height":6.5,"note":"姣忓垪璇曞墏閫氶亾搴曢儴鍏ュ彛鎰熷簲鐏紱鍚庣鍙寜鎺т欢ID鏇存柊鐘舵€侀鑹诧紱鍥鹃潰涓嶆樉绀烘枃瀛楁爣璇?},{"id":103.0,"category":"璇曞墏鍏ュ彛鎰熷簲","name":"璇曞墏閫氶亾4_鍏ュ彛鎰熷簲","controlId":"svg-reagent-lane-4-entry-sensor","row":null,"col":4.0,"x":241.25,"y":202.3,"shape":"square","radius":null,"width":6.5,"height":6.5,"note":"姣忓垪璇曞墏閫氶亾搴曢儴鍏ュ彛鎰熷簲鐏紱鍚庣鍙寜鎺т欢ID鏇存柊鐘舵€侀鑹诧紱鍥鹃潰涓嶆樉绀烘枃瀛楁爣璇?},{"id":104.0,"category":"璇曞墏鍏ュ彛鎰熷簲","name":"璇曞墏閫氶亾5_鍏ュ彛鎰熷簲","controlId":"svg-reagent-lane-5-entry-sensor","row":null,"col":5.0,"x":216.25,"y":202.3,"shape":"square","radius":null,"width":6.5,"height":6.5,"note":"姣忓垪璇曞墏閫氶亾搴曢儴鍏ュ彛鎰熷簲鐏紱鍚庣鍙寜鎺т欢ID鏇存柊鐘舵€侀鑹诧紱鍥鹃潰涓嶆樉绀烘枃瀛楁爣璇?},{"id":105.0,"category":"璇曞墏鍥句緥鎬婚噺","name":"璇曞墏鍥句緥_涓€鎶梍鎬诲墿浣欓噺","controlId":"svg-reagent-legend-primary-antibody-remaining-ml","row":null,"col":null,"x":null,"y":null,"shape":"","radius":null,"width":null,"height":null,"note":"璇曞墏鍖哄浘渚嬩腑鈥滀竴鎶椻€濆悗鎷彿鍐呯殑鎬诲墿浣欐鍗囨暟锛屽悗绔彲鐩存帴鏇存柊璇ユ帶浠?},{"id":106.0,"category":"璇曞墏鍥句緥鎬婚噺","name":"璇曞墏鍥句緥_浜屾姉_鎬诲墿浣欓噺","controlId":"svg-reagent-legend-secondary-antibody-remaining-ml","row":null,"col":null,"x":null,"y":null,"shape":"","radius":null,"width":null,"height":null,"note":"璇曞墏鍖哄浘渚嬩腑鈥滀簩鎶椻€濆悗鎷彿鍐呯殑鎬诲墿浣欐鍗囨暟锛屽悗绔彲鐩存帴鏇存柊璇ユ帶浠?},{"id":107.0,"category":"璇曞墏鍥句緥鎬婚噺","name":"璇曞墏鍥句緥_鑻忔湪绱燺鎬诲墿浣欓噺","controlId":"svg-reagent-legend-hematoxylin-remaining-ml","row":null,"col":null,"x":null,"y":null,"shape":"","radius":null,"width":null,"height":null,"note":"璇曞墏鍖哄浘渚嬩腑鈥滆嫃鏈ㄧ礌鈥濆悗鎷彿鍐呯殑鎬诲墿浣欐鍗囨暟锛屽悗绔彲鐩存帴鏇存柊璇ユ帶浠?},{"id":108.0,"category":"璇曞墏鍥句緥鎬婚噺","name":"璇曞墏鍥句緥_浼婄孩_鎬诲墿浣欓噺","controlId":"svg-reagent-legend-eosin-remaining-ml","row":null,"col":null,"x":null,"y":null,"shape":"","radius":null,"width":null,"height":null,"note":"璇曞墏鍖哄浘渚嬩腑鈥滀紛绾⑩€濆悗鎷彿鍐呯殑鎬诲墿浣欐鍗囨暟锛屽悗绔彲鐩存帴鏇存柊璇ユ帶浠?},{"id":109.0,"category":"璇曞墏鍥句緥鎬婚噺","name":"璇曞墏鍥句緥_鏃犳按閰掔簿_鎬诲墿浣欓噺","controlId":"svg-reagent-legend-absolute-ethanol-remaining-ml","row":null,"col":null,"x":null,"y":null,"shape":"","radius":null,"width":null,"height":null,"note":"璇曞墏鍖哄浘渚嬩腑鈥滄棤姘撮厭绮锯€濆悗鎷彿鍐呯殑鎬诲墿浣欐鍗囨暟锛屽悗绔彲鐩存帴鏇存柊璇ユ帶浠?},{"id":110.0,"category":"璇曞墏鍥句緥鎬婚噺","name":"璇曞墏鍥句緥_閰告礂娑瞋鎬诲墿浣欓噺","controlId":"svg-reagent-legend-acid-wash-remaining-ml","row":null,"col":null,"x":null,"y":null,"shape":"","radius":null,"width":null,"height":null,"note":"璇曞墏鍖哄浘渚嬩腑鈥滈吀娲楁恫鈥濆悗鎷彿鍐呯殑鎬诲墿浣欐鍗囨暟锛屽悗绔彲鐩存帴鏇存柊璇ユ帶浠?},{"id":111.0,"category":"璇曞墏鍥句緥鎬婚噺","name":"璇曞墏鍥句緥_鍐呮簮鎬ч叾闃绘柇鍓俖鎬诲墿浣欓噺","controlId":"svg-reagent-legend-endogenous-enzyme-blocker-remaining-ml","row":null,"col":null,"x":null,"y":null,"shape":"","radius":null,"width":null,"height":null,"note":"璇曞墏鍖哄浘渚嬩腑鈥滃唴婧愭€ч叾闃绘柇鍓傗€濆悗鎷彿鍐呯殑鎬诲墿浣欐鍗囨暟锛屽悗绔彲鐩存帴鏇存柊璇ユ帶浠?},{"id":112.0,"category":"閰嶆恫鐡剁姸鎬?,"name":"閰嶆恫鐡剁┖鐡舵€荤姸鎬?,"controlId":"svg-mix-empty-bottle-status","row":null,"col":null,"x":177.75,"y":214.0,"shape":"bar","radius":null,"width":30.0,"height":8.0,"note":"P11~P42鍏?涓厤娑茬摱鐨勭┖鐡舵暟閲忔眹鎬伙紝浣跨敤8鏍艰繘搴︽潯鏄剧ず"},{"id":113.0,"category":"閰嶆恫鐡剁姸鎬佹牸","name":"閰嶆恫鐡剁┖鐡剁姸鎬佹牸_1","controlId":"svg-mix-empty-bottle-cell-1","row":null,"col":null,"x":166.55,"y":217.6,"shape":"square","radius":null,"width":2.5,"height":3.2,"note":"閰嶆恫鐡剁┖鐡剁姸鎬佹潯鐨勫崟鏍硷紱缁胯壊琛ㄧず瀹屽叏鏈厤娑茬┖鐡?},{"id":114.0,"category":"閰嶆恫鐡剁姸鎬佹牸","name":"閰嶆恫鐡剁┖鐡剁姸鎬佹牸_2","controlId":"svg-mix-empty-bottle-cell-2","row":null,"col":null,"x":169.75,"y":217.6,"shape":"square","radius":null,"width":2.5,"height":3.2,"note":"閰嶆恫鐡剁┖鐡剁姸鎬佹潯鐨勫崟鏍硷紱缁胯壊琛ㄧず瀹屽叏鏈厤娑茬┖鐡?},{"id":115.0,"category":"閰嶆恫鐡剁姸鎬佹牸","name":"閰嶆恫鐡剁┖鐡剁姸鎬佹牸_3","controlId":"svg-mix-empty-bottle-cell-3","row":null,"col":null,"x":172.95,"y":217.6,"shape":"square","radius":null,"width":2.5,"height":3.2,"note":"閰嶆恫鐡剁┖鐡剁姸鎬佹潯鐨勫崟鏍硷紱缁胯壊琛ㄧず瀹屽叏鏈厤娑茬┖鐡?},{"id":116.0,"category":"閰嶆恫鐡剁姸鎬佹牸","name":"閰嶆恫鐡剁┖鐡剁姸鎬佹牸_4","controlId":"svg-mix-empty-bottle-cell-4","row":null,"col":null,"x":176.15,"y":217.6,"shape":"square","radius":null,"width":2.5,"height":3.2,"note":"閰嶆恫鐡剁┖鐡剁姸鎬佹潯鐨勫崟鏍硷紱缁胯壊琛ㄧず瀹屽叏鏈厤娑茬┖鐡?},{"id":117.0,"category":"閰嶆恫鐡剁姸鎬佹牸","name":"閰嶆恫鐡剁┖鐡剁姸鎬佹牸_5","controlId":"svg-mix-empty-bottle-cell-5","row":null,"col":null,"x":179.35,"y":217.6,"shape":"square","radius":null,"width":2.5,"height":3.2,"note":"閰嶆恫鐡剁┖鐡剁姸鎬佹潯鐨勫崟鏍硷紱缁胯壊琛ㄧず瀹屽叏鏈厤娑茬┖鐡?},{"id":118.0,"category":"閰嶆恫鐡剁姸鎬佹牸","name":"閰嶆恫鐡剁┖鐡剁姸鎬佹牸_6","controlId":"svg-mix-empty-bottle-cell-6","row":null,"col":null,"x":182.55,"y":217.6,"shape":"square","radius":null,"width":2.5,"height":3.2,"note":"閰嶆恫鐡剁┖鐡剁姸鎬佹潯鐨勫崟鏍硷紱缁胯壊琛ㄧず瀹屽叏鏈厤娑茬┖鐡?},{"id":119.0,"category":"閰嶆恫鐡剁姸鎬佹牸","name":"閰嶆恫鐡剁┖鐡剁姸鎬佹牸_7","controlId":"svg-mix-empty-bottle-cell-7","row":null,"col":null,"x":185.75,"y":217.6,"shape":"square","radius":null,"width":2.5,"height":3.2,"note":"閰嶆恫鐡剁┖鐡剁姸鎬佹潯鐨勫崟鏍硷紱缁胯壊琛ㄧず瀹屽叏鏈厤娑茬┖鐡?},{"id":120.0,"category":"閰嶆恫鐡剁姸鎬佹牸","name":"閰嶆恫鐡剁┖鐡剁姸鎬佹牸_8","controlId":"svg-mix-empty-bottle-cell-8","row":null,"col":null,"x":188.95,"y":217.6,"shape":"square","radius":null,"width":2.5,"height":3.2,"note":"閰嶆恫鐡剁┖鐡剁姸鎬佹潯鐨勫崟鏍硷紱缁胯壊琛ㄧず瀹屽叏鏈厤娑茬┖鐡?},{"id":121.0,"category":"鐣岄潰鎺т欢","name":"鍙充晶涓籘ab_璋冭瘯","controlId":"debugTab","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":122.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯椤甸潰","controlId":"debugPane","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":123.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯妯″潡","controlId":"debugModule","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":124.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯鎺у埗鍙?,"controlId":"debugCommandConsole","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":125.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯_COM鍗＄墖","controlId":"debugComCard","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":126.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯_COM鍙ｉ€夋嫨","controlId":"debugComPortSelect","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":127.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯_娉㈢壒鐜?,"controlId":"debugBaudRateSelect","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":128.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯_鏁版嵁浣?,"controlId":"debugDataBitsSelect","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":129.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯_鍋滄浣?,"controlId":"debugStopBitsSelect","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":130.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯_鏍￠獙浣?,"controlId":"debugParitySelect","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":131.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯_COM鐘舵€?,"controlId":"debugComStatusText","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":132.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯_绉讳綅绮惧害X","controlId":"debugMovePrecisionXInput","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":133.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯_绉讳綅绮惧害Y","controlId":"debugMovePrecisionYInput","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":134.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯_鍔犳牱浣撶Н鐩爣","controlId":"debugDispenseTargetInput","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":135.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯_鍔犳牱浣撶Н瀹炴祴","controlId":"debugDispenseMeasuredInput","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":136.0,"category":"鐣岄潰鎺т欢","name":"璋冭瘯_鍗曟ā鍧楁祴璇曞尯","controlId":"debugModuleControlGrid","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":137.0,"category":"鐣岄潰鎺т欢","name":"閰嶇疆_娑蹭綋绫诲瀷椤甸潰","controlId":"configSectionLiquidClass","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":138.0,"category":"鐣岄潰鎺т欢","name":"娑蹭綋绫诲瀷_褰撳墠绫诲瀷","controlId":"liquidClassSelect","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":139.0,"category":"鐣岄潰鎺т欢","name":"娑蹭綋绫诲瀷_鏂板缓鍚嶇О","controlId":"liquidClassNewNameInput","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":140.0,"category":"鐣岄潰鎺т欢","name":"娑蹭綋绫诲瀷_鍚告恫鍗＄墖","controlId":"liquidAspirationCard","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":141.0,"category":"鐣岄潰鎺т欢","name":"娑蹭綋绫诲瀷_鍔犳恫鍗＄墖","controlId":"liquidDispenseCard","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":142.0,"category":"鐣岄潰鎺т欢","name":"閰嶇疆_娓呮礂娣峰寑椤甸潰","controlId":"configSectionThermal","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":143.0,"category":"鐣岄潰鎺т欢","name":"娓呮礂娣峰寑_娣峰寑鍔犵儹鍗＄墖","controlId":"thermalMixCard","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":144.0,"category":"鐣岄潰鎺т欢","name":"娓呮礂娣峰寑_閫氶亾閫夋嫨","controlId":"thermalChannelSelect","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":145.0,"category":"鐣岄潰鎺т欢","name":"娓呮礂娣峰寑_鏍锋湰鐩爣娓╁害","controlId":"thermalSampleTargetInput","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":146.0,"category":"鐣岄潰鎺т欢","name":"娓呮礂娣峰寑_鏍锋湰娓呮礂鏃堕棿","controlId":"sampleWashTimeInput","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":147.0,"category":"鐣岄潰鎺т欢","name":"娓呮礂娣峰寑_鏍锋湰娓呮礂娓╁害","controlId":"sampleWashTempInput","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":148.0,"category":"鐣岄潰鎺т欢","name":"娓呮礂娣峰寑_鍒跺喎娓╁害","controlId":"coolingTargetInput","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":149.0,"category":"鐣岄潰鎺т欢","name":"娓呮礂娣峰寑_鍑烘按鐩爣娓╁害","controlId":"waterOutletTargetTempInput","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":150.0,"category":"鐣岄潰鎺т欢","name":"娓呮礂娣峰寑_鍑烘按姘撮噺","controlId":"waterOutletVolumeInput","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":151.0,"category":"鐣岄潰鎺т欢","name":"娓呮礂娣峰寑_鍑烘按娴侀€?,"controlId":"waterOutletFlowRateInput","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"宸ョ▼甯堣皟璇?璁惧閰嶇疆鐣岄潰鎺т欢锛涙棤鐗╃悊鍧愭爣锛屽悗绔彲鎸夋帶浠禝D璇诲彇鎴栨洿鏂般€?},{"id":152.0,"category":"鐧诲綍椤?,"name":"鐧诲綍椤甸潰","controlId":"loginScreen","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐢ㄦ埛鐧诲綍閬僵锛涚鐞嗗憳/瀹為獙鍛樹袱绫诲叆鍙ｃ€?},{"id":153.0,"category":"鐧诲綍椤?,"name":"绠＄悊鍛樼櫥褰曟寜閽?,"controlId":"adminLoginBtn","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"绠＄悊鍛樼櫥褰曟彁浜ゆ寜閽€?},{"id":154.0,"category":"鐧诲綍椤?,"name":"瀹為獙鍛樼櫥褰曟寜閽?,"controlId":"operatorLoginBtn","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"瀹為獙鍛樼櫥褰曟彁浜ゆ寜閽€?},{"id":155.0,"category":"鐧诲綍椤?,"name":"鐧诲綍閿欒鎻愮ず","controlId":"loginErrorText","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐧诲綍鏍￠獙鎻愮ず銆?},{"id":156.0,"category":"鐢ㄦ埛绠＄悊","name":"鐢ㄦ埛绠＄悊琛?,"controlId":"userManagementTable","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"绠＄悊鍛樼敤鎴风鐞嗛〉闈㈢敤鎴峰垪琛ㄣ€?},{"id":157.0,"category":"鐢ㄦ埛绠＄悊","name":"鏂板鐢ㄦ埛鍗＄墖","controlId":"userCreateCard","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"绠＄悊鍛樻柊澧炴垨淇敼鐢ㄦ埛銆?},{"id":158.0,"category":"鐢ㄦ埛绠＄悊","name":"鏂板鐢ㄦ埛鎸夐挳","controlId":"userAddBtn","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鏂板鎴栨洿鏂扮敤鎴枫€?},{"id":159.0,"category":"鏉冮檺鎺у埗","name":"椤堕儴璋冭瘯妯″紡","controlId":"modeDebugBtn","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"浠呯鐞嗗憳鍙鐨勯《閮ㄨ皟璇曟ā寮忔寜閽€?},{"id":160.0,"category":"鐧诲綍椤?,"name":"鐧诲綍琛ㄥ崟鍗＄墖","controlId":"simpleLoginCard","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"绠€鍖栫櫥褰曡〃鍗曪紱璐﹀彿瀵嗙爜鍏辩敤杈撳叆妗嗐€?},{"id":161.0,"category":"鐧诲綍椤?,"name":"鐧诲綍璐﹀彿杈撳叆妗?,"controlId":"loginUsername","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"璐﹀彿杈撳叆妗嗭紱鏍规嵁鐐瑰嚮鐨勭鐞嗗憳/瀹為獙鍛樻寜閽牎楠屽搴旇鑹层€?},{"id":162.0,"category":"鐧诲綍椤?,"name":"鐧诲綍瀵嗙爜杈撳叆妗?,"controlId":"loginPassword","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"瀵嗙爜杈撳叆妗嗭紱鏍规嵁鐐瑰嚮鐨勭鐞嗗憳/瀹為獙鍛樻寜閽牎楠屽搴旇鑹层€?},{"id":163.0,"category":"鐧诲綍椤?,"name":"鐧诲綍韬唤鎸夐挳鍖?,"controlId":"loginRoleActions","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鍖呭惈绠＄悊鍛樼櫥褰曞拰瀹為獙鍛樼櫥褰曚袱涓寜閽€?},{"id":164.0,"category":"鐣岄潰鎺т欢","name":"鍙充晶鏍忓搴︽寜閽粍","controlId":"rightPanelWidthToggleGroup","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鍙充晶杈规爮搴曢儴宸ュ叿琛屽乏渚х殑鍗曚釜妯悜瀹藉害蹇嵎鎸夐挳瀹瑰櫒銆?},{"id":165.0,"category":"鐣岄潰鎺т欢","name":"鍙充晶鏍忓搴﹀垏鎹㈡寜閽?,"controlId":"rightPanelQuickToggle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鍗曚釜妯悜鎸夐挳锛涗綅浜庡彸渚ф爮搴曢儴宸ュ叿琛屽乏渚э紝鏍规嵁褰撳墠瀹藉害鍦?30% 鏀剁缉鍜?70% 灞曞紑涔嬮棿鍒囨崲銆?},{"id":166.0,"category":"鐣岄潰鎺т欢","name":"閰嶇疆鏂囦欢_杞婚噺鎶藉眽","controlId":"configProfileFold","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閰嶇疆/娴佺▼椤甸《閮ㄩ厤缃枃浠舵娊灞夛紱榛樿鏀惰捣锛屽彧鏄剧ず褰撳墠閰嶇疆鍚嶇О銆佺被鍨嬨€佹鏁板拰鎺ф俯鐘舵€侊紱鐐瑰嚮鍚庨€夋嫨銆佹柊寤恒€佸鍏ュ鍑恒€?},{"id":167.0,"category":"鐣岄潰鎺т欢","name":"娴佺▼缂栬緫_涓诲伐浣滃彴","controlId":"configFlowFold","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閰嶇疆/娴佺▼椤典富缂栬緫鍖哄煙锛涢粯璁ゅ睍寮€锛屽寘鍚搷浣滃簱銆佹祦绋嬬爾鍧楁椂闂寸嚎鍜屽綋鍓嶆楠ょ紪杈戝櫒锛屼繚璇佹祦绋嬬紪杈戞湁涓昏鍙绌洪棿銆?},{"id":176.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s11-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":177.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_ROI Left","controlId":"svg-reagent-s11-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":178.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_ROI Top","controlId":"svg-reagent-s11-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":179.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_ROI Width","controlId":"svg-reagent-s11-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":180.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_ROI Height","controlId":"svg-reagent-s11-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":181.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s11-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":182.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_娑蹭綋绫诲瀷","controlId":"svg-reagent-s11-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":183.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s11-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":184.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s11-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":185.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s11-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":186.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s11-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":187.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s11-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":188.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_绉绘恫瀛斾綅","controlId":"svg-reagent-s11-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":189.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_绉绘恫娑查噺","controlId":"svg-reagent-s11-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":190.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S11_鎺у埗閽?,"controlId":"svg-reagent-s11-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":191.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s21-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":192.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_ROI Left","controlId":"svg-reagent-s21-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":193.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_ROI Top","controlId":"svg-reagent-s21-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":194.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_ROI Width","controlId":"svg-reagent-s21-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":195.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_ROI Height","controlId":"svg-reagent-s21-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":196.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s21-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":197.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_娑蹭綋绫诲瀷","controlId":"svg-reagent-s21-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":198.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s21-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":199.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s21-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":200.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s21-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":201.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s21-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":202.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s21-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":203.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_绉绘恫瀛斾綅","controlId":"svg-reagent-s21-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":204.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_绉绘恫娑查噺","controlId":"svg-reagent-s21-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":205.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S21_鎺у埗閽?,"controlId":"svg-reagent-s21-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":206.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s31-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":207.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_ROI Left","controlId":"svg-reagent-s31-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":208.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_ROI Top","controlId":"svg-reagent-s31-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":209.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_ROI Width","controlId":"svg-reagent-s31-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":210.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_ROI Height","controlId":"svg-reagent-s31-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":211.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s31-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":212.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_娑蹭綋绫诲瀷","controlId":"svg-reagent-s31-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":213.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s31-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":214.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s31-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":215.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s31-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":216.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s31-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":217.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s31-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":218.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_绉绘恫瀛斾綅","controlId":"svg-reagent-s31-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":219.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_绉绘恫娑查噺","controlId":"svg-reagent-s31-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":220.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S31_鎺у埗閽?,"controlId":"svg-reagent-s31-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":221.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s41-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":222.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_ROI Left","controlId":"svg-reagent-s41-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":223.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_ROI Top","controlId":"svg-reagent-s41-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":224.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_ROI Width","controlId":"svg-reagent-s41-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":225.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_ROI Height","controlId":"svg-reagent-s41-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":226.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s41-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":227.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_娑蹭綋绫诲瀷","controlId":"svg-reagent-s41-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":228.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s41-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":229.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s41-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":230.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s41-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":231.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s41-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":232.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s41-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":233.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_绉绘恫瀛斾綅","controlId":"svg-reagent-s41-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":234.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_绉绘恫娑查噺","controlId":"svg-reagent-s41-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":235.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S41_鎺у埗閽?,"controlId":"svg-reagent-s41-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":236.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s51-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":237.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_ROI Left","controlId":"svg-reagent-s51-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":238.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_ROI Top","controlId":"svg-reagent-s51-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":239.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_ROI Width","controlId":"svg-reagent-s51-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":240.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_ROI Height","controlId":"svg-reagent-s51-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":241.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s51-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":242.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_娑蹭綋绫诲瀷","controlId":"svg-reagent-s51-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":243.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s51-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":244.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s51-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":245.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s51-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":246.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s51-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":247.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s51-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":248.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_绉绘恫瀛斾綅","controlId":"svg-reagent-s51-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":249.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_绉绘恫娑查噺","controlId":"svg-reagent-s51-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":250.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S51_鎺у埗閽?,"controlId":"svg-reagent-s51-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":251.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s12-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":252.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_ROI Left","controlId":"svg-reagent-s12-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":253.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_ROI Top","controlId":"svg-reagent-s12-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":254.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_ROI Width","controlId":"svg-reagent-s12-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":255.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_ROI Height","controlId":"svg-reagent-s12-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":256.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s12-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":257.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_娑蹭綋绫诲瀷","controlId":"svg-reagent-s12-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":258.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s12-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":259.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s12-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":260.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s12-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":261.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s12-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":262.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s12-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":263.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_绉绘恫瀛斾綅","controlId":"svg-reagent-s12-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":264.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_绉绘恫娑查噺","controlId":"svg-reagent-s12-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":265.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S12_鎺у埗閽?,"controlId":"svg-reagent-s12-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":266.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s22-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":267.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_ROI Left","controlId":"svg-reagent-s22-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":268.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_ROI Top","controlId":"svg-reagent-s22-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":269.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_ROI Width","controlId":"svg-reagent-s22-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":270.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_ROI Height","controlId":"svg-reagent-s22-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":271.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s22-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":272.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_娑蹭綋绫诲瀷","controlId":"svg-reagent-s22-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":273.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s22-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":274.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s22-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":275.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s22-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":276.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s22-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":277.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s22-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":278.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_绉绘恫瀛斾綅","controlId":"svg-reagent-s22-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":279.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_绉绘恫娑查噺","controlId":"svg-reagent-s22-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":280.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S22_鎺у埗閽?,"controlId":"svg-reagent-s22-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":281.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s32-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":282.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_ROI Left","controlId":"svg-reagent-s32-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":283.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_ROI Top","controlId":"svg-reagent-s32-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":284.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_ROI Width","controlId":"svg-reagent-s32-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":285.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_ROI Height","controlId":"svg-reagent-s32-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":286.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s32-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":287.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_娑蹭綋绫诲瀷","controlId":"svg-reagent-s32-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":288.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s32-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":289.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s32-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":290.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s32-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":291.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s32-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":292.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s32-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":293.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_绉绘恫瀛斾綅","controlId":"svg-reagent-s32-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":294.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_绉绘恫娑查噺","controlId":"svg-reagent-s32-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":295.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S32_鎺у埗閽?,"controlId":"svg-reagent-s32-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":296.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s42-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":297.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_ROI Left","controlId":"svg-reagent-s42-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":298.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_ROI Top","controlId":"svg-reagent-s42-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":299.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_ROI Width","controlId":"svg-reagent-s42-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":300.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_ROI Height","controlId":"svg-reagent-s42-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":301.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s42-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":302.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_娑蹭綋绫诲瀷","controlId":"svg-reagent-s42-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":303.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s42-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":304.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s42-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":305.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s42-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":306.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s42-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":307.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s42-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":308.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_绉绘恫瀛斾綅","controlId":"svg-reagent-s42-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":309.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_绉绘恫娑查噺","controlId":"svg-reagent-s42-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":310.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S42_鎺у埗閽?,"controlId":"svg-reagent-s42-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":311.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s52-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":312.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_ROI Left","controlId":"svg-reagent-s52-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":313.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_ROI Top","controlId":"svg-reagent-s52-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":314.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_ROI Width","controlId":"svg-reagent-s52-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":315.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_ROI Height","controlId":"svg-reagent-s52-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":316.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s52-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":317.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_娑蹭綋绫诲瀷","controlId":"svg-reagent-s52-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":318.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s52-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":319.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s52-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":320.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s52-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":321.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s52-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":322.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s52-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":323.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_绉绘恫瀛斾綅","controlId":"svg-reagent-s52-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":324.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_绉绘恫娑查噺","controlId":"svg-reagent-s52-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":325.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S52_鎺у埗閽?,"controlId":"svg-reagent-s52-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":326.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s13-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":327.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_ROI Left","controlId":"svg-reagent-s13-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":328.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_ROI Top","controlId":"svg-reagent-s13-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":329.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_ROI Width","controlId":"svg-reagent-s13-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":330.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_ROI Height","controlId":"svg-reagent-s13-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":331.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s13-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":332.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_娑蹭綋绫诲瀷","controlId":"svg-reagent-s13-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":333.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s13-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":334.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s13-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":335.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s13-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":336.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s13-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":337.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s13-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":338.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_绉绘恫瀛斾綅","controlId":"svg-reagent-s13-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":339.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_绉绘恫娑查噺","controlId":"svg-reagent-s13-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":340.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S13_鎺у埗閽?,"controlId":"svg-reagent-s13-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":341.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s23-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":342.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_ROI Left","controlId":"svg-reagent-s23-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":343.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_ROI Top","controlId":"svg-reagent-s23-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":344.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_ROI Width","controlId":"svg-reagent-s23-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":345.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_ROI Height","controlId":"svg-reagent-s23-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":346.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s23-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":347.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_娑蹭綋绫诲瀷","controlId":"svg-reagent-s23-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":348.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s23-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":349.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s23-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":350.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s23-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":351.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s23-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":352.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s23-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":353.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_绉绘恫瀛斾綅","controlId":"svg-reagent-s23-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":354.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_绉绘恫娑查噺","controlId":"svg-reagent-s23-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":355.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S23_鎺у埗閽?,"controlId":"svg-reagent-s23-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":356.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s33-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":357.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_ROI Left","controlId":"svg-reagent-s33-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":358.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_ROI Top","controlId":"svg-reagent-s33-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":359.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_ROI Width","controlId":"svg-reagent-s33-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":360.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_ROI Height","controlId":"svg-reagent-s33-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":361.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s33-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":362.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_娑蹭綋绫诲瀷","controlId":"svg-reagent-s33-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":363.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s33-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":364.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s33-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":365.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s33-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":366.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s33-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":367.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s33-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":368.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_绉绘恫瀛斾綅","controlId":"svg-reagent-s33-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":369.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_绉绘恫娑查噺","controlId":"svg-reagent-s33-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":370.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S33_鎺у埗閽?,"controlId":"svg-reagent-s33-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":371.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s43-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":372.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_ROI Left","controlId":"svg-reagent-s43-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":373.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_ROI Top","controlId":"svg-reagent-s43-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":374.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_ROI Width","controlId":"svg-reagent-s43-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":375.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_ROI Height","controlId":"svg-reagent-s43-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":376.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s43-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":377.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_娑蹭綋绫诲瀷","controlId":"svg-reagent-s43-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":378.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s43-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":379.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s43-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":380.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s43-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":381.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s43-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":382.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s43-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":383.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_绉绘恫瀛斾綅","controlId":"svg-reagent-s43-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":384.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_绉绘恫娑查噺","controlId":"svg-reagent-s43-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":385.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S43_鎺у埗閽?,"controlId":"svg-reagent-s43-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":386.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s53-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":387.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_ROI Left","controlId":"svg-reagent-s53-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":388.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_ROI Top","controlId":"svg-reagent-s53-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":389.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_ROI Width","controlId":"svg-reagent-s53-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":390.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_ROI Height","controlId":"svg-reagent-s53-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":391.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s53-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":392.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_娑蹭綋绫诲瀷","controlId":"svg-reagent-s53-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":393.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s53-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":394.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s53-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":395.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s53-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":396.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s53-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":397.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s53-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":398.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_绉绘恫瀛斾綅","controlId":"svg-reagent-s53-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":399.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_绉绘恫娑查噺","controlId":"svg-reagent-s53-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":400.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S53_鎺у埗閽?,"controlId":"svg-reagent-s53-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":401.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s14-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":402.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_ROI Left","controlId":"svg-reagent-s14-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":403.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_ROI Top","controlId":"svg-reagent-s14-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":404.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_ROI Width","controlId":"svg-reagent-s14-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":405.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_ROI Height","controlId":"svg-reagent-s14-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":406.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s14-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":407.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_娑蹭綋绫诲瀷","controlId":"svg-reagent-s14-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":408.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s14-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":409.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s14-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":410.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s14-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":411.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s14-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":412.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s14-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":413.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_绉绘恫瀛斾綅","controlId":"svg-reagent-s14-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":414.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_绉绘恫娑查噺","controlId":"svg-reagent-s14-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":415.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S14_鎺у埗閽?,"controlId":"svg-reagent-s14-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":416.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s24-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":417.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_ROI Left","controlId":"svg-reagent-s24-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":418.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_ROI Top","controlId":"svg-reagent-s24-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":419.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_ROI Width","controlId":"svg-reagent-s24-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":420.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_ROI Height","controlId":"svg-reagent-s24-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":421.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s24-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":422.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_娑蹭綋绫诲瀷","controlId":"svg-reagent-s24-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":423.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s24-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":424.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s24-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":425.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s24-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":426.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s24-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":427.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s24-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":428.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_绉绘恫瀛斾綅","controlId":"svg-reagent-s24-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":429.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_绉绘恫娑查噺","controlId":"svg-reagent-s24-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":430.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S24_鎺у埗閽?,"controlId":"svg-reagent-s24-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":431.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s34-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":432.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_ROI Left","controlId":"svg-reagent-s34-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":433.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_ROI Top","controlId":"svg-reagent-s34-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":434.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_ROI Width","controlId":"svg-reagent-s34-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":435.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_ROI Height","controlId":"svg-reagent-s34-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":436.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s34-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":437.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_娑蹭綋绫诲瀷","controlId":"svg-reagent-s34-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":438.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s34-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":439.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s34-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":440.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s34-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":441.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s34-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":442.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s34-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":443.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_绉绘恫瀛斾綅","controlId":"svg-reagent-s34-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":444.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_绉绘恫娑查噺","controlId":"svg-reagent-s34-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":445.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S34_鎺у埗閽?,"controlId":"svg-reagent-s34-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":446.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s44-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":447.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_ROI Left","controlId":"svg-reagent-s44-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":448.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_ROI Top","controlId":"svg-reagent-s44-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":449.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_ROI Width","controlId":"svg-reagent-s44-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":450.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_ROI Height","controlId":"svg-reagent-s44-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":451.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s44-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":452.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_娑蹭綋绫诲瀷","controlId":"svg-reagent-s44-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":453.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s44-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":454.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s44-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":455.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s44-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":456.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s44-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":457.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s44-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":458.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_绉绘恫瀛斾綅","controlId":"svg-reagent-s44-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":459.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_绉绘恫娑查噺","controlId":"svg-reagent-s44-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":460.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S44_鎺у埗閽?,"controlId":"svg-reagent-s44-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":461.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s54-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":462.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_ROI Left","controlId":"svg-reagent-s54-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":463.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_ROI Top","controlId":"svg-reagent-s54-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":464.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_ROI Width","controlId":"svg-reagent-s54-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":465.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_ROI Height","controlId":"svg-reagent-s54-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":466.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s54-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":467.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_娑蹭綋绫诲瀷","controlId":"svg-reagent-s54-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":468.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s54-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":469.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s54-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":470.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s54-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":471.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s54-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":472.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s54-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":473.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_绉绘恫瀛斾綅","controlId":"svg-reagent-s54-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":474.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_绉绘恫娑查噺","controlId":"svg-reagent-s54-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":475.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S54_鎺у埗閽?,"controlId":"svg-reagent-s54-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":476.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s15-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":477.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_ROI Left","controlId":"svg-reagent-s15-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":478.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_ROI Top","controlId":"svg-reagent-s15-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":479.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_ROI Width","controlId":"svg-reagent-s15-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":480.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_ROI Height","controlId":"svg-reagent-s15-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":481.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s15-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":482.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_娑蹭綋绫诲瀷","controlId":"svg-reagent-s15-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":483.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s15-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":484.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s15-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":485.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s15-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":486.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s15-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":487.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s15-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":488.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_绉绘恫瀛斾綅","controlId":"svg-reagent-s15-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":489.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_绉绘恫娑查噺","controlId":"svg-reagent-s15-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":490.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S15_鎺у埗閽?,"controlId":"svg-reagent-s15-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":491.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s25-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":492.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_ROI Left","controlId":"svg-reagent-s25-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":493.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_ROI Top","controlId":"svg-reagent-s25-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":494.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_ROI Width","controlId":"svg-reagent-s25-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":495.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_ROI Height","controlId":"svg-reagent-s25-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":496.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s25-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":497.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_娑蹭綋绫诲瀷","controlId":"svg-reagent-s25-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":498.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s25-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":499.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s25-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":500.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s25-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":501.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s25-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":502.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s25-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":503.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_绉绘恫瀛斾綅","controlId":"svg-reagent-s25-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":504.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_绉绘恫娑查噺","controlId":"svg-reagent-s25-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":505.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S25_鎺у埗閽?,"controlId":"svg-reagent-s25-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":506.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s35-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":507.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_ROI Left","controlId":"svg-reagent-s35-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":508.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_ROI Top","controlId":"svg-reagent-s35-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":509.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_ROI Width","controlId":"svg-reagent-s35-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":510.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_ROI Height","controlId":"svg-reagent-s35-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":511.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s35-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":512.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_娑蹭綋绫诲瀷","controlId":"svg-reagent-s35-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":513.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s35-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":514.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s35-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":515.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s35-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":516.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s35-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":517.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s35-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":518.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_绉绘恫瀛斾綅","controlId":"svg-reagent-s35-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":519.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_绉绘恫娑查噺","controlId":"svg-reagent-s35-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":520.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S35_鎺у埗閽?,"controlId":"svg-reagent-s35-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":521.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s45-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":522.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_ROI Left","controlId":"svg-reagent-s45-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":523.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_ROI Top","controlId":"svg-reagent-s45-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":524.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_ROI Width","controlId":"svg-reagent-s45-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":525.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_ROI Height","controlId":"svg-reagent-s45-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":526.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s45-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":527.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_娑蹭綋绫诲瀷","controlId":"svg-reagent-s45-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":528.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s45-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":529.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s45-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":530.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s45-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":531.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s45-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":532.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s45-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":533.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_绉绘恫瀛斾綅","controlId":"svg-reagent-s45-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":534.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_绉绘恫娑查噺","controlId":"svg-reagent-s45-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":535.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S45_鎺у埗閽?,"controlId":"svg-reagent-s45-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":536.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s55-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":537.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_ROI Left","controlId":"svg-reagent-s55-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":538.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_ROI Top","controlId":"svg-reagent-s55-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":539.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_ROI Width","controlId":"svg-reagent-s55-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":540.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_ROI Height","controlId":"svg-reagent-s55-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":541.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s55-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":542.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_娑蹭綋绫诲瀷","controlId":"svg-reagent-s55-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":543.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s55-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":544.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s55-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":545.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s55-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":546.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s55-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":547.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s55-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":548.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_绉绘恫瀛斾綅","controlId":"svg-reagent-s55-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":549.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_绉绘恫娑查噺","controlId":"svg-reagent-s55-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":550.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S55_鎺у埗閽?,"controlId":"svg-reagent-s55-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":551.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s16-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":552.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_ROI Left","controlId":"svg-reagent-s16-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":553.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_ROI Top","controlId":"svg-reagent-s16-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":554.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_ROI Width","controlId":"svg-reagent-s16-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":555.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_ROI Height","controlId":"svg-reagent-s16-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":556.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s16-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":557.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_娑蹭綋绫诲瀷","controlId":"svg-reagent-s16-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":558.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s16-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":559.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s16-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":560.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s16-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":561.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s16-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":562.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s16-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":563.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_绉绘恫瀛斾綅","controlId":"svg-reagent-s16-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":564.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_绉绘恫娑查噺","controlId":"svg-reagent-s16-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":565.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S16_鎺у埗閽?,"controlId":"svg-reagent-s16-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":566.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s26-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":567.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_ROI Left","controlId":"svg-reagent-s26-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":568.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_ROI Top","controlId":"svg-reagent-s26-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":569.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_ROI Width","controlId":"svg-reagent-s26-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":570.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_ROI Height","controlId":"svg-reagent-s26-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":571.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s26-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":572.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_娑蹭綋绫诲瀷","controlId":"svg-reagent-s26-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":573.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s26-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":574.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s26-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":575.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s26-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":576.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s26-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":577.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s26-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":578.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_绉绘恫瀛斾綅","controlId":"svg-reagent-s26-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":579.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_绉绘恫娑查噺","controlId":"svg-reagent-s26-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":580.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S26_鎺у埗閽?,"controlId":"svg-reagent-s26-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":581.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s36-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":582.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_ROI Left","controlId":"svg-reagent-s36-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":583.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_ROI Top","controlId":"svg-reagent-s36-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":584.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_ROI Width","controlId":"svg-reagent-s36-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":585.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_ROI Height","controlId":"svg-reagent-s36-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":586.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s36-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":587.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_娑蹭綋绫诲瀷","controlId":"svg-reagent-s36-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":588.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s36-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":589.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s36-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":590.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s36-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":591.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s36-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":592.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s36-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":593.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_绉绘恫瀛斾綅","controlId":"svg-reagent-s36-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":594.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_绉绘恫娑查噺","controlId":"svg-reagent-s36-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":595.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S36_鎺у埗閽?,"controlId":"svg-reagent-s36-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":596.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s46-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":597.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_ROI Left","controlId":"svg-reagent-s46-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":598.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_ROI Top","controlId":"svg-reagent-s46-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":599.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_ROI Width","controlId":"svg-reagent-s46-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":600.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_ROI Height","controlId":"svg-reagent-s46-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":601.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s46-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":602.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_娑蹭綋绫诲瀷","controlId":"svg-reagent-s46-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":603.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s46-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":604.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s46-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":605.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s46-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":606.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s46-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":607.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s46-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":608.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_绉绘恫瀛斾綅","controlId":"svg-reagent-s46-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":609.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_绉绘恫娑查噺","controlId":"svg-reagent-s46-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":610.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S46_鎺у埗閽?,"controlId":"svg-reagent-s46-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":611.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s56-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":612.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_ROI Left","controlId":"svg-reagent-s56-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":613.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_ROI Top","controlId":"svg-reagent-s56-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":614.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_ROI Width","controlId":"svg-reagent-s56-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":615.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_ROI Height","controlId":"svg-reagent-s56-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":616.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s56-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":617.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_娑蹭綋绫诲瀷","controlId":"svg-reagent-s56-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":618.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s56-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":619.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s56-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":620.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s56-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":621.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s56-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":622.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s56-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":623.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_绉绘恫瀛斾綅","controlId":"svg-reagent-s56-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":624.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_绉绘恫娑查噺","controlId":"svg-reagent-s56-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":625.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S56_鎺у埗閽?,"controlId":"svg-reagent-s56-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":626.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s17-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":627.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_ROI Left","controlId":"svg-reagent-s17-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":628.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_ROI Top","controlId":"svg-reagent-s17-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":629.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_ROI Width","controlId":"svg-reagent-s17-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":630.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_ROI Height","controlId":"svg-reagent-s17-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":631.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s17-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":632.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_娑蹭綋绫诲瀷","controlId":"svg-reagent-s17-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":633.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s17-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":634.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s17-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":635.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s17-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":636.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s17-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":637.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s17-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":638.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_绉绘恫瀛斾綅","controlId":"svg-reagent-s17-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":639.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_绉绘恫娑查噺","controlId":"svg-reagent-s17-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":640.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S17_鎺у埗閽?,"controlId":"svg-reagent-s17-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":641.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s27-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":642.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_ROI Left","controlId":"svg-reagent-s27-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":643.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_ROI Top","controlId":"svg-reagent-s27-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":644.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_ROI Width","controlId":"svg-reagent-s27-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":645.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_ROI Height","controlId":"svg-reagent-s27-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":646.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s27-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":647.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_娑蹭綋绫诲瀷","controlId":"svg-reagent-s27-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":648.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s27-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":649.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s27-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":650.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s27-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":651.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s27-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":652.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s27-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":653.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_绉绘恫瀛斾綅","controlId":"svg-reagent-s27-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":654.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_绉绘恫娑查噺","controlId":"svg-reagent-s27-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":655.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S27_鎺у埗閽?,"controlId":"svg-reagent-s27-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":656.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s37-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":657.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_ROI Left","controlId":"svg-reagent-s37-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":658.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_ROI Top","controlId":"svg-reagent-s37-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":659.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_ROI Width","controlId":"svg-reagent-s37-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":660.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_ROI Height","controlId":"svg-reagent-s37-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":661.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s37-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":662.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_娑蹭綋绫诲瀷","controlId":"svg-reagent-s37-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":663.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s37-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":664.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s37-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":665.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s37-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":666.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s37-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":667.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s37-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":668.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_绉绘恫瀛斾綅","controlId":"svg-reagent-s37-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":669.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_绉绘恫娑查噺","controlId":"svg-reagent-s37-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":670.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S37_鎺у埗閽?,"controlId":"svg-reagent-s37-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":671.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s47-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":672.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_ROI Left","controlId":"svg-reagent-s47-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":673.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_ROI Top","controlId":"svg-reagent-s47-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":674.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_ROI Width","controlId":"svg-reagent-s47-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":675.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_ROI Height","controlId":"svg-reagent-s47-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":676.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s47-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":677.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_娑蹭綋绫诲瀷","controlId":"svg-reagent-s47-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":678.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s47-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":679.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s47-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":680.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s47-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":681.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s47-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":682.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s47-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":683.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_绉绘恫瀛斾綅","controlId":"svg-reagent-s47-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":684.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_绉绘恫娑查噺","controlId":"svg-reagent-s47-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":685.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S47_鎺у埗閽?,"controlId":"svg-reagent-s47-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":686.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s57-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":687.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_ROI Left","controlId":"svg-reagent-s57-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":688.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_ROI Top","controlId":"svg-reagent-s57-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":689.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_ROI Width","controlId":"svg-reagent-s57-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":690.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_ROI Height","controlId":"svg-reagent-s57-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":691.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s57-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":692.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_娑蹭綋绫诲瀷","controlId":"svg-reagent-s57-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":693.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s57-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":694.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s57-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":695.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s57-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":696.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s57-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":697.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s57-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":698.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_绉绘恫瀛斾綅","controlId":"svg-reagent-s57-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":699.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_绉绘恫娑查噺","controlId":"svg-reagent-s57-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":700.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S57_鎺у埗閽?,"controlId":"svg-reagent-s57-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":701.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s18-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":702.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_ROI Left","controlId":"svg-reagent-s18-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":703.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_ROI Top","controlId":"svg-reagent-s18-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":704.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_ROI Width","controlId":"svg-reagent-s18-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":705.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_ROI Height","controlId":"svg-reagent-s18-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":706.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s18-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":707.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_娑蹭綋绫诲瀷","controlId":"svg-reagent-s18-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":708.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s18-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":709.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s18-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":710.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s18-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":711.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s18-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":712.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s18-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":713.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_绉绘恫瀛斾綅","controlId":"svg-reagent-s18-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":714.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_绉绘恫娑查噺","controlId":"svg-reagent-s18-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":715.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S18_鎺у埗閽?,"controlId":"svg-reagent-s18-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":716.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s28-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":717.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_ROI Left","controlId":"svg-reagent-s28-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":718.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_ROI Top","controlId":"svg-reagent-s28-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":719.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_ROI Width","controlId":"svg-reagent-s28-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":720.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_ROI Height","controlId":"svg-reagent-s28-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":721.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s28-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":722.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_娑蹭綋绫诲瀷","controlId":"svg-reagent-s28-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":723.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s28-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":724.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s28-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":725.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s28-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":726.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s28-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":727.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s28-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":728.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_绉绘恫瀛斾綅","controlId":"svg-reagent-s28-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":729.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_绉绘恫娑查噺","controlId":"svg-reagent-s28-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":730.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S28_鎺у埗閽?,"controlId":"svg-reagent-s28-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":731.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s38-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":732.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_ROI Left","controlId":"svg-reagent-s38-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":733.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_ROI Top","controlId":"svg-reagent-s38-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":734.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_ROI Width","controlId":"svg-reagent-s38-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":735.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_ROI Height","controlId":"svg-reagent-s38-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":736.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s38-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":737.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_娑蹭綋绫诲瀷","controlId":"svg-reagent-s38-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":738.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s38-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":739.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s38-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":740.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s38-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":741.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s38-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":742.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s38-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":743.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_绉绘恫瀛斾綅","controlId":"svg-reagent-s38-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":744.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_绉绘恫娑查噺","controlId":"svg-reagent-s38-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":745.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S38_鎺у埗閽?,"controlId":"svg-reagent-s38-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":746.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s48-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":747.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_ROI Left","controlId":"svg-reagent-s48-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":748.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_ROI Top","controlId":"svg-reagent-s48-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":749.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_ROI Width","controlId":"svg-reagent-s48-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":750.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_ROI Height","controlId":"svg-reagent-s48-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":751.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s48-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":752.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_娑蹭綋绫诲瀷","controlId":"svg-reagent-s48-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":753.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s48-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":754.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s48-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":755.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s48-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":756.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s48-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":757.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s48-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":758.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_绉绘恫瀛斾綅","controlId":"svg-reagent-s48-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":759.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_绉绘恫娑查噺","controlId":"svg-reagent-s48-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":760.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S48_鎺у埗閽?,"controlId":"svg-reagent-s48-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":761.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_鏉＄爜ROI闈㈡澘","controlId":"svg-reagent-s58-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":762.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_ROI Left","controlId":"svg-reagent-s58-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":763.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_ROI Top","controlId":"svg-reagent-s58-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":764.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_ROI Width","controlId":"svg-reagent-s58-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":765.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_ROI Height","controlId":"svg-reagent-s58-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":766.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-reagent-s58-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":767.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_娑蹭綋绫诲瀷","controlId":"svg-reagent-s58-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":768.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-reagent-s58-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":769.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_Z-Start鎺㈡恫楂樺害","controlId":"svg-reagent-s58-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":770.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-reagent-s58-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":771.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-reagent-s58-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":772.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_閫氶亾绉绘恫闈㈡澘","controlId":"svg-reagent-s58-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":773.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_绉绘恫瀛斾綅","controlId":"svg-reagent-s58-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":774.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_绉绘恫娑查噺","controlId":"svg-reagent-s58-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":775.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏_S58_鎺у埗閽?,"controlId":"svg-reagent-s58-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":776.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C1_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-mix-p11-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":777.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C1_娑蹭綋绫诲瀷","controlId":"svg-mix-p11-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":778.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C1_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-mix-p11-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":779.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C1_Z-Start鎺㈡恫楂樺害","controlId":"svg-mix-p11-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":780.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C1_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-mix-p11-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":781.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C1_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-mix-p11-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":782.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C1_閫氶亾绉绘恫闈㈡澘","controlId":"svg-mix-p11-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":783.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C1_绉绘恫瀛斾綅","controlId":"svg-mix-p11-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":784.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C1_绉绘恫娑查噺","controlId":"svg-mix-p11-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":785.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C1_鎺у埗閽?,"controlId":"svg-mix-p11-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":786.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C2_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-mix-p12-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":787.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C2_娑蹭綋绫诲瀷","controlId":"svg-mix-p12-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":788.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C2_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-mix-p12-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":789.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C2_Z-Start鎺㈡恫楂樺害","controlId":"svg-mix-p12-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":790.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C2_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-mix-p12-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":791.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C2_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-mix-p12-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":792.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C2_閫氶亾绉绘恫闈㈡澘","controlId":"svg-mix-p12-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":793.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C2_绉绘恫瀛斾綅","controlId":"svg-mix-p12-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":794.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C2_绉绘恫娑查噺","controlId":"svg-mix-p12-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":795.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R1_C2_鎺у埗閽?,"controlId":"svg-mix-p12-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":796.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C1_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-mix-p21-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":797.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C1_娑蹭綋绫诲瀷","controlId":"svg-mix-p21-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":798.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C1_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-mix-p21-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":799.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C1_Z-Start鎺㈡恫楂樺害","controlId":"svg-mix-p21-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":800.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C1_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-mix-p21-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":801.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C1_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-mix-p21-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":802.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C1_閫氶亾绉绘恫闈㈡澘","controlId":"svg-mix-p21-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":803.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C1_绉绘恫瀛斾綅","controlId":"svg-mix-p21-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":804.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C1_绉绘恫娑查噺","controlId":"svg-mix-p21-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":805.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C1_鎺у埗閽?,"controlId":"svg-mix-p21-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":806.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C2_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-mix-p22-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":807.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C2_娑蹭綋绫诲瀷","controlId":"svg-mix-p22-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":808.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C2_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-mix-p22-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":809.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C2_Z-Start鎺㈡恫楂樺害","controlId":"svg-mix-p22-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":810.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C2_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-mix-p22-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":811.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C2_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-mix-p22-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":812.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C2_閫氶亾绉绘恫闈㈡澘","controlId":"svg-mix-p22-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":813.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C2_绉绘恫瀛斾綅","controlId":"svg-mix-p22-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":814.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C2_绉绘恫娑查噺","controlId":"svg-mix-p22-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":815.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R2_C2_鎺у埗閽?,"controlId":"svg-mix-p22-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":816.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C1_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-mix-p31-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":817.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C1_娑蹭綋绫诲瀷","controlId":"svg-mix-p31-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":818.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C1_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-mix-p31-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":819.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C1_Z-Start鎺㈡恫楂樺害","controlId":"svg-mix-p31-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":820.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C1_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-mix-p31-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":821.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C1_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-mix-p31-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":822.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C1_閫氶亾绉绘恫闈㈡澘","controlId":"svg-mix-p31-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":823.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C1_绉绘恫瀛斾綅","controlId":"svg-mix-p31-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":824.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C1_绉绘恫娑查噺","controlId":"svg-mix-p31-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":825.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C1_鎺у埗閽?,"controlId":"svg-mix-p31-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":826.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C2_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-mix-p32-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":827.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C2_娑蹭綋绫诲瀷","controlId":"svg-mix-p32-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":828.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C2_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-mix-p32-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":829.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C2_Z-Start鎺㈡恫楂樺害","controlId":"svg-mix-p32-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":830.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C2_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-mix-p32-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":831.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C2_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-mix-p32-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":832.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C2_閫氶亾绉绘恫闈㈡澘","controlId":"svg-mix-p32-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":833.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C2_绉绘恫瀛斾綅","controlId":"svg-mix-p32-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":834.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C2_绉绘恫娑查噺","controlId":"svg-mix-p32-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":835.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R3_C2_鎺у埗閽?,"controlId":"svg-mix-p32-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":836.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C1_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-mix-p41-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":837.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C1_娑蹭綋绫诲瀷","controlId":"svg-mix-p41-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":838.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C1_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-mix-p41-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":839.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C1_Z-Start鎺㈡恫楂樺害","controlId":"svg-mix-p41-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":840.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C1_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-mix-p41-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":841.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C1_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-mix-p41-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":842.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C1_閫氶亾绉绘恫闈㈡澘","controlId":"svg-mix-p41-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":843.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C1_绉绘恫瀛斾綅","controlId":"svg-mix-p41-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":844.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C1_绉绘恫娑查噺","controlId":"svg-mix-p41-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":845.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C1_鎺у埗閽?,"controlId":"svg-mix-p41-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":846.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C2_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-mix-p42-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":847.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C2_娑蹭綋绫诲瀷","controlId":"svg-mix-p42-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":848.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C2_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-mix-p42-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":849.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C2_Z-Start鎺㈡恫楂樺害","controlId":"svg-mix-p42-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":850.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C2_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-mix-p42-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":851.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C2_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-mix-p42-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":852.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C2_閫氶亾绉绘恫闈㈡澘","controlId":"svg-mix-p42-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":853.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C2_绉绘恫瀛斾綅","controlId":"svg-mix-p42-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":854.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C2_绉绘恫娑查噺","controlId":"svg-mix-p42-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":855.0,"category":"鐣岄潰鎺т欢","name":"閰嶆恫_R4_C2_鎺у埗閽?,"controlId":"svg-mix-p42-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":856.0,"category":"鐣岄潰鎺т欢","name":"A娑瞋瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-liquid-a-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":857.0,"category":"鐣岄潰鎺т欢","name":"A娑瞋娑蹭綋绫诲瀷","controlId":"svg-liquid-a-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":858.0,"category":"鐣岄潰鎺т欢","name":"A娑瞋Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-liquid-a-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":859.0,"category":"鐣岄潰鎺т欢","name":"A娑瞋Z-Start鎺㈡恫楂樺害","controlId":"svg-liquid-a-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":860.0,"category":"鐣岄潰鎺т欢","name":"A娑瞋Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-liquid-a-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":861.0,"category":"鐣岄潰鎺т欢","name":"A娑瞋Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-liquid-a-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":862.0,"category":"鐣岄潰鎺т欢","name":"A娑瞋閫氶亾绉绘恫闈㈡澘","controlId":"svg-liquid-a-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":863.0,"category":"鐣岄潰鎺т欢","name":"A娑瞋绉绘恫瀛斾綅","controlId":"svg-liquid-a-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":864.0,"category":"鐣岄潰鎺т欢","name":"A娑瞋绉绘恫娑查噺","controlId":"svg-liquid-a-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":865.0,"category":"鐣岄潰鎺т欢","name":"A娑瞋鎺у埗閽?,"controlId":"svg-liquid-a-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":866.0,"category":"鐣岄潰鎺т欢","name":"B娑瞋瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-liquid-b-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":867.0,"category":"鐣岄潰鎺т欢","name":"B娑瞋娑蹭綋绫诲瀷","controlId":"svg-liquid-b-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":868.0,"category":"鐣岄潰鎺т欢","name":"B娑瞋Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-liquid-b-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":869.0,"category":"鐣岄潰鎺т欢","name":"B娑瞋Z-Start鎺㈡恫楂樺害","controlId":"svg-liquid-b-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":870.0,"category":"鐣岄潰鎺т欢","name":"B娑瞋Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-liquid-b-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":871.0,"category":"鐣岄潰鎺т欢","name":"B娑瞋Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-liquid-b-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":872.0,"category":"鐣岄潰鎺т欢","name":"B娑瞋閫氶亾绉绘恫闈㈡澘","controlId":"svg-liquid-b-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":873.0,"category":"鐣岄潰鎺т欢","name":"B娑瞋绉绘恫瀛斾綅","controlId":"svg-liquid-b-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":874.0,"category":"鐣岄潰鎺т欢","name":"B娑瞋绉绘恫娑查噺","controlId":"svg-liquid-b-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":875.0,"category":"鐣岄潰鎺т欢","name":"B娑瞋鎺у埗閽?,"controlId":"svg-liquid-b-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":876.0,"category":"鐣岄潰鎺т欢","name":"R11_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r11-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":877.0,"category":"鐣岄潰鎺т欢","name":"R11_ROI Left","controlId":"svg-slide-r11-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":878.0,"category":"鐣岄潰鎺т欢","name":"R11_ROI Top","controlId":"svg-slide-r11-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":879.0,"category":"鐣岄潰鎺т欢","name":"R11_ROI Width","controlId":"svg-slide-r11-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":880.0,"category":"鐣岄潰鎺т欢","name":"R11_ROI Height","controlId":"svg-slide-r11-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":881.0,"category":"鐣岄潰鎺т欢","name":"R11_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r11-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":882.0,"category":"鐣岄潰鎺т欢","name":"R11_娑蹭綋绫诲瀷","controlId":"svg-slide-r11-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":883.0,"category":"鐣岄潰鎺т欢","name":"R11_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r11-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":884.0,"category":"鐣岄潰鎺т欢","name":"R11_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r11-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":885.0,"category":"鐣岄潰鎺т欢","name":"R11_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r11-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":886.0,"category":"鐣岄潰鎺т欢","name":"R11_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r11-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":887.0,"category":"鐣岄潰鎺т欢","name":"R11_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r11-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":888.0,"category":"鐣岄潰鎺т欢","name":"R11_绉绘恫瀛斾綅","controlId":"svg-slide-r11-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":889.0,"category":"鐣岄潰鎺т欢","name":"R11_绉绘恫娑查噺","controlId":"svg-slide-r11-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":890.0,"category":"鐣岄潰鎺т欢","name":"R11_鎺у埗閽?,"controlId":"svg-slide-r11-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":891.0,"category":"鐣岄潰鎺т欢","name":"R12_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r12-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":892.0,"category":"鐣岄潰鎺т欢","name":"R12_ROI Left","controlId":"svg-slide-r12-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":893.0,"category":"鐣岄潰鎺т欢","name":"R12_ROI Top","controlId":"svg-slide-r12-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":894.0,"category":"鐣岄潰鎺т欢","name":"R12_ROI Width","controlId":"svg-slide-r12-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":895.0,"category":"鐣岄潰鎺т欢","name":"R12_ROI Height","controlId":"svg-slide-r12-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":896.0,"category":"鐣岄潰鎺т欢","name":"R12_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r12-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":897.0,"category":"鐣岄潰鎺т欢","name":"R12_娑蹭綋绫诲瀷","controlId":"svg-slide-r12-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":898.0,"category":"鐣岄潰鎺т欢","name":"R12_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r12-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":899.0,"category":"鐣岄潰鎺т欢","name":"R12_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r12-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":900.0,"category":"鐣岄潰鎺т欢","name":"R12_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r12-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":901.0,"category":"鐣岄潰鎺т欢","name":"R12_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r12-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":902.0,"category":"鐣岄潰鎺т欢","name":"R12_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r12-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":903.0,"category":"鐣岄潰鎺т欢","name":"R12_绉绘恫瀛斾綅","controlId":"svg-slide-r12-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":904.0,"category":"鐣岄潰鎺т欢","name":"R12_绉绘恫娑查噺","controlId":"svg-slide-r12-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":905.0,"category":"鐣岄潰鎺т欢","name":"R12_鎺у埗閽?,"controlId":"svg-slide-r12-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":906.0,"category":"鐣岄潰鎺т欢","name":"R13_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r13-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":907.0,"category":"鐣岄潰鎺т欢","name":"R13_ROI Left","controlId":"svg-slide-r13-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":908.0,"category":"鐣岄潰鎺т欢","name":"R13_ROI Top","controlId":"svg-slide-r13-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":909.0,"category":"鐣岄潰鎺т欢","name":"R13_ROI Width","controlId":"svg-slide-r13-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":910.0,"category":"鐣岄潰鎺т欢","name":"R13_ROI Height","controlId":"svg-slide-r13-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":911.0,"category":"鐣岄潰鎺т欢","name":"R13_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r13-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":912.0,"category":"鐣岄潰鎺т欢","name":"R13_娑蹭綋绫诲瀷","controlId":"svg-slide-r13-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":913.0,"category":"鐣岄潰鎺т欢","name":"R13_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r13-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":914.0,"category":"鐣岄潰鎺т欢","name":"R13_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r13-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":915.0,"category":"鐣岄潰鎺т欢","name":"R13_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r13-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":916.0,"category":"鐣岄潰鎺т欢","name":"R13_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r13-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":917.0,"category":"鐣岄潰鎺т欢","name":"R13_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r13-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":918.0,"category":"鐣岄潰鎺т欢","name":"R13_绉绘恫瀛斾綅","controlId":"svg-slide-r13-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":919.0,"category":"鐣岄潰鎺т欢","name":"R13_绉绘恫娑查噺","controlId":"svg-slide-r13-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":920.0,"category":"鐣岄潰鎺т欢","name":"R13_鎺у埗閽?,"controlId":"svg-slide-r13-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":921.0,"category":"鐣岄潰鎺т欢","name":"R14_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r14-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":922.0,"category":"鐣岄潰鎺т欢","name":"R14_ROI Left","controlId":"svg-slide-r14-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":923.0,"category":"鐣岄潰鎺т欢","name":"R14_ROI Top","controlId":"svg-slide-r14-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":924.0,"category":"鐣岄潰鎺т欢","name":"R14_ROI Width","controlId":"svg-slide-r14-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":925.0,"category":"鐣岄潰鎺т欢","name":"R14_ROI Height","controlId":"svg-slide-r14-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":926.0,"category":"鐣岄潰鎺т欢","name":"R14_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r14-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":927.0,"category":"鐣岄潰鎺т欢","name":"R14_娑蹭綋绫诲瀷","controlId":"svg-slide-r14-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":928.0,"category":"鐣岄潰鎺т欢","name":"R14_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r14-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":929.0,"category":"鐣岄潰鎺т欢","name":"R14_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r14-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":930.0,"category":"鐣岄潰鎺т欢","name":"R14_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r14-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":931.0,"category":"鐣岄潰鎺т欢","name":"R14_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r14-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":932.0,"category":"鐣岄潰鎺т欢","name":"R14_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r14-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":933.0,"category":"鐣岄潰鎺т欢","name":"R14_绉绘恫瀛斾綅","controlId":"svg-slide-r14-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":934.0,"category":"鐣岄潰鎺т欢","name":"R14_绉绘恫娑查噺","controlId":"svg-slide-r14-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":935.0,"category":"鐣岄潰鎺т欢","name":"R14_鎺у埗閽?,"controlId":"svg-slide-r14-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":936.0,"category":"鐣岄潰鎺т欢","name":"R21_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r21-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":937.0,"category":"鐣岄潰鎺т欢","name":"R21_ROI Left","controlId":"svg-slide-r21-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":938.0,"category":"鐣岄潰鎺т欢","name":"R21_ROI Top","controlId":"svg-slide-r21-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":939.0,"category":"鐣岄潰鎺т欢","name":"R21_ROI Width","controlId":"svg-slide-r21-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":940.0,"category":"鐣岄潰鎺т欢","name":"R21_ROI Height","controlId":"svg-slide-r21-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":941.0,"category":"鐣岄潰鎺т欢","name":"R21_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r21-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":942.0,"category":"鐣岄潰鎺т欢","name":"R21_娑蹭綋绫诲瀷","controlId":"svg-slide-r21-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":943.0,"category":"鐣岄潰鎺т欢","name":"R21_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r21-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":944.0,"category":"鐣岄潰鎺т欢","name":"R21_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r21-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":945.0,"category":"鐣岄潰鎺т欢","name":"R21_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r21-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":946.0,"category":"鐣岄潰鎺т欢","name":"R21_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r21-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":947.0,"category":"鐣岄潰鎺т欢","name":"R21_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r21-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":948.0,"category":"鐣岄潰鎺т欢","name":"R21_绉绘恫瀛斾綅","controlId":"svg-slide-r21-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":949.0,"category":"鐣岄潰鎺т欢","name":"R21_绉绘恫娑查噺","controlId":"svg-slide-r21-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":950.0,"category":"鐣岄潰鎺т欢","name":"R21_鎺у埗閽?,"controlId":"svg-slide-r21-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":951.0,"category":"鐣岄潰鎺т欢","name":"R22_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r22-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":952.0,"category":"鐣岄潰鎺т欢","name":"R22_ROI Left","controlId":"svg-slide-r22-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":953.0,"category":"鐣岄潰鎺т欢","name":"R22_ROI Top","controlId":"svg-slide-r22-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":954.0,"category":"鐣岄潰鎺т欢","name":"R22_ROI Width","controlId":"svg-slide-r22-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":955.0,"category":"鐣岄潰鎺т欢","name":"R22_ROI Height","controlId":"svg-slide-r22-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":956.0,"category":"鐣岄潰鎺т欢","name":"R22_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r22-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":957.0,"category":"鐣岄潰鎺т欢","name":"R22_娑蹭綋绫诲瀷","controlId":"svg-slide-r22-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":958.0,"category":"鐣岄潰鎺т欢","name":"R22_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r22-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":959.0,"category":"鐣岄潰鎺т欢","name":"R22_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r22-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":960.0,"category":"鐣岄潰鎺т欢","name":"R22_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r22-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":961.0,"category":"鐣岄潰鎺т欢","name":"R22_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r22-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":962.0,"category":"鐣岄潰鎺т欢","name":"R22_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r22-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":963.0,"category":"鐣岄潰鎺т欢","name":"R22_绉绘恫瀛斾綅","controlId":"svg-slide-r22-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":964.0,"category":"鐣岄潰鎺т欢","name":"R22_绉绘恫娑查噺","controlId":"svg-slide-r22-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":965.0,"category":"鐣岄潰鎺т欢","name":"R22_鎺у埗閽?,"controlId":"svg-slide-r22-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":966.0,"category":"鐣岄潰鎺т欢","name":"R23_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r23-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":967.0,"category":"鐣岄潰鎺т欢","name":"R23_ROI Left","controlId":"svg-slide-r23-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":968.0,"category":"鐣岄潰鎺т欢","name":"R23_ROI Top","controlId":"svg-slide-r23-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":969.0,"category":"鐣岄潰鎺т欢","name":"R23_ROI Width","controlId":"svg-slide-r23-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":970.0,"category":"鐣岄潰鎺т欢","name":"R23_ROI Height","controlId":"svg-slide-r23-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":971.0,"category":"鐣岄潰鎺т欢","name":"R23_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r23-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":972.0,"category":"鐣岄潰鎺т欢","name":"R23_娑蹭綋绫诲瀷","controlId":"svg-slide-r23-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":973.0,"category":"鐣岄潰鎺т欢","name":"R23_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r23-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":974.0,"category":"鐣岄潰鎺т欢","name":"R23_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r23-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":975.0,"category":"鐣岄潰鎺т欢","name":"R23_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r23-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":976.0,"category":"鐣岄潰鎺т欢","name":"R23_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r23-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":977.0,"category":"鐣岄潰鎺т欢","name":"R23_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r23-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":978.0,"category":"鐣岄潰鎺т欢","name":"R23_绉绘恫瀛斾綅","controlId":"svg-slide-r23-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":979.0,"category":"鐣岄潰鎺т欢","name":"R23_绉绘恫娑查噺","controlId":"svg-slide-r23-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":980.0,"category":"鐣岄潰鎺т欢","name":"R23_鎺у埗閽?,"controlId":"svg-slide-r23-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":981.0,"category":"鐣岄潰鎺т欢","name":"R24_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r24-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":982.0,"category":"鐣岄潰鎺т欢","name":"R24_ROI Left","controlId":"svg-slide-r24-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":983.0,"category":"鐣岄潰鎺т欢","name":"R24_ROI Top","controlId":"svg-slide-r24-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":984.0,"category":"鐣岄潰鎺т欢","name":"R24_ROI Width","controlId":"svg-slide-r24-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":985.0,"category":"鐣岄潰鎺т欢","name":"R24_ROI Height","controlId":"svg-slide-r24-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":986.0,"category":"鐣岄潰鎺т欢","name":"R24_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r24-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":987.0,"category":"鐣岄潰鎺т欢","name":"R24_娑蹭綋绫诲瀷","controlId":"svg-slide-r24-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":988.0,"category":"鐣岄潰鎺т欢","name":"R24_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r24-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":989.0,"category":"鐣岄潰鎺т欢","name":"R24_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r24-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":990.0,"category":"鐣岄潰鎺т欢","name":"R24_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r24-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":991.0,"category":"鐣岄潰鎺т欢","name":"R24_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r24-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":992.0,"category":"鐣岄潰鎺т欢","name":"R24_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r24-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":993.0,"category":"鐣岄潰鎺т欢","name":"R24_绉绘恫瀛斾綅","controlId":"svg-slide-r24-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":994.0,"category":"鐣岄潰鎺т欢","name":"R24_绉绘恫娑查噺","controlId":"svg-slide-r24-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":995.0,"category":"鐣岄潰鎺т欢","name":"R24_鎺у埗閽?,"controlId":"svg-slide-r24-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":996.0,"category":"鐣岄潰鎺т欢","name":"R31_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r31-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":997.0,"category":"鐣岄潰鎺т欢","name":"R31_ROI Left","controlId":"svg-slide-r31-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":998.0,"category":"鐣岄潰鎺т欢","name":"R31_ROI Top","controlId":"svg-slide-r31-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":999.0,"category":"鐣岄潰鎺т欢","name":"R31_ROI Width","controlId":"svg-slide-r31-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1000.0,"category":"鐣岄潰鎺т欢","name":"R31_ROI Height","controlId":"svg-slide-r31-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1001.0,"category":"鐣岄潰鎺т欢","name":"R31_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r31-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":1002.0,"category":"鐣岄潰鎺т欢","name":"R31_娑蹭綋绫诲瀷","controlId":"svg-slide-r31-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1003.0,"category":"鐣岄潰鎺т欢","name":"R31_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r31-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1004.0,"category":"鐣岄潰鎺т欢","name":"R31_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r31-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1005.0,"category":"鐣岄潰鎺т欢","name":"R31_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r31-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1006.0,"category":"鐣岄潰鎺т欢","name":"R31_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r31-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1007.0,"category":"鐣岄潰鎺т欢","name":"R31_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r31-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":1008.0,"category":"鐣岄潰鎺т欢","name":"R31_绉绘恫瀛斾綅","controlId":"svg-slide-r31-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1009.0,"category":"鐣岄潰鎺т欢","name":"R31_绉绘恫娑查噺","controlId":"svg-slide-r31-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1010.0,"category":"鐣岄潰鎺т欢","name":"R31_鎺у埗閽?,"controlId":"svg-slide-r31-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1011.0,"category":"鐣岄潰鎺т欢","name":"R32_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r32-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":1012.0,"category":"鐣岄潰鎺т欢","name":"R32_ROI Left","controlId":"svg-slide-r32-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1013.0,"category":"鐣岄潰鎺т欢","name":"R32_ROI Top","controlId":"svg-slide-r32-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1014.0,"category":"鐣岄潰鎺т欢","name":"R32_ROI Width","controlId":"svg-slide-r32-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1015.0,"category":"鐣岄潰鎺т欢","name":"R32_ROI Height","controlId":"svg-slide-r32-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1016.0,"category":"鐣岄潰鎺т欢","name":"R32_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r32-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":1017.0,"category":"鐣岄潰鎺т欢","name":"R32_娑蹭綋绫诲瀷","controlId":"svg-slide-r32-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1018.0,"category":"鐣岄潰鎺т欢","name":"R32_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r32-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1019.0,"category":"鐣岄潰鎺т欢","name":"R32_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r32-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1020.0,"category":"鐣岄潰鎺т欢","name":"R32_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r32-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1021.0,"category":"鐣岄潰鎺т欢","name":"R32_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r32-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1022.0,"category":"鐣岄潰鎺т欢","name":"R32_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r32-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":1023.0,"category":"鐣岄潰鎺т欢","name":"R32_绉绘恫瀛斾綅","controlId":"svg-slide-r32-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1024.0,"category":"鐣岄潰鎺т欢","name":"R32_绉绘恫娑查噺","controlId":"svg-slide-r32-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1025.0,"category":"鐣岄潰鎺т欢","name":"R32_鎺у埗閽?,"controlId":"svg-slide-r32-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1026.0,"category":"鐣岄潰鎺т欢","name":"R33_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r33-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":1027.0,"category":"鐣岄潰鎺т欢","name":"R33_ROI Left","controlId":"svg-slide-r33-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1028.0,"category":"鐣岄潰鎺т欢","name":"R33_ROI Top","controlId":"svg-slide-r33-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1029.0,"category":"鐣岄潰鎺т欢","name":"R33_ROI Width","controlId":"svg-slide-r33-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1030.0,"category":"鐣岄潰鎺т欢","name":"R33_ROI Height","controlId":"svg-slide-r33-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1031.0,"category":"鐣岄潰鎺т欢","name":"R33_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r33-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":1032.0,"category":"鐣岄潰鎺т欢","name":"R33_娑蹭綋绫诲瀷","controlId":"svg-slide-r33-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1033.0,"category":"鐣岄潰鎺т欢","name":"R33_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r33-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1034.0,"category":"鐣岄潰鎺т欢","name":"R33_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r33-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1035.0,"category":"鐣岄潰鎺т欢","name":"R33_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r33-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1036.0,"category":"鐣岄潰鎺т欢","name":"R33_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r33-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1037.0,"category":"鐣岄潰鎺т欢","name":"R33_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r33-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":1038.0,"category":"鐣岄潰鎺т欢","name":"R33_绉绘恫瀛斾綅","controlId":"svg-slide-r33-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1039.0,"category":"鐣岄潰鎺т欢","name":"R33_绉绘恫娑查噺","controlId":"svg-slide-r33-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1040.0,"category":"鐣岄潰鎺т欢","name":"R33_鎺у埗閽?,"controlId":"svg-slide-r33-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1041.0,"category":"鐣岄潰鎺т欢","name":"R34_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r34-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":1042.0,"category":"鐣岄潰鎺т欢","name":"R34_ROI Left","controlId":"svg-slide-r34-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1043.0,"category":"鐣岄潰鎺т欢","name":"R34_ROI Top","controlId":"svg-slide-r34-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1044.0,"category":"鐣岄潰鎺т欢","name":"R34_ROI Width","controlId":"svg-slide-r34-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1045.0,"category":"鐣岄潰鎺т欢","name":"R34_ROI Height","controlId":"svg-slide-r34-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1046.0,"category":"鐣岄潰鎺т欢","name":"R34_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r34-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":1047.0,"category":"鐣岄潰鎺т欢","name":"R34_娑蹭綋绫诲瀷","controlId":"svg-slide-r34-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1048.0,"category":"鐣岄潰鎺т欢","name":"R34_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r34-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1049.0,"category":"鐣岄潰鎺т欢","name":"R34_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r34-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1050.0,"category":"鐣岄潰鎺т欢","name":"R34_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r34-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1051.0,"category":"鐣岄潰鎺т欢","name":"R34_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r34-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1052.0,"category":"鐣岄潰鎺т欢","name":"R34_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r34-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":1053.0,"category":"鐣岄潰鎺т欢","name":"R34_绉绘恫瀛斾綅","controlId":"svg-slide-r34-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1054.0,"category":"鐣岄潰鎺т欢","name":"R34_绉绘恫娑查噺","controlId":"svg-slide-r34-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1055.0,"category":"鐣岄潰鎺т欢","name":"R34_鎺у埗閽?,"controlId":"svg-slide-r34-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1056.0,"category":"鐣岄潰鎺т欢","name":"R41_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r41-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":1057.0,"category":"鐣岄潰鎺т欢","name":"R41_ROI Left","controlId":"svg-slide-r41-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1058.0,"category":"鐣岄潰鎺т欢","name":"R41_ROI Top","controlId":"svg-slide-r41-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1059.0,"category":"鐣岄潰鎺т欢","name":"R41_ROI Width","controlId":"svg-slide-r41-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1060.0,"category":"鐣岄潰鎺т欢","name":"R41_ROI Height","controlId":"svg-slide-r41-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1061.0,"category":"鐣岄潰鎺т欢","name":"R41_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r41-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":1062.0,"category":"鐣岄潰鎺т欢","name":"R41_娑蹭綋绫诲瀷","controlId":"svg-slide-r41-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1063.0,"category":"鐣岄潰鎺т欢","name":"R41_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r41-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1064.0,"category":"鐣岄潰鎺т欢","name":"R41_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r41-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1065.0,"category":"鐣岄潰鎺т欢","name":"R41_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r41-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1066.0,"category":"鐣岄潰鎺т欢","name":"R41_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r41-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1067.0,"category":"鐣岄潰鎺т欢","name":"R41_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r41-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":1068.0,"category":"鐣岄潰鎺т欢","name":"R41_绉绘恫瀛斾綅","controlId":"svg-slide-r41-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1069.0,"category":"鐣岄潰鎺т欢","name":"R41_绉绘恫娑查噺","controlId":"svg-slide-r41-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1070.0,"category":"鐣岄潰鎺т欢","name":"R41_鎺у埗閽?,"controlId":"svg-slide-r41-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1071.0,"category":"鐣岄潰鎺т欢","name":"R42_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r42-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":1072.0,"category":"鐣岄潰鎺т欢","name":"R42_ROI Left","controlId":"svg-slide-r42-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1073.0,"category":"鐣岄潰鎺т欢","name":"R42_ROI Top","controlId":"svg-slide-r42-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1074.0,"category":"鐣岄潰鎺т欢","name":"R42_ROI Width","controlId":"svg-slide-r42-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1075.0,"category":"鐣岄潰鎺т欢","name":"R42_ROI Height","controlId":"svg-slide-r42-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1076.0,"category":"鐣岄潰鎺т欢","name":"R42_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r42-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":1077.0,"category":"鐣岄潰鎺т欢","name":"R42_娑蹭綋绫诲瀷","controlId":"svg-slide-r42-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1078.0,"category":"鐣岄潰鎺т欢","name":"R42_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r42-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1079.0,"category":"鐣岄潰鎺т欢","name":"R42_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r42-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1080.0,"category":"鐣岄潰鎺т欢","name":"R42_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r42-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1081.0,"category":"鐣岄潰鎺т欢","name":"R42_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r42-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1082.0,"category":"鐣岄潰鎺т欢","name":"R42_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r42-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":1083.0,"category":"鐣岄潰鎺т欢","name":"R42_绉绘恫瀛斾綅","controlId":"svg-slide-r42-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1084.0,"category":"鐣岄潰鎺т欢","name":"R42_绉绘恫娑查噺","controlId":"svg-slide-r42-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1085.0,"category":"鐣岄潰鎺т欢","name":"R42_鎺у埗閽?,"controlId":"svg-slide-r42-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1086.0,"category":"鐣岄潰鎺т欢","name":"R43_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r43-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":1087.0,"category":"鐣岄潰鎺т欢","name":"R43_ROI Left","controlId":"svg-slide-r43-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1088.0,"category":"鐣岄潰鎺т欢","name":"R43_ROI Top","controlId":"svg-slide-r43-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1089.0,"category":"鐣岄潰鎺т欢","name":"R43_ROI Width","controlId":"svg-slide-r43-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1090.0,"category":"鐣岄潰鎺т欢","name":"R43_ROI Height","controlId":"svg-slide-r43-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1091.0,"category":"鐣岄潰鎺т欢","name":"R43_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r43-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":1092.0,"category":"鐣岄潰鎺т欢","name":"R43_娑蹭綋绫诲瀷","controlId":"svg-slide-r43-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1093.0,"category":"鐣岄潰鎺т欢","name":"R43_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r43-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1094.0,"category":"鐣岄潰鎺т欢","name":"R43_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r43-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1095.0,"category":"鐣岄潰鎺т欢","name":"R43_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r43-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1096.0,"category":"鐣岄潰鎺т欢","name":"R43_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r43-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1097.0,"category":"鐣岄潰鎺т欢","name":"R43_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r43-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":1098.0,"category":"鐣岄潰鎺т欢","name":"R43_绉绘恫瀛斾綅","controlId":"svg-slide-r43-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1099.0,"category":"鐣岄潰鎺т欢","name":"R43_绉绘恫娑查噺","controlId":"svg-slide-r43-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1100.0,"category":"鐣岄潰鎺т欢","name":"R43_鎺у埗閽?,"controlId":"svg-slide-r43-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1101.0,"category":"鐣岄潰鎺т欢","name":"R44_鏉＄爜ROI闈㈡澘","controlId":"svg-slide-r44-barcode-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆璇ョ幓鐗?璇曞墏瀵硅薄鐨勬潯鐮佹壂鎻?ROI銆?},{"id":1102.0,"category":"鐣岄潰鎺т欢","name":"R44_ROI Left","controlId":"svg-slide-r44-barcode-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1103.0,"category":"鐣岄潰鎺т欢","name":"R44_ROI Top","controlId":"svg-slide-r44-barcode-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1104.0,"category":"鐣岄潰鎺т欢","name":"R44_ROI Width","controlId":"svg-slide-r44-barcode-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1105.0,"category":"鐣岄潰鎺т欢","name":"R44_ROI Height","controlId":"svg-slide-r44-barcode-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鏉＄爜 ROI 鍙傛暟杈撳叆妗嗐€?},{"id":1106.0,"category":"鐣岄潰鎺т欢","name":"R44_瀛斾綅Z鍙傛暟闈㈡澘","controlId":"svg-slide-r44-well-engineering-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鐢ㄤ簬璁剧疆娑蹭綋绫诲瀷銆乑-Travel銆乑-Start銆乑-End銆乑-Dispens銆?},{"id":1107.0,"category":"鐣岄潰鎺т欢","name":"R44_娑蹭綋绫诲瀷","controlId":"svg-slide-r44-liquid-class","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1108.0,"category":"鐣岄潰鎺т欢","name":"R44_Z-Travel瀹夊叏绉诲姩楂樺害","controlId":"svg-slide-r44-z-travel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1109.0,"category":"鐣岄潰鎺т欢","name":"R44_Z-Start鎺㈡恫楂樺害","controlId":"svg-slide-r44-z-start","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1110.0,"category":"鐣岄潰鎺т欢","name":"R44_Z-End閽堝皷鏈€澶т笅闄嶆繁搴?,"controlId":"svg-slide-r44-z-end","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1111.0,"category":"鐣岄潰鎺т欢","name":"R44_Z-Dispens鍚告恫/鎺掓恫楂樺害","controlId":"svg-slide-r44-z-dispens","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱瀛斾綅绉讳綅宸ョ▼鍙傛暟銆?},{"id":1112.0,"category":"鐣岄潰鎺т欢","name":"R44_閫氶亾绉绘恫闈㈡澘","controlId":"svg-slide-r44-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱鍩轰簬褰撳墠瀛斾綅鎵ц鍚告恫銆佹墦娑层€佹恫闈㈡帰娴嬪拰娓呯┖閫氶亾銆?},{"id":1113.0,"category":"鐣岄潰鎺т欢","name":"R44_绉绘恫瀛斾綅","controlId":"svg-slide-r44-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1114.0,"category":"鐣岄潰鎺т欢","name":"R44_绉绘恫娑查噺","controlId":"svg-slide-r44-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1115.0,"category":"鐣岄潰鎺т欢","name":"R44_鎺у埗閽?,"controlId":"svg-slide-r44-pipette-needle","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閫変腑瀵硅薄鍚庢樉绀猴紱閫氶亾绉绘恫娴嬭瘯鍙傛暟銆?},{"id":1116.0,"category":"鐣岄潰鎺т欢","name":"鏈烘鑷傜浉鏈篲鏍锋湰鎵爜鍣╛COM璁剧疆闈㈡澘","controlId":"svg-arm-camera-follow-scanner-com-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮鏈烘鑷傞殢鍔ㄧ浉鏈哄悗鍦ㄧ姸鎬佽鎯呴〉鏄剧ず锛汣OM璁剧疆闈㈡澘銆?},{"id":1117.0,"category":"鐣岄潰鎺т欢","name":"鏈烘鑷傜浉鏈篲鏍锋湰鎵爜鍣╛ROI璁剧疆闈㈡澘","controlId":"svg-arm-camera-follow-scanner-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮鏈烘鑷傞殢鍔ㄧ浉鏈哄悗鍦ㄧ姸鎬佽鎯呴〉鏄剧ず锛汻OI璁剧疆闈㈡澘銆?},{"id":1118.0,"category":"鐣岄潰鎺т欢","name":"鏈烘鑷傜浉鏈篲鏍锋湰鎵爜鍣╛鏍￠獙鍏夊垵濮嬪寲闈㈡澘","controlId":"svg-arm-camera-follow-scanner-light-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮鏈烘鑷傞殢鍔ㄧ浉鏈哄悗鍦ㄧ姸鎬佽鎯呴〉鏄剧ず锛涙牎楠屽厜鍒濆鍖栭潰鏉裤€?},{"id":1119.0,"category":"鐣岄潰鎺т欢","name":"鏈烘鑷傜浉鏈篲鏍锋湰鎵爜鍣╛COM鍙?,"controlId":"svg-arm-camera-follow-com","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮鏈烘鑷傞殢鍔ㄧ浉鏈哄悗鍦ㄧ姸鎬佽鎯呴〉鏄剧ず锛汣OM鍙ｃ€?},{"id":1120.0,"category":"鐣岄潰鎺т欢","name":"鏈烘鑷傜浉鏈篲鏍锋湰鎵爜鍣╛娉㈢壒鐜?,"controlId":"svg-arm-camera-follow-baud","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮鏈烘鑷傞殢鍔ㄧ浉鏈哄悗鍦ㄧ姸鎬佽鎯呴〉鏄剧ず锛涙尝鐗圭巼銆?},{"id":1121.0,"category":"鐣岄潰鎺т欢","name":"鏈烘鑷傜浉鏈篲鏍锋湰鎵爜鍣╛ROI Left","controlId":"svg-arm-camera-follow-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮鏈烘鑷傞殢鍔ㄧ浉鏈哄悗鍦ㄧ姸鎬佽鎯呴〉鏄剧ず锛汻OI Left銆?},{"id":1122.0,"category":"鐣岄潰鎺т欢","name":"鏈烘鑷傜浉鏈篲鏍锋湰鎵爜鍣╛ROI Top","controlId":"svg-arm-camera-follow-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮鏈烘鑷傞殢鍔ㄧ浉鏈哄悗鍦ㄧ姸鎬佽鎯呴〉鏄剧ず锛汻OI Top銆?},{"id":1123.0,"category":"鐣岄潰鎺т欢","name":"鏈烘鑷傜浉鏈篲鏍锋湰鎵爜鍣╛ROI Width","controlId":"svg-arm-camera-follow-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮鏈烘鑷傞殢鍔ㄧ浉鏈哄悗鍦ㄧ姸鎬佽鎯呴〉鏄剧ず锛汻OI Width銆?},{"id":1124.0,"category":"鐣岄潰鎺т欢","name":"鏈烘鑷傜浉鏈篲鏍锋湰鎵爜鍣╛ROI Height","controlId":"svg-arm-camera-follow-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮鏈烘鑷傞殢鍔ㄧ浉鏈哄悗鍦ㄧ姸鎬佽鎯呴〉鏄剧ず锛汻OI Height銆?},{"id":1125.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏鎵爜鍣╛COM璁剧疆闈㈡澘","controlId":"svg-camera-reagent-scanner-scanner-com-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮璇曞墏鎵爜鍣ㄦ憚鍍忓ご鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛汣OM璁剧疆闈㈡澘銆?},{"id":1126.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏鎵爜鍣╛ROI璁剧疆闈㈡澘","controlId":"svg-camera-reagent-scanner-scanner-roi-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮璇曞墏鎵爜鍣ㄦ憚鍍忓ご鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛汻OI璁剧疆闈㈡澘銆?},{"id":1127.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏鎵爜鍣╛鏍￠獙鍏夊垵濮嬪寲闈㈡澘","controlId":"svg-camera-reagent-scanner-scanner-light-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮璇曞墏鎵爜鍣ㄦ憚鍍忓ご鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛涙牎楠屽厜鍒濆鍖栭潰鏉裤€?},{"id":1128.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏鎵爜鍣╛COM鍙?,"controlId":"svg-camera-reagent-scanner-com","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮璇曞墏鎵爜鍣ㄦ憚鍍忓ご鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛汣OM鍙ｃ€?},{"id":1129.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏鎵爜鍣╛娉㈢壒鐜?,"controlId":"svg-camera-reagent-scanner-baud","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮璇曞墏鎵爜鍣ㄦ憚鍍忓ご鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛涙尝鐗圭巼銆?},{"id":1130.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏鎵爜鍣╛ROI Left","controlId":"svg-camera-reagent-scanner-roi-left","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮璇曞墏鎵爜鍣ㄦ憚鍍忓ご鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛汻OI Left銆?},{"id":1131.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏鎵爜鍣╛ROI Top","controlId":"svg-camera-reagent-scanner-roi-top","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮璇曞墏鎵爜鍣ㄦ憚鍍忓ご鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛汻OI Top銆?},{"id":1132.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏鎵爜鍣╛ROI Width","controlId":"svg-camera-reagent-scanner-roi-width","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮璇曞墏鎵爜鍣ㄦ憚鍍忓ご鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛汻OI Width銆?},{"id":1133.0,"category":"鐣岄潰鎺т欢","name":"璇曞墏鎵爜鍣╛ROI Height","controlId":"svg-camera-reagent-scanner-roi-height","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮璇曞墏鎵爜鍣ㄦ憚鍍忓ご鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛汻OI Height銆?},{"id":1134.0,"category":"鐣岄潰鎺т欢","name":"閽堝ご_Z1_鍙岄拡鏈烘鑷傝缃潰鏉?,"controlId":"svg-arm-needle-z1-arm-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閽堝ご_Z1鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛涘弻閽堟満姊拌噦璁剧疆闈㈡澘銆?},{"id":1135.0,"category":"鐣岄潰鎺т欢","name":"閽堝ご_Z1_閽堝ご绉绘恫娴嬭瘯闈㈡澘","controlId":"svg-arm-needle-z1-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閽堝ご_Z1鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛涢拡澶寸Щ娑叉祴璇曢潰鏉裤€?},{"id":1136.0,"category":"鐣岄潰鎺т欢","name":"閽堝ご_Z1_閽堥棿璺?,"controlId":"svg-arm-needle-z1-gap","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閽堝ご_Z1鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛涢拡闂磋窛銆?},{"id":1137.0,"category":"鐣岄潰鎺т欢","name":"閽堝ご_Z1_Z瀹夊叏楂樺害","controlId":"svg-arm-needle-z1-safe-z","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閽堝ご_Z1鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛沍瀹夊叏楂樺害銆?},{"id":1138.0,"category":"鐣岄潰鎺т欢","name":"閽堝ご_Z1_鐩爣瀛斾綅","controlId":"svg-arm-needle-z1-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閽堝ご_Z1鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛涚洰鏍囧瓟浣嶃€?},{"id":1139.0,"category":"鐣岄潰鎺т欢","name":"閽堝ご_Z1_娑查噺","controlId":"svg-arm-needle-z1-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閽堝ご_Z1鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛涙恫閲忋€?},{"id":1140.0,"category":"鐣岄潰鎺т欢","name":"閽堝ご_Z2_鍙岄拡鏈烘鑷傝缃潰鏉?,"controlId":"svg-arm-needle-z2-arm-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閽堝ご_Z2鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛涘弻閽堟満姊拌噦璁剧疆闈㈡澘銆?},{"id":1141.0,"category":"鐣岄潰鎺т欢","name":"閽堝ご_Z2_閽堝ご绉绘恫娴嬭瘯闈㈡澘","controlId":"svg-arm-needle-z2-pipette-card","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閽堝ご_Z2鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛涢拡澶寸Щ娑叉祴璇曢潰鏉裤€?},{"id":1142.0,"category":"鐣岄潰鎺т欢","name":"閽堝ご_Z2_閽堥棿璺?,"controlId":"svg-arm-needle-z2-gap","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閽堝ご_Z2鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛涢拡闂磋窛銆?},{"id":1143.0,"category":"鐣岄潰鎺т欢","name":"閽堝ご_Z2_Z瀹夊叏楂樺害","controlId":"svg-arm-needle-z2-safe-z","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閽堝ご_Z2鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛沍瀹夊叏楂樺害銆?},{"id":1144.0,"category":"鐣岄潰鎺т欢","name":"閽堝ご_Z2_鐩爣瀛斾綅","controlId":"svg-arm-needle-z2-pipette-well","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閽堝ご_Z2鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛涚洰鏍囧瓟浣嶃€?},{"id":1145.0,"category":"鐣岄潰鎺т欢","name":"閽堝ご_Z2_娑查噺","controlId":"svg-arm-needle-z2-pipette-volume","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閽堝ご_Z2鍚庡湪鐘舵€佽鎯呴〉鏄剧ず锛涙恫閲忋€?},{"id":1146.0,"category":"鐣岄潰鎺т欢","name":"閰嶇疆鏂囦欢_鎽樿鎸夐挳","controlId":"configProfileSummary","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閰嶇疆/娴佺▼椤甸《閮ㄥ綋鍓嶉厤缃憳瑕侊紱鐐瑰嚮灞曞紑閰嶇疆鏂囦欢閫夋嫨涓庢柊寤恒€?},{"id":1147.0,"category":"鐣岄潰鎺т欢","name":"褰撳墠閰嶇疆_鎽樿鏉?,"controlId":"configCurrentProfileBar","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閰嶇疆/娴佺▼椤甸《閮ㄥ父椹绘憳瑕佹潯锛涙樉绀哄綋鍓嶉厤缃悕绉板拰鍏冧俊鎭€?},{"id":1148.0,"category":"鐣岄潰鎺т欢","name":"褰撳墠閰嶇疆_鍚嶇О","controlId":"configCurrentProfileName","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閰嶇疆/娴佺▼椤甸《閮ㄥ父椹诲瓧娈碉紱鏄剧ず褰撳墠閰嶇疆鍚嶇О銆?},{"id":1149.0,"category":"鐣岄潰鎺т欢","name":"褰撳墠閰嶇疆_鍏冧俊鎭?,"controlId":"configCurrentProfileMeta","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閰嶇疆/娴佺▼椤甸《閮ㄥ父椹诲瓧娈碉紱鏄剧ず鏌撹壊绫诲瀷銆佹祦绋嬫鏁般€佹帶娓╃姸鎬併€?},{"id":1150.0,"category":"鐣岄潰鎺т欢","name":"閰嶇疆鏂囦欢_缂栬緫闈㈡澘","controlId":"configProfileEditorPanel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閰嶇疆鏂囦欢鎶藉眽鍐呴儴缂栬緫鍖猴紱缁存姢閰嶇疆鍚嶇О銆佹煋鑹茬被鍨嬨€佽鏄庛€?},{"id":1151.0,"category":"鐣岄潰鎺т欢","name":"閰嶇疆鏂囦欢_琛ㄥ崟缃戞牸","controlId":"configProfileFormGrid","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閰嶇疆鏂囦欢鎶藉眽鍐呴儴琛ㄥ崟缃戞牸锛涘帇缂╁崰鐢ㄩ潰绉€?},{"id":1152.0,"category":"鐣岄潰鎺т欢","name":"閰嶇疆鏂囦欢_鎿嶄綔鎸夐挳缁?,"controlId":"configProfileActions","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閰嶇疆鏂囦欢鎶藉眽鍐呴儴鎿嶄綔鍖猴紱鏂板缓銆佸鍒躲€佷繚瀛樸€佸垹闄ゃ€佸鍏ュ鍑恒€?},{"id":1153.0,"category":"鐣岄潰鎺т欢","name":"閰嶇疆鏂囦欢_瀵煎叆鏂囦欢鎺т欢","controlId":"configImportInput","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閰嶇疆鏂囦欢鎶藉眽鍐呴儴闅愯棌鏂囦欢杈撳叆锛涚敤浜庡鍏?JSON銆?},{"id":1154.0,"category":"鐣岄潰鎺т欢","name":"閰嶇疆鏂囦欢_鍒楄〃闈㈡澘","controlId":"configProfilePickerPanel","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閰嶇疆鏂囦欢鎶藉眽鍐呴儴閰嶇疆鍒楄〃闈㈡澘锛涚敤浜庨€夋嫨宸叉湁閰嶇疆銆?},{"id":1155.0,"category":"鐣岄潰鎺т欢","name":"閰嶇疆鏂囦欢_鍒楄〃","controlId":"configProfileFileList","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"閰嶇疆鏂囦欢鎶藉眽鍐呴儴閰嶇疆鍗＄墖鍒楄〃锛涢€夋嫨鍚庢娊灞夋敹璧峰苟鍥炲埌娴佺▼缂栬緫銆?},{"id":1156.0,"category":"鐣岄潰鎺т欢","name":"娴佺▼缂栬緫_甯冨眬瀹瑰櫒","controlId":"configEditLayout","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"娴佺▼缂栬緫涓诲伐浣滃彴甯冨眬瀹瑰櫒锛涗笁鏍忥細鎿嶄綔搴撱€佹祦绋嬬爾鍧楁椂闂寸嚎銆佸綋鍓嶆楠ょ紪杈戝櫒銆?},{"id":1157.0,"category":"鐣岄潰鎺т欢","name":"娴佺▼缂栬緫_鎿嶄綔搴撳垪","controlId":"configOpColumn","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"娴佺▼缂栬緫涓诲伐浣滃彴宸︽爮锛涘彧鏀炬搷浣滃簱锛屼笉鍐嶅閰嶇疆鍒楄〃銆?},{"id":1158.0,"category":"鐣岄潰鎺т欢","name":"娴佺▼缂栬緫_鏃堕棿绾垮垪","controlId":"configFlowColumn","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"娴佺▼缂栬緫涓诲伐浣滃彴涓爮锛涗富瑙嗚鍖哄煙锛屾樉绀哄苟缂栬緫娴佺▼鐮栧潡銆?},{"id":1159.0,"category":"鐣岄潰鎺т欢","name":"娴佺▼缂栬緫_姝ラ缂栬緫鍒?,"controlId":"configStepColumn","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"娴佺▼缂栬緫涓诲伐浣滃彴鍙虫爮锛涚紪杈戝綋鍓嶉€変腑姝ラ銆?},{"id":1160.0,"category":"鐣岄潰鎺т欢","name":"閫氶亾1_璁剧疆榛樿閰嶇疆鎸夐挳","controlId":"statusChannel1SetDefaultConfigBtn","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閫氶亾瀵硅薄鍚庣殑鐘舵€佽鎯呴〉鎸夐挳锛涘皢褰撳墠閫夋嫨鐨勯厤缃枃浠朵繚瀛樹负璇ラ€氶亾榛樿閰嶇疆銆?},{"id":1161.0,"category":"鐣岄潰鎺т欢","name":"閫氶亾2_璁剧疆榛樿閰嶇疆鎸夐挳","controlId":"statusChannel2SetDefaultConfigBtn","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閫氶亾瀵硅薄鍚庣殑鐘舵€佽鎯呴〉鎸夐挳锛涘皢褰撳墠閫夋嫨鐨勯厤缃枃浠朵繚瀛樹负璇ラ€氶亾榛樿閰嶇疆銆?},{"id":1162.0,"category":"鐣岄潰鎺т欢","name":"閫氶亾3_璁剧疆榛樿閰嶇疆鎸夐挳","controlId":"statusChannel3SetDefaultConfigBtn","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閫氶亾瀵硅薄鍚庣殑鐘舵€佽鎯呴〉鎸夐挳锛涘皢褰撳墠閫夋嫨鐨勯厤缃枃浠朵繚瀛樹负璇ラ€氶亾榛樿閰嶇疆銆?},{"id":1163.0,"category":"鐣岄潰鎺т欢","name":"閫氶亾4_璁剧疆榛樿閰嶇疆鎸夐挳","controlId":"statusChannel4SetDefaultConfigBtn","row":null,"col":null,"x":null,"y":null,"shape":"html-control","radius":null,"width":null,"height":null,"note":"鐐瑰嚮閫氶亾瀵硅薄鍚庣殑鐘舵€佽鎯呴〉鎸夐挳锛涘皢褰撳墠閫夋嫨鐨勯厤缃枃浠朵繚瀛樹负璇ラ€氶亾榛樿閰嶇疆銆?}];

const STATUS_TEXT = { idle:'绌洪棽', scanned:'宸叉壂鎻?, ready:'灏辩华', running:'杩愯涓?, complete:'瀹屾垚', low:'涓嶈冻', error:'鏁呴殰', open:'宸叉娊鍑?, active:'鎵ц涓? };
const CATEGORY_CLASS = { '璇曞墏鍖?:'reagent', '娲楅拡澶?:'wash', '娣峰悎娑蹭綋閰嶆恫鍖?:'mix', 'A/B娑?:'ab', '娣峰寑鐢垫満':'motor', '鐜荤墖閫氶亾':'slide' };
const AUX_COORD_CATEGORIES = new Set(['搴熸恫瀛?,'鎺掓瘨瀛?,'娓呮礂瀛?,'鐩告満','鏈烘鑷傜浉瀵逛綅缃?,'璇曞墏鍖洪€氶亾鍒嗛殧绾?,'璇曞墏鍒颁綅鎰熷簲','璇曞墏鍏ュ彛鎰熷簲','閰嶆恫鐡剁姸鎬?,'閰嶆恫鐡剁姸鎬佹牸','璇曞墏鍥句緥鎬婚噺','鐣岄潰鎺т欢','鐧诲綍椤?,'鐢ㄦ埛绠＄悊','鏉冮檺鎺у埗']);
function isPhysicalDrawableItem(item) {
  return item && !AUX_COORD_CATEGORIES.has(item.category) && Number.isFinite(item.x) && Number.isFinite(item.y) && item.shape !== 'html-control';
}
const VIEW = { w:1180, h:700, padL:72, padT:38, padR:10, padB:26 };
const HOME_TARGET_NAME = '娲楅拡澶確鍙冲垪_娲楀唴澹乢R1';
const STAIN_COLORS = { HE:'#fecdd3', IHC:'#bfdbfe', PAS:'#fde68a', MAS:'#bbf7d0', IF:'#ddd6fe' };
const MIX_LIQUID_COLOR = '#374151';
const REAGENT_TYPES = [
  { key:'primary-antibody', label:'涓€鎶?, color:'#f97316' },
  { key:'secondary-antibody', label:'浜屾姉', color:'#ef4444' },
  { key:'hematoxylin', label:'鑻忔湪绱?, color:'#7c3aed' },
  { key:'eosin', label:'浼婄孩', color:'#ec4899' },
  { key:'absolute-ethanol', label:'鏃犳按閰掔簿', color:'#0891b2' },
  { key:'acid-wash', label:'閰告礂娑?, color:'#eab308' },
  { key:'endogenous-enzyme-blocker', label:'鍐呮簮鎬ч叾闃绘柇鍓?, color:'#22c55e' }
];
const REAGENT_TYPE_LAYOUT_BY_ROW = [0,0,1,2,3,4,5,6];
const DEFAULT_CHANNEL_STAINS = [
  ['HE','IHC','PAS','IF'],
  ['IHC','HE','MAS','HE'],
  ['PAS','HE','IHC','MAS'],
  ['IF','PAS','HE','IHC']
];
const OP_DEFS = [
  { key:'hematoxylin', label:'鑻忔湪绱犳煋鑹?, shape:'circle', color:'#7c3aed' },
  { key:'acid', label:'閰告礂', shape:'square', color:'#eab308' },
  { key:'water', label:'姘存礂', shape:'circle', color:'#0891b2' },
  { key:'ethanol', label:'鏃犳按涔欓唶娓呮礂', shape:'diamond', color:'#14b8a6' },
  { key:'eosin', label:'浼婄孩鏌撹壊', shape:'circle', color:'#ec4899' },
  { key:'block', label:'鍐呮簮鎬ч叾闃绘柇', shape:'square', color:'#22c55e' },
  { key:'primary', label:'涓€鎶楀鑲?, shape:'triangle', color:'#f97316' },
  { key:'secondary', label:'浜屾姉瀛佃偛', shape:'triangle', color:'#ef4444' },
  { key:'dabMix', label:'DAB 閰嶅埗', shape:'diamond', color:'#64748b' },
  { key:'dabColor', label:'DAB 鏄捐壊', shape:'circle', color:'#a16207' },
  { key:'temp', label:'娓╁害鎺у埗', shape:'square', color:'#2563eb' },
  { key:'reagentMix', label:'璇曞墏娣峰寑', shape:'diamond', color:'#10b981' },
  { key:'needleWash', label:'鍔犳牱閽堟竻娲?, shape:'circle', color:'#06b6d4' }
];

const OP_DEF_BY_KEY = Object.fromEntries(OP_DEFS.map(op => [op.key, op]));
const CONFIG_STORAGE_KEY = 'pathologyStainer.configProfiles.v1';
const CONFIG_ASSIGN_STORAGE_KEY = 'pathologyStainer.channelConfigAssignments.v1';
const SETTINGS_STORAGE_KEY = 'pathologyStainer.runtimeSettings.v1';
const DEFAULT_APP_SETTINGS = {
  dataInterface:'WebSocket 棰勭暀', hostAddress:'ws://127.0.0.1:9001/digital-twin', heartbeatSec:3,
  reagentBottleCapacityMl:10, reagentTargetTempC:8, workTargetTempC:40, needleGapMm:25,
  pureLowThresholdPct:20, pbsLowThresholdPct:20, wasteFullThresholdPct:85, toxicFullThresholdPct:85,
  maxVisibleSlideSteps:12, logRetention:300, autoRunPrecheck:false
};
const DEFAULT_CONFIG_PROFILES = [
  {
    id:'ihc-standard-40c',
    name:'IHC 鏍囧噯娴佺▼ 40鈩?,
    stainType:'IHC',
    version:'1.0.0',
    description:'鍐呮簮鎬ч叾闃绘柇銆佷竴鎶椼€佷簩鎶椼€丏AB 鏄捐壊銆佸鏌撴按娲楋紱姝ラ4寮€濮嬮粯璁ゆ帶娓┿€?,
    targetTempC:40,
    tempControlFromStep:4,
    allowMultiPrimary:true,
    dabRatio:{ a:1, b:1, pureWater:18, preparePolicy:'per_run' },
    notes:'涓€鎶楀彲鎸夌幓鐗?閫氶亾鏄犲皠涓嶅悓鎶椾綋锛涘叡鐢ㄨ瘯鍓傜粺涓€璋冨害锛汥AB 鎺ㄨ崘姣忚疆鐜伴厤銆?,
    steps:[
      { id:'ihc-s01', opKey:'block', label:'鍐呮簮鎬ч叾闃绘柇鍓?, durationSec:20, toleranceSec:0, immediateAfterPrev:false, reagentRole:'blocker', notes:'闃绘柇鍐呮簮鎬ч叾娲绘€? },
      { id:'ihc-s02', opKey:'water', label:'娓呮礂娑?, durationSec:15, toleranceSec:0, immediateAfterPrev:true, reagentRole:'wash', notes:'闃绘柇鍚庣珛鍗虫竻娲楋紝閬垮厤娈嬬暀褰卞搷涓€鎶? },
      { id:'ihc-s03', opKey:'primary', label:'涓€鎶?, durationSec:270, toleranceSec:0, immediateAfterPrev:false, reagentRole:'primary', allowMultiPrimary:true, notes:'涓€鎶楃绫诲彲澶氳矾鏄犲皠锛涢粯璁?4.5 min' },
      { id:'ihc-s04', opKey:'water', label:'娓呮礂娑?, durationSec:15, toleranceSec:0, immediateAfterPrev:true, requiresTemp:true, targetTempC:40, reagentRole:'wash', notes:'涓€鎶楀悗绔嬪嵆娓呮礂锛涗粠鏈寮€濮嬭繘鍏?40鈩?鎺ф俯娈? },
      { id:'ihc-s05', opKey:'secondary', label:'浜屾姉', durationSec:90, toleranceSec:0, immediateAfterPrev:false, requiresTemp:true, targetTempC:40, reagentRole:'secondary', notes:'榛樿 1.5 min' },
      { id:'ihc-s06', opKey:'water', label:'娓呮礂娑?, durationSec:15, toleranceSec:0, immediateAfterPrev:true, requiresTemp:true, targetTempC:40, reagentRole:'wash', notes:'浜屾姉鍚庣珛鍗虫竻娲? },
      { id:'ihc-s07', opKey:'dabColor', label:'DAB', durationSec:90, toleranceSec:0, immediateAfterPrev:false, requiresTemp:true, targetTempC:40, reagentRole:'dab', notes:'DAB A:B:绾按 = 1:1:18锛屽缓璁瘡杞幇閰? },
      { id:'ihc-s08', opKey:'water', label:'姘存礂', durationSec:5, toleranceSec:0, immediateAfterPrev:true, requiresTemp:true, targetTempC:40, reagentRole:'water', notes:'DAB 缁堟姘存礂' },
      { id:'ihc-s09', opKey:'hematoxylin', label:'鑻忔湪绱?, durationSec:10, toleranceSec:0, immediateAfterPrev:false, requiresTemp:true, targetTempC:40, reagentRole:'hematoxylin', notes:'鏍稿鏌? },
      { id:'ihc-s10', opKey:'water', label:'姘存礂', durationSec:10, toleranceSec:0, immediateAfterPrev:true, requiresTemp:true, targetTempC:40, reagentRole:'water', notes:'澶嶆煋鍚庢按娲? }
    ]
  },
  {
    id:'he-fast-basic',
    name:'HE 蹇€熸煋鑹叉ā鏉?,
    stainType:'HE',
    version:'1.0.0',
    description:'鐢ㄤ簬楠岃瘉閰嶇疆妯″潡鐨勭浜屼釜妯℃澘锛屽彲澶嶅埗鍚庢寜瀹為獙瑕佹眰璋冩暣銆?,
    targetTempC:null,
    tempControlFromStep:null,
    allowMultiPrimary:false,
    dabRatio:null,
    notes:'HE 妯℃澘浠呯敤浜庢紨绀哄閰嶇疆鏂囦欢绠＄悊銆?,
    steps:[
      { id:'he-s01', opKey:'hematoxylin', label:'鑻忔湪绱犳煋鑹?, durationSec:10, toleranceSec:0, immediateAfterPrev:false },
      { id:'he-s02', opKey:'water', label:'姘存礂', durationSec:10, toleranceSec:0, immediateAfterPrev:true },
      { id:'he-s03', opKey:'acid', label:'閰告礂', durationSec:5, toleranceSec:0, immediateAfterPrev:true },
      { id:'he-s04', opKey:'eosin', label:'浼婄孩鏌撹壊', durationSec:8, toleranceSec:0, immediateAfterPrev:false },
      { id:'he-s05', opKey:'ethanol', label:'鏃犳按涔欓唶娓呮礂', durationSec:12, toleranceSec:0, immediateAfterPrev:true }
    ]
  }
];
let coords = DEFAULT_COORDS.map(normalizeItem);
let defaultCoordByName = new Map(coords.map(item => [item.name, { x:item.x, y:item.y }]));
let transform = null;
let itemState = new Map();
let itemLevels = new Map();
let slideOps = new Map();
let slideTemps = new Map();
let selectedName = null;
let selectedSvgControlId = null;
let detailMessage = null;
let selectedChannel = 1;
let running = false;
let paused = false;
let cancelAnimation = false;
let currentStepIndex = 0;
let estimatedFinishAt = null;
let totalRuns = 0;
let demoPromise = null;
let arm = { x:177.75, y:17.0, z1:0, z2:0, fluid1:null, fluid2:null };
const cameraStates = { reagent:'idle', arm:'idle' };
let currentTarget = null;
let logSerial = 0;
const liquids = { pure:82, pbs:76, waste:25, toxic:18 };
const headerMetrics = { today:0, active:0, total:0 };
let warningCount = 0;
let uiMode = 'twin';
let appSettings = loadAppSettings();
let channelScanningBusy = false;
const channels = Array.from({length:4}, (_,i)=>({ id:i+1, state:'idle', progress:0, slides:[['HE','IHC','PAS','IF'], ['IHC','HE','MAS','HE'], ['PAS','HE','IHC','MAS'], ['IF','PAS','HE','IHC']][i], pulled:false, configProfileId:null }));
const PRECHECK_STEPS = [
  { label:'涓绘帶杩炴帴', api:'precheck.mainControl.connect' },
  { label:'鏈烘鑷傚洖闆?, api:'precheck.arm.home' },
  { label:'鍒跺喎杩炴帴', api:'precheck.cooling.connect' },
  { label:'鏍锋湰鎵爜鍣ㄥ湪绾?, api:'precheck.sampleScanner.online' },
  { label:'璇曞墏鎵爜鍣ㄥ湪绾?, api:'precheck.reagentScanner.online' },
  { label:'娑蹭綅/浼犳劅鍣ㄨ鍙?, api:'precheck.levelSensor.read' },
  { label:'娲楅拡鍑嗗', api:'precheck.needleWash.ready' },
  { label:'绾按鍙敤', api:'precheck.pureWater.available' },
  { label:'PBS鍙敤', api:'precheck.pbs.available' },
  { label:'搴熸恫鏈弧', api:'precheck.waste.notFull' },
  { label:'鎺掓瘨妗舵湭婊?, api:'precheck.toxic.notFull' }
];
let precheckPassed = false;
let precheckRunning = false;
let precheckState = PRECHECK_STEPS.map(() => 'idle');
let CURRENT_USER = { id:'', username:'', name:'鏈櫥褰?, role:'guest' };
let backendUsers = [];
let backendRoles = [];
let backendUsersLoaded = false;
let backendUsersLoading = false;
const byName = new Map();
const byCategory = new Map();
let configProfiles = loadConfigProfiles();
let channelConfigAssignments = loadChannelConfigAssignments();
let selectedConfigId = configProfiles[0]?.id || null;
let activeConfigSection = 'files';
let activeProductionSection = 'status';
let activeDebugSection = 'com';
let selectedConfigStepIndex = 0;
let configBasicsCollapsed = true;

const svg = document.getElementById('twinSvg');
const zonesLayer = document.getElementById('zonesLayer');
const gridLayer = document.getElementById('gridLayer');
const traysLayer = document.getElementById('traysLayer');
const auxLayer = document.getElementById('auxLayer');
const dataLayer = document.getElementById('dataLayer');
const opsLayer = document.getElementById('opsLayer');
const pathLayer = document.getElementById('pathLayer');
const armLayer = document.getElementById('armLayer');
const axisLayer = document.getElementById('axisLayer');
const haloLayer = document.getElementById('haloLayer');
const labelLayer = document.getElementById('labelLayer');

function normalizeItem(r) {
  return {
    id: Number(r.id ?? r['搴忓彿']), category: String(r.category ?? r['绫诲埆'] ?? ''), name: String(r.name ?? r['鍚嶇О'] ?? ''),
    controlId: String(r.controlId ?? r.control_id ?? r['鎺т欢ID'] ?? ''),
    row: nullableNumber(r.row ?? r['琛?]), col: nullableNumber(r.col ?? r['鍒?]),
    x: nullableNumber(r.x ?? r['x_mm']), y: nullableNumber(r.y ?? r['y_mm']), shape: String(r.shape ?? r['褰㈢姸'] ?? ''),
    radius: nullableNumber(r.radius ?? r['鍗婂緞_mm']), width: nullableNumber(r.width ?? r['瀹藉害_mm']), height: nullableNumber(r.height ?? r['楂樺害_mm']),
    note: String(r.note ?? r['澶囨敞'] ?? '')
  };
}
function nullableNumber(v) { if(v === null || v === undefined || v === '') return null; const n = Number(v); return Number.isFinite(n) ? n : null; }
function rebuildIndexes() {
  byName.clear(); byCategory.clear();
  coords.forEach(item => { byName.set(item.name, item); if(!byCategory.has(item.category)) byCategory.set(item.category, []); byCategory.get(item.category).push(item); });
}
function calcTransform() {
  const drawable = coords.filter(d => !['鏈烘鑷傜浉瀵逛綅缃?].includes(d.category));
  const xs = drawable.map(d=>d.x).filter(Number.isFinite), ys = drawable.map(d=>d.y).filter(Number.isFinite);
  const minX = Math.min(-36, ...xs) - 5, maxX = Math.max(342, ...xs) + 5;
  const minY = Math.min(-18, ...ys) - 1, maxY = Math.max(222, ...ys) + 3;
  const scale = Math.min((VIEW.w - VIEW.padL - VIEW.padR) / (maxX - minX), (VIEW.h - VIEW.padT - VIEW.padB) / (maxY - minY));
  transform = { minX, maxX, minY, maxY, scale };
}
function mmToPx(x, y) { return [VIEW.padL + (transform.maxX - x) * transform.scale, VIEW.padT + (y - transform.minY) * transform.scale]; }
function pxToMm(px, py) { return [transform.maxX - (px - VIEW.padL) / transform.scale, transform.minY + (py - VIEW.padT) / transform.scale]; }
function mmSize(v) { return v * transform.scale; }
function el(name, attrs={}, children=[]) {
  const node = document.createElementNS('http://www.w3.org/2000/svg', name);
  for(const [k,v] of Object.entries(attrs)) { if(v !== null && v !== undefined) node.setAttribute(k, String(v)); }
  for(const child of children) node.appendChild(child);
  return node;
}

function normalizeControlId(value) {
  return String(value || '')
    .trim()
    .replace(/璇曞墏_/g, 'reagent-')
    .replace(/閰嶆恫_R(\d+)_C(\d+)/g, 'mix-p$1$2')
    .replace(/娣峰寑鐢垫満_(\d+)/g, 'motor-m$1')
    .replace(/娲楅拡澶確宸﹀垪_娲楀澹乢R(\d+)/g, 'wash-outer-row-$1')
    .replace(/娲楅拡澶確鍙冲垪_娲楀唴澹乢R(\d+)/g, 'wash-inner-row-$1')
    .replace(/閫氶亾(\d+)_娓呮礂瀛?g, 'clean-channel-$1')
    .replace(/搴熸恫瀛擾M(\d+)/g, 'waste-m$1')
    .replace(/鎺掓瘨瀛擾M(\d+)/g, 'toxic-m$1')
    .replace(/璇曞墏鎵爜鐩告満/g, 'camera-reagent-scanner')
    .replace(/鏈烘鑷傞殢鍔ㄧ浉鏈?g, 'camera-arm-follow')
    .replace(/閽堝ご_Z(\d+)/g, 'needle-z$1')
    .replace(/A娑?g, 'liquid-a')
    .replace(/B娑?g, 'liquid-b')
    .replace(/[\s_路/]+/g, '-')
    .replace(/[^A-Za-z0-9\-]+/g, '-')
    .replace(/-+/g, '-')
    .replace(/^-|-$/g, '')
    .toLowerCase();
}
function controlAttrs(id, attrs={}) {
  return { id, 'data-control-id': id, ...attrs };
}
function itemControlId(item) {
  if(!item) return 'svg-item-unknown';
  if(item.controlId) return item.controlId;
  if(item.category === '璇曞墏鍖?) return `svg-reagent-${String(item.name).match(/S\d+/i)?.[0].toLowerCase() || item.id}`;
  if(item.category === '娣峰悎娑蹭綋閰嶆恫鍖?) return `svg-mix-p${Number(item.row || 0)}${Number(item.col || 0)}`;
  if(item.category === 'A/B娑?) return `svg-${item.name === 'A娑? ? 'liquid-a' : 'liquid-b'}`;
  if(item.category === '娣峰寑鐢垫満') return `svg-motor-m${Number(item.col || item.id)}`;
  if(item.category === '鐜荤墖閫氶亾') return `svg-slide-${String(item.name).toLowerCase()}`;
  if(item.category === '娲楅拡澶?) return `svg-wash-${Number(item.col) === 2 ? 'inner' : 'outer'}-row-${Number(item.row || 0)}`;
  return `svg-${normalizeControlId(item.name || item.category || item.id)}`;
}
function clearLayer(layer) { while(layer.firstChild) layer.removeChild(layer.firstChild); }
function rectFromMm(xMin, yMin, xMax, yMax, cls='zone') {
  const [left, top] = mmToPx(xMax, yMin);
  const [right, bottom] = mmToPx(xMin, yMax);
  return el('rect', { x:left, y:top, width:right-left, height:bottom-top, class:cls });
}
function textSvg(x, y, text, cls='svg-label', anchor='middle') { return el('text', { x, y, class:cls, 'text-anchor':anchor, 'dominant-baseline':'central' }, [document.createTextNode(text)]); }

function badgeAtMm(centerX, centerY, widthMm, heightMm, text, variant='') {
  const [left, top] = mmToPx(centerX + widthMm/2, centerY - heightMm/2);
  const [right, bottom] = mmToPx(centerX - widthMm/2, centerY + heightMm/2);
  const g = el('g');
  g.appendChild(el('rect', { x:left, y:top, width:right-left, height:bottom-top, class:`zone-badge ${variant}` }));
  g.appendChild(textSvg((left+right)/2, (top+bottom)/2, text, 'zone-badge-text'));
  return g;
}
function drawWashSideLabels() {
  const wash = byCategory.get('娲楅拡澶?) || [];
  const outer = wash.filter(item => Number(item.col) === 1);
  const inner = wash.filter(item => Number(item.col) === 2);
  if(!outer.length || !inner.length) return;
  const avgY = arr => arr.reduce((sum, item) => sum + item.y, 0) / arr.length;
  const outerX = outer.reduce((sum, item) => sum + item.x, 0) / outer.length;
  const innerX = inner.reduce((sum, item) => sum + item.x, 0) / inner.length;
  const [xo, yo] = mmToPx(outerX + 5.0, avgY(outer));
  const [xi, yi] = mmToPx(innerX - 5.0, avgY(inner));
  const outerLabel = textSvg(xo, yo, '澶?, 'wash-side-label'); outerLabel.setAttribute('id', 'svg-wash-outer-side-label'); outerLabel.setAttribute('data-control-id', 'svg-wash-outer-side-label');
  const innerLabel = textSvg(xi, yi, '鍐?, 'wash-side-label'); innerLabel.setAttribute('id', 'svg-wash-inner-side-label'); innerLabel.setAttribute('data-control-id', 'svg-wash-inner-side-label');
  labelLayer.appendChild(outerLabel); labelLayer.appendChild(innerLabel);
}

function drawTopLabels() {
  const reagentBadge = badgeAtMm(268, -14, 82, 13, '璇曞墏鍖?, 'reagent'); reagentBadge.setAttribute('id', 'svg-zone-title-reagent'); reagentBadge.setAttribute('data-control-id', 'svg-zone-title-reagent'); labelLayer.appendChild(reagentBadge);
  const workBadge = badgeAtMm(61, -14, 118, 13, '鏌撹壊鍖?, 'work'); workBadge.setAttribute('id', 'svg-zone-title-work'); workBadge.setAttribute('data-control-id', 'svg-zone-title-work'); labelLayer.appendChild(workBadge);
  const serviceBadge = badgeAtMm(178, -14, 72, 13, '娲楅拡閰嶆恫鍖?, 'service'); serviceBadge.setAttribute('id', 'svg-zone-title-service'); serviceBadge.setAttribute('data-control-id', 'svg-zone-title-service'); labelLayer.appendChild(serviceBadge);
}

function drawReagentZoneLegend() {
  const rows = [
    [REAGENT_TYPES[0], REAGENT_TYPES[1], REAGENT_TYPES[2]],
    [REAGENT_TYPES[3], REAGENT_TYPES[4], REAGENT_TYPES[5]],
    [REAGENT_TYPES[6]]
  ];
  const xs = [316, 276, 236];
  const ys = [211.2, 215.0, 218.8];
  rows.forEach((row, r) => {
    row.forEach((rt, c) => {
      const [x, y] = mmToPx(xs[c], ys[r]);
      const legendId = `svg-reagent-legend-${rt.key || normalizeControlId(rt.label)}`;
      const g = el('g', controlAttrs(legendId, { class:'reagent-zone-legend-item' }));
      g.appendChild(el('circle', { id:`${legendId}-dot`, cx:x, cy:y, r:mmSize(1.7), fill:rt.color, class:'reagent-zone-legend-dot' }));
      const labelNode = el('text', { id:`${legendId}-label`, x:x + mmSize(3.4), y, class:'reagent-zone-legend-text', 'text-anchor':'start', 'dominant-baseline':'central' });
      labelNode.appendChild(document.createTextNode(`${rt.label}(`));
      labelNode.appendChild(el('tspan', { id:`${legendId}-remaining-ml`, 'data-control-id':`${legendId}-remaining-ml` }, [document.createTextNode(String(getReagentRemainingMlByType(rt)))]));
      labelNode.appendChild(document.createTextNode('ml)'));
      g.appendChild(labelNode);
      labelLayer.appendChild(g);
    });
  });
}

function getReagentLaneCenters() {
  const reagents = byCategory.get('璇曞墏鍖?) || [];
  const lanes = [];
  for(let c=1; c<=5; c++) {
    const items = reagents.filter(item => Number(item.col) === c);
    if(items.length) lanes.push({ lane:c, x:items.reduce((sum, item) => sum + item.x, 0) / items.length, items });
  }
  return lanes;
}
function drawReagentChannelSeparators() {
  const lanes = getReagentLaneCenters().sort((a,b) => a.lane - b.lane);
  if(lanes.length < 2) return;
  const all = lanes.flatMap(lane => lane.items);
  const y1Mm = Math.min(...all.map(item => item.y)) - 10.5;
  const y2Mm = Math.max(...all.map(item => item.y)) + 10.5;
  for(let i=0; i<lanes.length-1; i++) {
    const a = lanes[i], b = lanes[i+1];
    const xMm = (a.x + b.x) / 2;
    const [x1, y1] = mmToPx(xMm, y1Mm);
    const [x2, y2] = mmToPx(xMm, y2Mm);
    const lineId = `svg-reagent-lane-separator-${a.lane}-${b.lane}`;
    zonesLayer.appendChild(el('line', { id:lineId, 'data-control-id':lineId, x1, y1, x2, y2, class:'reagent-channel-separator' }));
  }
}
function sensorClassFromState(state) {
  return ['ready','active','error'].includes(state) ? state : 'idle';
}
function drawReagentSensors() {
  const lanes = getReagentLaneCenters().sort((a,b) => a.lane - b.lane);
  if(!lanes.length) return;
  const all = lanes.flatMap(lane => lane.items);
  const yTop = Math.min(...all.map(item => item.y)) - 13.0;
  const yBottom = Math.max(...all.map(item => item.y)) + 10.8;
  const statusText = state => state === 'ready' ? '鍒颁綅' : state === 'active' ? '妫€娴嬩腑' : state === 'error' ? '寮傚父' : '寰呮満';
  lanes.forEach(lane => {
    const [x, y] = mmToPx(lane.x, yTop);
    const size = mmSize(6.0);
    const sensorId = `svg-reagent-lane-${lane.lane}-position-sensor`;
    const state = sensorClassFromState(itemState.get(sensorId) || itemState.get(`璇曞墏閫氶亾${lane.lane}_鍒颁綅鎰熷簲`) || 'idle');
    const g = clickableGroup('g', controlAttrs(sensorId, { class:'reagent-lane-sensor', 'aria-label':`璇曞墏閫氶亾${lane.lane}鍒颁綅鎰熷簲` }));
    g.appendChild(el('rect', { id:`${sensorId}-light`, x:x-size/2, y:y-size/2, width:size, height:size, class:`sensor-light ${state}` }));
    g.addEventListener('click', evt => { evt.stopPropagation(); flashSvgControl(g); showPortDetail(`璇曞墏閫氶亾${lane.lane}鍒颁綅鎰熷簲`, [`鎺т欢ID锛?{sensorId}`, `鐘舵€侊細${statusText(state)}`, `浣嶇疆锛歑 ${Number(lane.x).toFixed(1)} / Y ${Number(yTop).toFixed(1)}`]); });
    auxLayer.appendChild(g);
  });
  lanes.forEach(lane => {
    const [ex, ey] = mmToPx(lane.x, yBottom);
    const size = mmSize(6.5);
    const entryId = `svg-reagent-lane-${lane.lane}-entry-sensor`;
    const state = sensorClassFromState(itemState.get(entryId) || itemState.get(`璇曞墏閫氶亾${lane.lane}_鍏ュ彛鎰熷簲`) || 'idle');
    const entry = clickableGroup('g', controlAttrs(entryId, { class:'reagent-entry-sensor', 'aria-label':`璇曞墏閫氶亾${lane.lane}鍏ュ彛鎰熷簲` }));
    entry.appendChild(el('rect', { id:`${entryId}-light`, x:ex-size/2, y:ey-size/2, width:size, height:size, class:`sensor-light ${state}` }));
    entry.addEventListener('click', evt => { evt.stopPropagation(); flashSvgControl(entry); showPortDetail(`璇曞墏閫氶亾${lane.lane}鍏ュ彛鎰熷簲`, [`鎺т欢ID锛?{entryId}`, `鐘舵€侊細${statusText(state)}`, `浣嶇疆锛歑 ${Number(lane.x).toFixed(1)} / Y ${Number(yBottom).toFixed(1)}`]); });
    auxLayer.appendChild(entry);
  });
}
function getMixBottleItems() {
  return (byCategory.get('娣峰悎娑蹭綋閰嶆恫鍖?) || []).slice().sort((a,b) => Number(a.row - b.row) || Number(a.col - b.col));
}
function isMixBottleEmpty(item) {
  return Number(itemLevels.get(item.name) ?? defaultLevelFor(item)) <= 0;
}
function isMixBottlePristine(item) {
  const state = itemState.get(item.name) || 'idle';
  return isMixBottleEmpty(item) && !['used','complete','running','ready','scanned'].includes(state);
}
function mixBottleVisualClass(item) {
  const level = Number(itemLevels.get(item.name) ?? defaultLevelFor(item));
  if(level > 0) return 'active';
  return isMixBottlePristine(item) ? 'empty' : 'used';
}
function drawMixBottleStatus() {
  const bottles = getMixBottleItems();
  if(!bottles.length) return;
  const emptyCount = bottles.filter(isMixBottlePristine).length;
  const anchor = byName.get('B娑?) || { x:177.75, y:191.5 };
  const [x, y] = mmToPx(anchor.x, anchor.y + 25.5);
  const cellW = mmSize(3.0), cellH = mmSize(4.2), gap = mmSize(0.85);
  const totalW = bottles.length * cellW + (bottles.length - 1) * gap;
  const g = clickableGroup('g', controlAttrs('svg-mix-empty-bottle-status', { class:'mix-empty-status' }));
  const boxW = totalW + mmSize(8), boxH = mmSize(14.5);
  g.appendChild(el('rect', { id:'svg-mix-empty-bottle-status-outline', x:x-boxW/2, y:y-boxH/2, width:boxW, height:boxH, class:'mix-empty-status-outline' }));
  const title = textSvg(x, y - mmSize(4.0), `绌虹摱 ${emptyCount}/8`, 'mix-empty-status-label'); title.setAttribute('id', 'svg-mix-empty-bottle-status-count'); title.setAttribute('data-control-id', 'svg-mix-empty-bottle-status-count'); g.appendChild(title);
  bottles.forEach((item, i) => {
    const cx = x - totalW/2 + i*(cellW+gap) + cellW/2;
    const cellId = `svg-mix-empty-bottle-cell-${i+1}`;
    const cls = mixBottleVisualClass(item);
    g.appendChild(el('rect', { id:cellId, 'data-control-id':cellId, x:cx-cellW/2, y:y + mmSize(1.2), width:cellW, height:cellH, class:`mix-empty-cell ${cls}` }));
  });
  g.addEventListener('click', evt => { evt.stopPropagation(); flashSvgControl(g); showPortDetail('閰嶆恫鐡剁┖鐡剁姸鎬?, [`鎺т欢ID锛歴vg-mix-empty-bottle-status`, `鍓╀綑绌虹摱锛?{emptyCount}/8`, `璇存槑锛氱豢鑹茶〃绀哄畬鍏ㄦ病鏈夐厤杩囨恫鐨勭┖鐡讹紝钃濊壊/娣辩伆琛ㄧず宸叉湁閰嶆恫璁板綍鎴栨鍦ㄤ娇鐢╜]); });
  auxLayer.appendChild(g);
}

function drawOperationLegend() {
  const [x1, y1] = mmToPx(151, 39.0);
  const [x2, y2] = mmToPx(-30, 66.2);
  const left = Math.min(x1, x2), top = Math.min(y1, y2);
  const width = Math.abs(x2 - x1), height = Math.abs(y2 - y1);
  const g = el('g', controlAttrs('svg-work-step-legend', { class:'work-op-legend' }));
  g.appendChild(el('rect', { id:'svg-work-step-legend-box', x:left, y:top, width, height, class:'work-op-legend-box' }));
  const xs = [145, 101, 57, 13];
  const legendRows = Math.ceil(OP_DEFS.length / 4);
  const legendStepY = 5.4;
  const legendCenterY = (39.0 + 66.2) / 2;
  const ys = Array.from({length:legendRows}, (_, row) => legendCenterY - ((legendRows - 1) * legendStepY / 2) + row * legendStepY);
  OP_DEFS.forEach((op, idx) => {
    const col = idx % 4;
    const row = Math.floor(idx / 4);
    const [cx, cy] = mmToPx(xs[col], ys[row]);
    const opLegendId = `svg-work-step-legend-${op.key}`;
    g.appendChild(createOpShape(op.shape, cx, cy, 3.15, false, op.color, '', { id:`${opLegendId}-marker`, 'data-control-id':opLegendId }));
    g.appendChild(el('text', { id:`${opLegendId}-label`, x:cx + mmSize(2.8), y:cy, class:'work-op-legend-text', 'text-anchor':'start', 'dominant-baseline':'central' }, [document.createTextNode(op.label)]));
  });
  zonesLayer.appendChild(g);
}
function polarToCartesian(cx, cy, r, angle) {
  const rad = (angle - 90) * Math.PI / 180.0;
  return { x: cx + r * Math.cos(rad), y: cy + r * Math.sin(rad) };
}
function describeArc(cx, cy, r, startAngle, endAngle) {
  const start = polarToCartesian(cx, cy, r, endAngle);
  const end = polarToCartesian(cx, cy, r, startAngle);
  const largeArcFlag = endAngle - startAngle <= 180 ? '0' : '1';
  return ['M', start.x, start.y, 'A', r, r, 0, largeArcFlag, 0, end.x, end.y].join(' ');
}
function describeSector(cx, cy, r, startAngle, endAngle) {
  const safeEnd = Math.max(startAngle + 1, Math.min(endAngle, startAngle + 359.99));
  const start = polarToCartesian(cx, cy, r, startAngle);
  const end = polarToCartesian(cx, cy, r, safeEnd);
  const largeArcFlag = safeEnd - startAngle <= 180 ? '0' : '1';
  return ['M', cx, cy, 'L', start.x, start.y, 'A', r, r, 0, largeArcFlag, 1, end.x, end.y, 'Z'].join(' ');
}
function createOpShape(shape, cx, cy, size, filled, color, label='', attrs={}) {
  const common = { ...attrs, class:`op-marker ${attrs.class || ''}`.trim(), stroke:color, fill:filled ? color : '#ffffff' };
  if(shape === 'circle') return el('circle', { cx, cy, r:size, ...common });
  if(shape === 'square') return el('rect', { x:cx-size, y:cy-size, width:size*2, height:size*2, rx:2, ...common });
  if(shape === 'triangle') return el('path', { d:`M ${cx} ${cy-size} L ${cx+size} ${cy+size} L ${cx-size} ${cy+size} Z`, ...common });
  return el('path', { d:`M ${cx} ${cy-size} L ${cx+size} ${cy} L ${cx} ${cy+size} L ${cx-size} ${cy} Z`, ...common });
}
function getHomePosition() {
  const t = findTarget(HOME_TARGET_NAME);
  return t ? { x:t.x, y:t.y } : { x:177.75, y:17.0 };
}
function ensureVisualData() {
  const valid = new Set(coords.map(d => d.name));
  for(const key of [...itemLevels.keys()]) if(!valid.has(key)) itemLevels.delete(key);
  for(const key of [...slideOps.keys()]) if(!valid.has(key)) slideOps.delete(key);
  for(const key of [...slideTemps.keys()]) if(!valid.has(key)) slideTemps.delete(key);
  coords.forEach(item => {
    if(['璇曞墏鍖?,'娣峰悎娑蹭綋閰嶆恫鍖?,'A/B娑?].includes(item.category) && !itemLevels.has(item.name)) itemLevels.set(item.name, defaultLevelFor(item));
    if(item.category === '鐜荤墖閫氶亾') {
      const ch = getChannelIdFromName(item.name);
      const stepLen = getChannelStepDefs(ch).length;
      const existing = slideOps.get(item.name);
      if(!Array.isArray(existing) || existing.length !== stepLen) slideOps.set(item.name, Array.from({length:stepLen}, () => false));
      if(!slideTemps.has(item.name)) slideTemps.set(item.name, defaultSlideTemp(item));
    }
  });
}
function defaultLevelFor(item) {
  return null;
}

function defaultSlideTemp(item) {
  return null;
}
function getSlideTemp(itemOrName) {
  const name = typeof itemOrName === 'string' ? itemOrName : itemOrName?.name;
  if(!name) return null;
  return slideTemps.has(name) ? slideTemps.get(name) : null;
}
function formatDbValue(value, suffix='', formatter=null) {
  if(value === null || value === undefined || value === '' || !Number.isFinite(Number(value))) return '鈥?;
  return (formatter ? formatter(Number(value)) : String(value)) + suffix;
}
function getStepDef(idx, context=null) {
  const channelId = typeof context === 'number' ? context : getChannelIdFromName(typeof context === 'string' ? context : context?.name);
  const dynamicSteps = channelId ? getChannelStepDefs(channelId) : [];
  if(dynamicSteps[idx]) return dynamicSteps[idx];
  return OP_DEFS[idx] || { key:`step${idx+1}`, label:`姝ラ${idx+1}`, shape:OP_DEFS[idx % OP_DEFS.length].shape, color:OP_DEFS[idx % OP_DEFS.length].color };
}
function getSlideOpSlots(cx, cy, w, h, count) {
  const off = mmSize(3.7), sideOff = mmSize(3.9);
  const leftX = cx - w/2 - sideOff;
  const rightX = cx + w/2 + sideOff;
  const bottomY = cy + h/2 + off;
  const leftYs = [cy - h*0.42, cy - h*0.14, cy + h*0.14, cy + h*0.42];
  const bottomXs = [cx - w*0.42, cx - w*0.14, cx + w*0.14, cx + w*0.42];
  const rightYs = [cy + h*0.42, cy + h*0.14, cy - h*0.14, cy - h*0.42];
  const slots = [
    [leftX, leftYs[0]], [leftX, leftYs[1]], [leftX, leftYs[2]], [leftX, leftYs[3]],
    [bottomXs[0], bottomY], [bottomXs[1], bottomY], [bottomXs[2], bottomY], [bottomXs[3], bottomY],
    [rightX, rightYs[0]], [rightX, rightYs[1]], [rightX, rightYs[2]], [rightX, rightYs[3]]
  ];
  return slots.slice(0, Math.min(count, 12));
}
function setInfoPanel(title, lines=[]) {
  detailMessage = `<strong>${escapeHtml(title)}</strong>${(Array.isArray(lines)?lines:[lines]).filter(Boolean).map(line => `<div>${escapeHtml(String(line))}</div>`).join('')}`;
  renderDetail(null);
}
function showQueueInfo() {
  const lines = DEMO_STEPS.map((step, idx) => `${idx+1}. ${step.label}`);
  setInfoPanel('娴佺▼闃熷垪 / 褰撳墠婕旂ず', lines);
}

function loadAppSettings() {
  try {
    const raw = localStorage.getItem(SETTINGS_STORAGE_KEY);
    return { ...DEFAULT_APP_SETTINGS, ...(raw ? JSON.parse(raw) : {}) };
  } catch(e) { return { ...DEFAULT_APP_SETTINGS }; }
}
function saveAppSettings() {
  try { localStorage.setItem(SETTINGS_STORAGE_KEY, JSON.stringify(appSettings)); } catch(e) {}
}
function resetAppSettings() {
  appSettings = { ...DEFAULT_APP_SETTINGS };
  saveAppSettings();
  openSettingsPage('绯荤粺璁剧疆');
  renderAll();
  log('璁剧疆宸叉仮澶嶉粯璁ゅ€?, 'warn');
}
function getReagentBottleCapacityMl() {
  return Math.max(0, Number(appSettings?.reagentBottleCapacityMl ?? DEFAULT_APP_SETTINGS.reagentBottleCapacityMl));
}
function getReagentRemainingMlByType(rt) {
  const capacity = getReagentBottleCapacityMl();
  const reagents = (byCategory.get('璇曞墏鍖?) || []).filter(item => reagentType(item).key === rt.key);
  const total = reagents.reduce((sum, item) => sum + ((itemLevels.get(item.name) ?? defaultLevelFor(item)) / 100) * capacity, 0);
  return Math.round(total);
}
function saveSettingsFromPane() {
  const get = id => document.getElementById(id);
  appSettings = {
    ...appSettings,
    dataInterface:get('settingsDataInterfaceInput')?.value || DEFAULT_APP_SETTINGS.dataInterface,
    hostAddress:get('settingsHostAddressInput')?.value.trim() || DEFAULT_APP_SETTINGS.hostAddress,
    heartbeatSec:Math.max(1, Number(get('settingsHeartbeatInput')?.value || DEFAULT_APP_SETTINGS.heartbeatSec)),
    reagentBottleCapacityMl:(() => { const v = get('settingsReagentCapacityInput')?.value; return v === undefined || v === '' ? null : Math.max(0, Number(v)); })(),
    reagentTargetTempC:(() => { const v = get('settingsReagentTargetInput')?.value; return v === undefined || v === '' ? null : Number(v); })(),
    workTargetTempC:(() => { const v = get('settingsWorkTargetInput')?.value; return v === undefined || v === '' ? null : Number(v); })(),
    needleGapMm:(() => { const v = get('settingsNeedleGapInput')?.value; return v === undefined || v === '' ? null : Math.max(0, Number(v)); })(),
    pureLowThresholdPct:(() => { const v = get('settingsPureThresholdInput')?.value; return v === undefined || v === '' ? null : Math.max(0, Number(v)); })(),
    pbsLowThresholdPct:(() => { const v = get('settingsPbsThresholdInput')?.value; return v === undefined || v === '' ? null : Math.max(0, Number(v)); })(),
    wasteFullThresholdPct:(() => { const v = get('settingsWasteThresholdInput')?.value; return v === undefined || v === '' ? null : Math.max(0, Number(v)); })(),
    toxicFullThresholdPct:(() => { const v = get('settingsToxicThresholdInput')?.value; return v === undefined || v === '' ? null : Math.max(0, Number(v)); })(),
    maxVisibleSlideSteps:12,
    logRetention:Math.max(50, Number(get('settingsLogRetentionInput')?.value || DEFAULT_APP_SETTINGS.logRetention)),
    autoRunPrecheck:(get('settingsAutoPrecheckInput')?.value === 'true')
  };
  saveAppSettings();
  renderAll();
  updateKpis();
  log('绯荤粺璁剧疆宸蹭繚瀛橈細鏈湴鍘熷瀷缂撳瓨宸叉洿鏂?, 'ok');
}


function reagentType(item) {
  const rowIndex = Math.max(1, Number(item?.row || 1));
  const mapped = REAGENT_TYPE_LAYOUT_BY_ROW[Math.min(REAGENT_TYPE_LAYOUT_BY_ROW.length - 1, rowIndex - 1)];
  return REAGENT_TYPES[mapped] || REAGENT_TYPES[0];
}
function reagentTypeLabel(item) {
  if(item.category !== '璇曞墏鍖?) return '';
  return reagentType(item).label;
}
function letterForChannel(channelId) { return 'ABCD'[(channelId - 1)] || 'A'; }

// 鏍锋湰鍏ュ簱 = 鍒涘缓鏌撹壊浠诲姟銆傛敞鎰忥細褰撳墠鍚庣娌℃湁涓撶敤鐨勩€屾牱鏈壂鐮佸櫒/鏍锋湰鍏ュ簱銆嶇‖浠?API锛?
// 鍙湁 /api/device/reagent-scanner/qr/*锛堣瘯鍓傛壂鐮佸櫒锛夈€傛牱鏈晶鐨勭湡瀹炶惤鐐规槸浠诲姟鍒涘缓閾捐矾锛?
// 鍥犳杩欓噷璧?channel-batches/active + experiment-type-selection + tasks/he|ihc锛?
// 缁濅笉璇帴鍒?reagent-scanner/qr銆傜‖浠舵牱鏈壂鐮佸櫒鍏ュ簱 API 寰呭悗绔ˉ鍏咃紙TODO锛夈€?
async function intakeChannelSamples(channelId) {
  const letter = letterForChannel(channelId);
  const ch = channels[channelId - 1];
  if(!ch) return;
  if(channelScanningBusy) { log(`閫氶亾${letter} 鎵爜绻佸繖锛岃绋嶅悗鍐嶈瘯`, 'warn', channelId); return; }
  const experimentType = String(prompt(`閫氶亾 ${letter} 鏍锋湰鍏ュ簱\n瀹為獙绫诲瀷锛圚E / IHC锛夛細`, 'HE') || '').trim().toUpperCase();
  if(!['HE','IHC'].includes(experimentType)) { log(`閫氶亾${letter} 鏍锋湰鍏ュ簱宸插彇娑坄, 'warn', channelId); return; }
  let rawCode = '';
  if(experimentType === 'IHC') {
    rawCode = String(prompt(`閫氶亾 ${letter} IHC锛氳緭鍏ユ牱鏈爜 / 涓€鎶楃爜`, 'P01') || '').trim();
    if(!rawCode) { log('IHC 鏍锋湰闇€瑕佹牱鏈爜 / 涓€鎶楃爜', 'warn', channelId); return; }
  }
  channelScanningBusy = true;
  try {
    setInfoPanel(`閫氶亾${letter} 鏍锋湰鍏ュ簱涓璥, ['姝ｅ湪寤虹珛閫氶亾鎵规骞跺垱寤轰换鍔?..']);
    const activeResp = await writeApi('/api/channel-batches/active', { method:'POST', body: JSON.stringify({ commandId: makeCommandId('channel-active'), drawerCode: letter }) });
    if(activeResp?.workflowSelectionStatus !== 'Selected') {
      await writeApi('/api/channel-batches/experiment-type-selection', { method:'POST', body: JSON.stringify({ commandId: makeCommandId('channel-exp-type'), drawerCode: letter, experimentType }) });
    }
    const channelBatchId = activeResp?.channelBatchId || null;
    if(!channelBatchId) { log(`閫氶亾${letter} 鏈幏鍙栧埌娲诲姩閫氶亾鎵规`, 'err', channelId); return; }
    let lis = null;
    if(experimentType === 'IHC') {
      lis = await writeApi('/api/lis/mock-query', { method:'POST', body: JSON.stringify({ commandId: makeCommandId('lis-query'), rawCode }) });
    }
    let created = null, stopped = false, lastSlotCode = '';
    for(let slotNo=1; slotNo<=4 && !stopped; slotNo++) {
      const slotCode = `${letter}-${String(slotNo).padStart(2,'0')}`;
      lastSlotCode = slotCode;
      const body = experimentType === 'HE'
        ? { commandId: makeCommandId('task-he'), slotCode, drawerCode: letter, channelBatchId }
        : { commandId: makeCommandId('task-ihc'), inputMode:'PrimaryAntibody', rawCode, slotCode, drawerCode: letter, channelBatchId, selectedPrimaryAntibodyCode: lis?.candidatePrimaryAntibodyCodes?.[0] || rawCode, lisQueryLogId: lis?.lisQueryLogId || null };
      try {
        created = await backendApi('/api/tasks/' + (experimentType === 'HE' ? 'he' : 'ihc'), { method:'POST', body: JSON.stringify(body) });
        log(`閫氶亾${letter} 鏍锋湰鍏ュ簱瀹屾垚锛?{slotCode} (${experimentType})`, 'ok', channelId);
        break;
      } catch(err) {
        if(err?.status === 409 && (err?.data?.code === 'slot_not_idle' || /not idle/i.test(err.message || ''))) { continue; }
        routeWriteError(err); stopped = true;
      }
    }
    if(stopped) { /* non-slot error already routed */ }
    else if(!created) { log(`閫氶亾${letter} 1-4 鍙蜂綅鍧囦笉鍙敤锛堟棤绌洪棽 Slot锛塦, 'warn', channelId); }
    else { ch.barcodeReady = true; }
    await loadDatabaseSnapshot();
  } catch(err) { /* routed by writeApi */ }
  finally { channelScanningBusy = false; }
}

async function startChannelBarcode(channelId) {
  const ch = channels[channelId - 1]; if(!ch) return;
  if(channelScanningBusy) {
    setInfoPanel('鎵爜浠诲姟绻佸繖', ['鏈烘鑷傛牱鏈壂鐮佸櫒姝ｅ湪鎵ц鍏朵粬閫氶亾鐨勬壂鐮佷换鍔★紝璇风◢鍚庡啀璇曘€?]);
    log(`閫氶亾${channelId}鎵爜浠诲姟琚嫤鎴細鏈烘鑷傛牱鏈壂鐮佸櫒姝ｅ湪蹇欑`, 'warn', channelId);
    return;
  }
  if(running && !paused) {
    setInfoPanel('娴佺▼杩愯涓?, [`涓绘紨绀烘祦绋嬫鍦ㄨ繍琛岋紝閫氶亾${channelId}鐨勬壂鐮佷换鍔℃棤娉曚笅鍙戙€俙, '璇峰厛鐐瑰嚮鏆傚仠锛屾垨绛夊緟涓绘祦绋嬪畬鎴愬悗鍐嶈瘯銆?]);
    log(`閫氶亾${channelId}鎵爜浠诲姟琚嫤鎴細涓绘紨绀烘祦绋嬭繍琛屼腑`, 'warn', channelId);
    return;
  }
  const allSlides = (byCategory.get('鐜荤墖閫氶亾') || []).filter(s => getChannelIdFromName(s.name) === channelId);
  const newSlides = allSlides.filter(s => itemState.get(s.name) !== 'scanned');
  if(!newSlides.length) {
    selectedChannel = channelId;
    setInfoPanel(`閫氶亾${channelId}鏃犻渶鏂板鎵爜`, [`閫氶亾${channelId} 鐨勬墍鏈夌幓鐗囧凡瀹屾垚鏉＄爜鎵弿鍏ュ簱銆俙, `鐜荤墖鍒楄〃锛?{allSlides.map(s=>s.name).join('銆?)}`]);
    showChannelDetail(channelId);
    log(`閫氶亾${channelId}鐐瑰嚮寮€濮嬶細鎵€鏈夌幓鐗囧凡鎵弿锛屾棤闇€閲嶅鎿嶄綔`, '', channelId);
    return;
  }
  channelScanningBusy = true;
  cancelAnimation = false;
  selectedChannel = channelId;
  cameraStates.arm = 'active';
  updateArmVisual();
  const slideNames = newSlides.map(s => s.name);
  setInfoPanel(`閫氶亾${channelId}宸蹭笅鍙戞壂鐮佷换鍔, [
    '鏈烘鑷傛牱鏈壂鐮佸櫒姝ｅ湪鍓嶅線鎵弿浠ヤ笅鏂板鐜荤墖锛?,
    ...slideNames.map((name, i) => `${i+1}. ${name} (X ${Number(newSlides[i].x).toFixed(1)} / Y ${Number(newSlides[i].y).toFixed(1)})`),
    '璇风瓑寰呮壂鎻忓畬鎴?..'
  ]);
  log(`閫氶亾${channelId}宸蹭笅鍙戞壂鐮佷换鍔★細鏈烘鑷傛牱鏈壂鐮佸櫒姝ｅ湪鍓嶅線鎵弿 ${newSlides.length} 寮犳柊澧炵幓鐗嘸, 'ok', channelId);
  let scanned = 0;
  for(let i=0; i<newSlides.length; i++) {
    if(cancelAnimation) break;
    const s = newSlides[i];
    const targetX = Number(s.x), targetY = Number(s.y);
    log(`閫氶亾${channelId}鎵爜杩涘害锛氭満姊拌噦绉诲姩鍒?${s.name}`, '', channelId);
    const ok = await animateArmTo(targetX, targetY, 0.18, 0.18, 800);
    if(!ok) {
      log(`閫氶亾${channelId}鎵爜浠诲姟涓柇锛氬姩鐢昏鍙栨秷锛堝凡鎵弿 ${scanned}/${newSlides.length}锛塦, 'warn', channelId);
      break;
    }
    await sleep(200);
    itemState.set(s.name, 'scanned');
    setSlideOp(s.name, 0, true);
    scanned++;
    const remaining = newSlides.slice(i+1).map(x => x.name);
    setInfoPanel(`閫氶亾${channelId}鎵爜杩涘害 (${scanned}/${newSlides.length})`, [
      `宸叉壂鐮侊細${s.name} 鉁揱,
      ...(remaining.length ? ['寰呮壂鐮侊細' + remaining.join('銆?)] : ['鍏ㄩ儴鐜荤墖鎵弿瀹屾垚锛?]),
      `閫氶亾${channelId} 鏂板鐜荤墖姝ｅ湪鍔犲叆鏌撹壊浠诲姟...`
    ]);
    log(`閫氶亾${channelId}鎵爜锛?{s.name} 鎵弿瀹屾垚骞跺叆搴?(${scanned}/${newSlides.length})`, 'ok', channelId);
    updateVisualStates();
    drawSlideOps();
  }
  cameraStates.arm = 'complete';
  updateArmVisual();
  if(scanned > 0) {
    ch.barcodeReady = true;
    ch.progress = Math.max(ch.progress, 5);
    ch.state = ch.state === 'idle' ? 'ready' : ch.state;
    allSlides.forEach(s => {
      if(itemState.get(s.name) !== 'scanned') itemState.set(s.name, 'scanned');
      setSlideOp(s.name, 0, true);
    });
  }
  channelScanningBusy = false;
  updateVisualStates();
  renderChannelCards();
  if(scanned === newSlides.length) {
    log(`閫氶亾${channelId}鎵爜浠诲姟瀹屾垚锛?{scanned} 寮犵幓鐗囧凡鍏ュ簱锛屾煋鑹蹭换鍔″凡灏辩华`, 'ok', channelId);
  }
  showChannelDetail(channelId);
}
function showReagentCoolingDetail(xMm=null, yMm=null) {
  selectedName = null;
  selectedSvgControlId = 'svg-reagent-temperature-control';
  showSideTab('production');
  const box = document.getElementById('detailBox'); if(!box) return;
  box.innerHTML = `<div class="scanner-detail-title"><strong>璇曞墏娓╁害 / 璇曞墏鍒跺喎鎺у埗</strong><span class="engineering-pill">svg-reagent-temperature-control</span></div>
    <div>褰撳墠娓╁害锛?{currentReagentTemp()}銆€鐩爣娓╁害锛?{Number(appSettings?.reagentTargetTempC ?? 8).toFixed(1)}鈩冦€€浣嶇疆锛歑 ${Number(xMm ?? 0).toFixed(1)} / Y ${Number(yMm ?? 0).toFixed(1)}</div>
    <div class="object-engineering-panel" id="svg-reagent-temperature-control-object-engineering" data-control-id="svg-reagent-temperature-control-object-engineering">
      <section class="object-engineering-card" id="reagentCoolingControlCard" data-control-id="reagentCoolingControlCard"><strong>璇曞墏鍒跺喎鎺у埗<span class="engineering-pill">瀵硅薄璇︽儏</span></strong><div class="object-form-grid"><label>鍒跺喎娓╁害<input id="reagentCoolingTargetInput" data-control-id="reagentCoolingTargetInput" value="${Number(appSettings?.reagentTargetTempC ?? 8).toFixed(1)}鈩?></label><label>褰撳墠娓╁害<input id="reagentCoolingCurrentInput" data-control-id="reagentCoolingCurrentInput" value="${currentReagentTemp()}" readonly></label></div><div class="object-actions"><button type="button" class="ok-lite" data-object-action="璁剧疆璇曞墏鍒跺喎娓╁害" data-object-name="璇曞墏娓╁害">璁剧疆娓╁害</button><button type="button" data-object-action="鏌ヨ璇曞墏鍒跺喎娓╁害" data-object-name="璇曞墏娓╁害">鏌ヨ娓╁害</button><button type="button" class="primary-lite" data-object-action="鍚姩璇曞墏鍒跺喎" data-object-name="璇曞墏娓╁害">鍚姩鍒跺喎</button><button type="button" class="warn-lite" data-object-action="鍋滄璇曞墏鍒跺喎" data-object-name="璇曞墏娓╁害">鍋滄鍒跺喎</button></div><div class="object-note">璇ュ尯鐢卞師鈥滈厤缃?鈫?娓呮礂娣峰寑 鈫?璇曞墏鍒跺喎鎺у埗鈥濊縼绉昏€屾潵锛岄€氳繃鐐瑰嚮瀛敓椤甸潰璇曞墏娓╁害杩涘叆銆?/div></section>
    </div>`;
  bindGenericObjectActionButtons(box);
}
function showWaterModuleDetail(channelId, point={}) {
  selectedName = null;
  selectedSvgControlId = `svg-port-clean-channel-${channelId}`;
  showSideTab('production');
  const box = document.getElementById('detailBox'); if(!box) return;
  box.innerHTML = `<div class="scanner-detail-title"><strong>閫氶亾${channelId}渚涙按瀛?/ 渚涙按妯″潡鎺у埗</strong><span class="engineering-pill">svg-port-clean-channel-${channelId}</span></div>
    <div>閫氶亾杩涘害锛?{Math.round(channels[channelId-1]?.progress || 0)}%銆€浣嶇疆锛歑 ${Number(point.x ?? 0).toFixed(1)} / Y ${Number(point.y ?? 0).toFixed(1)}</div>
    <div class="object-engineering-panel" id="svg-port-clean-channel-${channelId}-object-engineering" data-control-id="svg-port-clean-channel-${channelId}-object-engineering">
      <section class="object-engineering-card" id="waterModuleChannel${channelId}Card" data-control-id="waterModuleChannel${channelId}Card"><strong>渚涙按妯″潡鎺у埗<span class="engineering-pill">閫氶亾${channelId}</span></strong><div class="object-form-grid three"><label>杩涙按娓╁害<input id="waterChannel${channelId}InletTempInput" data-control-id="waterChannel${channelId}InletTempInput" value="25鈩?></label><label>鍑烘按鐩爣娓╁害<input id="waterChannel${channelId}OutletTargetTempInput" data-control-id="waterChannel${channelId}OutletTargetTempInput" value="45鈩?></label><label>鍑烘按娓╁害<input id="waterChannel${channelId}OutletTempInput" data-control-id="waterChannel${channelId}OutletTempInput" value="25鈩?></label><label>鍑烘按姘撮噺<input id="waterChannel${channelId}OutletVolumeInput" data-control-id="waterChannel${channelId}OutletVolumeInput" value="2500mL"></label><label>鍑烘按娴侀€?input id="waterChannel${channelId}OutletFlowInput" data-control-id="waterChannel${channelId}OutletFlowInput" value="250mL/min"></label><label>鍑烘按寮€鍏?select id="waterChannel${channelId}OutletSwitchSelect" data-control-id="waterChannel${channelId}OutletSwitchSelect"><option>鍏抽棴</option><option>鎵撳紑</option></select></label></div><div class="object-actions"><button type="button" data-object-action="鏌ヨ杩涙按娓╁害" data-object-name="閫氶亾${channelId}渚涙按瀛?>鏌ヨ杩涙按娓╁害</button><button type="button" data-object-action="鏌ヨ鍑烘按娓╁害" data-object-name="閫氶亾${channelId}渚涙按瀛?>鏌ヨ鍑烘按娓╁害</button><button type="button" data-object-action="鏌ヨ鍑烘按鐩爣娓╁害" data-object-name="閫氶亾${channelId}渚涙按瀛?>鏌ヨ鐩爣娓╁害</button><button type="button" data-object-action="鏌ヨ鍑烘按姘撮噺" data-object-name="閫氶亾${channelId}渚涙按瀛?>鏌ヨ姘撮噺</button><button type="button" data-object-action="鏌ヨ鍑烘按娴侀€? data-object-name="閫氶亾${channelId}渚涙按瀛?>鏌ヨ娴侀€?/button><button type="button" class="primary-lite" data-object-action="鍒囨崲鍑烘按寮€鍏? data-object-name="閫氶亾${channelId}渚涙按瀛?>鍑烘按寮€鍏?/button></div><div class="object-note">璇ュ尯鐢卞師鈥滈厤缃?鈫?娓呮礂娣峰寑 鈫?渚涙按妯″潡鎺у埗鈥濊縼绉昏€屾潵锛岄€氳繃鐐瑰嚮鏌撹壊鍖?4 涓緵姘?娓呮礂瀛旇繘鍏ャ€?/div></section>
    </div>`;
  bindGenericObjectActionButtons(box);
}
function showChannelDetail(channelId) {
  const ch = channels[channelId - 1]; if(!ch) return;
  selectedChannel = channelId;
  showSideTab('production');
  const box = document.getElementById('detailBox'); if(!box) return;
  const state = ch.pulled ? 'open' : ch.state;
  box.innerHTML = `<div class="channel-config-box" id="channel${channelId}ConfigBox" data-control-id="channel${channelId}ConfigBox">
    <div class="channel-config-line"><strong>閫氶亾${channelId}鐘舵€?/strong><span>鐘舵€侊細${escapeHtml(STATUS_TEXT[state] || state)}銆€杩涘害锛?{Math.round(ch.progress)}%銆€鐜荤墖鏌撹壊锛?{escapeHtml(ch.slides.join(' / '))}銆€鏉＄爜鍏ュ簱锛?{ch.barcodeReady ? '宸插紑濮?宸插叆搴? : '鏈紑濮?}</span></div>
    ${renderChannelBindingEditor(channelId, 'status')}
  </div>`;
  bindChannelBindingHandlers(box);
  updateVisualStates(); renderChannelCards();
}
function showPortDetail(title, lines=[]) { setInfoPanel(title, lines); showSideTab('status'); }
function scannerLabelByKey(key) { return key === 'sample' ? '鏍锋湰鎵爜鍣紙鏈烘鑷傞殢鍔ㄧ浉鏈猴級' : '璇曞墏鎵爜鍣?; }
function scannerSvgIdByKey(key) { return key === 'sample' ? 'svg-arm-camera-follow' : 'svg-camera-reagent-scanner'; }
function showScannerDetail(key='reagent', xMm=null, yMm=null) {
  const scannerKey = key === 'sample' ? 'sample' : 'reagent';
  const label = scannerLabelByKey(scannerKey);
  const svgId = scannerSvgIdByKey(scannerKey);
  selectedName = null;
  selectedSvgControlId = svgId;
  showSideTab('production');
  const box = document.getElementById('detailBox'); if(!box) return;
  const defaults = scannerKey === 'sample'
    ? { com:'COM1', left:12, top:16, width:140, height:90, title:'鏍锋湰鎵爜鍣?ROI' }
    : { com:'COM2', left:0, top:0, width:120, height:80, title:'璇曞墏鎵爜鍣?ROI' };
  box.innerHTML = `<div class="scanner-detail-title"><strong>${label}</strong><span class="engineering-pill">${svgId}</span></div>
    <div>鐘舵€侊細${cameraStateLabel(scannerKey === 'reagent' ? cameraStates.reagent : cameraStates.arm)}銆€浣嶇疆锛歑 ${Number(xMm ?? 0).toFixed(1)} / Y ${Number(yMm ?? 0).toFixed(1)}</div>
    <div class="object-engineering-panel" id="${svgId}-object-engineering" data-control-id="${svgId}-object-engineering">
      <section class="object-engineering-card" id="${svgId}-scanner-com-card" data-control-id="${svgId}-scanner-com-card"><strong>COM 鍙ｈ缃?span class="engineering-pill">${label}</span></strong><div class="object-form-grid three"><label>璁惧<input value="${label}" readonly></label><label>COM鍙?select id="${svgId}-com" data-control-id="${svgId}-com"><option ${defaults.com==='COM1'?'selected':''}>COM1</option><option ${defaults.com==='COM2'?'selected':''}>COM2</option><option>COM3</option><option>COM4</option></select></label><label>娉㈢壒鐜?select id="${svgId}-baud" data-control-id="${svgId}-baud"><option selected>115200</option><option>57600</option><option>38400</option><option>9600</option></select></label><label>鏁版嵁浣?input value="8 bits" readonly></label><label>鍋滄浣?input value="1 bits" readonly></label><label>鏍￠獙浣?input value="鏃? readonly></label></div><div class="object-actions"><button type="button" class="ok-lite" data-object-action="璁剧疆COM鍙? data-object-name="${label}">璁剧疆COM鍙?/button><button type="button" data-object-action="璇诲彇COM鍙? data-object-name="${label}">璇诲彇COM鍙?/button><button type="button" class="warn-lite" data-object-action="閲嶆柊鍚姩" data-object-name="${label}">閲嶆柊鍚姩</button></div></section>
      <section class="object-engineering-card" id="${svgId}-scanner-init-card" data-control-id="${svgId}-scanner-init-card"><strong>鍒濆鍖?/ 閫氫俊妯″紡<span class="engineering-pill">涓插彛</span></strong><div class="object-actions"><button type="button" data-object-action="鎭㈠鍑哄巶" data-object-name="${label}">鎭㈠鍑哄巶</button><button type="button" data-object-action="Raw Model妯″紡" data-object-name="${label}">Raw Model</button><button type="button" data-object-action="婵€娲?6杩涘埗涓插彛閫氫俊" data-object-name="${label}">16杩涘埗閫氫俊</button></div></section>
      <section class="object-engineering-card" id="${svgId}-scanner-light-trigger-card" data-control-id="${svgId}-scanner-light-trigger-card"><strong>鏍″噯鍏?/ 瑙﹀彂妯″紡<span class="engineering-pill">DCR55</span></strong><div class="object-actions"><button type="button" class="primary-lite" data-object-action="鎵撳紑鏍″噯鍏? data-object-name="${label}">鎵撳紑鏍″噯鍏?/button><button type="button" data-object-action="鍏抽棴鏍″噯鍏? data-object-name="${label}">鍏抽棴鏍″噯鍏?/button><button type="button" class="primary-lite" data-object-action="婵€娲诲崟娆¤Е鍙? data-object-name="${label}">鍗曟瑙﹀彂</button><button type="button" data-object-action="鍋滄鍗曟瑙﹀彂" data-object-name="${label}">鍋滄鍗曟</button><button type="button" class="primary-lite" data-object-action="婵€娲昏繛缁Е鍙? data-object-name="${label}">杩炵画瑙﹀彂</button><button type="button" class="warn-lite" data-object-action="鍋滄杩炵画瑙﹀彂" data-object-name="${label}">鍋滄杩炵画</button></div></section>
      <section class="object-engineering-card" id="${svgId}-scanner-roi-card" data-control-id="${svgId}-scanner-roi-card"><strong>${defaults.title}<span class="engineering-pill">ROI</span></strong><div class="object-form-grid"><label>宸?input id="${svgId}-roi-left" data-control-id="${svgId}-roi-left" type="number" value="${defaults.left}"></label><label>涓?input id="${svgId}-roi-top" data-control-id="${svgId}-roi-top" type="number" value="${defaults.top}"></label><label>瀹?input id="${svgId}-roi-width" data-control-id="${svgId}-roi-width" type="number" value="${defaults.width}"></label><label>楂?input id="${svgId}-roi-height" data-control-id="${svgId}-roi-height" type="number" value="${defaults.height}"></label></div><div class="object-actions"><button type="button" class="ok-lite" data-object-action="璁剧疆ROI" data-object-name="${label}">璁剧疆ROI</button><button type="button" class="primary-lite" data-object-action="璇诲彇鏉＄爜" data-object-name="${label}">璇诲彇鏉＄爜</button><button type="button" data-object-action="璇诲彇鍥轰欢淇℃伅" data-object-name="${label}">璇诲彇鍥轰欢</button></div><div class="object-form-grid"><label>鏉＄爜璇诲彇缁撴灉<input id="${svgId}-barcode-read" data-control-id="${svgId}-barcode-read" placeholder="璇疯緭鍏ュ唴瀹?></label><label>鍥轰欢淇℃伅<input id="${svgId}-firmware-info" data-control-id="${svgId}-firmware-info" placeholder="璇疯緭鍏ュ唴瀹?></label></div></section>
    </div>`;
  bindGenericObjectActionButtons(box);
}
function showNeedleDetail(needle='Z1') {
  const z = String(needle).toUpperCase() === 'Z2' ? 'Z2' : 'Z1';
  const idx = z === 'Z1' ? 1 : 2;
  const currentZ = z === 'Z1' ? arm.z1 : arm.z2;
  const fluid = z === 'Z1' ? arm.fluid1 : arm.fluid2;
  selectedName = null;
  selectedSvgControlId = `svg-arm-needle-z${idx}`;
  showSideTab('production');
  const box = document.getElementById('detailBox'); if(!box) return;
  const baseId = `svg-arm-needle-z${idx}`;
  box.innerHTML = `<strong>閽堝ご ${z}</strong><div>浼哥缉鐘舵€侊細${needleStateText(currentZ)}銆€褰撳墠娑蹭綋锛?{fluidLabel(fluid)}</div><div class="object-engineering-panel" id="${baseId}-object-engineering" data-control-id="${baseId}-object-engineering"><section class="object-engineering-card" id="${baseId}-arm-card" data-control-id="${baseId}-arm-card"><strong>鍙岄拡鏈烘鑷傝缃?span class="engineering-pill">${z}</span></strong><div class="object-form-grid three"><label>褰撳墠鎺у埗閽?input value="${z}" readonly></label><label>閽堥棿璺?/ mm<input id="${baseId}-gap" data-control-id="${baseId}-gap" type="number" value="25"></label><label>Z 瀹夊叏楂樺害<input id="${baseId}-safe-z" data-control-id="${baseId}-safe-z" type="number" value="30"></label></div><div class="object-actions"><button type="button" class="primary-lite" data-object-action="${z} 鍗曠嫭涓嬮檷" data-object-name="閽堝ご${z}">${z}涓嬮檷</button><button type="button" data-object-action="${z} 鍗曠嫭涓婂崌" data-object-name="閽堝ご${z}">${z}涓婂崌</button><button type="button" class="warn-lite" data-object-action="鍙岄拡鍥炲畨鍏ㄩ珮搴? data-object-name="閽堝ご${z}">鍥炲畨鍏ㄩ珮搴?/button></div></section><section class="object-engineering-card" id="${baseId}-pipette-card" data-control-id="${baseId}-pipette-card"><strong>閫氶亾绉绘恫娴嬭瘯<span class="engineering-pill">鎸夐拡澶?/span></strong><div class="object-form-grid three"><label>鐩爣瀛斾綅<input id="${baseId}-pipette-well" data-control-id="${baseId}-pipette-well" placeholder="鐐瑰嚮瀛斾綅鍚庤嚜鍔ㄥ甫鍏?></label><label>娑查噺 / 渭L<input id="${baseId}-pipette-volume" data-control-id="${baseId}-pipette-volume" type="number" value="100"></label><label>鎺у埗閽?input value="${z}" readonly></label></div><div class="object-actions"><button type="button" class="primary-lite" data-object-action="閽堝ご鍚告恫" data-object-name="閽堝ご${z}">鍚告恫</button><button type="button" data-object-action="閽堝ご鎵撴恫" data-object-name="閽堝ご${z}">鎵撴恫</button><button type="button" data-object-action="閽堝ご娑查潰鎺㈡祴" data-object-name="閽堝ご${z}">娑查潰鎺㈡祴</button><button type="button" data-object-action="閽堝ご鎵撶郴缁熸恫" data-object-name="閽堝ご${z}">鎵撶郴缁熸恫</button><button type="button" class="warn-lite" data-object-action="閽堝ご娓呯┖閫氶亾" data-object-name="閽堝ご${z}">娓呯┖閫氶亾</button><button type="button" data-object-action="鍋滄閽堝ご鍔ㄤ綔" data-object-name="閽堝ご${z}">鍋滄鍔ㄤ綔</button></div></section></div>`;
  bindGenericObjectActionButtons(box);
}
function bindGenericObjectActionButtons(root=document) {
  root.querySelectorAll('[data-object-action]').forEach(btn => {
    if(btn.dataset.objectActionBound === 'true') return;
    btn.dataset.objectActionBound = 'true';
    btn.addEventListener('click', () => {
      const action = btn.getAttribute('data-object-action') || btn.textContent.trim();
      const name = btn.getAttribute('data-object-name') || '瀵硅薄';
      log(`${action}锛?{name} 鐨勫璞″寲宸ョ▼鎸囦护宸茶繘鍏ユā鎷熼槦鍒梎, 'ok');
      flashControl(btn);
    });
  });
}

function getControlIdRegistry() {
  const svgItems = coords
    .filter(item => !AUX_COORD_CATEGORIES.has(item.category))
    .map(item => ({ id:itemControlId(item), name:item.name, category:item.category, row:item.row, col:item.col, x:item.x, y:item.y }));
  const fixedSvgControls = [];
  for(let c=1; c<=4; c++) {
    fixedSvgControls.push({ id:`svg-channel-${c}-button`, name:`閫氶亾${c}鎸夐挳`, category:'閫氶亾鎸夐挳' });
    fixedSvgControls.push({ id:`svg-channel-${c}-start-button`, name:`閫氶亾${c}寮€濮嬫壂鐮佸叆搴撴寜閽甡, category:'閫氶亾鎸夐挳' });
    fixedSvgControls.push({ id:`svg-channel-${c}-tray`, name:`閫氶亾${c}鎵樼洏`, category:'閫氶亾鎵樼洏' });
    fixedSvgControls.push({ id:`svg-port-waste-m${c}`, name:`搴熸恫瀛擾M${c}`, category:'搴熸恫瀛? });
    fixedSvgControls.push({ id:`svg-port-toxic-m${c}`, name:`鎺掓瘨瀛擾M${c}`, category:'鎺掓瘨瀛? });
    fixedSvgControls.push({ id:`svg-port-clean-channel-${c}`, name:`閫氶亾${c}_渚涙按/娓呮礂瀛擿, category:'娓呮礂瀛? });
  }
  for(let c=1; c<=5; c++) fixedSvgControls.push({ id:`svg-reagent-lane-${c}-position-sensor`, name:`璇曞墏閫氶亾${c}_鍒颁綅鎰熷簲`, category:'璇曞墏浼犳劅鍣? });
  for(let c=1; c<=4; c++) fixedSvgControls.push({ id:`svg-reagent-lane-separator-${c}-${c+1}`, name:`璇曞墏閫氶亾${c}-${c+1}_鍒嗛殧铏氱嚎`, category:'璇曞墏鍖鸿緟鍔╃嚎' });
  for(let c=1; c<=5; c++) fixedSvgControls.push({ id:`svg-reagent-lane-${c}-entry-sensor`, name:`璇曞墏閫氶亾${c}_鍏ュ彛鎰熷簲`, category:'璇曞墏浼犳劅鍣? });
  fixedSvgControls.push({ id:'svg-mix-empty-bottle-status', name:'閰嶆恫鐡剁┖鐡舵€荤姸鎬?, category:'閰嶆恫鐡剁姸鎬? });
  fixedSvgControls.push({ id:'svg-mix-empty-bottle-status-count', name:'閰嶆恫鐡剁┖鐡舵暟閲?, category:'閰嶆恫鐡剁姸鎬? });
  REAGENT_TYPES.forEach(rt => fixedSvgControls.push({ id:`svg-reagent-legend-${rt.key}-remaining-ml`, name:`璇曞墏鍥句緥_${rt.label}_鎬诲墿浣欓噺`, category:'璇曞墏鍥句緥鎬婚噺' }));
  fixedSvgControls.push({ id:'svg-reagent-temperature-control', name:'璇曞墏娓╁害', category:'璇曞墏鍒跺喎' });
  fixedSvgControls.push({ id:'reagentTempText', name:'璇曞墏娓╁害鏂囨湰', category:'璇曞墏鍒跺喎' });
  fixedSvgControls.push({ id:'svg-camera-reagent-scanner', name:'璇曞墏鎵爜鐩告満', category:'鐩告満' });
  fixedSvgControls.push({ id:'svg-arm-needle-z1', name:'閽堝ご_Z1', category:'鏈烘鑷? });
  fixedSvgControls.push({ id:'svg-arm-needle-z2', name:'閽堝ご_Z2', category:'鏈烘鑷? });
  fixedSvgControls.push({ id:'svg-arm-camera-follow', name:'鏈烘鑷傞殢鍔ㄧ浉鏈?, category:'鏈烘鑷? });
  const staticHtmlControls = [
    'modeTwinBtn','modeDebugBtn','modeProductionBtn','startBtn','pauseBtn','userBtn',
    'precheckTab','debugTab','configTab','productionTab','settingsTab',
    'metricTotalSamples','metricTodaySamples','metricActiveSamples','precheckRunAllBtn','productionPane','statusPane','productionLogPanel','productionWarnPanel','detailBox','precheckList','logList','warnList','logFilterInput','logFilterClearBtn','logChannelFilterSelect','warnFilterInput','warnFilterClearBtn','warnChannelFilterSelect','rightPanelWidthToggleGroup','rightPanelQuickToggle','rightPanelTools','sideTimeReadout','sideClockText','sideEtaLine','sideEtaText','debugPane','debugModule','debugCommandConsole','configSectionLiquidClass','configSectionMixHeat','configSectionThermal','configPane','settingsPane','configRulesCard','configPipetteWorkbench','configPipetteParamCard','configPipetteActionCard','configPipetteConsoleCard','mixHeatMainCard',
    'configProfileSelect','configNewProfileBtn','configDuplicateProfileBtn','configDeleteProfileBtn','configExportBtn','configImportBtn','configEditorTimeline','configStepEditor','settingsTabRuntime','settingsTabCommunication','settingsTabDevice','settingsTabSafety','settingsTabUi','settingsSaveBtn','settingsResetBtn'
  ];
  const htmlControls = Array.from(new Set([...staticHtmlControls, ...Array.from(document.querySelectorAll('[id]')).map(node => node.id)])).filter(Boolean);
  const liveSvgControls = Array.from(svg.querySelectorAll('[data-control-id]')).map(node => ({ id:node.getAttribute('data-control-id'), domId:node.id || '', category:'SVG鍔ㄦ€佹帶浠? })).filter(item => item.id);
  const svgControlMap = new Map([...svgItems, ...fixedSvgControls, ...liveSvgControls].map(item => [item.id, item]));
  return { htmlControls, svgControls:Array.from(svgControlMap.values()) };
}
function clickableGroup(tag='g', attrs={}) { return el(tag, { ...attrs, class:`${attrs.class || ''} clickable-part`.trim() }); }

function getItemTypeColor(item) {
  if(item.category === 'A/B娑? || item.category === '娣峰悎娑蹭綋閰嶆恫鍖?) return MIX_LIQUID_COLOR;
  if(item.category === '璇曞墏鍖?) return reagentType(item).color;
  return '#94a3b8';
}
function getLevelColor(level, baseColor, item=null) {
  const numeric = Number(level);
  if(!Number.isFinite(numeric)) return '#cbd5e1';
  if(item && item.category === '娣峰悎娑蹭綋閰嶆恫鍖? && numeric <= 0) return '#22c55e';
  if(item && (item.category === 'A/B娑? || item.category === '娣峰悎娑蹭綋閰嶆恫鍖?)) return baseColor;
  if(numeric <= 18) return '#dc2626';
  if(numeric <= 35) return '#f59e0b';
  return baseColor;
}
function getReagentCoreRadius(baseR) {
  return Math.max(10, baseR * 0.54);
}
function getSlideStain(item) {
  const ch = getChannelIdFromName(item.name), idx = getSlideIndexFromName(item.name);
  if(!ch || !idx) return 'HE';
  return (channels[ch-1]?.slides[idx-1]) || 'HE';
}
function getSlideStainColor(item) {
  return STAIN_COLORS[getSlideStain(item)] || '#e2e8f0';
}
function setSlideOp(slideName, stepIndex, completed=true) {
  const ch = getChannelIdFromName(slideName);
  const configuredLen = getChannelStepDefs(ch).length;
  const len = Math.min(12, Math.max(configuredLen, stepIndex + 1));
  const arr = slideOps.get(slideName) || Array.from({length:len}, () => false);
  while(arr.length < len) arr.push(false);
  if(stepIndex >= 0 && stepIndex < arr.length) arr[stepIndex] = completed;
  slideOps.set(slideName, arr.slice(0,12));
}
function setSlideOpsAll(name, completed=true) {
  const ch = getChannelIdFromName(name);
  const len = Math.min(12, Math.max(getChannelStepDefs(ch).length, (slideOps.get(name) || []).length));
  const base = slideOps.get(name) || Array.from({length:len}, () => false);
  slideOps.set(name, base.slice(0, len || 12).map(() => completed));
}
function setChannelSlideStep(channelId, stepIndex, onlyTargetName=null) {
  const slides = (byCategory.get('鐜荤墖閫氶亾') || []).filter(s => getChannelIdFromName(s.name) === channelId);
  slides.forEach(slide => {
    if(!onlyTargetName || slide.name === onlyTargetName) setSlideOp(slide.name, stepIndex, true);
  });
}
function fluidClass(fluid) {
  if(fluid === 'A') return 'arm-fluid-a';
  if(fluid === 'B') return 'arm-fluid-b';
  if(fluid === 'Mix') return 'arm-fluid-mix';
  if(fluid === 'Reagent') return 'arm-fluid-reagent';
  return 'arm-fluid-empty';
}
function fluidLabel(fluid) {
  if(fluid === 'A') return 'A娑?;
  if(fluid === 'B') return 'B娑?;
  if(fluid === 'Mix') return '娣峰悎娑?;
  if(fluid === 'Reagent') return '璇曞墏';
  return '绌?;
}
function needleStateText(z) {
  if(z >= 0.65) return '浼稿嚭';
  if(z >= 0.25) return '涓綅';
  return '鍥炵缉';
}

function drawZones() {
  clearLayer(zonesLayer); clearLayer(gridLayer); clearLayer(axisLayer); clearLayer(labelLayer);
  const reagentZone = rectFromMm(205, -2, 330, 226, 'zone reagent'); reagentZone.setAttribute('id', 'svg-zone-reagent'); reagentZone.setAttribute('data-control-id', 'svg-zone-reagent'); zonesLayer.appendChild(reagentZone);
  const workZone = rectFromMm(-36, -2, 156, 226, 'zone work'); workZone.setAttribute('id', 'svg-zone-work'); workZone.setAttribute('data-control-id', 'svg-zone-work'); zonesLayer.appendChild(workZone);
  const serviceZone = rectFromMm(158, -2, 198, 226, 'zone service'); serviceZone.setAttribute('id', 'svg-zone-service'); serviceZone.setAttribute('data-control-id', 'svg-zone-service'); zonesLayer.appendChild(serviceZone);
  drawTopLabels();
  drawReagentChannelSeparators();
  drawReagentZoneLegend();
  drawOperationLegend();
  drawWashSideLabels();
  for(let x = Math.ceil(transform.minX/50)*50; x <= transform.maxX; x += 50) {
    const [x1,y1] = mmToPx(x, transform.minY); const [x2,y2] = mmToPx(x, transform.maxY);
    gridLayer.appendChild(el('line', { id:`svg-grid-x-${String(x).replace(/[^A-Za-z0-9-]/g,'')}`, x1, y1, x2, y2, class:'grid-line' }));
  }
  for(let y = Math.ceil(transform.minY/50)*50; y <= transform.maxY; y += 50) {
    const [x1,y1] = mmToPx(transform.minX, y); const [x2,y2] = mmToPx(transform.maxX, y);
    gridLayer.appendChild(el('line', { id:`svg-grid-y-${String(y).replace(/[^A-Za-z0-9-]/g,'')}`, x1, y1, x2, y2, class:'grid-line' }));
  }
}

function getChannelIdFromName(name) {
  const s = String(name || '');
  const code = s.match(/^R([1-4])([1-4])$/); if(code) return Number(code[1]);
  const m = s.match(/閫氶亾(\d+)/); return m ? Number(m[1]) : null;
}
function getSlideIndexFromName(name) {
  const s = String(name || '');
  const code = s.match(/^R([1-4])([1-4])$/); if(code) return Number(code[2]);
  const m = s.match(/鐜荤墖(\d+)/); return m ? Number(m[1]) : null;
}
function getSlideDisplayCode(itemOrName) {
  const name = typeof itemOrName === 'string' ? itemOrName : itemOrName?.name;
  if(/^R[1-4][1-4]$/.test(String(name || ''))) return String(name);
  const ch = getChannelIdFromName(name), idx = getSlideIndexFromName(name);
  return ch && idx ? `R${ch}${idx}` : String(name || '');
}
function drawTrays() {
  clearLayer(traysLayer);
  const slides = byCategory.get('鐜荤墖閫氶亾') || [];
  for(let c=1;c<=4;c++) {
    const items = slides.filter(s => getChannelIdFromName(s.name) === c);
    if(!items.length) continue;
    const x = items[0].x;
    const minY = Math.min(...items.map(d=>d.y)) - 16;
    const maxY = Math.max(...items.map(d=>d.y)) + 15;
    const trayId = `svg-channel-${c}-tray`;
    const tray = rectFromMm(x-16, minY, x+16, maxY, 'channel-tray');
    tray.setAttribute('id', trayId); tray.setAttribute('data-control-id', trayId);
    tray.dataset.channel = c;
    tray.addEventListener('click', evt => { evt.stopPropagation(); selectSvgControl(tray); selectedChannel = c; selectedName = null; showChannelDetail(c); updateVisualStates(); });
    traysLayer.appendChild(tray);
    const [sx, sy] = mmToPx(x, minY - 14.6);
    const sw = mmSize(22), sh = mmSize(7.8);
    const startId = `svg-channel-${c}-start-button`;
    const startBtn = clickableGroup('g', controlAttrs(startId, { 'data-channel-start': c }));
    startBtn.appendChild(el('rect', { id:`${startId}-rect`, x:sx-sw/2, y:sy-sh/2, width:sw, height:sh, class:`channel-start-btn ${channels[c-1]?.barcodeReady?'active':''}` }));
    const startText = textSvg(sx, sy, '寮€濮?, 'channel-start-btn-text'); startText.setAttribute('id', `${startId}-label`); startBtn.appendChild(startText);
    startBtn.addEventListener('click', evt => { evt.stopPropagation(); selectSvgControl(startBtn); intakeChannelSamples(c); });
    traysLayer.appendChild(startBtn);
    const [bx, by] = mmToPx(x, minY - 4.7);
    const bw = mmSize(24), bh = mmSize(8.5);
    const btnId = `svg-channel-${c}-button`;
    const btn = clickableGroup('g', controlAttrs(btnId, { 'data-channel-btn': c }));
    btn.appendChild(el('rect', { id:`${btnId}-rect`, x:bx-bw/2, y:by-bh/2, width:bw, height:bh, class:`channel-btn ${selectedChannel===c?'active':''}` }));
    const btnText = textSvg(bx, by, '閫氶亾' + c, 'channel-btn-text'); btnText.setAttribute('id', `${btnId}-label`); btn.appendChild(btnText);
    btn.addEventListener('click', evt => { evt.stopPropagation(); selectSvgControl(btn); selectedChannel = c; selectedName = null; showChannelDetail(c); updateVisualStates(); });
    traysLayer.appendChild(btn);
  }
}

function cameraStateLabel(state) {
  return state === 'active' ? '鎵弿涓? : state === 'complete' ? '瀹屾垚' : state === 'error' ? '寮傚父' : '寰呮満';
}
function drawCameraIcon(parent, x, y, opts={}) {
  const state = opts.state || 'idle';
  const scale = opts.scale || 1;
  const label = opts.label || '';
  const showText = opts.showText !== false;
  const w = mmSize(15 * scale), h = mmSize(9.5 * scale);
  const mountW = w * 0.42, mountH = mmSize(2.2 * scale);
  const cameraId = opts.id || `svg-camera-${normalizeControlId(label || state || 'unit')}`;
  const g = clickableGroup('g', controlAttrs(cameraId, { class:`camera-unit camera-${state}` }));
  if(typeof opts.onClick === 'function') g.addEventListener('click', evt => { evt.stopPropagation(); selectedName = null; selectSvgControl(g); opts.onClick(evt); });
  if(state === 'active') {
    g.appendChild(el('path', { id:`${cameraId}-beam`, d:`M ${x-w*0.18} ${y+h/2} L ${x-w*.72} ${y+h*1.55} L ${x+w*.72} ${y+h*1.55} L ${x+w*.18} ${y+h/2} Z`, class:'camera-beam' }));
  }
  g.appendChild(el('rect', { id:`${cameraId}-mount`, x:x-mountW/2, y:y-h/2-mountH*1.15, width:mountW, height:mountH, rx:mountH/2, class:'camera-mount' }));
  g.appendChild(el('rect', { id:`${cameraId}-housing`, x:x-w/2, y:y-h/2, width:w, height:h, rx:mmSize(2.6*scale), class:'camera-housing' }));
  g.appendChild(el('circle', { id:`${cameraId}-lens`, cx:x+w*0.08, cy:y, r:h*0.32, class:'camera-lens' }));
  g.appendChild(el('circle', { id:`${cameraId}-lens-inner`, cx:x+w*0.08, cy:y, r:h*0.15, class:'camera-inner' }));
  g.appendChild(el('circle', { id:`${cameraId}-status`, cx:x-w*0.32, cy:y-h*0.18, r:mmSize(1.55*scale), class:`camera-status ${state}` }));
  if(showText && label) {
    const markText = textSvg(x, y + h/2 + mmSize(4.0*scale), label, 'camera-mark'); markText.setAttribute('id', `${cameraId}-label`); g.appendChild(markText);
    const stateText = textSvg(x, y + h/2 + mmSize(8.6*scale), cameraStateLabel(state), 'camera-state-text'); stateText.setAttribute('id', `${cameraId}-state-label`); g.appendChild(stateText);
  }
  parent.appendChild(g);
  return g;
}
function currentReagentTemp() {
  const value = window.digitalTwinDbSnapshot?.scalars?.reagent_current_temperature_c;
  return Number.isFinite(Number(value)) ? Number(value).toFixed(1) + '鈩? : '鈥?;
}
function updateReagentTempReadout() {
  const text = document.getElementById('reagentTempText');
  if(text) text.textContent = currentReagentTemp();
}
function drawCameraMarker(xMm, yMm, label='璇曞墏鎵爜', state=cameraStates.reagent) {
  const [x, y] = mmToPx(xMm, yMm);
  const tempGroup = clickableGroup('g', controlAttrs('svg-reagent-temperature-control', { class:'reagent-temp-click-target', 'aria-label':'璇曞墏娓╁害' }));
  tempGroup.appendChild(el('text', { id:'reagentTempText', 'data-control-id':'reagentTempText', x, y:y - mmSize(12.2), class:'reagent-temp-text', 'text-anchor':'middle', 'dominant-baseline':'central' }, [document.createTextNode(currentReagentTemp())]));
  tempGroup.addEventListener('click', evt => { evt.stopPropagation(); selectSvgControl(tempGroup); showReagentCoolingDetail(xMm, yMm); });
  auxLayer.appendChild(tempGroup);
  drawCameraIcon(auxLayer, x, y, {
    id:'svg-camera-reagent-scanner',
    label, state, scale:1.0, showText:true,
    onClick:() => showScannerDetail('reagent', xMm, yMm)
  });
}


function auxPoint(name, fallback) {
  const item = byName.get(name);
  if(item && Number.isFinite(item.x) && Number.isFinite(item.y)) return item;
  return { name, x:fallback.x, y:fallback.y, col:fallback.col ?? null, row:fallback.row ?? null };
}
function drawAuxPorts() {
  clearLayer(auxLayer);
  const motors = byCategory.get('娣峰寑鐢垫満') || [];
  motors.forEach(item => {
    [
      { name:`搴熸恫瀛擾M${item.col}`, dx:4.6, label:`搴?{Number(item.col)}`, cls:'waste', title:'搴熸恫瀛? },
      { name:`鎺掓瘨瀛擾M${item.col}`, dx:-4.6, label:`姣?{Number(item.col)}`, cls:'toxic', title:'鎺掓瘨瀛? }
    ].forEach(port => {
      const p = auxPoint(port.name, { x:item.x + port.dx, y:item.y - 13.5, col:item.col });
      const [cx, cy] = mmToPx(p.x, p.y);
      const portId = `svg-port-${port.cls === 'waste' ? 'waste' : 'toxic'}-m${Number(item.col)}`;
      const group = clickableGroup('g', controlAttrs(portId));
      group.appendChild(el('circle', { id:`${portId}-circle`, cx, cy, r:mmSize(2.5), class:`port-hole ${port.cls}` }));
      const labelX = port.cls === 'toxic' ? cx + mmSize(4.6) : cx - mmSize(4.6);
      const labelAnchor = port.cls === 'toxic' ? 'start' : 'end';
      const portText = textSvg(labelX, cy, port.label, 'port-label', labelAnchor); portText.setAttribute('id', `${portId}-label`); group.appendChild(portText);
      group.addEventListener('click', evt => {
        evt.stopPropagation();
        selectedName = null;
        selectSvgControl(group);
        const val = port.cls === 'waste' ? liquids.waste : liquids.toxic;
        showPortDetail(p.name || port.title, [
          `鎵€灞烇細${item.name}`,
          `鐘舵€侊細${val > 90 ? '鎶ヨ' : val > 75 ? '鎺ヨ繎闃堝€? : '姝ｅ父'}`,
          `褰撳墠娑蹭綅锛?{Math.round(val)}%`,
          `浣嶇疆锛歑 ${Number(p.x).toFixed(1)} / Y ${Number(p.y).toFixed(1)}`
        ]);
      });
      auxLayer.appendChild(group);
    });
  });
  const slides = byCategory.get('鐜荤墖閫氶亾') || [];
  for(let c=1;c<=4;c++) {
    const items = slides.filter(s => getChannelIdFromName(s.name) === c);
    if(!items.length) continue;
    const x = items[0].x;
    const maxY = Math.max(...items.map(d=>d.y)) + 15;
    const p = auxPoint(`閫氶亾${c}_娓呮礂瀛擿, { x, y:maxY + 11, col:c });
    const [cx, cy] = mmToPx(p.x, p.y);
    const cleanId = `svg-port-clean-channel-${c}`;
    const group = clickableGroup('g', controlAttrs(cleanId));
    group.appendChild(el('circle', { id:`${cleanId}-circle`, cx, cy, r:mmSize(2.8), class:'port-hole clean' }));
    const cleanText = textSvg(cx, cy + mmSize(5), `渚涙按${c}`, 'port-label'); cleanText.setAttribute('id', `${cleanId}-label`); group.appendChild(cleanText);
    group.addEventListener('click', evt => { evt.stopPropagation(); selectedName = null; selectSvgControl(group); showWaterModuleDetail(c, p); });
    auxLayer.appendChild(group);
  }
  drawReagentSensors();
  drawMixBottleStatus();
  const cameraPoint = auxPoint('璇曞墏鎵爜鐩告満', { x:342, y:211 });
  drawCameraMarker(cameraPoint.x, cameraPoint.y, '璇曞墏鎵爜');
}

function getMixValidityCountdown(item) {
  const key = `P${Number(item.row || 0)}${Number(item.col || 0)}`;
  const minutesByKey = { P11:152, P12:143, P21:96, P22:83, P31:61, P32:45, P41:23, P42:11 };
  const total = minutesByKey[key] ?? 0;
  const h = Math.floor(total / 60);
  const m = total % 60;
  return `${h}:${String(m).padStart(2, '0')}`;
}
function drawPart(item) {
  const baseId = itemControlId(item);
  const g = el('g', controlAttrs(baseId, { class:`part ${CATEGORY_CLASS[item.category] || ''}`, 'data-name':item.name, 'data-category':item.category, 'data-state':itemState.get(item.name) || 'idle' }));
  const [x,y] = mmToPx(item.x, item.y);
  if(item.shape === 'circle') {
    const baseR = mmSize(item.radius || 5);
    const fillColor = getItemTypeColor(item);
    if(['璇曞墏鍖?,'娣峰悎娑蹭綋閰嶆恫鍖?,'A/B娑?].includes(item.category)) {
      const rawLevel = itemLevels.has(item.name) ? itemLevels.get(item.name) : defaultLevelFor(item);
      const level = Number.isFinite(Number(rawLevel)) ? Number(rawLevel) : null;
      const sectorColor = getLevelColor(level, fillColor, item);
      if(level === null) {
        g.appendChild(el('circle', { id:`${baseId}-level-background`, cx:x, cy:y, r:baseR, fill:'#e5e7eb', class:'level-bg db-null-level' }));
      } else if(item.category === '娣峰悎娑蹭綋閰嶆恫鍖? && level <= 0) {
        const pristine = isMixBottlePristine(item);
        g.appendChild(el('circle', { id:`${baseId}-level-background`, cx:x, cy:y, r:baseR, fill:pristine ? '#22c55e' : '#6b7280', class:`level-bg mix-${pristine ? 'empty' : 'used'}-bottle` }));
      } else {
        g.appendChild(el('circle', { id:`${baseId}-level-background`, cx:x, cy:y, r:baseR, class:'level-bg' }));
        g.appendChild(el('path', { id:`${baseId}-level-fill`, d:describeSector(x, y, baseR * 0.98, 0, Math.max(4, level * 3.6)), fill:sectorColor, class:'level-sector' }));
      }
      if(item.category === '璇曞墏鍖?) {
        g.appendChild(el('circle', { id:`${baseId}-level-core`, cx:x, cy:y, r:getReagentCoreRadius(baseR), class:'level-core reagent-core' }));
      }
      if(item.category === '娣峰悎娑蹭綋閰嶆恫鍖?) {
        const mixLabel = textSvg(x, y - baseR - 10, `P${item.row}${item.col}`, 'mix-code-text'); mixLabel.setAttribute('id', `${baseId}-label`); g.appendChild(mixLabel);
        const countdown = textSvg(x, y + baseR + 8, getMixValidityCountdown(item), 'mix-countdown-text');
        countdown.setAttribute('id', `${baseId}-validity-countdown`);
        countdown.setAttribute('data-control-id', `${baseId}-validity-countdown`);
        g.appendChild(countdown);
      }
    } else {
      g.appendChild(el('circle', { id:`${baseId}-outer`, cx:x, cy:y, r:baseR, fill:'#dbeafe', 'fill-opacity':0.8 }));
      g.appendChild(el('circle', { id:`${baseId}-inner`, cx:x, cy:y, r:Math.max(2.2, baseR-1.2), fill:'#ffffff', 'fill-opacity':0.9 }));
    }
  } else {
    const isSlide = item.category === '鐜荤墖閫氶亾';
    const w = mmSize(isSlide ? 13.5 : (item.width || (item.category === '娣峰寑鐢垫満' ? 18 : 10)));
    const h = mmSize(isSlide ? 13.5 : (item.height || (item.category === '娣峰寑鐢垫満' ? 18 : 10)));
    const fill = isSlide ? getSlideStainColor(item) : '#d1d5db';
    g.appendChild(el('rect', { id:`${baseId}-body`, x:x-w/2, y:y-h/2, width:w, height:h, rx:item.category === '娣峰寑鐢垫満' ? 5 : 3, fill:fill, 'fill-opacity': isSlide ? 0.88 : 1 }));
    if(isSlide) {
      const slideCode = textSvg(x, y - h*0.18, getSlideDisplayCode(item), 'slide-index-text'); slideCode.setAttribute('id', `${baseId}-code`); g.appendChild(slideCode);
      const slideTempValue = getSlideTemp(item);
      const slideTemp = textSvg(x, y + h*0.22, Number.isFinite(slideTempValue) ? `${Math.round(slideTempValue)}鈩僠 : '鈥?, 'slide-temp-text'); slideTemp.setAttribute('id', `${baseId}-temperature`); g.appendChild(slideTemp);
    }
  }
  const label = item.category === '鐜荤墖閫氶亾' ? '' : shortLabel(item);
  const labelClass = item.category === '璇曞墏鍖? ? 'axis-text' : item.category === '鐜荤墖閫氶亾' ? 'slide-step-label' : 'svg-label';
  if(label) { const labelNode = textSvg(x, y, label, labelClass); labelNode.setAttribute('id', `${baseId}-label`); g.appendChild(labelNode); }
  g.addEventListener('click', (evt) => { evt.stopPropagation(); selectItem(item.name); });
  return g;
}
function shortLabel(item) {
  if(item.category === '璇曞墏鍖?) { const m = String(item.name || '').match(/S\d+/i); return m ? m[0].toUpperCase() : 'S' + item.id; }
  if(item.category === '鐜荤墖閫氶亾') return getSlideDisplayCode(item);
  if(item.category === '娣峰寑鐢垫満') return 'M' + item.col;
  if(item.category === 'A/B娑?) return item.name.replace('娑?,'');
  if(item.category === '娣峰悎娑蹭綋閰嶆恫鍖?) return '';
  if(item.category === '娲楅拡澶?) return '';
  return '';
}
function drawData() { clearLayer(dataLayer); coords.filter(isPhysicalDrawableItem).forEach(item => dataLayer.appendChild(drawPart(item))); }
function drawSlideOps() {
  clearLayer(opsLayer);
  const slides = byCategory.get('鐜荤墖閫氶亾') || [];
  slides.forEach(item => {
    const ch = getChannelIdFromName(item.name);
    const stepDefs = getChannelStepDefs(ch).slice(0, 12);
    if(!stepDefs.length) return;
    const [x,y] = mmToPx(item.x, item.y);
    const w = mmSize(13.5), h = mmSize(13.5);
    const baseStates = slideOps.get(item.name) || Array.from({length:stepDefs.length}, () => false);
    const states = baseStates.slice(0, stepDefs.length);
    const slots = getSlideOpSlots(x, y, w, h, states.length);
    states.forEach((done, idx) => {
      const op = stepDefs[idx] || getStepDef(idx, ch);
      const [sx, sy] = slots[idx];
      const stepId = `${itemControlId(item)}-step-${idx + 1}`;
      opsLayer.appendChild(createOpShape(op.shape, sx, sy, 3.1, !!done, op.color, '', { id:stepId, 'data-control-id':stepId }));
    });
  });
}
function drawPath() {
  clearLayer(pathLayer);
  const pts = DEMO_STEPS.map(s => s.target ? findTarget(s.target) : null).filter(Boolean).map(t => mmToPx(t.x, t.y));
  if(pts.length < 2) return;
  const cursor = Math.max(0, Math.min(currentStepIndex, pts.length - 1));
  const startSeg = Math.max(0, cursor - 3);
  const endSeg = Math.min(pts.length - 2, cursor + 2);
  for(let i = startSeg; i <= endSeg; i++) {
    const cls = i < cursor ? 'path-segment path-past' : 'path-segment path-future';
    const p1 = pts[i], p2 = pts[i+1];
    pathLayer.appendChild(el('line', { id:`svg-demo-path-segment-${i + 1}`, 'data-control-id':`svg-demo-path-segment-${i + 1}`, x1:p1[0], y1:p1[1], x2:p2[0], y2:p2[1], class:cls }));
  }
}

function updateVisualStates() {
  document.querySelectorAll('.part').forEach(g => {
    const name = g.dataset.name;
    g.dataset.state = itemState.get(name) || 'idle';
    g.dataset.active = name === selectedName || (currentTarget && currentTarget.name === name) ? 'true' : 'false';
  });
  document.querySelectorAll('.svg-control-selected').forEach(el => { if(el.id !== selectedSvgControlId) el.classList.remove('svg-control-selected'); });
  if(selectedSvgControlId) { const selectedNode = document.getElementById(selectedSvgControlId); if(selectedNode) selectedNode.classList.add('svg-control-selected'); }
  document.querySelectorAll('.channel-tray').forEach(t => {
    const c = Number(t.dataset.channel); const state = channels[c-1].pulled ? 'open' : channels[c-1].state;
    t.setAttribute('class', `channel-tray ${state} ${selectedChannel===c?'selected':''}`);
  });
  document.querySelectorAll('[data-channel-btn]').forEach(btn => {
    const c = Number(btn.getAttribute('data-channel-btn'));
    const rect = btn.querySelector('rect'); if(rect) rect.setAttribute('class', `channel-btn ${selectedChannel===c?'active':''}`);
  });
  document.querySelectorAll('[data-channel-start]').forEach(btn => {
    const c = Number(btn.getAttribute('data-channel-start'));
    const rect = btn.querySelector('rect'); if(rect) rect.setAttribute('class', `channel-start-btn ${channels[c-1]?.barcodeReady?'active':''}`);
  });
  drawSlideOps();
  drawPath();
  renderHalo();
}
function renderHalo() {
  clearLayer(haloLayer);
  if(!currentTarget) return;
  const [x,y] = mmToPx(currentTarget.x, currentTarget.y);
  haloLayer.appendChild(el('circle', { id:'svg-current-target-halo', cx:x, cy:y, r:8, class:'target-halo' }));
}
function renderAll() {
  ensureVisualData(); rebuildIndexes(); calcTransform(); drawZones(); drawTrays(); drawAuxPorts(); drawData(); drawSlideOps(); drawPath(); renderArmStatic(); renderChannelCards(); renderLiquids(); updateVisualStates(); updateKpis(); updateHeaderMetrics({ active:getCurrentSlideCount() });
}

function renderArmStatic() {
  clearLayer(armLayer);
  const [left, railY] = mmToPx(transform.maxX, -5);
  const [right] = mmToPx(transform.minX, -5);
  armLayer.appendChild(el('line', { id:'svg-arm-x-rail', 'data-control-id':'svg-arm-x-rail', x1:left, y1:railY, x2:right, y2:railY, class:'arm-rail' }));
  armLayer.appendChild(el('line', { id:'svg-arm-x-rail-highlight', x1:left+10, y1:railY, x2:right-10, y2:railY, class:'arm-rail-light' }));
  armLayer.appendChild(el('g', { id:'armDynamic' }));
  updateArmVisual();
}
function addNeedleTag(parent, cx, cy, text, idBase) {
  const w = Math.max(mmSize(18), text.length * 6.2);
  const h = 18;
  parent.appendChild(el('rect', { id:`${idBase}-background`, x:cx - w/2, y:cy - h/2, width:w, height:h, class:'needle-tag-bg' }));
  parent.appendChild(el('text', { id:`${idBase}-text`, x:cx, y:cy, class:'needle-tag-text', 'text-anchor':'middle', 'dominant-baseline':'central' }, [document.createTextNode(text)]));
}
function updateArmVisual() {
  const g = document.getElementById('armDynamic'); if(!g) return;
  while(g.firstChild) g.removeChild(g.firstChild);
  const [p1x,p1y] = mmToPx(arm.x, arm.y);
  const [p2x,p2y] = mmToPx(arm.x, arm.y + 25);
  const [,railY] = mmToPx(arm.x, -5);
  const ext1 = 8 + arm.z1 * 22, ext2 = 8 + arm.z2 * 22;
  g.appendChild(el('line', { id:'svg-arm-y-axis', 'data-control-id':'svg-arm-y-axis', x1:p1x, y1:railY, x2:p1x, y2:p2y+30, class:'arm-y' }));
  g.appendChild(el('rect', { id:'svg-arm-head', 'data-control-id':'svg-arm-head', x:p1x-18, y:railY-15, width:36, height:30, rx:8, class:'arm-head' }));
  g.appendChild(el('line', { id:'svg-arm-needle-z1-stroke', x1:p1x, y1:p1y-ext1, x2:p1x, y2:p1y+ext1, class:'needle-line' }));
  g.appendChild(el('line', { id:'svg-arm-needle-z2-stroke', x1:p2x, y1:p2y-ext2, x2:p2x, y2:p2y+ext2, class:'needle-line' }));
  const n1 = clickableGroup('g', controlAttrs('svg-arm-needle-z1'));
  n1.appendChild(el('circle', { id:'svg-arm-needle-z1-shell', cx:p1x, cy:p1y, r:6 + arm.z1 * 4, class:`needle-shell ${fluidClass(arm.fluid1)}` }));
  n1.appendChild(el('circle', { id:'svg-arm-needle-z1-liquid-ring', cx:p1x, cy:p1y, r:10 + arm.z1 * 2, class:'needle-liquid-ring', stroke:arm.fluid1 ? '#0f172a' : '#cbd5e1' }));
  n1.addEventListener('click', evt => { evt.stopPropagation(); selectedName = null; selectSvgControl(n1); showNeedleDetail('Z1'); });
  g.appendChild(n1);
  const n2 = clickableGroup('g', controlAttrs('svg-arm-needle-z2'));
  n2.appendChild(el('circle', { id:'svg-arm-needle-z2-shell', cx:p2x, cy:p2y, r:6 + arm.z2 * 4, class:`needle-shell ${fluidClass(arm.fluid2)}` }));
  n2.appendChild(el('circle', { id:'svg-arm-needle-z2-liquid-ring', cx:p2x, cy:p2y, r:10 + arm.z2 * 2, class:'needle-liquid-ring', stroke:arm.fluid2 ? '#0f172a' : '#cbd5e1' }));
  n2.addEventListener('click', evt => { evt.stopPropagation(); selectedName = null; selectSvgControl(n2); showNeedleDetail('Z2'); });
  g.appendChild(n2);
  addNeedleTag(g, p1x, p1y - 16, `Z1 路 ${fluidLabel(arm.fluid1)}`, 'svg-arm-needle-z1-label');
  addNeedleTag(g, p2x, p2y + 16, `Z2 路 ${fluidLabel(arm.fluid2)}`, 'svg-arm-needle-z2-label');
  const camX = p1x;
  const camY = p2y + 40;
  drawCameraIcon(g, camX, camY, { id:'svg-arm-camera-follow', state:cameraStates.arm, scale:.62, showText:false, onClick:() => showScannerDetail('sample', arm.x, arm.y + 43) });
}
function findTarget(nameOrRegex) {
  if(nameOrRegex instanceof RegExp) return coords.find(d => nameOrRegex.test(d.name));
  return byName.get(nameOrRegex) || coords.find(d => d.name === nameOrRegex || d.name.includes(nameOrRegex));
}

const DEMO_STEPS = [
  { label:'鍒濆鍖栵細鏈烘鑷傚埌娲楀唴閽堝浣嶄綅锛屽埗鍐峰惎鍔?, target:HOME_TARGET_NAME, z1:0, z2:0, duration:700, action:'home' },
  { label:'娲楅拡锛氬厛娲楀唴澹?, target:'娲楅拡澶確鍙冲垪_娲楀唴澹乢R1', z1:.8, z2:.15, duration:950, action:'wash' },
  { label:'娲楅拡锛氬啀娲楀澹?, target:'娲楅拡澶確宸﹀垪_娲楀澹乢R1', z1:.7, z2:.15, duration:800, action:'wash' },
  { label:'鍚稿彇 A 娑?, target:'A娑?, z1:.9, z2:0, duration:1050, action:'aspirateA' },
  { label:'鍚稿彇 B 娑?, target:'B娑?, z1:.9, z2:0, duration:750, action:'aspirateB' },
  { label:'2脳4 閰嶆恫鍖烘贩鍚?A/B 娑?, target:'閰嶆恫_R2_C1', z1:.65, z2:.2, duration:1050, action:'mix' },
  { label:'璇诲彇璇曞墏 S13锛屾墽琛屽姞鏍?, target:'璇曞墏_S13', z1:.8, z2:0, duration:1050, action:'reagent' },
  { label:'閫氶亾 1 鐜荤墖鍔犳牱', target:'R11', z1:.75, z2:.55, duration:1200, action:'slide1' },
  { label:'閫氶亾 1 娣峰寑鐢垫満鍚姩', target:'娣峰寑鐢垫満_1', z1:.2, z2:0, duration:850, action:'motor1' },
  { label:'閫氶亾 2 鐜荤墖鍔犳牱', target:'R22', z1:.75, z2:.55, duration:1200, action:'slide2' },
  { label:'閫氶亾 2 娣峰寑鐢垫満鍚姩', target:'娣峰寑鐢垫満_2', z1:.2, z2:0, duration:850, action:'motor2' },
  { label:'閫氶亾 3 鐜荤墖鍔犳牱', target:'R33', z1:.75, z2:.55, duration:1200, action:'slide3' },
  { label:'閫氶亾 3 娣峰寑鐢垫満鍚姩', target:'娣峰寑鐢垫満_3', z1:.2, z2:0, duration:850, action:'motor3' },
  { label:'閫氶亾 4 鐜荤墖鍔犳牱', target:'R44', z1:.75, z2:.55, duration:1200, action:'slide4' },
  { label:'閫氶亾 4 娣峰寑鐢垫満鍚姩', target:'娣峰寑鐢垫満_4', z1:.2, z2:0, duration:850, action:'motor4' },
  { label:'鍥炲埌娲楅拡绔欙紝閫氶亾娓呮礂', target:'娲楅拡澶確鍙冲垪_娲楀唴澹乢R2', z1:.85, z2:.25, duration:1050, action:'washEnd' },
  { label:'娴佺▼瀹屾垚锛屾満姊拌噦寰呮満', target:HOME_TARGET_NAME, z1:0, z2:0, duration:900, action:'done' }
];

function remainingDemoMsFrom(index=currentStepIndex) {
  const start = Math.max(0, Math.min(DEMO_STEPS.length, Number(index) || 0));
  return DEMO_STEPS.slice(start).reduce((sum, step) => sum + Number(step.duration || 900) + 260, 0);
}
function refreshEstimatedFinish() {
  estimatedFinishAt = Date.now() + remainingDemoMsFrom(currentStepIndex);
  updateSideHeaderStatus();
}
function formatDateTimeZh(date) {
  return date.toLocaleString('zh-CN', { year:'numeric', month:'2-digit', day:'2-digit', hour:'2-digit', minute:'2-digit', second:'2-digit', hour12:false });
}
function formatClockZh(date) {
  return date.toLocaleTimeString('zh-CN', { hour:'2-digit', minute:'2-digit', second:'2-digit', hour12:false });
}
function updateSideHeaderStatus() {
  const clock = document.getElementById('sideClockText');
  if(clock) clock.textContent = formatDateTimeZh(new Date());
  const etaLine = document.getElementById('sideEtaLine');
  const etaText = document.getElementById('sideEtaText');
  const shouldShowEta = running && estimatedFinishAt;
  if(etaLine) etaLine.hidden = !shouldShowEta;
  if(etaText && shouldShowEta) etaText.textContent = paused ? '宸叉殏鍋? : formatClockZh(new Date(estimatedFinishAt));
}

function applyStepAction(step, begin=false) {
  const phase = document.getElementById('phaseText');
  if(phase) phase.textContent = step.label;
  if(step.target) { currentTarget = findTarget(step.target); } else { currentTarget = null; }
  if(currentTarget) itemState.set(currentTarget.name, 'running');
  if(begin) setInfoPanel('娴佺▼鎵ц璇︽儏', [`褰撳墠姝ラ锛?{step.label}`, currentTarget ? `鐩爣浣嶇疆锛?{currentTarget.name}` : '鐩爣浣嶇疆锛氣€?]);
  switch(step.action) {
    case 'home': {
      const hp = getHomePosition();
      arm.x = hp.x; arm.y = hp.y; arm.fluid1 = null; arm.fluid2 = null;
      channels.forEach(ch => { if(ch.state !== 'complete') ch.state = 'idle'; });
      break;
    }
    case 'wash':
      arm.fluid1 = null; arm.fluid2 = null;
      log('娓呮礂閽堝ご锛氬厛娲楀唴澹侊紝鍐嶆礂澶栧锛岄槻姝氦鍙夋薄鏌?, 'ok');
      break;
    case 'washEnd':
      arm.fluid1 = null; arm.fluid2 = null;
      (byCategory.get('鐜荤墖閫氶亾') || []).forEach(s => setSlideOp(s.name, 4, true));
      log('娓呮礂閽堝ご锛氬厛娲楀唴澹侊紝鍐嶆礂澶栧锛岄槻姝氦鍙夋薄鏌?, 'ok');
      break;
    case 'aspirateA':
      arm.fluid1 = 'A'; arm.fluid2 = null; itemLevels.set('A娑?, Math.max(0, (itemLevels.get('A娑?) || 0) - 6)); setLiquidDelta('pure', -2);
      log('A 娑插惛鍙栧畬鎴愶細鍑嗗涓?B 娑叉贩鍚?, 'ok');
      break;
    case 'aspirateB':
      arm.fluid1 = 'B'; arm.fluid2 = 'A'; itemLevels.set('B娑?, Math.max(0, (itemLevels.get('B娑?) || 0) - 5)); setLiquidDelta('pbs', -2);
      log('B 娑插惛鍙栧畬鎴愶細杩涘叆娣峰悎閰嶆恫浣?, 'ok');
      break;
    case 'mix':
      arm.fluid1 = 'Mix'; arm.fluid2 = 'Mix';
      if(currentTarget) itemLevels.set(currentTarget.name, 72);
      log('閰嶆恫浣嶆贩鍚堬細2脳4 灏忓瓟浣嶇敤浜庝复鏃堕厤娑?, 'ok');
      break;
    case 'reagent':
      arm.fluid1 = 'Reagent'; arm.fluid2 = null;
      if(currentTarget) { itemLevels.set(currentTarget.name, Math.max(8, (itemLevels.get(currentTarget.name) || 0) - 18)); itemState.set(currentTarget.name, 'low'); }
      log('妯℃嫙锛氬綋鍓嶈瘯鍓備綅浣欓噺鎺ヨ繎鎶ヨ闃堝€?, 'warn');
      break;
    case 'slide1':
      arm.fluid1 = 'Reagent'; arm.fluid2 = 'Reagent';
      if(currentTarget) { setSlideOp(currentTarget.name, 1, true); setSlideOp(currentTarget.name, 3, true); }
      updateChannelRunning(1, 28, '閫氶亾1寮€濮嬫煋鑹?);
      break;
    case 'motor1':
      arm.fluid1 = null; arm.fluid2 = null; setChannelSlideStep(1, 2); updateChannelRunning(1, 46, '閫氶亾1娣峰寑鐢垫満鍚姩');
      break;
    case 'slide2':
      arm.fluid1 = 'Reagent'; arm.fluid2 = 'Reagent';
      if(currentTarget) { setSlideOp(currentTarget.name, 1, true); setSlideOp(currentTarget.name, 3, true); }
      updateChannelRunning(2, 18, '閫氶亾2寮€濮嬫煋鑹?);
      break;
    case 'motor2':
      arm.fluid1 = null; arm.fluid2 = null; setChannelSlideStep(2, 2); updateChannelRunning(2, 34, '閫氶亾2娣峰寑鐢垫満鍚姩');
      break;
    case 'slide3':
      arm.fluid1 = 'Reagent'; arm.fluid2 = 'Reagent';
      if(currentTarget) { setSlideOp(currentTarget.name, 1, true); setSlideOp(currentTarget.name, 3, true); }
      updateChannelRunning(3, 12, '閫氶亾3寮€濮嬫煋鑹?);
      break;
    case 'motor3':
      arm.fluid1 = null; arm.fluid2 = null; setChannelSlideStep(3, 2); updateChannelRunning(3, 30, '閫氶亾3娣峰寑鐢垫満鍚姩');
      break;
    case 'slide4':
      arm.fluid1 = 'Reagent'; arm.fluid2 = 'Reagent';
      if(currentTarget) { setSlideOp(currentTarget.name, 1, true); setSlideOp(currentTarget.name, 3, true); }
      updateChannelRunning(4, 8, '閫氶亾4寮€濮嬫煋鑹?);
      break;
    case 'motor4':
      arm.fluid1 = null; arm.fluid2 = null; setChannelSlideStep(4, 2); updateChannelRunning(4, 24, '閫氶亾4娣峰寑鐢垫満鍚姩');
      break;
    case 'done':
      arm.fluid1 = null; arm.fluid2 = null;
      channels.forEach(ch => { ch.state = ch.pulled ? 'open' : 'complete'; ch.progress = 100; });
      (byCategory.get('鐜荤墖閫氶亾') || []).forEach(s => { setSlideOpsAll(s.name, true); itemState.set(s.name, 'complete'); });
      cameraStates.arm = 'complete'; updateArmVisual();
      log('婕旂ず娴佺▼瀹屾垚锛? 閫氶亾鐘舵€佸凡鍚屾鏇存柊', 'ok');
      break;
  }
  drawData(); drawSlideOps(); updateVisualStates(); renderChannelCards(); renderLiquids(); updateKpis();
}
function updateChannelRunning(id, progress, message) {
  const ch = channels[id-1]; ch.state = ch.pulled ? 'open' : 'running'; ch.progress = Math.max(ch.progress, progress);
  const slideItems = (byCategory.get('鐜荤墖閫氶亾') || []).filter(s => getChannelIdFromName(s.name) === id);
  slideItems.forEach(s => itemState.set(s.name, 'running'));
  log(message, 'ok');
}
function completeCurrentTarget() { if(currentTarget && itemState.get(currentTarget.name)==='running') itemState.set(currentTarget.name, 'complete'); updateVisualStates(); }
function animateArmTo(x, y, z1, z2, duration=900) {
  const start = {...arm}, end = { x, y, z1, z2 };
  const startTime = performance.now();
  return new Promise(resolve => {
    function frame(now) {
      if(cancelAnimation) return resolve(false);
      const t = Math.min(1, (now - startTime) / duration); const ease = t < .5 ? 2*t*t : 1 - Math.pow(-2*t+2,2)/2;
      arm.x = start.x + (end.x-start.x)*ease; arm.y = start.y + (end.y-start.y)*ease; arm.z1 = start.z1 + (end.z1-start.z1)*ease; arm.z2 = start.z2 + (end.z2-start.z2)*ease;
      updateArmVisual(); updateKpis();
      if(t < 1) requestAnimationFrame(frame); else resolve(true);
    }
    requestAnimationFrame(frame);
  });
}
async function runDemo() {
  if(running) return;
  if(!precheckPassed) { showSideTab('precheck'); renderPrecheckList(); setInfoPanel('灏氭湭瀹屾垚妫€娴?, ['璇峰厛瀹屾垚鍙充晶鈥滄娴嬧€濓紝鍏ㄩ儴閫氳繃鍚庡啀寮€濮嬨€?]); log('寮€濮嬭鎷︽埅锛氭娴嬪皻鏈€氳繃', 'warn'); return; }
  running = true; paused = false; cancelAnimation = false; totalRuns++;
  refreshEstimatedFinish();
  setButtonState('startBtn', { disabled:true }); setButtonState('pauseBtn', { disabled:false, textContent:'鏆傚仠' });
  cameraStates.arm = 'active'; updateArmVisual(); log('婕旂ず娴佺▼鍚姩锛氭壂鐮併€佹礂閽堛€丄/B 閰嶆恫銆佸姞鏍枫€佹贩鍖€銆佹竻娲?, 'ok');
  while(currentStepIndex < DEMO_STEPS.length && !cancelAnimation) {
    while(paused && !cancelAnimation) await sleep(150);
    const step = DEMO_STEPS[currentStepIndex];
    const target = step.target ? findTarget(step.target) : null;
    const tx = Number.isFinite(step.x) ? step.x : target.x; const ty = Number.isFinite(step.y) ? step.y : target.y;
    currentTarget = target || null;
    applyStepAction(step, true);
    log(`姝ラ ${currentStepIndex+1}/${DEMO_STEPS.length}锛?{step.label}`, '');
    const ok = await animateArmTo(tx, ty, step.z1 ?? 0, step.z2 ?? 0, step.duration || 900);
    if(!ok) break;
    await sleep(260);
    completeCurrentTarget();
    channels.forEach(ch => { if(ch.state === 'running') ch.progress = Math.min(96, ch.progress + 6); });
    renderChannelCards(); updateKpis();
    currentStepIndex++;
    if(running && currentStepIndex < DEMO_STEPS.length) refreshEstimatedFinish();
  }
  if(currentStepIndex >= DEMO_STEPS.length) { setPhaseText('瀹屾垚'); updateHeaderMetrics({ active:getCurrentSlideCount(), today:headerMetrics.today + getCurrentSlideCount(), total:headerMetrics.total + getCurrentSlideCount() }); currentStepIndex = 0; }
  running = false; paused = false; currentTarget = null; estimatedFinishAt = null; updateSideHeaderStatus();
  setButtonState('startBtn', { disabled:!precheckPassed }); setButtonState('pauseBtn', { disabled:true, textContent:'鏆傚仠' });
  updateVisualStates(); updateKpis();
}
function sleep(ms) { return new Promise(r => setTimeout(r, ms)); }
function resetDemo() {
  cancelAnimation = true; running = false; paused = false; channelScanningBusy = false; currentStepIndex = 0; estimatedFinishAt = null; updateSideHeaderStatus(); currentTarget = null; selectedName = null; selectedSvgControlId = null;
  itemState = new Map(); itemLevels = new Map(); slideOps = new Map(); slideTemps = new Map(); channels.forEach((ch, idx) => { ch.state='idle'; ch.progress=0; ch.pulled=false; ch.barcodeReady=false; ch.slides=[...DEFAULT_CHANNEL_STAINS[idx]]; ch.configProfileId = channelConfigAssignments[ch.id] || null; });
  liquids.pure=82; liquids.pbs=76; liquids.waste=25; liquids.toxic=18;
  const hp = getHomePosition();
  arm = { x:hp.x, y:hp.y, z1:0, z2:0, fluid1:null, fluid2:null };
  setPhaseText('寰呮満'); setLegacySampleCount('0'); updateHeaderMetrics({ active:getCurrentSlideCount() });
  setButtonState('startBtn', { disabled:!precheckPassed }); setButtonState('pauseBtn', { disabled:true, textContent:'鏆傚仠' });
  cameraStates.reagent = 'idle'; cameraStates.arm = 'idle'; renderAll(); if(!detailMessage) renderDetail(null); updateArmVisual(); log('绯荤粺宸插浣嶏細鏈烘鑷傚洖鍒版礂閽?娲楀唴閽堝師鐐癸紝鐘舵€佹竻绌?, 'ok'); setConn('ok');
}

function selectItem(name) { detailMessage = null; selectedName = name; const item = byName.get(name); selectedSvgControlId = item ? itemControlId(item) : null; renderDetail(item); showSideTab('status'); const ch = getChannelIdFromName(name); if(ch) selectedChannel = ch; updateVisualStates(); renderChannelCards(); const node = selectedSvgControlId ? document.getElementById(selectedSvgControlId) : null; if(node && item?.category !== '璇曞墏鍖?) flashSvgControl(node); }
function renderDetail(item) {
  const box = document.getElementById('detailBox');
  if(!item) {
    if(detailMessage) { box.innerHTML = detailMessage; return; }
    box.innerHTML = '<strong>鏈€変腑瀵硅薄</strong>鐐瑰嚮浠绘剰璇曞墏銆侀厤娑插瓟銆丄/B娑层€佺幓鐗囥€侀€氶亾鎸夐挳銆侀拡澶淬€佺浉鏈烘垨瀛斾綅锛屽彲鏌ョ湅鐘舵€併€佷綑閲忓拰姝ラ杩涘害銆?;
    return;
  }
  const state = itemState.get(item.name) || 'idle';
  const extra = [];
  if(item.category === '璇曞墏鍖? || item.category === '娣峰悎娑蹭綋閰嶆恫鍖? || item.category === 'A/B娑?) extra.push(`浣欓噺锛?{formatDbValue(itemLevels.get(item.name), '%', v => Math.round(v))}`);
  if(item.category === '璇曞墏鍖?) extra.push(`璇曞墏绫诲埆锛?{reagentTypeLabel(item)}`);
  if(item.category === '鐜荤墖閫氶亾') {
    const ch = getChannelIdFromName(item.name);
    const profile = getChannelProfile(ch);
    const stepDefs = getChannelStepDefs(ch);
    const ops = stepDefs.length ? (slideOps.get(item.name) || []).map((done, idx) => `${idx+1}.${getStepDef(idx, item).label}${done?'鉁?:'鈼?}`).join(' / ') : '鏈€夋嫨閫氶亾閰嶇疆鏂囦欢';
    extra.push(`鏌撹壊锛?{getSlideStain(item)}`);
    extra.push(`娓╁害锛?{formatDbValue(getSlideTemp(item), '鈩?, v => v.toFixed(1))}`);
    extra.push(`閰嶇疆锛?{profile ? profile.name : '鏈€夋嫨'}`);
    extra.push(`姝ラ锛?{ops}`);
  }
  if(item.category === '娣峰悎娑蹭綋閰嶆恫鍖?) extra.push(`瀛斾綅缂栧彿锛歅${item.row}${item.col}`);
  const baseId = itemControlId(item);
  const coordEditor = renderCoordinateEditor(item, baseId);
  const objectEngineering = renderObjectEngineeringDetail(item, baseId);
  box.innerHTML = `<strong>${escapeHtml(item.name)}</strong>
    绫诲埆锛?{escapeHtml(item.category)}銆€鐘舵€侊細${STATUS_TEXT[state] || state}<br>
    鍧愭爣锛歑 ${Number(item.x).toFixed(2)} mm / Y ${Number(item.y).toFixed(2)} mm<br>
    ${extra.length ? extra.join('<br>') : '鐐瑰嚮杩愯鎸夐挳鍚庡彲鏌ョ湅璇ュ璞＄姸鎬佸彉鍖栥€?}
    ${coordEditor}${objectEngineering}`;
  bindCoordinateEditor(item.name, baseId);
  bindObjectEngineeringDetail(item, baseId);
}

function renderObjectEngineeringDetail(item, baseId) {
  const parts = [];
  if(item.category === '鐜荤墖閫氶亾' || item.category === '璇曞墏鍖?) parts.push(renderBarcodeRoiEditor(item, baseId));
  if(['璇曞墏鍖?,'鐜荤墖閫氶亾','娣峰悎娑蹭綋閰嶆恫鍖?,'A/B娑?].includes(item.category)) {
    parts.push(renderWellEngineeringEditor(item, baseId));
    parts.push(renderObjectPipetteEditor(item, baseId));
  }
  return parts.length ? `<div class="object-engineering-panel" id="${baseId}-object-engineering" data-control-id="${baseId}-object-engineering">${parts.join('')}</div>` : '';
}
function renderBarcodeRoiEditor(item, baseId) {
  const isSlide = item.category === '鐜荤墖閫氶亾';
  const kind = isSlide ? '鏍锋湰鏉＄爜鎵弿' : '璇曞墏鏉＄爜鎵弿';
  const scanner = isSlide ? '鏍锋湰鎵爜鍣紙鏈烘鑷傞殢鍔ㄧ浉鏈猴級' : '璇曞墏鎵爜鍣?;
  const prefix = isSlide ? 'sample' : 'reagent';
  const objectCode = escapeHtml(item.name);
  return `<section class="object-engineering-card" id="${baseId}-barcode-scan-card" data-control-id="${baseId}-barcode-scan-card"><strong>${kind}<span class="engineering-pill">${scanner}</span></strong>
    <div class="object-form-grid">
      <label>ROI Left<input id="${baseId}-barcode-roi-left" data-control-id="${baseId}-barcode-roi-left" type="number" value="0"></label>
      <label>ROI Top<input id="${baseId}-barcode-roi-top" data-control-id="${baseId}-barcode-roi-top" type="number" value="0"></label>
      <label>ROI Width<input id="${baseId}-barcode-roi-width" data-control-id="${baseId}-barcode-roi-width" type="number" value="120"></label>
      <label>ROI Height<input id="${baseId}-barcode-roi-height" data-control-id="${baseId}-barcode-roi-height" type="number" value="80"></label>
    </div>
    <div class="object-actions"><button type="button" data-object-action="浠庢壂鐮佸櫒璇诲彇ROI鍙傛暟" data-object-name="${objectCode}">璇绘壂鐮佸櫒ROI</button><button type="button" data-object-action="浠庨厤缃枃浠惰鍙朢OI鍙傛暟" data-object-name="${objectCode}">璇婚厤缃甊OI</button><button type="button" class="ok-lite" data-object-action="淇濆瓨ROI鍒版壂鐮佸櫒鍜岄厤缃枃浠? data-object-name="${objectCode}">淇濆瓨ROI</button></div>
    <div class="object-form-grid three">
      <label>鎵爜瀵硅薄<input id="${baseId}-barcode-object" data-control-id="${baseId}-barcode-object" value="${objectCode}" readonly></label>
      <label>X 杞村潗鏍?input id="${baseId}-barcode-x" data-control-id="${baseId}-barcode-x" type="number" value="${Number(item.x).toFixed(2)}"></label>
      <label>Y 杞村潗鏍?input id="${baseId}-barcode-y" data-control-id="${baseId}-barcode-y" type="number" value="${Number(item.y).toFixed(2)}"></label>
    </div>
    <div class="object-actions"><button type="button" data-object-action="浠庨厤缃枃浠惰鍙栨壂鐮佸潗鏍? data-object-name="${objectCode}">璇诲潗鏍?/button><button type="button" class="ok-lite" data-object-action="淇濆瓨鎵爜XY鍧愭爣鍒伴厤缃枃浠? data-object-name="${objectCode}">淇濆瓨鍧愭爣</button><button type="button" class="primary-lite" data-object-action="寮€濮?{isSlide?'鏍锋湰':'璇曞墏'}鏉＄爜鎵弿" data-object-name="${objectCode}">寮€濮嬫壂鎻?/button><button type="button" class="warn-lite" data-object-action="鍋滄${isSlide?'鏍锋湰':'璇曞墏'}鏉＄爜鎵弿" data-object-name="${objectCode}">鍋滄鎵弿</button><button type="button" class="warn-lite" data-object-action="娓呯┖${isSlide?'鏍锋湰':'璇曞墏'}鏉＄爜" data-object-name="${objectCode}">娓呯┖鏉＄爜</button></div>
    <div class="object-note">璇ュ尯鐢卞師鈥滈厤缃?鈫?鏉＄爜鎵弿鈥濊縼绉昏€屾潵锛屾寜${isSlide?'鐜荤墖':'璇曞墏鐡?}瀵硅薄淇濆瓨 ROI 涓?XY 鎵爜鍧愭爣锛涘尯鍩熺被鍨嬶細${prefix}銆?/div>
  </section>`;
}
function renderWellEngineeringEditor(item, baseId) {
  const liquid = item.category === '璇曞墏鍖? ? reagentTypeLabel(item) : 'Wash';
  return `<section class="object-engineering-card" id="${baseId}-well-engineering-card" data-control-id="${baseId}-well-engineering-card"><strong>瀛斾綅绉讳綅 / Z 楂樺害閰嶇疆<span class="engineering-pill">瀵硅薄鍖?/span></strong><div class="object-form-grid three"><label>娑蹭綋绫诲瀷<select id="${baseId}-liquid-class" data-control-id="${baseId}-liquid-class"><option ${liquid==='Ab'?'selected':''}>Ab</option><option ${liquid==='AR'?'selected':''}>AR</option><option ${liquid==='DAB'?'selected':''}>DAB</option><option ${liquid==='Wash'?'selected':''}>Wash</option><option>Custom</option></select></label><label>Z-Travel 瀹夊叏绉诲姩楂樺害<input id="${baseId}-z-travel" data-control-id="${baseId}-z-travel" type="number" value="30"></label><label>Z-Start 鎺㈡恫楂樺害<input id="${baseId}-z-start" data-control-id="${baseId}-z-start" type="number" value="8"></label><label>Z-End 閽堝皷鏈€澶т笅闄嶆繁搴?input id="${baseId}-z-end" data-control-id="${baseId}-z-end" type="number" value="2"></label><label>Z-Dispens 鍚告恫/鎺掓恫楂樺害<input id="${baseId}-z-dispens" data-control-id="${baseId}-z-dispens" type="number" value="6"></label><label>瀛斾綅瀵硅薄<input value="${escapeHtml(item.name)}" readonly></label></div><div class="object-actions"><button type="button" data-object-action="璇诲彇瀛斾綅鍙傛暟" data-object-name="${escapeHtml(item.name)}">璇诲彇</button><button type="button" class="ok-lite" data-object-action="淇濆瓨瀛斾綅鍙傛暟" data-object-name="${escapeHtml(item.name)}">淇濆瓨</button><button type="button" class="primary-lite" data-object-action="绉诲姩鍒板瓟浣? data-object-name="${escapeHtml(item.name)}">绉诲姩</button></div></section>`;
}
function renderObjectPipetteEditor(item, baseId) {
  return `<section class="object-engineering-card" id="${baseId}-pipette-card" data-control-id="${baseId}-pipette-card"><strong>閫氶亾绉绘恫娴嬭瘯<span class="engineering-pill">褰撳墠瀛斾綅</span></strong><div class="object-form-grid three"><label>瀛斾綅<input id="${baseId}-pipette-well" data-control-id="${baseId}-pipette-well" value="${escapeHtml(item.name)}" readonly></label><label>娑查噺 / 渭L<input id="${baseId}-pipette-volume" data-control-id="${baseId}-pipette-volume" type="number" value="100"></label><label>鎺у埗閽?select id="${baseId}-pipette-needle" data-control-id="${baseId}-pipette-needle"><option>Z1</option><option>Z2</option><option>鍙岄拡</option></select></label></div><div class="object-actions"><button type="button" class="primary-lite" data-object-action="鍚告恫" data-object-name="${escapeHtml(item.name)}">鍚告恫</button><button type="button" data-object-action="鎵撴恫" data-object-name="${escapeHtml(item.name)}">鎵撴恫</button><button type="button" data-object-action="鎵撶郴缁熸恫" data-object-name="${escapeHtml(item.name)}">鎵撶郴缁熸恫</button><button type="button" data-object-action="娑查潰鎺㈡祴" data-object-name="${escapeHtml(item.name)}">娑查潰鎺㈡祴</button><button type="button" class="warn-lite" data-object-action="娓呯┖閫氶亾" data-object-name="${escapeHtml(item.name)}">娓呯┖閫氶亾</button><button type="button" data-object-action="鍋滄鍔ㄤ綔" data-object-name="${escapeHtml(item.name)}">鍋滄鍔ㄤ綔</button></div></section>`;
}
// 閫氶亾绉绘恫鍔ㄤ綔 -> pipetting-tests 绔偣锛堟槧灏勬潵鑷?/api/engineering/pipetting-tests/types锛?
const PIPETTE_ACTIONS = { '鍚告恫':'aspirate', '鎵撴恫':'dispense', '鎵撶郴缁熸恫':'flush', '娑查潰鎺㈡祴':'liquid-detect', '娓呯┖閫氶亾':'wash' };

async function runPipetteTest(action, item, baseId) {
  const endpoint = PIPETTE_ACTIONS[action];
  const well = (document.getElementById(`${baseId}-pipette-well`)?.value || item?.name || '').trim();
  const volume = Number(document.getElementById(`${baseId}-pipette-volume`)?.value);
  const needleRaw = String(document.getElementById(`${baseId}-pipette-needle`)?.value || 'Z1').trim();
  const channel = needleRaw === 'Z2' ? '2' : '1'; // Z1/鍙岄拡->閫氶亾1, Z2->閫氶亾2锛堣澶囧弻閽堬級
  await writeApi(`/api/engineering/pipetting-tests/${endpoint}`, { method:'POST', body: JSON.stringify({
    commandId: makeCommandId('pipette-' + endpoint),
    channel,
    position: well,
    coordinatePointCode: well,
    volumeUl: Number.isFinite(volume) ? volume : null,
    reason: `twin 绉绘恫娴嬭瘯 ${action}@${well}`,
    target: `pipetting-test:${endpoint}:${well}`,
    dangerousOperationConfirmed: false
  }) });
  log(`${action}锛?{well}锛夊凡涓嬪彂锛岄渶绠＄悊鍛?+ 宸ョ▼浼氳瘽`, 'ok');
  await loadDatabaseSnapshot();
}

async function applyScannerRoi(baseId, scannerType) {
  const num = id => { const v = Number(document.getElementById(`${baseId}-barcode-roi-${id}`)?.value); return Number.isFinite(v) ? v : 0; };
  const sc = await resolveScannerId(scannerType);
  if(!sc) { log('娌℃湁宸查厤缃殑鎵爜鍣紝鏃犳硶淇濆瓨 ROI锛堣鍏堛€屽垱寤烘壂鐮佸櫒銆嶏級', 'warn'); return; }
  await writeApi(`/api/scanners/${encodeURIComponent(sc.id)}/roi/apply`, { method:'POST', body: JSON.stringify({
    commandId: makeCommandId('scanner-roi'),
    left: num('left'), top: num('top'), width: num('width'), height: num('height'),
    reason: `twin ROI 淇濆瓨 (${scannerType})`
  }) });
  log(`ROI 宸蹭繚瀛樺埌鎵爜鍣?${sc.name || sc.id}`, 'ok');
}

// 鎵爜鍣?profile 瑙ｆ瀽锛氭寜 Name 鍚€岃瘯鍓?鏍锋湰銆嶅尮閰嶈鑹诧紝鎵句笉鍒板垯鍙栫涓€鏉★紱鏃犱换浣?profile 杩斿洖 null銆?
async function resolveScannerId(role) {
  let list = [];
  try { list = await backendApi('/api/scanners'); } catch(e) { list = []; }
  if(!Array.isArray(list) || !list.length) return null;
  const wantReagent = role === 'reagent';
  return list.find(s => wantReagent ? /璇曞墏/i.test(String(s.name || '')) : /鏍锋湰/i.test(String(s.name || ''))) || list[0];
}

async function createScannerProfile() {
  const role = String(document.getElementById('scannerTargetSelect')?.value || 'reagent');
  const port = document.getElementById('scannerComPortSelect')?.value || 'COM1';
  const baud = Number(document.getElementById('scannerBaudRateSelect')?.value) || 115200;
  const roi = k => { const v = Number(document.getElementById('scannerRoi' + k + 'Input')?.value); return Number.isFinite(v) ? v : null; };
  await writeApi('/api/scanners', { method:'POST', body: JSON.stringify({
    commandId: makeCommandId('scanner-create'),
    name: role === 'reagent' ? '璇曞墏鎵爜鍣? : '鏍锋湰鎵爜鍣?,
    scannerType: 'Dcr55',
    enabled: true,
    port,
    baudRate: baud,
    timeoutMilliseconds: 2000,
    triggerMode: 'Software',
    deviceParameters: { roiX: roi('Left'), roiY: roi('Top'), roiWidth: roi('Width'), roiHeight: roi('Height') },
    reason: 'twin 鍒涘缓鎵爜鍣?
  }) });
  log(`宸插垱寤烘壂鐮佸櫒閰嶇疆锛?{role === 'reagent' ? '璇曞墏鎵爜鍣? : '鏍锋湰鎵爜鍣?}锛圫cannerType=Dcr55, TriggerMode=Software锛塦, 'ok');
  await loadDatabaseSnapshot();
}

async function scannerControl(action) {
  const role = String(document.getElementById('scannerTargetSelect')?.value || 'reagent');
  const sc = await resolveScannerId(role);
  if(!sc) { log('娌℃湁宸查厤缃殑鎵爜鍣紝璇峰厛銆屽垱寤烘壂鐮佸櫒銆?, 'warn'); return; }
  const id = encodeURIComponent(sc.id);
  if(action === '閰嶇疆/鎵爜鍣細璁剧疆ROI') {
    const roi = k => { const v = Number(document.getElementById('scannerRoi' + k + 'Input')?.value); return Number.isFinite(v) ? v : 0; };
    await writeApi(`/api/scanners/${id}/roi/apply`, { method:'POST', body: JSON.stringify({ commandId: makeCommandId('scanner-roi'), left: roi('Left'), top: roi('Top'), width: roi('Width'), height: roi('Height'), reason: 'twin 璁剧疆ROI' }) });
  } else if(action === '閰嶇疆/鎵爜鍣細鎵撳紑鏍￠獙鍏?) {
    await writeApi(`/api/scanners/${id}/calibration-light/enable`, { method:'POST', body: JSON.stringify({ commandId: makeCommandId('scanner-light-on'), reason: 'twin 鎵撳紑鏍￠獙鍏? }) });
  } else if(action === '閰嶇疆/鎵爜鍣細鍏抽棴鏍￠獙鍏?) {
    await writeApi(`/api/scanners/${id}/calibration-light/disable`, { method:'POST', body: JSON.stringify({ commandId: makeCommandId('scanner-light-off'), reason: 'twin 鍏抽棴鏍￠獙鍏? }) });
  } else if(action === '閰嶇疆/鎵爜鍣細閲嶆柊鍚姩') {
    await writeApi(`/api/scanners/${id}/restart`, { method:'POST', body: JSON.stringify({ commandId: makeCommandId('scanner-restart'), reason: 'twin 閲嶅惎鎵爜鍣? }) });
  }
  log(`${action} -> 鎵爜鍣?${sc.name || sc.id} 宸蹭笅鍙慲, 'ok');
  await loadDatabaseSnapshot();
}

// 瀵硅薄鍖栧伐绋嬪姩浣滅粺涓€鍒嗗彂鍣ㄣ€傛寜 action + item.category 璺敱鍒扮湡瀹炲悗绔懡浠ゃ€?
async function dispatchObjectAction(action, item, baseId, btn) {
  if(btn) flashControl(btn);
  const name = item?.name || '';
  const isReagent = item?.category === '璇曞墏鍖?;
  try {
    if(isReagent) {
      const rackCode = reagentRackCodeFromItem(item);
      if(action === '寮€濮嬭瘯鍓傛潯鐮佹壂鎻?) return await reagentPositionScan(rackCode, 'scan', name);
      if(action === '鍋滄璇曞墏鏉＄爜鎵弿') return await reagentPositionScan(rackCode, 'stop', name);
      if(action === '娓呯┖璇曞墏鏉＄爜') return await reagentPositionScan(rackCode, 'clear', name);
    }
    if(action === '淇濆瓨ROI鍒版壂鐮佸櫒鍜岄厤缃枃浠?) return await applyScannerRoi(baseId, isReagent ? 'reagent' : 'sample');
    if(PIPETTE_ACTIONS[action]) return await runPipetteTest(action, item, baseId);
    if(action === '鍋滄鍔ㄤ綔') { log('鍋滄鍔ㄤ綔锛歁ock 妯″紡鏃犵嫭绔嬪仠姝㈢鐐癸紝璁惧闅忓伐绋嬩細璇濈粨鏉熷仠姝?, 'warn'); return; }
    log(`${action}锛?{name} 鏆傛湭鎺ュ叆鍚庣锛堟ā鎷熼槦鍒楋級`, 'warn');
  } catch(err) { /* routed by writeApi where used */ }
}

async function ensureReagentScanSession() {
  const overview = await backendApi('/api/reagents/scan-sessions/overview');
  if(overview?.activeSession?.scanSessionId) return overview.activeSession.scanSessionId;
  const started = await writeApi('/api/reagents/scan-sessions/start', { method:'POST', body: JSON.stringify({ commandId: makeCommandId('reagent-scan-start') }) });
  return started?.session?.scanSessionId || null;
}

async function completeReagentScanSession() {
  try {
    const overview = await backendApi('/api/reagents/scan-sessions/overview');
    const sid = overview?.activeSession?.scanSessionId;
    if(!sid) { log('褰撳墠娌℃湁娲诲姩鐨勮瘯鍓傛壂鐮佷細璇?, 'warn'); return; }
    await writeApi(`/api/reagents/scan-sessions/${encodeURIComponent(sid)}/complete`, { method:'POST', body: JSON.stringify({ commandId: makeCommandId('reagent-scan-complete') }) });
    log('璇曞墏鎵爜浼氳瘽宸插畬鎴?, 'ok');
    await loadDatabaseSnapshot();
  } catch(err) { /* routed by writeApi */ }
}

// twin 璇曞墏浣嶅懡鍚嶄负 璇曞墏_S<col><row>锛坈ol=閫氶亾1-5, row=鐡朵綅1-8锛夛紱鍚庣 ReagentRackPosition.Code 涓?
// 鍒椾富搴?R{positionNo}锛宲ositionNo=(col-1)*8+row锛堝疄娴?R1=col1row1, R8=col1row8, R9=col2row1, R40=col5row8锛夈€?
function reagentRackCodeFromItem(item) {
  const col = Number(item?.col), row = Number(item?.row);
  if(Number.isFinite(col) && Number.isFinite(row) && col >= 1 && col <= 5 && row >= 1 && row <= 8) return `R${(col - 1) * 8 + row}`;
  const m = /S\s*(\d)\s*(\d)/i.exec(String(item?.name || ''));
  if(m) return `R${(Number(m[1]) - 1) * 8 + Number(m[2])}`;
  return String(item?.name || '');
}

// 璇曞墏浣嶆壂鐮侊細rackCode 褰㈠ 'R1'锛堝悗绔?Code锛夈€傚尯鍒嗙‖浠舵壂鐮佸櫒鎺у埗(qr/*)涓庝笟鍔℃壂鐮佷細璇?scan-confirm)銆?
// 纭欢鍛戒护鎴愬姛鍚庝笉鐩存帴浼€犺瘯鍓備綅鐘舵€侊紝璇曞墏浣?UI 浠?/api/reagents/rack 鎴?snapshot 涓哄噯銆?
async function reagentPositionScan(rackCode, mode, label) {
  const display = label || rackCode;
  try {
    const scanSessionId = await ensureReagentScanSession();
    if(!scanSessionId) { log('鏈兘寤虹珛璇曞墏鎵爜浼氳瘽', 'err'); return; }
    const qrAction = mode === 'stop' ? 'stop' : (mode === 'clear' ? 'clear' : 'start');
    await writeApi(`/api/device/reagent-scanner/qr/${qrAction}`, { method:'POST', body: JSON.stringify({ commandId: makeCommandId('reagent-qr-' + qrAction), position: rackCode, scanSessionId }) });
    if(mode === 'scan') {
      const rawBarcode = String(prompt(`璇曞墏浣?${display} (${rackCode})锛氳緭鍏ユ壂鎻忓埌鐨勬潯鐮侊紙Mock锛塦, 'HEM080220260712001') || '').trim();
      if(!rawBarcode) { log(`璇曞墏浣?${display} (${rackCode}) 鎵爜宸插彇娑坄, 'warn'); return; }
      await writeApi('/api/device/reagent-scanner/qr/report', { method:'POST', body: JSON.stringify({ commandId: makeCommandId('reagent-qr-report'), position: rackCode, scanSessionId, text: rawBarcode }) });
      await writeApi('/api/reagents/scan-confirm', { method:'POST', body: JSON.stringify({
        commandId: makeCommandId('reagent-scan-confirm'),
        scanSessionId,
        items: [{ position: rackCode, scanResult: 'VALID', rawBarcode, locatorCode: rackCode }]
      }) });
      log(`璇曞墏浣?${display} (${rackCode}) 鎵爜骞剁‘璁ゅ畬鎴恅, 'ok');
    } else if(mode === 'stop') {
      log(`璇曞墏浣?${display} (${rackCode}) 鎵弿宸插仠姝, 'ok');
    } else {
      log(`璇曞墏浣?${display} (${rackCode}) 鏉＄爜宸叉竻绌猴紙纭欢锛塦, 'ok');
    }
    await loadDatabaseSnapshot();
  } catch(err) { /* routed by writeApi */ }
}

function bindObjectEngineeringDetail(item, baseId) {
  const objectRoot = document.getElementById(`${baseId}-object-engineering`);
  if(!objectRoot) return;
  objectRoot.querySelectorAll('[data-object-action]').forEach(btn => {
    if(btn.dataset.objectActionBound === 'true') return;
    btn.dataset.objectActionBound = 'true';
    btn.addEventListener('click', () => {
      const action = btn.getAttribute('data-object-action') || btn.textContent.trim();
      dispatchObjectAction(action, item, baseId, btn);
    });
  });
}

function renderCoordinateEditor(item, baseId) {
  if(!['璇曞墏鍖?,'鐜荤墖閫氶亾','娣峰悎娑蹭綋閰嶆恫鍖?,'A/B娑?].includes(item.category)) return '';
  return `<div class="coord-editor" id="${baseId}-coord-editor" data-control-id="${baseId}-coord-editor"><div class="coord-editor-title">鍧愭爣淇</div><div class="coord-editor-grid"><label>X / mm<input id="${baseId}-coord-x" type="number" step="0.01" value="${Number(item.x).toFixed(2)}"></label><label>Y / mm<input id="${baseId}-coord-y" type="number" step="0.01" value="${Number(item.y).toFixed(2)}"></label></div><div class="coord-editor-actions"><button type="button" class="coord-save-btn" id="${baseId}-coord-save" data-control-id="${baseId}-coord-save">璁剧疆骞跺埛鏂?/button><button type="button" class="coord-reset-btn" id="${baseId}-coord-reset" data-control-id="${baseId}-coord-reset">鎭㈠榛樿</button><button type="button" class="arm-inspect-btn" id="${baseId}-arm-inspect" data-control-id="${baseId}-arm-inspect">鏈烘鑷傛鏌?/button></div><div class="config-note">鍧愭爣绯伙細鍙充笂瑙掑師鐐癸紝X 鍚戝乏涓烘锛孻 鍚戜笅涓烘銆傚師鍨嬩腑浼氱珛鍗虫洿鏂板鐢熼〉闈紱鍚庣画鍙帴涓婁綅鏈哄潗鏍囧啓鍏ユ帴鍙ｃ€?/div><div id="${baseId}-arm-inspect-view" data-control-id="${baseId}-arm-inspect-view"></div></div>`;
}
function bindCoordinateEditor(name, baseId) {
  const save = document.getElementById(`${baseId}-coord-save`);
  const reset = document.getElementById(`${baseId}-coord-reset`);
  if(save) save.addEventListener('click', () => updateItemCoordinate(name, baseId));
  if(reset) reset.addEventListener('click', () => restoreDefaultCoordinate(name, baseId));
  const inspect = document.getElementById(`${baseId}-arm-inspect`);
  if(inspect) inspect.addEventListener('click', () => runArmInspection(name));
}

async function runArmInspection(name) {
  const item = byName.get(name);
  if(!item) return;
  if(running && !paused) { log('鏈烘鑷傛鏌ヨ鎷︽埅锛氬綋鍓嶆祦绋嬭繍琛屼腑锛岃鍏堢偣鍑绘殏鍋?, 'warn'); return; }
  const baseId = itemControlId(item);
  const inputX = Number(document.getElementById(`${baseId}-coord-x`)?.value);
  const inputY = Number(document.getElementById(`${baseId}-coord-y`)?.value);
  const targetX = Number.isFinite(inputX) ? inputX : item.x;
  const targetY = Number.isFinite(inputY) ? inputY : item.y;
  if(!Number.isFinite(targetX) || !Number.isFinite(targetY)) { log(`鏈烘鑷傛鏌ュ潗鏍囨棤鏁堬細${name}`, 'err'); return; }
  const inspectTarget = { ...item, x:targetX, y:targetY };
  const view = document.getElementById(`${baseId}-arm-inspect-view`);
  if(view) view.innerHTML = `<div class="arm-inspect-view"><div class="arm-inspect-screen"><span class="arm-inspect-dot"></span><span class="arm-inspect-focus"></span></div><div class="arm-inspect-caption">鏈烘鑷傛鍦ㄧЩ鍔ㄥ埌褰撳墠濉啓鍧愭爣锛?{escapeHtml(item.name)} 路 X ${targetX.toFixed(2)} mm / Y ${targetY.toFixed(2)} mm锛岄殢鍔ㄧ浉鏈哄噯澶囬噰闆嗗疄鏃剁敾闈⑩€︹€?/div></div>`;
  cancelAnimation = false;
  cameraStates.arm = 'active';
  currentTarget = inspectTarget;
  itemState.set(item.name, 'running');
  updateVisualStates(); updateArmVisual();
  log(`鏈烘鑷傛鏌ワ細绉诲姩鍒?${item.name} 褰撳墠濉啓鍧愭爣 X ${targetX.toFixed(2)} / Y ${targetY.toFixed(2)}锛屽噯澶囩浉鏈洪噰闆哷, 'ok');
  await animateArmTo(targetX, targetY, 0.18, 0.18, 800);
  cameraStates.arm = 'complete';
  itemState.set(item.name, 'ready');
  currentTarget = null;
  updateVisualStates(); updateArmVisual();
  const refreshed = document.getElementById(`${baseId}-arm-inspect-view`);
  if(refreshed) refreshed.innerHTML = `<div class="arm-inspect-view"><div class="arm-inspect-screen"><span class="arm-inspect-dot"></span><span class="arm-inspect-focus"></span></div><div class="arm-inspect-caption">瀹炴椂鐢婚潰 / 鎷嶇収棰勮宸茬敓鎴愶細${escapeHtml(item.name)} 路 X ${targetX.toFixed(2)} mm / Y ${targetY.toFixed(2)} mm銆傚悗缁彲鏇挎崲涓虹浉鏈鸿棰戞祦鎴栨姄鎷嶆帴鍙ｃ€?/div></div>`;
  log(`鏈烘鑷傛鏌ュ畬鎴愶細${item.name} 鐢婚潰宸叉樉绀哄湪鍓嶇`, 'ok');
}

function updateItemCoordinate(name, baseId) {
  const item = byName.get(name); if(!item) return;
  const x = Number(document.getElementById(`${baseId}-coord-x`)?.value);
  const y = Number(document.getElementById(`${baseId}-coord-y`)?.value);
  if(!Number.isFinite(x) || !Number.isFinite(y)) { log(`鍧愭爣鏃犳晥锛?{name}`, 'err'); return; }
  item.x = x; item.y = y;
  coords = coords.map(row => row.name === name ? { ...row, x, y } : row);
  rebuildIndexes(); renderAll(); selectedName = name; renderDetail(byName.get(name)); showSideTab('status');
  log(`鍧愭爣宸叉洿鏂帮細${name} -> X ${x.toFixed(2)} / Y ${y.toFixed(2)}`, 'ok');
}
function restoreDefaultCoordinate(name, baseId) {
  const item = byName.get(name); if(!item) return;
  const fallback = defaultCoordByName.get(name);
  if(!fallback || !Number.isFinite(fallback.x) || !Number.isFinite(fallback.y)) { log(`娌℃湁鍙仮澶嶇殑榛樿鍧愭爣锛?{name}`, 'warn'); return; }
  const xInput = document.getElementById(`${baseId}-coord-x`);
  const yInput = document.getElementById(`${baseId}-coord-y`);
  if(xInput) xInput.value = Number(fallback.x).toFixed(2);
  if(yInput) yInput.value = Number(fallback.y).toFixed(2);
  item.x = fallback.x; item.y = fallback.y;
  coords = coords.map(row => row.name === name ? { ...row, x:fallback.x, y:fallback.y } : row);
  rebuildIndexes(); renderAll(); selectedName = name; renderDetail(byName.get(name)); showSideTab('status');
  log(`鍧愭爣宸叉仮澶嶉粯璁わ細${name} -> X ${Number(fallback.x).toFixed(2)} / Y ${Number(fallback.y).toFixed(2)}`, 'ok');
}
function escapeHtml(s) { return String(s).replace(/[&<>"']/g, m => ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[m])); }
function renderChannelCards() {
  const wrap = document.getElementById('channelCards'); if(!wrap) return; wrap.innerHTML = '';
  channels.forEach(ch => {
    const card = document.createElement('div'); card.className = `channel-card ${selectedChannel===ch.id?'selected':''} ${ch.pulled?'open':''}`; card.dataset.channel = ch.id;
    const state = ch.pulled ? 'open' : ch.state;
    const chipCls = state === 'running' ? 'running' : state === 'complete' ? 'complete' : state === 'error' ? 'error' : state === 'open' ? 'open' : '';
    const slides = ch.slides.map((s) => { const color = STAIN_COLORS[s] || '#e2e8f0'; return `<i class="${ch.progress>=100?'done':ch.progress>0?'run':'has'}" style="background:${color};color:#0f172a">${s}</i>`; }).join('');
    const profile = getChannelProfile(ch.id);
    card.innerHTML = `<div class="channel-head"><span>閫氶亾 ${ch.id}</span><span class="state-chip ${chipCls}">${STATUS_TEXT[state] || state}</span></div><div class="slide-tags">${slides}</div><div class="config-meta">閰嶇疆锛?{escapeHtml(profile ? profile.name : '鏈€夋嫨')}</div><div class="progress"><i style="width:${ch.progress}%"></i></div>`;
    card.addEventListener('click', () => { selectedChannel = ch.id; showChannelDetail(ch.id); });
    wrap.appendChild(card);
  });
}
function setLiquidDelta(key, delta) { liquids[key] = Math.max(0, Math.min(100, liquids[key] + delta)); }
function renderLiquids() {
  const wrap = document.getElementById('liquidList');
  if(!wrap) return;
  const labels = { pure:'绾按', pbs:'PBS', waste:'搴熸恫', toxic:'鎺掓瘨' };
  wrap.innerHTML = '';
  for(const key of ['pure','pbs','waste','toxic']) {
    const val = liquids[key]; const row = document.createElement('div'); row.className='liquid-row';
    const warn = (key==='waste' || key==='toxic') ? val>78 : val<25;
    const err = (key==='waste' || key==='toxic') ? val>90 : val<12;
    row.innerHTML = `<span>${labels[key]}</span><span class="bar ${err?'err':warn?'warn':''}"><i style="width:${val}%"></i></span><b>${Math.round(val)}%</b>`;
    wrap.appendChild(row);
  }
}
function getCurrentSlideCount() {
  const slides = byCategory.get('鐜荤墖閫氶亾') || [];
  return slides.length;
}
function updateHeaderMetrics(patch={}) {
  const incoming = { ...patch };
  Object.assign(headerMetrics, incoming);
  if(!Number.isFinite(Number(incoming.active)) && Number.isFinite(Number(incoming.currentSlides))) headerMetrics.active = Number(incoming.currentSlides);
  if(!Number.isFinite(Number(incoming.active)) && !Number.isFinite(Number(incoming.currentSlides))) headerMetrics.active = getCurrentSlideCount();
  const today = document.getElementById('todaySampleCount');
  const active = document.getElementById('activeSampleCount');
  const total = document.getElementById('totalSampleCount');
  if(today) today.textContent = headerMetrics.today;
  if(active) active.textContent = headerMetrics.active;
  if(total) total.textContent = headerMetrics.total;
}
function setPhaseText(text) {
  const el = document.getElementById('phaseText');
  if(el) el.textContent = text;
}
function setLegacySampleCount(value) {
  const el = document.getElementById('sampleCount');
  if(el) el.textContent = value;
}
function setButtonState(id, props={}) {
  const btn = document.getElementById(id);
  if(!btn) return;
  if(Object.prototype.hasOwnProperty.call(props, 'textContent')) btn.textContent = props.textContent;
  if(Object.prototype.hasOwnProperty.call(props, 'disabled')) {
    const guarded = id === 'startBtn' || id === 'pauseBtn';
    if(guarded) {
      btn.disabled = false;
      btn.dataset.runGuard = props.disabled ? 'precheck' : 'ready';
      btn.classList.toggle('pending', !!props.disabled);
      btn.classList.toggle('running', !props.disabled);
      btn.setAttribute('aria-disabled', props.disabled ? 'true' : 'false');
    } else {
      btn.disabled = props.disabled;
    }
  }
}

function updateKpis() {
  const avg = Math.round(channels.reduce((s,c)=>s+c.progress,0)/channels.length);
  const progress = document.getElementById('kpiProgress'); if(progress) progress.textContent = avg + '%';
  const overall = document.getElementById('overallBar'); if(overall) overall.style.width = avg + '%';
  const step = document.getElementById('kpiStep'); if(step) step.textContent = `${Math.min(currentStepIndex, DEMO_STEPS.length)}/${DEMO_STEPS.length}`;
  const stepBar = document.getElementById('stepBar'); if(stepBar) stepBar.style.width = (currentStepIndex / DEMO_STEPS.length * 100) + '%';
  const temp = document.getElementById('kpiTemp'); if(temp) temp.textContent = currentReagentTemp();
  updateReagentTempReadout();
}
function inferLogChannel(message) {
  const text = String(message || '');
  const direct = text.match(/閫氶亾\s*([1-4])/);
  if(direct) return direct[1];
  const slide = text.match(/R([1-4])[1-4]/i);
  if(slide) return slide[1];
  return 'none';
}
function applyLogFilter() {
  const input = document.getElementById('logFilterInput');
  const channelSelect = document.getElementById('logChannelFilterSelect');
  const query = String(input?.value || '').trim().toLowerCase();
  const channel = String(channelSelect?.value || 'all');
  const logList = document.getElementById('logList');
  if(!logList) return;
  Array.from(logList.children).forEach(node => {
    const textHit = !query || node.textContent.toLowerCase().includes(query);
    const channelHit = channel === 'all' || String(node.dataset.channel || 'none') === channel;
    node.classList.toggle('log-hidden', !(textHit && channelHit));
  });
}
function bindLogFilter() {
  const input = document.getElementById('logFilterInput');
  const clear = document.getElementById('logFilterClearBtn');
  const channelSelect = document.getElementById('logChannelFilterSelect');
  if(input && !input.dataset.bound) {
    input.dataset.bound = 'true';
    input.addEventListener('input', applyLogFilter);
  }
  if(channelSelect && !channelSelect.dataset.bound) {
    channelSelect.dataset.bound = 'true';
    channelSelect.addEventListener('change', applyLogFilter);
  }
  if(clear && !clear.dataset.bound) {
    clear.dataset.bound = 'true';
    clear.addEventListener('click', () => { if(input) input.value = ''; applyLogFilter(); input?.focus(); });
  }
}
function applyWarnFilter() {
  const input = document.getElementById('warnFilterInput');
  const channelSelect = document.getElementById('warnChannelFilterSelect');
  const query = String(input?.value || '').trim().toLowerCase();
  const channel = String(channelSelect?.value || 'all');
  const warnList = document.getElementById('warnList');
  if(!warnList) return;
  Array.from(warnList.children).forEach(node => {
    const textHit = !query || node.textContent.toLowerCase().includes(query);
    const channelHit = channel === 'all' || String(node.dataset.channel || 'none') === channel;
    node.classList.toggle('log-hidden', !(textHit && channelHit));
  });
}
function bindWarnFilter() {
  const input = document.getElementById('warnFilterInput');
  const clear = document.getElementById('warnFilterClearBtn');
  const channelSelect = document.getElementById('warnChannelFilterSelect');
  if(input && !input.dataset.bound) {
    input.dataset.bound = 'true';
    input.addEventListener('input', applyWarnFilter);
  }
  if(channelSelect && !channelSelect.dataset.bound) {
    channelSelect.dataset.bound = 'true';
    channelSelect.addEventListener('change', applyWarnFilter);
  }
  if(clear && !clear.dataset.bound) {
    clear.dataset.bound = 'true';
    clear.addEventListener('click', () => { if(input) input.value = ''; applyWarnFilter(); input?.focus(); });
  }
}
function log(message, type='', channelHint=null) {
  const now = new Date();
  const text = `[${String(now.getHours()).padStart(2,'0')}:${String(now.getMinutes()).padStart(2,'0')}:${String(now.getSeconds()).padStart(2,'0')}] #${++logSerial} ${message}`;
  const logList = document.getElementById('logList');
  if(logList) {
    const p = document.createElement('p');
    const channel = channelHint ? String(channelHint) : inferLogChannel(message);
    p.id = `logEntry${logSerial}`; p.dataset.controlId = p.id; p.dataset.channel = channel; p.className = type; p.textContent = text;
    logList.prepend(p);
    while(logList.children.length > 80) logList.removeChild(logList.lastChild);
    applyLogFilter();
  }
  if(type === 'warn' || type === 'err') {
    warningCount += 1;
    const counter = document.getElementById('warnTabCount');
    if(counter) counter.textContent = warningCount;
    const panelCounter = document.getElementById('warnPanelCount');
    if(panelCounter) panelCounter.textContent = warningCount;
    const warnList = document.getElementById('warnList');
    if(warnList) {
      const wp = document.createElement('p');
      const warnChannel = channelHint ? String(channelHint) : inferLogChannel(message);
      wp.id = `warnEntry${warningCount}`; wp.dataset.controlId = wp.id; wp.dataset.channel = warnChannel; wp.className = type; wp.textContent = text;
      warnList.prepend(wp);
      while(warnList.children.length > 80) warnList.removeChild(warnList.lastChild);
      applyWarnFilter();
    }
  }
}
function setConn(kind) {
  const dot = document.getElementById('connDot');
  if(dot) dot.className = 'dot' + (kind==='warn'?' warn':kind==='err'?' err':'');
  const txt = document.getElementById('connText');
  if(txt) txt.textContent = kind==='err' ? '鎶ヨ / 闇€浜哄伐纭' : kind==='warn' ? '璁惧鍦ㄧ嚎 / 鏈夋彁閱? : '璁惧鍦ㄧ嚎 / 妯℃嫙妯″紡';
}

function scanSamples() {
  setInfoPanel('鏍锋湰鍖烘壂鎻?, ['璇嗗埆鐜荤墖銆佺櫥璁版煋鑹茬被鍨嬶紝骞舵洿鏂版瘡涓幓鐗囩殑姝ラ鐘舵€併€?]);
  cameraStates.arm = 'active'; updateArmVisual();
  const slides = byCategory.get('鐜荤墖閫氶亾') || [];
  slides.forEach(s => { itemState.set(s.name, 'ready'); setSlideOp(s.name, 0, true); });
  channels.forEach(ch => { ch.state='idle'; ch.progress = Math.max(ch.progress, 0); });
  setLegacySampleCount(slides.length);
  updateHeaderMetrics({ active:slides.length });
  setPhaseText('鏍锋湰鍖烘壂鎻忓畬鎴?);
  cameraStates.arm = 'complete'; updateArmVisual();
  log(`鏍锋湰鍖烘壂鎻忓畬鎴愶細璇嗗埆 ${slides.length} 寮犵幓鐗嘸, 'ok'); drawSlideOps(); updateVisualStates(); renderChannelCards();
}
function scanReagents() {
  setInfoPanel('璇曞墏鍖烘壂鎻?, ['鎵弿璇曞墏浣嶃€佹洿鏂拌瘯鍓傛槧灏勶紝骞跺埛鏂颁綑閲忕姸鎬併€?]);
  cameraStates.reagent = 'active'; drawAuxPorts();
  (byCategory.get('璇曞墏鍖?) || []).forEach(r => itemState.set(r.name, 'scanned'));
  setPhaseText('璇曞墏鍖烘壂鎻忓畬鎴?);
  cameraStates.reagent = 'complete'; drawAuxPorts();
  log('璇曞墏鍖烘壂鎻忓畬鎴愶細5鍒椕?璇曞墏浣嶅凡鏄犲皠锛圫11~S58锛?, 'ok'); updateVisualStates();
}
function simulateLowReagent() {
  setInfoPanel('璇曞墏涓嶈冻鎻愰啋', ['浣庢恫浣嶈瘯鍓備綅宸插湪璇曞墏鍖洪珮浜紝鍙偣鍑诲叿浣撹瘯鍓傛煡鐪嬩綑閲忋€?]);
  const reagents = byCategory.get('璇曞墏鍖?) || [];
  [4,12,27].forEach(i => { if(reagents[i]) { itemState.set(reagents[i].name, 'low'); itemLevels.set(reagents[i].name, 10 + i % 7); } });
  setPhaseText('璇曞墏涓嶈冻鎻愰啋');
  setConn('warn'); log('鎶ヨ锛氭娴嬪埌閮ㄥ垎璇曞墏浣欓噺浣庝簬闃堝€硷紝璇疯ˉ鍏呮垨鏇存崲', 'warn'); drawData(); updateVisualStates();
}
function togglePullChannel() {
  const ch = channels[selectedChannel-1];
  setInfoPanel('閫氶亾鎶藉嚭/鎺ㄥ洖', [`鎿嶄綔瀵硅薄锛氶€氶亾${selectedChannel}`, '鐩稿叧璇︽儏灏嗙粺涓€鏄剧ず鍦ㄦ鍖哄煙銆?]);
  ch.pulled = !ch.pulled; ch.state = ch.pulled ? 'open' : (ch.progress >= 100 ? 'complete' : ch.progress>0 ? 'running' : 'idle');
  const slides = (byCategory.get('鐜荤墖閫氶亾') || []).filter(s => getChannelIdFromName(s.name) === selectedChannel);
  slides.forEach(s => itemState.set(s.name, ch.pulled ? 'error' : (ch.progress ? 'running' : 'ready')));
  log(`閫氶亾${selectedChannel}${ch.pulled?'宸叉娊鍑猴細鐣岄潰闂儊鎻愰啋':'宸叉帹鍥烇細鎭㈠鐘舵€佹樉绀?}`, ch.pulled?'warn':'ok', selectedChannel);
  updateVisualStates(); renderChannelCards();
}
function simulateAlarm() {
  setInfoPanel('娑蹭綅寮傚父', ['绾按涓嶈冻銆佸簾娑叉《鎺ヨ繎婊★紝璇峰鐞嗗悗鍐嶇户缁祦绋嬨€?]);
  liquids.pure = 9; liquids.pbs = 18; liquids.waste = 93; liquids.toxic = 87;
  setConn('err'); setPhaseText('娑蹭綅寮傚父');
  log('娑蹭綅寮傚父锛氱函姘翠笉瓒?/ 搴熸恫妗舵帴杩戞弧锛岃澶勭悊鍚庣户缁?, 'err'); renderLiquids();
}


function showProductionSubTab(section='status') {
  const normalized = ({ production:'status', status:'status', log:'log', logs:'log', warn:'warn', warning:'warn', warnings:'warn' }[section] || 'status');
  activeProductionSection = normalized;
  document.querySelectorAll('[data-production-subtab]').forEach(btn => {
    const active = btn.getAttribute('data-production-subtab') === normalized;
    btn.classList.toggle('active', active);
    btn.setAttribute('aria-selected', active ? 'true' : 'false');
  });
  document.querySelectorAll('[data-production-pane]').forEach(pane => {
    pane.classList.toggle('active', pane.getAttribute('data-production-pane') === normalized);
  });
}
function bindProductionSubTabs() {
  document.querySelectorAll('[data-production-subtab]').forEach(btn => {
    if(btn.dataset.productionSubtabBound === 'true') return;
    btn.dataset.productionSubtabBound = 'true';
    btn.addEventListener('click', () => showProductionSubTab(btn.getAttribute('data-production-subtab') || 'status'));
  });
  showProductionSubTab(activeProductionSection || 'status');
}

let sideTabSwitching = false;
let pendingConfigRender = null;
let pendingConfigRenderFrame = null;
let configRenderToken = 0;
function cancelScheduledConfigRender() {
  if(pendingConfigRender) {
    clearTimeout(pendingConfigRender);
    pendingConfigRender = null;
  }
  if(pendingConfigRenderFrame) {
    cancelAnimationFrame(pendingConfigRenderFrame);
    pendingConfigRenderFrame = null;
  }
}
function scheduleConfigPaneRender(section=activeConfigSection) {
  const normalized = normalizeConfigSectionKey(section);
  activeConfigSection = normalized;
  cancelScheduledConfigRender();
  const token = ++configRenderToken;
  renderConfigLoadingShell(normalized);
  pendingConfigRenderFrame = requestAnimationFrame(() => {
    pendingConfigRenderFrame = null;
    pendingConfigRender = setTimeout(() => {
      pendingConfigRender = null;
      if(token !== configRenderToken) return;
      renderConfigPane(normalized);
    }, 0);
  });
}
function showSideTab(target) {
  if(sideTabSwitching) return;
  sideTabSwitching = true;
  try {
  if(target === 'debug' && CURRENT_USER.role !== 'admin') {
    target = 'production';
    log('瀹為獙鍛樻潈闄愰殣钘忚皟璇曢〉闈紝璇蜂娇鐢ㄧ鐞嗗憳璐﹀彿鐧诲綍鍚庡啀杩涘叆璋冭瘯銆?, 'warn');
  }
  const productionAliases = { status:'status', production:'status', log:'log', logs:'log', warn:'warn', warning:'warn', warnings:'warn' };
  const requestedProductionSection = Object.prototype.hasOwnProperty.call(productionAliases, target) ? productionAliases[target] : null;
  const panes = { production:'productionPane', precheck:'precheckPane', debug:'debugPane', config:'configPane', settings:'settingsPane' };
  const view = requestedProductionSection ? 'production' : (Object.prototype.hasOwnProperty.call(panes, target) ? target : 'production');
  const card = document.getElementById('rightConsoleCard');
  const flowWideView = view === 'config' && normalizeConfigSectionKey(activeConfigSection) === 'files';
  if(card) card.dataset.view = flowWideView ? 'config-edit' : view;
  if(view !== 'config') cancelScheduledConfigRender();
  const configPaneEl = document.getElementById('configPane');
  const leavingConfig = view !== 'config' && configPaneEl?.classList.contains('active');
  if(leavingConfig) configPaneEl.replaceChildren();
  if(view === 'config') enterFlowWidePanel(); else leaveFlowWidePanel();
  Object.entries(panes).forEach(([key, id]) => {
    const pane = document.getElementById(id);
    if(pane) pane.classList.toggle('active', key === view);
  });
  document.querySelectorAll('[data-side-tab]').forEach(btn => {
    const btnTarget = btn.getAttribute('data-side-tab');
    const active = btnTarget === view;
    btn.classList.toggle('active', active);
    btn.setAttribute('aria-selected', active ? 'true' : 'false');
  });
  if(view === 'production') {
    bindProductionSubTabs();
    showProductionSubTab(requestedProductionSection || activeProductionSection || 'status');
  }
  if(view === 'config') scheduleConfigPaneRender(activeConfigSection);
  if(view === 'debug') renderDebugPane();
  } finally {
    sideTabSwitching = false;
  }
}


function renderPrecheckList() {
  const wrap = document.getElementById('precheckList');
  const runAllBtn = document.getElementById('precheckRunAllBtn');
  if(runAllBtn) { runAllBtn.disabled = precheckRunning; runAllBtn.onclick = runPrecheck; }
  if(!wrap) return;
  const statusLabel = { idle:'寰呮鏌?, running:'妫€鏌ヤ腑', pass:'閫氳繃', error:'寮傚父' };
  wrap.innerHTML = PRECHECK_STEPS.map((step, idx) => {
    const state = precheckState[idx] || 'idle';
    const stepId = `precheckStep${idx + 1}`;
    return `<div class="precheck-step ${state}" id="${stepId}" data-control-id="${stepId}"><span class="precheck-dot" id="${stepId}StatusDot"></span><span class="precheck-main" id="${stepId}Content"><span class="precheck-name" id="${stepId}Name">${escapeHtml(step.label)}</span></span><span class="precheck-status" id="${stepId}StatusText">${statusLabel[state] || state}</span><button type="button" class="precheck-run-btn" id="${stepId}RunBtn" data-precheck-run="${idx}" ${precheckRunning?'disabled':''}>妫€娴?/button></div>`;
  }).join('');
  document.querySelectorAll('[data-precheck-run]').forEach(btn => { btn.onclick = () => runSinglePrecheck(Number(btn.dataset.precheckRun)); });
}
async function runSinglePrecheck(index) {
  if(precheckRunning || !PRECHECK_STEPS[index]) return;
  precheckRunning = true;
  precheckPassed = false;
  setButtonState('startBtn', { disabled:true });
  showSideTab('precheck');
  precheckState[index] = 'running';
  renderPrecheckList();
  log(`鍗曢」妫€娴嬶細${PRECHECK_STEPS[index].label}`, 'ok');
  await sleep(260);
  const precheckResults = window.digitalTwinPrecheckResults || {};
  const precheckFailures = window.digitalTwinPrecheckFailures || [];
  const ok = precheckResults[PRECHECK_STEPS[index].label] !== false && !precheckFailures.includes(PRECHECK_STEPS[index].label);
  precheckState[index] = ok ? 'pass' : 'error';
  precheckRunning = false;
  precheckPassed = precheckState.every(state => state === 'pass');
  setButtonState('startBtn', { disabled:!precheckPassed });
  renderPrecheckList();
  if(ok) log(`鍗曢」妫€娴嬮€氳繃锛?{PRECHECK_STEPS[index].label}`, 'ok');
  else log(`鍗曢」妫€娴嬪け璐ワ細${PRECHECK_STEPS[index].label}`, 'err');
}
async function runPrecheck() {
  if(precheckRunning) return;
  precheckRunning = true;
  precheckPassed = false;
  precheckState = PRECHECK_STEPS.map(() => 'idle');
  setButtonState('startBtn', { disabled:true });
  showSideTab('precheck');
  renderPrecheckList();
  log('妫€娴嬪惎鍔細璇诲彇鍚庣璁惧鍒濆鍖栦笌杩愯鍓嶆牎楠?, 'ok');
  let deviceInit = null;
  let preflight = null;
  try {
    // 瑙﹀彂璁惧鍒濆鍖?POST)锛屽け璐?濡傚凡鏈夊垵濮嬪寲鍦ㄨ窇)鍒欏洖閫€鍒拌鍙栨渶杩戜竴娆¤褰?GET)
    try {
      deviceInit = await backendApi('/api/device-initialization', { method:'POST', body: JSON.stringify({ commandId: makeCommandId('device-init') }) });
    } catch(e) { deviceInit = await apiGetOrNull('/api/device-initialization'); }
    preflight = await apiGetOrNull('/api/run/preflight');
  } catch(err) {
    precheckRunning = false;
    setButtonState('startBtn', { disabled:false });
    await loadDatabaseSnapshot();
    return;
  }
  const deviceOk = !!deviceInit?.ok || !!deviceInit?.completedAtUtc;
  const failures = (preflight?.issues || []).filter(x => String(x.severity || 'Fail').toLowerCase() !== 'warning');
  const deviceFailure = failures.some(x => x.area === 'Device');
  const reagentFailure = failures.some(x => x.area === 'Reagents');
  const realResults = {};
  PRECHECK_STEPS.forEach((step, idx) => {
    const isReagentStep = idx >= 7;
    realResults[step.label] = isReagentStep ? !reagentFailure : (deviceOk && !deviceFailure);
  });
  window.digitalTwinPrecheckResults = realResults;
  window.digitalTwinPrecheckFailures = [];
  for(let i=0; i<PRECHECK_STEPS.length; i++) {
    precheckState[i] = 'running';
    renderPrecheckList();
    log(`妫€娴嬶細${PRECHECK_STEPS[i].label}`, '');
    await sleep(180);
    const ok = realResults[PRECHECK_STEPS[i].label] !== false;
    precheckState[i] = ok ? 'pass' : 'error';
    renderPrecheckList();
    if(!ok) {
      precheckRunning = false;
      precheckPassed = false;
      setButtonState('startBtn', { disabled:true });
      const failText = document.getElementById('settingsPrecheckText'); if(failText) failText.textContent = '鏈畬鎴?;
      setInfoPanel('妫€娴嬫湭閫氳繃', [`澶辫触椤癸細${PRECHECK_STEPS[i].label}`, '璇峰鐞嗗紓甯稿悗閲嶆柊妫€娴嬨€?]);
      log(`妫€娴嬪け璐ワ細${PRECHECK_STEPS[i].label}`, 'err');
      await loadDatabaseSnapshot();
      return;
    }
    await sleep(60);
  }
  precheckRunning = false;
  precheckPassed = !!preflight?.ok && Object.values(realResults).every(v => v !== false);
  const settingsPreText = document.getElementById('settingsPrecheckText'); if(settingsPreText) settingsPreText.textContent = precheckPassed ? '宸查€氳繃' : '鏈畬鎴?;
  setButtonState('startBtn', { disabled:!precheckPassed });
  renderPrecheckList();
  setInfoPanel(precheckPassed ? '妫€娴嬮€氳繃' : '妫€娴嬫湭閫氳繃', precheckPassed
    ? ['涓绘帶銆佹満姊拌噦銆佸埗鍐枫€佹壂鐮佸櫒銆佹恫浣嶄紶鎰熷櫒銆佹礂閽堝拰搴熸恫/鎺掓瘨妗剁姸鎬佸潎姝ｅ父銆?, '鐜板湪鍙互鐐瑰嚮椤堕儴鈥滃紑濮嬧€濇墽琛屾煋鑹叉祦绋嬨€?]
    : ['鍚庣棰勬鎴栬澶囧垵濮嬪寲鏈€氳繃锛岃澶勭悊寮傚父鍚庨噸鏂版娴嬨€?]);
  log(precheckPassed ? '妫€娴嬮€氳繃锛氬紑濮嬫寜閽凡鍚敤' : '妫€娴嬫湭閫氳繃锛氬悗绔妫€鎴栬澶囧垵濮嬪寲寮傚父', precheckPassed ? 'ok' : 'err');
  await loadDatabaseSnapshot();
}

async function collectStainingTaskIds() {
  try {
    const snap = await backendApi('/api/operator/snapshot');
    return (snap?.channels || [])
      .flatMap(ch => ch.slides || [])
      .map(s => s?.stainingTaskId)
      .filter(Boolean);
  } catch(err) { return []; }
}

async function startRealRun() {
  if(running) return;
  setButtonState('startBtn', { disabled:true });
  try {
    const report = await writeApi('/api/run/preflight', { method:'GET' });
    if(!report?.ok) {
      log('鍚姩鍓嶆牎楠屾湭閫氳繃锛岃鍏堝畬鎴愭娴?, 'err');
      showSideTab('precheck');
      renderPrecheckList();
      setButtonState('startBtn', { disabled:!precheckPassed });
      return;
    }
    let run = await apiGetOrNull('/api/runs/current');
    if(!run || ['Completed','Stopped','completed','stopped'].includes(run.status)) {
      const taskIds = await collectStainingTaskIds();
      if(!taskIds.length) {
        setInfoPanel('鏆傛棤鍙繍琛屼换鍔?, ['璇峰厛鎵爜/鍒涘缓浠诲姟鍚庡啀寮€濮嬨€?]);
        log('娌℃湁鍙繍琛岀殑浠诲姟锛氳鍏堟壂鐮?鍒涘缓浠诲姟', 'warn');
        setButtonState('startBtn', { disabled:!precheckPassed });
        return;
      }
      const created = await writeApi('/api/runs', { method:'POST', body: JSON.stringify({ commandId: makeCommandId('run-create'), stainingTaskIds: taskIds, preflightStateHash: report.stateHash || null }) });
      run = await apiGetOrNull(`/api/runs/${encodeURIComponent(created.runId)}`) || { id: created.runId, status: created.status };
    }
    const runId = run?.id || run?.runId;
    if(!runId) { log('鏈幏鍙栧埌杩愯 ID', 'err'); setButtonState('startBtn', { disabled:!precheckPassed }); return; }
    await writeApi(`/api/runs/${encodeURIComponent(runId)}/start`, { method:'POST', body: JSON.stringify({ commandId: makeCommandId('run-start'), preflightStateHash: report.stateHash || null }) });
    running = true; paused = false; cancelAnimation = false;
    setButtonState('startBtn', { disabled:true }); setButtonState('pauseBtn', { disabled:false, textContent:'鏆傚仠' });
    log('杩愯宸插惎鍔?, 'ok');
    await loadDatabaseSnapshot();
  } catch(err) {
    setButtonState('startBtn', { disabled:!precheckPassed });
  }
}

async function pauseOrResumeRun() {
  try {
    const run = await apiGetOrNull('/api/runs/current');
    const runId = run?.id || run?.runId;
    if(!runId) { log('褰撳墠娌℃湁鍙殏鍋?鎭㈠鐨勮繍琛?, 'warn'); return; }
    const status = String(run.status || '').toLowerCase();
    if(status === 'paused') {
      await writeApi(`/api/runs/${encodeURIComponent(runId)}/resume`, { method:'POST', body: JSON.stringify({ commandId: makeCommandId('run-resume') }) });
      paused = false;
      log('杩愯宸叉仮澶?, 'ok');
    } else if(status === 'running') {
      await writeApi(`/api/runs/${encodeURIComponent(runId)}/pause`, { method:'POST', body: JSON.stringify({ commandId: makeCommandId('run-pause') }) });
      paused = true;
      log('杩愯宸叉殏鍋?, 'ok');
    } else {
      log(`褰撳墠杩愯鐘舵€佷负銆?{run.status || '鏈煡'}銆嶏紝鏃犳硶鏆傚仠/鎭㈠锛堜粎 Running 鍙殏鍋溿€丳aused 鍙仮澶嶏級`, 'warn');
      return;
    }
    const pauseBtnEl = document.getElementById('pauseBtn');
    if(pauseBtnEl) pauseBtnEl.textContent = paused ? '缁х画' : '鏆傚仠';
    await loadDatabaseSnapshot();
  } catch(err) { /* routed by writeApi */ }
}

function parseCsv(text) {
  const lines = text.replace(/^锘?, '').split(/\r?\n/).filter(line => line.trim());
  const headers = splitCsvLine(lines.shift());
  return lines.map(line => { const cols = splitCsvLine(line); const row = Object.fromEntries(headers.map((h,i)=>[h, cols[i] ?? ''])); return normalizeItem(row); }).filter(d => Number.isFinite(d.x) && Number.isFinite(d.y));
}
function splitCsvLine(line) {
  const out=[]; let cur='', quote=false;
  for(let i=0;i<line.length;i++) { const ch=line[i];
    if(ch==='"' && line[i+1]==='"') { cur+='"'; i++; }
    else if(ch==='"') quote=!quote;
    else if(ch===',' && !quote) { out.push(cur); cur=''; }
    else cur+=ch;
  } out.push(cur); return out;
}
function loadCsvFile(file) {
  const reader = new FileReader();
  reader.onload = () => {
    try {
      const next = parseCsv(reader.result); if(next.length < 10) throw new Error('甯冨眬鐐规暟閲忚繃灏?);
      coords = next; defaultCoordByName = new Map(coords.map(item => [item.name, { x:item.x, y:item.y }])); itemState = new Map(); itemLevels = new Map(); slideOps = new Map(); slideTemps = new Map(); currentStepIndex = 0; selectedName = null; selectedSvgControlId = null; detailMessage = null; selectedChannel = 1;
      renderAll(); setInfoPanel('甯冨眬 CSV 宸插鍏?, [`鏂囦欢锛?{file.name}`, `浣嶇疆鎬绘暟锛?{next.length}`]); log(`宸插鍏ュ竷灞€ CSV锛?{file.name}锛屽叡 ${next.length} 涓綅缃甡, 'ok');
    } catch(err) { log('CSV 瀵煎叆澶辫触锛? + err.message, 'err'); }
  };
  reader.readAsText(file, 'utf-8');
}


function safeStorageGet(key) { try { return localStorage.getItem(key); } catch(e) { return null; } }
function safeStorageSet(key, value) { try { localStorage.setItem(key, value); } catch(e) { console.warn('localStorage unavailable', e); } }
function deepClone(obj) { return JSON.parse(JSON.stringify(obj)); }
function normalizeProfile(profile, fallbackName='鏈懡鍚嶉厤缃?) {
  const p = deepClone(profile || {});
  p.id = String(p.id || `profile-${Date.now()}-${Math.random().toString(36).slice(2,7)}`);
  p.name = String(p.name || fallbackName);
  p.stainType = String(p.stainType || 'IHC');
  p.version = String(p.version || '1.0.0');
  p.description = String(p.description || '');
  p.steps = Array.isArray(p.steps) ? p.steps.slice(0, 12).map((step, idx) => normalizeConfigStep(step, idx)) : [];
  p.allowMultiPrimary = !!p.allowMultiPrimary;
  return p;
}
function normalizeConfigStep(step={}, idx=0) {
  const opKey = step.opKey || step.key || 'water';
  const op = OP_DEF_BY_KEY[opKey] || OP_DEFS[0];
  return {
    id:String(step.id || `step-${Date.now()}-${idx}-${Math.random().toString(36).slice(2,6)}`),
    opKey:op.key,
    label:String(step.label || op.label),
    durationSec:Math.max(0, Number(step.durationSec ?? step.duration ?? 0) || 0),
    toleranceSec:Math.max(0, Number(step.toleranceSec ?? step.tolerance ?? 0) || 0),
    immediateAfterPrev:!!step.immediateAfterPrev,
    requiresTemp:!!step.requiresTemp,
    targetTempC:step.targetTempC === null || step.targetTempC === '' || step.targetTempC === undefined ? null : Number(step.targetTempC),
    reagentRole:String(step.reagentRole || ''),
    allowMultiPrimary:!!step.allowMultiPrimary,
    notes:String(step.notes || '')
  };
}
function loadConfigProfiles() {
  const raw = safeStorageGet(CONFIG_STORAGE_KEY);
  if(raw) {
    try {
      const parsed = JSON.parse(raw);
      const arr = Array.isArray(parsed) ? parsed : Array.isArray(parsed.profiles) ? parsed.profiles : [];
      const profiles = arr.map((p, idx) => normalizeProfile(p, `閰嶇疆 ${idx+1}`)).filter(p => p.steps.length);
      if(profiles.length) return ensureDefaultProfile(profiles);
    } catch(e) { console.warn('閰嶇疆鏂囦欢璇诲彇澶辫触锛屼娇鐢ㄩ粯璁ゆā鏉?, e); }
  }
  return DEFAULT_CONFIG_PROFILES.map(normalizeProfile);
}
function ensureDefaultProfile(profiles) {
  const ids = new Set(profiles.map(p => p.id));
  const merged = [...profiles];
  DEFAULT_CONFIG_PROFILES.forEach(p => { if(!ids.has(p.id)) merged.push(normalizeProfile(p)); });
  return merged;
}
function saveConfigProfiles() { saveConfigProfilesOnly(); persistAssignments(); }
function saveConfigProfilesOnly() { safeStorageSet(CONFIG_STORAGE_KEY, JSON.stringify(configProfiles, null, 2)); }
function loadChannelConfigAssignments() {
  const raw = safeStorageGet(CONFIG_ASSIGN_STORAGE_KEY);
  const base = {1:null,2:null,3:null,4:null};
  if(raw) {
    try { const parsed = JSON.parse(raw); [1,2,3,4].forEach(id => { if(parsed && typeof parsed[id] === 'string') base[id] = parsed[id]; }); } catch(e) {}
  }
  return base;
}
function persistAssignments() {
  safeStorageSet(CONFIG_ASSIGN_STORAGE_KEY, JSON.stringify(channelConfigAssignments, null, 2));
  channels.forEach(ch => { ch.configProfileId = channelConfigAssignments[ch.id] || null; });
}
function getProfileById(id) { return configProfiles.find(p => p.id === id) || null; }
function getSelectedProfile() { return getProfileById(selectedConfigId) || configProfiles[0] || null; }
function getChannelProfile(channelId) {
  const id = channelConfigAssignments[channelId] || channels[(channelId || 1)-1]?.configProfileId || null;
  return getProfileById(id);
}
function stepToDef(step, idx=0) {
  const op = OP_DEF_BY_KEY[step?.opKey] || OP_DEFS[idx % OP_DEFS.length];
  return { ...op, key:op.key, label:step?.label || op.label, durationSec:Number(step?.durationSec || 0), toleranceSec:Number(step?.toleranceSec || 0), immediateAfterPrev:!!step?.immediateAfterPrev, requiresTemp:!!step?.requiresTemp, targetTempC:step?.targetTempC ?? null, notes:step?.notes || '', opKey:op.key };
}
function getChannelStepDefs(channelId) {
  const profile = getChannelProfile(channelId);
  return profile ? profile.steps.slice(0,12).map(stepToDef) : [];
}
function formatSeconds(sec) {
  const value = Math.max(0, Number(sec) || 0);
  if(value >= 60) {
    const m = Math.floor(value / 60), s = Math.round(value % 60);
    return s ? `${m}min${s}s` : `${m}min`;
  }
  return `${Math.round(value)}s`;
}
function renderProfileMini(profile, id='profileMiniPreview') {
  if(!profile) return `<div class="channel-config-preview" id="${id}" data-control-id="${id}"><div class="config-note">灏氭湭閫夋嫨閰嶇疆鏂囦欢銆傞€夋嫨鍚庯紝瀛敓椤甸潰鐜荤墖宸?涓?鍙充笁渚т細鎸夎閰嶇疆鍔ㄦ€佸姞杞芥楠ゅ浘鏍囥€?/div></div>`;
  const rows = profile.steps.slice(0,12).map((step, idx) => {
    const def = stepToDef(step, idx);
    const tags = [formatSeconds(step.durationSec), `卤${formatSeconds(step.toleranceSec || 0)}`];
    if(step.immediateAfterPrev) tags.push('绱ч偦');
    if(step.requiresTemp || (profile.tempControlFromStep && idx + 1 >= profile.tempControlFromStep)) tags.push(`${step.targetTempC || profile.targetTempC || 40}鈩僠);
    return `<div class="channel-config-step"><b>${idx+1}</b><span>${escapeHtml(def.label)}</span><span>${escapeHtml(tags.join(' 路 '))}</span></div>`;
  }).join('');
  const dab = profile.dabRatio ? `DAB ${profile.dabRatio.a}:${profile.dabRatio.b}:${profile.dabRatio.pureWater}锛?{profile.dabRatio.preparePolicy === 'per_run' ? '姣忚疆鐜伴厤' : '鎸夐渶閰嶇疆'}` : '鏃?DAB 閰嶇疆';
  return `<div class="channel-config-preview" id="${id}" data-control-id="${id}"><div class="config-meta"><b>${escapeHtml(profile.name)}</b> 路 ${escapeHtml(profile.stainType || '')} 路 ${profile.steps.length} 姝?路 ${escapeHtml(dab)}</div>${rows || '<div class="config-note">璇ラ厤缃殏鏃犳楠ゃ€?/div>'}</div>`;
}
function renderChannelBindingEditor(channelId, context='status') {
  const id = Number(channelId);
  const ch = channels[id - 1];
  const selectedProfileId = channelConfigAssignments[id] || ch?.configProfileId || '';
  const profile = getProfileById(selectedProfileId);
  const prefix = context === 'config' ? `configChannel${id}` : `statusChannel${id}`;
  const summary = context === 'config'
    ? `<div class="channel-binding-summary"><strong>閫氶亾${id}</strong><span>${escapeHtml(ch?.slides?.join(' / ') || '')}</span></div>`
    : '';
  return `<div class="channel-binding-editor ${context === 'config' ? 'config-channel-binding-editor' : ''}" id="${prefix}BindingEditor" data-control-id="${prefix}BindingEditor" data-channel-binding-editor="${id}">
    ${summary}
    <label class="channel-config-line" for="${prefix}ConfigSelect"><span>閫夋嫨閰嶇疆鏂囦欢</span><select class="channel-config-select" id="${prefix}ConfigSelect" data-control-id="${prefix}ConfigSelect" data-channel-bind-select="${id}">${profileOptions(selectedProfileId, true)}</select></label>
    <div class="channel-config-actions"><button type="button" class="channel-config-action primary-lite" id="${prefix}SetDefaultConfigBtn" data-control-id="${prefix}SetDefaultConfigBtn" data-channel-set-default-config="${id}">璁剧疆榛樿閰嶇疆</button><button type="button" class="channel-config-action" id="${prefix}OpenConfigBtn" data-control-id="${prefix}OpenConfigBtn" data-channel-open-config="${id}">鎵撳紑閰嶇疆/娴佺▼</button><button type="button" class="channel-config-action" id="${prefix}ClearConfigBtn" data-control-id="${prefix}ClearConfigBtn" data-channel-clear-config="${id}">娓呯┖閰嶇疆</button></div>
    <div data-channel-profile-preview="${id}" data-preview-id="${prefix}ProfilePreview">${renderProfileMini(profile, `${prefix}ProfilePreview`)}</div>
  </div>`;
}
function refreshChannelBindingEditors(channelId=null) {
  const ids = channelId ? [Number(channelId)] : [1,2,3,4];
  ids.forEach(id => {
    const value = channelConfigAssignments[id] || '';
    document.querySelectorAll(`[data-channel-bind-select="${id}"]`).forEach(sel => { sel.value = value; });
    document.querySelectorAll(`[data-channel-profile-preview="${id}"]`).forEach(wrap => {
      const previewId = wrap.getAttribute('data-preview-id') || `channel${id}ProfilePreview`;
      wrap.innerHTML = renderProfileMini(getProfileById(value), previewId);
    });
  });
}
function bindChannelBindingHandlers(root=document) {
  root.querySelectorAll('[data-channel-bind-select]').forEach(sel => {
    sel.addEventListener('change', () => {
      const id = Number(sel.dataset.channelBindSelect);
      const wrap = sel.closest('[data-channel-binding-editor]');
      const previewWrap = wrap?.querySelector(`[data-channel-profile-preview="${id}"]`);
      if(previewWrap) {
        const previewId = previewWrap.getAttribute('data-preview-id') || `channel${id}ProfilePreview`;
        previewWrap.innerHTML = renderProfileMini(getProfileById(sel.value || null), previewId);
      }
    });
  });
  root.querySelectorAll('[data-channel-set-default-config]').forEach(btn => {
    btn.addEventListener('click', () => {
      const id = Number(btn.dataset.channelSetDefaultConfig);
      const wrap = btn.closest('[data-channel-binding-editor]') || root;
      const sel = wrap.querySelector(`[data-channel-bind-select="${id}"]`) || document.querySelector(`[data-channel-bind-select="${id}"]`);
      const profileId = sel?.value || null;
      assignConfigToChannel(id, profileId, false);
      const profile = getProfileById(profileId);
      log(profile ? `閫氶亾${id}榛樿閰嶇疆宸茶缃細${profile.name}` : `閫氶亾${id}榛樿閰嶇疆宸叉竻绌篳, profile ? 'ok' : 'warn', id);
      refreshChannelBindingEditors(id);
      flashControl(btn);
    });
  });
  root.querySelectorAll('[data-channel-open-config]').forEach(btn => {
    btn.addEventListener('click', () => {
      const id = Number(btn.dataset.channelOpenConfig);
      selectedChannel = id;
      selectedConfigId = channelConfigAssignments[id] || configProfiles[0]?.id || null;
      activeConfigSection = 'files';
      enterFlowWidePanel();
      showSideTab('config');
      renderConfigPane('files');
    });
  });
  root.querySelectorAll('[data-channel-clear-config]').forEach(btn => {
    btn.addEventListener('click', () => assignConfigToChannel(Number(btn.dataset.channelClearConfig), null, true));
  });
}
function assignConfigToChannel(channelId, profileId, announce=false) {
  const id = Number(channelId);
  const profile = profileId ? getProfileById(profileId) : null;
  channelConfigAssignments[id] = profile ? profile.id : null;
  if(channels[id-1]) channels[id-1].configProfileId = profile ? profile.id : null;
  const slides = (byCategory.get('鐜荤墖閫氶亾') || []).filter(s => getChannelIdFromName(s.name) === id);
  slides.forEach(slide => slideOps.set(slide.name, profile ? Array.from({length:Math.min(12, profile.steps.length)}, () => false) : []));
  persistAssignments();
  drawSlideOps(); updateVisualStates(); renderChannelCards(); refreshChannelBindingEditors(id);
  if(announce) {
    log(profile ? `閫氶亾${id}宸茬粦瀹氶厤缃細${profile.name}` : `閫氶亾${id}宸叉竻绌洪厤缃甡, profile ? 'ok' : 'warn', id);
    const configIsActive = document.getElementById('configPane')?.classList.contains('active');
    if(configIsActive && activeConfigSection === 'channels') {
      renderConfigPane('channels');
    } else {
      showChannelDetail(id);
    }
  }
}
function applyProfileChangesToAssignments(profileId) {
  [1,2,3,4].forEach(id => { if(channelConfigAssignments[id] === profileId) assignConfigToChannel(id, profileId, false); });
}
function profileOptions(selectedId, includeEmpty=false) {
  const empty = includeEmpty ? '<option value="">鏈€夋嫨閰嶇疆鏂囦欢</option>' : '';
  return empty + configProfiles.map(p => `<option value="${escapeHtml(p.id)}" ${p.id===selectedId?'selected':''}>${escapeHtml(p.name)} 路 ${escapeHtml(p.stainType || '鏈垎绫?)}</option>`).join('');
}
function opShapeHtml(op) {
  const color = op.color || '#64748b';
  const base = `style="color:${color}"`;
  const cls = op.shape === 'square' ? 'square' : op.shape === 'triangle' ? 'triangle' : op.shape === 'diamond' ? 'diamond' : 'circle';
  return `<span class="shape-icon ${cls}" ${base}></span>`;
}
const CONFIG_SECTION_META = [
  ['files','閰嶇疆/娴佺▼','閫夋嫨銆佸鍒躲€佹柊寤恒€佸鍏ュ鍑洪厤缃枃浠讹紱褰撳墠閰嶇疆鐨勬祦绋嬮瑙堜笌姝ラ缂栬緫鍦ㄥ悓涓€椤靛畬鎴愩€?],
  ['rules','瑙勫垯/璇曞墏','璁剧疆 DAB 閰嶆瘮銆佹帶娓╄捣濮嬫銆佸涓€鎶楀苟琛屽強鐥呯悊瑙勫垝绾︽潫銆?],
  ['liquid','娑蹭綋绫诲瀷','閰嶇疆 Liquid Class 鐨勫惛娑层€佸姞娑层€佺┖姘旀銆侀澶栭噺鍜岃皟鑺傞噺鍙傛暟銆?],
  ['position','閫氶亾绉讳綅','缁存姢缁濆鍧愭爣绉诲姩銆佸瓟浣嶅潗鏍囪鍙栥€佸瓟浣?Z 楂樺害涓庢恫浣撶被鍨嬮厤缃€?],
  ['pipette','閫氶亾绉绘恫','鎸夊瓟浣嶅拰閽堝ご鎵ц鍚告恫銆佹墦娑层€佹墦绯荤粺娑层€佹恫闈㈡帰娴嬩笌娓呯┖閫氶亾娴嬭瘯銆?],
  ['mixheat','娣峰寑鍔犵儹','鎺у埗閫氶亾鏍锋湰鐩爣娓╁害銆佸綋鍓嶆俯搴﹁鍙栥€佸紑濮嬪姞鐑€佸仠姝㈠姞鐑拰娓╁害鏌ヨ銆?],
  ['thermal','娓呮礂娣峰寑','缁存姢娣峰寑鍙傛暟銆佹牱鏈竻娲椼€佺數纾侀榾鍜屼粨浣嶅厜鑰︼紱璇曞墏鍒跺喎鍜屼緵姘村凡杩佺Щ鍒扮姸鎬佸璞¤鎯呫€?]
];
const CONFIG_SECTION_KEYS = new Set(CONFIG_SECTION_META.map(([key]) => key));
function normalizeConfigSectionKey(section) {
  const key = section === 'edit' ? 'files' : String(section || 'files');
  return CONFIG_SECTION_KEYS.has(key) ? key : 'files';
}
function renderConfigNavHtml(section=activeConfigSection) {
  const activeKey = normalizeConfigSectionKey(section);
  return CONFIG_SECTION_META.map(([key,label,tip]) => `<button type="button" class="config-tab-btn ${activeKey===key?'active':''}" id="configTab${key[0].toUpperCase()+key.slice(1)}" data-config-section="${key}" data-tip="${escapeHtml(tip)}" title="${escapeHtml(tip)}" aria-label="${escapeHtml(label + '锛? + tip)}">${label}</button>`).join('');
}
function renderConfigSection(section, profile) {
  switch(normalizeConfigSectionKey(section)) {
    case 'files': return renderConfigFilesSection(profile);
    case 'rules': return renderConfigRulesSection(profile);
    case 'liquid': return renderConfigLiquidClassSection();
    case 'position': return renderConfigPositionSection();
    case 'pipette': return renderConfigPipetteSection();
    case 'mixheat': return renderConfigMixHeatSection();
    case 'scanner': return renderConfigScannerSection();
    case 'barcode': return renderConfigBarcodeSection();
    case 'thermal': return renderConfigThermalSection();
    default: return renderConfigFilesSection(profile);
  }
}
function setConfigCardView(section=activeConfigSection) {
  const card = document.getElementById('rightConsoleCard');
  const pane = document.getElementById('configPane');
  const isFlowWorkspace = normalizeConfigSectionKey(section) === 'files';
  if(card && pane?.classList.contains('active')) card.dataset.view = isFlowWorkspace ? 'config-edit' : 'config';
  return isFlowWorkspace;
}
function renderConfigLoadingShell(section=activeConfigSection) {
  const pane = document.getElementById('configPane');
  if(!pane || !pane.classList.contains('active')) return;
  const normalized = normalizeConfigSectionKey(section);
  const isFlowWorkspace = setConfigCardView(normalized);
  enterFlowWidePanel();
  pane.innerHTML = `<div class="config-module ${isFlowWorkspace?'edit-wide':''}" id="configModule" data-control-id="configModule" data-rendered-section="loading-${normalized}">
    <div class="config-tabs config-tabs-wide" id="configTabs" data-control-id="configTabs">${renderConfigNavHtml(normalized)}</div>
    <div class="config-active-section" id="configActiveSection" data-control-id="configActiveSection"><div class="config-loading-card"><strong>姝ｅ湪鍔犺浇閰嶇疆瀛愰〉</strong><span>浠呮覆鏌撳綋鍓嶅瓙椤碉紝鏃у瓙椤?DOM 宸查噴鏀俱€?/span></div></div>
  </div>`;
  bindConfigNavHandlers(pane);
}
function renderConfigPane(section=activeConfigSection) {
  const pane = document.getElementById('configPane'); if(!pane) return;
  cancelScheduledConfigRender();
  configRenderToken++;
  activeConfigSection = normalizeConfigSectionKey(section);
  const card = document.getElementById('rightConsoleCard');
  const paneActive = pane.classList.contains('active');
  const isFlowWorkspace = activeConfigSection === 'files';
  if(card && !paneActive) card.dataset.view = card.dataset.view || 'production';
  if(!paneActive) {
    pane.dataset.pendingSection = activeConfigSection;
    return;
  }
  if(!getSelectedProfile() && configProfiles.length) selectedConfigId = configProfiles[0].id;
  const selected = getSelectedProfile();
  setConfigCardView(activeConfigSection);
  enterFlowWidePanel();
  const sectionHtml = renderConfigSection(activeConfigSection, selected);
  pane.innerHTML = `<div class="config-module ${isFlowWorkspace?'edit-wide':''}" id="configModule" data-control-id="configModule" data-rendered-section="${activeConfigSection}">
    <div class="config-tabs config-tabs-wide" id="configTabs" data-control-id="configTabs">${renderConfigNavHtml(activeConfigSection)}</div>
    <div class="config-active-section" id="configActiveSection" data-control-id="configActiveSection">${sectionHtml}</div>
  </div>`;
  bindConfigHandlers();
}
function renderConfigFilesSection(profile) {
  const opTiles = OP_DEFS.map(op => `<div class="config-op-tile" draggable="true" id="configOpTile-${op.key}" data-op-key="${op.key}" data-control-id="configOpTile-${op.key}">${opShapeHtml(op)}<span>${escapeHtml(op.label)}</span></div>`).join('');
  const steps = profile ? profile.steps : [];
  if(profile && selectedConfigStepIndex >= steps.length) selectedConfigStepIndex = Math.max(0, steps.length - 1);
  const currentName = profile?.name || '鏈€夋嫨閰嶇疆';
  const currentMeta = profile ? `${profile.stainType || '鏈垎绫?} 路 ${steps.length} 姝?路 ${profile.targetTempC ? profile.targetTempC + '鈩? : '鏈缃帶娓?}` : '璇烽€夋嫨鎴栨柊寤洪厤缃?;
  const cards = configProfiles.map(p => `<div class="config-file-card ${p.id===selectedConfigId?'active':''}" id="configFileCard-${escapeHtml(p.id)}" data-control-id="configFileCard-${escapeHtml(p.id)}"><div class="config-file-head"><strong>${escapeHtml(p.name)}</strong><button type="button" class="mini-action" data-select-profile="${escapeHtml(p.id)}">閫夋嫨</button></div><div class="config-meta">${escapeHtml(p.stainType || '鏈垎绫?)} 路 ${p.steps.length} 姝?路 ${p.targetTempC ? p.targetTempC + '鈩? : '鏈缃?}<br>${escapeHtml(p.description || p.notes || '')}</div></div>`).join('');
  const bricks = renderConfigTimelineStepsHtml(profile);
  const editor = renderStepEditor(profile, selectedConfigStepIndex);
  return `<section class="config-pane-section config-integrated-section ${activeConfigSection==='files'?'active':''}" id="configSectionFiles" data-control-id="configSectionFiles">
    <details class="config-profile-drawer" id="configProfileFold" data-control-id="configProfileFold">
      <summary id="configProfileSummary" data-control-id="configProfileSummary">
        <span class="config-profile-label">褰撳墠閰嶇疆</span>
        <span class="config-current-profile-line" id="configCurrentProfileBar" data-control-id="configCurrentProfileBar"><strong id="configCurrentProfileName" data-control-id="configCurrentProfileName">${escapeHtml(currentName)}</strong><span id="configCurrentProfileMeta" data-control-id="configCurrentProfileMeta">${escapeHtml(currentMeta)}</span></span>
      </summary>
      <div class="config-profile-studio" id="configProfileStudio" data-control-id="configProfileStudio">
        <div class="config-profile-editor-panel" id="configProfileEditorPanel" data-control-id="configProfileEditorPanel">
          <div class="config-edit-column-title"><strong>閰嶇疆鏂囦欢</strong><span>閫夋嫨銆佹柊寤恒€佸鍏ュ鍑猴紱瀹屾垚鍚庤嚜鍔ㄥ洖鍒版祦绋嬬紪杈?/span></div>
          <div class="config-profile-form-grid" id="configProfileFormGrid" data-control-id="configProfileFormGrid">
            <label>閫夋嫨宸叉湁閰嶇疆<select id="configEditProfileSelect" data-control-id="configEditProfileSelect">${profileOptions(profile?.id || '')}</select></label>
            <label>閰嶇疆鍚嶇О<input id="configProfileNameInput" data-control-id="configProfileNameInput" value="${escapeHtml(profile?.name || '')}" placeholder="閰嶇疆鍚嶇О"></label>
            <label>鏌撹壊绫诲瀷<input id="configProfileTypeInput" data-control-id="configProfileTypeInput" value="${escapeHtml(profile?.stainType || '')}" placeholder="IHC / HE / PAS"></label>
            <label>閰嶇疆璇存槑<textarea id="configProfileDescInput" data-control-id="configProfileDescInput" rows="1" placeholder="閰嶇疆璇存槑">${escapeHtml(profile?.description || '')}</textarea></label>
          </div>
          <div class="config-profile-actions" id="configProfileActions" data-control-id="configProfileActions">
            <button type="button" id="configNewProfileBtn" data-control-id="configNewProfileBtn">浠?IHC 妯℃澘鏂板缓</button>
            <button type="button" id="configDuplicateProfileBtn" data-control-id="configDuplicateProfileBtn">澶嶅埗涓烘柊鐗堟湰</button>
            <button type="button" id="configRenameQuickBtn" data-control-id="configRenameQuickBtn" class="primary-save">淇濆瓨鍩虹淇℃伅</button>
            <button type="button" id="configDeleteProfileBtn" data-control-id="configDeleteProfileBtn" class="danger">鍒犻櫎褰撳墠閰嶇疆</button>
            <button type="button" id="configExportBtn" data-control-id="configExportBtn">瀵煎嚭褰撳墠 JSON</button>
            <button type="button" id="configExportAllBtn" data-control-id="configExportAllBtn">瀵煎嚭鍏ㄩ儴</button>
            <button type="button" id="configImportBtn" data-control-id="configImportBtn">瀵煎叆 JSON</button>
            <input type="file" id="configImportInput" data-control-id="configImportInput" accept="application/json,.json" style="display:none">
          </div>
        </div>
        <div class="config-profile-picker-panel" id="configProfilePickerPanel" data-control-id="configProfilePickerPanel"><strong>閰嶇疆鍒楄〃</strong><div class="config-file-list compact" id="configProfileFileList" data-control-id="configProfileFileList">${cards}</div></div>
      </div>
    </details>
    <div class="config-flow-workbench-shell" id="configFlowFold" data-control-id="configFlowFold">
      <div class="config-edit-layout config-flow-workbench" id="configEditLayout" data-control-id="configEditLayout">
        <div class="config-edit-column config-op-column" id="configOpColumn" data-control-id="configOpColumn"><div class="config-edit-column-title"><strong>鎿嶄綔搴?/strong><span>鎷栧叆涓棿鏃堕棿绾?/span></div><div class="config-left-workbench-scroll"><div class="op-palette" id="configOpPalette" data-control-id="configOpPalette">${opTiles}</div></div></div>
        <div class="config-edit-column config-flow-column" id="configFlowColumn" data-control-id="configFlowColumn"><div class="config-edit-column-title"><strong>娴佺▼鐮栧潡鏃堕棿绾?/strong><span>${steps.length} 姝?路 鐐瑰嚮鐮栧潡鐩存帴缂栬緫</span></div><div class="config-timeline ${steps.length?'':'empty'}" id="configEditorTimeline" data-control-id="configEditorTimeline">${bricks}</div></div>
        <div class="config-edit-column config-step-column" id="configStepColumn" data-control-id="configStepColumn"><div class="config-edit-column-title"><strong>褰撳墠姝ラ缂栬緫鍣?/strong><span>淇濆瓨瀵硅薄锛氶€変腑鐨勮繖涓€姝?/span></div>${editor}</div>
      </div>
    </div>
  </section>`;
}
function renderConfigEditSection(profile) { return ''; }
function renderStepEditor(profile, idx) {
  if(!profile || !profile.steps.length) return `<div class="config-editor" id="configStepEditor" data-control-id="configStepEditor"><strong>褰撳墠娌℃湁姝ラ</strong><div class="config-note">浠庡乏渚ф搷浣滃簱鎷栧叆绗竴灞傦紝鎴栫偣鍑绘搷浣滃悕绉板揩閫熷姞鍏ャ€傚綋鍓嶉厤缃枃浠剁殑鍩虹淇℃伅璇峰湪涓婃柟鈥滀繚瀛樺熀纭€淇℃伅鈥濄€?/div></div>`;
  const safeIdx = Math.max(0, Math.min(idx, profile.steps.length - 1));
  const step = profile.steps[safeIdx];
  const op = stepToDef(step, safeIdx);
  return `<div class="config-editor" id="configStepEditor" data-control-id="configStepEditor"><strong>缂栬緫绗?${safeIdx+1} 灞傦細${escapeHtml(op.label)}</strong><div class="config-note">杩欓噷淇濆瓨鐨勬槸褰撳墠閫変腑鐨勬祦绋嬫楠わ紱閰嶇疆鍚嶇О銆佺被鍨嬨€佽鏄庤浣跨敤涓婃柟鈥滀繚瀛樺熀纭€淇℃伅鈥濄€?/div><div class="config-editor-grid"><label>鏄剧ず鍚嶇О<input id="configStepLabelInput" value="${escapeHtml(step.label)}"></label><label>鎿嶄綔绫诲瀷<select id="configStepOpSelect">${OP_DEFS.map(d => `<option value="${d.key}" ${d.key===step.opKey?'selected':''}>${escapeHtml(d.label)}</option>`).join('')}</select></label><label>鎸佺画鏃堕棿 / 绉?input id="configStepDurationInput" type="number" min="0" step="1" value="${Number(step.durationSec || 0)}"></label><label>瀹瑰樊 / 绉?input id="configStepToleranceInput" type="number" min="0" step="1" value="${Number(step.toleranceSec || 0)}"></label><label>鐩爣娓╁害 / 鈩?input id="configStepTempInput" type="number" step="0.1" value="${step.targetTempC ?? ''}" placeholder="缁ф壙閰嶇疆"></label><label>璇曞墏瑙掕壊<input id="configStepRoleInput" value="${escapeHtml(step.reagentRole || '')}" placeholder="primary / wash / dab"></label></div><label class="config-checkline"><input type="checkbox" id="configStepImmediateInput" ${step.immediateAfterPrev?'checked':''}>鏈眰蹇呴』鍦ㄤ笂涓€灞傜粨鏉熷悗绔嬪嵆鎵ц锛屼笉缁欐満姊拌噦瑙勫垝鐣欓棿闅?/label><label class="config-checkline"><input type="checkbox" id="configStepTempEnableInput" ${step.requiresTemp?'checked':''}>鏈眰闇€瑕佹帶娓?/label><label class="config-checkline"><input type="checkbox" id="configStepMultiPrimaryInput" ${step.allowMultiPrimary?'checked':''}>鍏佽鍚岃疆瀹為獙瀵逛笉鍚岀幓鐗?閫氶亾浣跨敤澶氱涓€鎶?/label><textarea id="configStepNotesInput" rows="2" placeholder="澶囨敞銆佺梾鐞嗙害鏉熸垨璋冨害鎻愮ず">${escapeHtml(step.notes || '')}</textarea><div class="config-toolbar-row"><button type="button" id="configSaveStepBtn">淇濆瓨褰撳墠姝ラ</button><button type="button" id="configMoveStepUpBtn">涓婄Щ</button><button type="button" id="configMoveStepDownBtn">涓嬬Щ</button><button type="button" id="configDeleteStepBtn" class="danger">鍒犻櫎</button></div></div>`;
}

function renderConfigTimelineStepsHtml(profile) {
  const steps = profile ? profile.steps : [];
  if(profile && selectedConfigStepIndex >= steps.length) selectedConfigStepIndex = Math.max(0, steps.length - 1);
  return steps.map((step, idx) => {
    const def = stepToDef(step, idx);
    const border = def.color || '#64748b';
    const tags = [`${formatSeconds(step.durationSec)}`, `卤${formatSeconds(step.toleranceSec || 0)}`];
    if(step.immediateAfterPrev) tags.push('绱ч偦');
    if(step.requiresTemp) tags.push(`${step.targetTempC || profile.targetTempC || appSettings.workTargetTempC || 40}鈩僠);
    return `<div class="config-step-brick ${selectedConfigStepIndex===idx?'active':''}" draggable="true" id="configStepBrick${idx+1}" data-step-index="${idx}" data-control-id="configStepBrick${idx+1}" style="border-left-color:${border}"><div class="config-step-index">${idx+1}</div><div class="config-step-main"><div class="config-step-title"><span>${escapeHtml(def.label)}</span></div><div class="config-step-sub"><span>${escapeHtml(tags.join(' 路 '))}</span></div></div><span>${opShapeHtml(def)}</span></div>`;
  }).join('');
}
function refreshConfigFlowTitle(profile=getSelectedProfile()) {
  const steps = profile?.steps || [];
  const title = document.querySelector('#configFlowColumn .config-edit-column-title span');
  if(title) title.textContent = `${steps.length} 姝?路 鐐瑰嚮鐮栧潡鐩存帴缂栬緫`;
}
function bindConfigStepBrickHandlers(root=document) {
  root.querySelectorAll('.config-step-brick').forEach(brick => {
    if(brick.dataset.stepBrickBound === 'true') return;
    brick.dataset.stepBrickBound = 'true';
    brick.addEventListener('click', evt => {
      evt.preventDefault();
      selectedConfigStepIndex = Number(brick.dataset.stepIndex || 0);
      updateConfigStepSelectionOnly();
      renderConfigStepEditorOnly();
    });
    brick.addEventListener('dragstart', evt => {
      evt.dataTransfer.setData('application/x-step-index', brick.dataset.stepIndex);
      evt.dataTransfer.effectAllowed = 'move';
    });
    brick.addEventListener('dragover', evt => evt.preventDefault());
  });
}
function updateConfigStepSelectionOnly() {
  const profile = getSelectedProfile();
  const max = Math.max(0, (profile?.steps?.length || 1) - 1);
  selectedConfigStepIndex = Math.max(0, Math.min(Number(selectedConfigStepIndex) || 0, max));
  document.querySelectorAll('#configEditorTimeline .config-step-brick').forEach(brick => {
    brick.classList.toggle('active', Number(brick.dataset.stepIndex || 0) === selectedConfigStepIndex);
  });
}
function renderConfigStepEditorOnly() {
  const column = document.getElementById('configStepColumn');
  if(!column) return;
  const oldEditor = document.getElementById('configStepEditor');
  const html = renderStepEditor(getSelectedProfile(), selectedConfigStepIndex);
  if(oldEditor) oldEditor.outerHTML = html;
  else column.insertAdjacentHTML('beforeend', html);
  bindClick('configSaveStepBtn', saveSelectedStep);
  bindClick('configMoveStepUpBtn', () => moveSelectedStep(-1));
  bindClick('configMoveStepDownBtn', () => moveSelectedStep(1));
  bindClick('configDeleteStepBtn', deleteSelectedStep);
}
function refreshConfigTimelineOnly() {
  const profile = getSelectedProfile();
  const timeline = document.getElementById('configEditorTimeline');
  if(!timeline) return;
  const keepTop = timeline.scrollTop;
  timeline.innerHTML = renderConfigTimelineStepsHtml(profile);
  timeline.classList.toggle('empty', !(profile?.steps?.length));
  timeline.scrollTop = keepTop;
  refreshConfigFlowTitle(profile);
  bindConfigStepBrickHandlers(timeline);
  updateConfigStepSelectionOnly();
}
function refreshConfigFlowWorkbenchOnly() {
  refreshConfigTimelineOnly();
  renderConfigStepEditorOnly();
}

function renderConfigRulesSection(profile) {
  const dab = profile?.dabRatio || { a:1, b:1, pureWater:18, preparePolicy:'per_run' };
  return `<section class="config-pane-section ${activeConfigSection==='rules'?'active':''}" id="configSectionRules" data-control-id="configSectionRules">
    <div class="config-editor config-rules-card" id="configRulesCard" data-control-id="configRulesCard"><strong>鐥呯悊鏌撹壊瑙勫垯 / 瑙勫垝绾︽潫</strong><div class="config-editor-grid"><label>榛樿鐩爣娓╁害 / 鈩?input id="configTargetTempInput" type="number" step="0.1" value="${profile?.targetTempC ?? ''}" placeholder="涓嶆帶娓?></label><label>浠庣鍑犳寮€濮嬫帶娓?input id="configTempFromInput" type="number" min="1" max="99" step="1" value="${profile?.tempControlFromStep ?? ''}" placeholder="渚嬪 4"></label><label>DAB A娑叉瘮渚?input id="configDabAInput" type="number" min="0" step="1" value="${dab.a ?? 1}"></label><label>DAB B娑叉瘮渚?input id="configDabBInput" type="number" min="0" step="1" value="${dab.b ?? 1}"></label><label>绾按姣斾緥<input id="configDabWaterInput" type="number" min="0" step="1" value="${dab.pureWater ?? 18}"></label><label>DAB閰嶇疆绛栫暐<select id="configDabPolicySelect"><option value="per_run" ${dab.preparePolicy==='per_run'?'selected':''}>姣忚疆瀹為獙杩囩▼涓厤缃?/option><option value="on_demand" ${dab.preparePolicy==='on_demand'?'selected':''}>鎸夐渶閰嶇疆</option><option value="pre_mix" ${dab.preparePolicy==='pre_mix'?'selected':''}>棰勫厛閰嶇疆</option></select></label></div><label class="config-checkline"><input type="checkbox" id="configAllowMultiPrimaryInput" ${profile?.allowMultiPrimary?'checked':''}>鍏佽鍚屼竴杞疄楠屼腑澶氱涓€鎶楀苟琛岃皟搴?/label><textarea id="configProfileNotesInput" rows="3" placeholder="閫氱敤瑙勫垯璇存槑">${escapeHtml(profile?.notes || '')}</textarea><button type="button" id="configSaveRulesBtn">淇濆瓨瑙勫垯</button><div class="config-note">杩炵画涓嶅彲闂撮殧鐨勬楠よ鍦ㄢ€滈厤缃?娴佺▼鈥濅腑鍕鹃€夆€滅揣閭绘墽琛屸€濄€備緥濡備竴鎶楀畬鎴愬悗娓呮礂娑查€氬父搴旂揣閭伙紱娓呮礂娑插畬鎴愬悗鍒颁簩鎶楀彲鏍规嵁瀹瑰樊鍜屾満姊拌噦璺緞瑙勫垝鐣欏嚭璋冨害绐楀彛銆?/div></div>
  </section>`;
}
function engineeringButton(label, action, cls='') {
  const className = cls ? ` class="${cls}"` : '';
  return `<button type="button"${className} data-action-log="${escapeHtml(action)}">${escapeHtml(label)}</button>`;
}
function renderDebugPane() {
  const pane = document.getElementById('debugPane'); if(!pane) return;
  const sections = [
    ['com', 'COM璁剧疆'],
    ['precision', '绮惧害鏍℃'],
    ['liquid', '娑蹭綋绫诲瀷'],
    ['module', '妯″潡娴嬭瘯']
  ];
  if(!sections.some(([key]) => key === activeDebugSection)) activeDebugSection = 'com';
  const consoleHtml = `<div class="test-console" id="debugCommandConsole" data-control-id="debugCommandConsole">[SIM] 宸ョ▼甯堣皟璇曟帶鍒跺彴灏辩华銆俓n[SIM] 褰撳墠瀛愰〉锛?{escapeHtml(sections.find(([key]) => key === activeDebugSection)?.[1] || 'COM璁剧疆')}銆?/div>`;
  const panes = {
    com: `<div class="subtab-pane debug-subpane ${activeDebugSection==='com'?'active':''}" id="debugSubpaneCom" data-debug-pane="com" role="tabpanel" aria-labelledby="debugSubtabCom"><section class="engineering-card" id="debugComCard" data-control-id="debugComCard"><strong>COM 璁剧疆 <span class="engineering-pill">榛樿 115200 / 8N1</span></strong><div class="engineering-form-grid"><label>COM鍙?select id="debugComPortSelect" data-control-id="debugComPortSelect"><option>COM1</option><option>COM2</option><option>COM3</option><option>COM4</option></select></label><label>娉㈢壒鐜?select id="debugBaudRateSelect" data-control-id="debugBaudRateSelect"><option selected>115200</option><option>57600</option><option>38400</option><option>9600</option></select></label><label>鏁版嵁浣?select id="debugDataBitsSelect" data-control-id="debugDataBitsSelect"><option selected>8 bits</option><option>7 bits</option></select></label><label>鍋滄浣?select id="debugStopBitsSelect" data-control-id="debugStopBitsSelect"><option selected>1 bits</option><option>2 bits</option></select></label><label>鏍￠獙浣?select id="debugParitySelect" data-control-id="debugParitySelect"><option selected>鏃?/option><option>Odd</option><option>Even</option></select></label><label>杩炴帴鐘舵€?input id="debugComStatusText" data-control-id="debugComStatusText" value="鏈繛鎺?/ 妯℃嫙" readonly></label></div><div class="engineering-actions">${engineeringButton('璁剧疆COM鍙?,'璋冭瘯锛氳缃瓹OM鍙ｅ弬鏁?,'primary-lite')}${engineeringButton('璇诲彇COM鍙?,'璋冭瘯锛氳鍙朇OM鍙ｅ弬鏁?)}${engineeringButton('閲嶅惎閫氳','璋冭瘯锛氶噸鍚覆鍙ｉ€氳','warn-lite')}</div><div class="engineering-note">涓插彛鍙傛暟棰勭疆涓?115200銆? 鏁版嵁浣嶃€? 鍋滄浣嶃€佹棤鏍￠獙锛涘悗缁彲鐩存帴鏄犲皠鍒颁笂浣嶆満涓插彛鍙傛暟銆?/div></section>${consoleHtml}</div>`,
    precision: `<div class="subtab-pane debug-subpane ${activeDebugSection==='precision'?'active':''}" id="debugSubpanePrecision" data-debug-pane="precision" role="tabpanel" aria-labelledby="debugSubtabPrecision"><section class="engineering-card" id="debugPrecisionCard" data-control-id="debugPrecisionCard"><strong>绮惧害鏍℃</strong><div class="engineering-form-grid"><label>X 鏍℃鍋忓樊 / mm<input id="debugMovePrecisionXInput" data-control-id="debugMovePrecisionXInput" type="number" step="0.001" value="0.000"></label><label>Y 鏍℃鍋忓樊 / mm<input id="debugMovePrecisionYInput" data-control-id="debugMovePrecisionYInput" type="number" step="0.001" value="0.000"></label><label>鍔犳牱浣撶Н鐩爣 / 渭L<input id="debugDispenseTargetInput" data-control-id="debugDispenseTargetInput" type="number" step="1" value="100"></label><label>瀹炴祴浣撶Н / 渭L<input id="debugDispenseMeasuredInput" data-control-id="debugDispenseMeasuredInput" type="number" step="0.1" placeholder="璇疯緭鍏?></label></div><div class="engineering-actions">${engineeringButton('鏍℃绉讳綅绮惧害纭','璋冭瘯锛氭牎姝ｇЩ浣嶇簿搴︾‘璁?,'ok-lite')}${engineeringButton('鏍℃鍔犳牱绮惧害纭','璋冭瘯锛氭牎姝ｅ姞鏍风簿搴︾‘璁?,'ok-lite')}</div><div class="engineering-note">涓や釜 Z 杞村彲鐙珛鎺у埗锛涜繖閲岄鐣欑Щ鍔ㄦ牎姝ｅ拰鍔犳牱绮惧害鏍℃鎸囦护銆?/div></section>${consoleHtml}</div>`,
    liquid: `<div class="subtab-pane debug-subpane ${activeDebugSection==='liquid'?'active':''}" id="debugSubpaneLiquid" data-debug-pane="liquid" role="tabpanel" aria-labelledby="debugSubtabLiquid"><section class="engineering-card" id="debugLiquidClassCard" data-control-id="debugLiquidClassCard"><strong>娑蹭綋绫诲瀷</strong><div class="engineering-kv"><div><span>褰撳墠 Liquid Class</span><b id="debugLiquidClassName">Ab</b></div><div><span>鍚告恫閫熷害</span><b id="debugLiquidAspSpeedText">寰呴厤缃?/b></div><div><span>鍔犳恫閫熷害</span><b id="debugLiquidDispSpeedText">寰呴厤缃?/b></div><div><span>绌烘皵娈靛弬鏁?/span><b>System / Leading / Trailing</b></div></div><div class="engineering-actions">${engineeringButton('鎵撳紑娑蹭綋绫诲瀷閰嶇疆','璋冭瘯锛氭墦寮€娑蹭綋绫诲瀷閰嶇疆','primary-lite')}${engineeringButton('璇诲彇褰撳墠娑蹭綋绫诲瀷','璋冭瘯锛氳鍙栧綋鍓嶆恫浣撶被鍨?)}${engineeringButton('淇濆瓨褰撳墠娑蹭綋绫诲瀷','璋冭瘯锛氫繚瀛樺綋鍓嶆恫浣撶被鍨?,'ok-lite')}</div><div class="engineering-note">瀹屾暣鍙傛暟鍦ㄢ€滈厤缃?鈫?娑蹭綋绫诲瀷鈥濅腑缁存姢锛屽寘鎷帰娑层€佺┖姘旀銆侀澶栭噺銆丆onditioning Volume 鍜?Blow-out銆?/div></section>${consoleHtml}</div>`,
    module: `<div class="subtab-pane debug-subpane ${activeDebugSection==='module'?'active':''}" id="debugSubpaneModule" data-debug-pane="module" role="tabpanel" aria-labelledby="debugSubtabModule"><section class="engineering-card" id="debugModuleTestCard" data-control-id="debugModuleTestCard"><strong>妯″潡娴嬭瘯</strong><div class="module-control-grid" id="debugModuleControlGrid" data-control-id="debugModuleControlGrid">${engineeringButton('鍔犳牱鑷傛祴璇?,'璋冭瘯锛氬姞鏍疯噦娴嬭瘯')}${engineeringButton('鏉＄爜鎵弿娴嬭瘯','璋冭瘯锛氭潯鐮佹壂鎻忔祴璇?)}${engineeringButton('娣峰寑娴嬭瘯','璋冭瘯锛氭贩鍖€娴嬭瘯')}${engineeringButton('娓呮礂鎺у埗娴嬭瘯','璋冭瘯锛氭竻娲楁帶鍒舵祴璇?)}${engineeringButton('鎵撳紑鏍￠獙鍏?,'璋冭瘯锛氭墦寮€鏍￠獙鍏?)}${engineeringButton('鍋滄鍏ㄩ儴娴嬭瘯','璋冭瘯锛氬仠姝㈠叏閮ㄦ祴璇?,'warn-lite')}</div><div class="engineering-note">鐢ㄤ簬鍗曠嫭楠岃瘉鍔犳牱鑷傘€佹潯鐮佹壂鎻忋€佹贩鍖€銆佹竻娲楃瓑妯″潡銆傚疄闄呭姩浣滄寚浠ゅ悗缁帴 DCR55銆佽繍鍔ㄦ帶鍒跺拰娉甸榾鎺у埗鍗忚銆?/div></section>${consoleHtml}</div>`
  };
  pane.innerHTML = `<div class="debug-module" id="debugModule" data-control-id="debugModule">
    <div class="subtab-nav debug-subtabs" id="debugSubtabs" role="tablist" aria-label="宸ョ▼甯堣皟璇曞瓙椤甸潰">
      ${sections.map(([key,label]) => `<button type="button" class="inner-tab-btn ${activeDebugSection===key?'active':''}" id="debugSubtab${key[0].toUpperCase()+key.slice(1)}" data-debug-section="${key}" role="tab" aria-selected="${activeDebugSection===key?'true':'false'}">${label}</button>`).join('')}
    </div>
    <div class="subtab-content" id="debugSubtabContent">${panes[activeDebugSection]}</div>
  </div>`;
  pane.querySelectorAll('[data-debug-section]').forEach(btn => {
    btn.addEventListener('click', () => { activeDebugSection = btn.getAttribute('data-debug-section') || 'com'; renderDebugPane(); });
  });
  bindEngineeringControlHandlers(pane);
}

function configNumberInput(id, label, unit='', value='', placeholder='璇疯緭鍏ュ唴瀹?) {
  return `<label>${escapeHtml(label)}<input id="${id}" data-control-id="${id}" type="number" value="${escapeHtml(String(value))}" placeholder="${escapeHtml(placeholder)}">${unit ? `<span class="subtle">${escapeHtml(unit)}</span>` : ''}</label>`;
}
function configTextInput(id, label, value='', placeholder='璇疯緭鍏ュ唴瀹?) {
  return `<label>${escapeHtml(label)}<input id="${id}" data-control-id="${id}" value="${escapeHtml(String(value))}" placeholder="${escapeHtml(placeholder)}"></label>`;
}
function configSelect(id, label, options, selected='') {
  const html = options.map(opt => {
    const value = Array.isArray(opt) ? opt[0] : opt;
    const text = Array.isArray(opt) ? opt[1] : opt;
    return `<option value="${escapeHtml(String(value))}" ${String(value)===String(selected)?'selected':''}>${escapeHtml(String(text))}</option>`;
  }).join('');
  return `<label>${escapeHtml(label)}<select id="${id}" data-control-id="${id}">${html}</select></label>`;
}
function configHoleOptions(selected='P11') {
  const sample = [];
  for(let ch=1; ch<=4; ch++) for(let i=1; i<=4; i++) sample.push(`R${ch}${i}`);
  const reagent = [];
  for(let col=1; col<=5; col++) for(let row=1; row<=8; row++) reagent.push(`S${col}${row}`);
  const mix = ['P11','P12','P21','P22','P31','P32','P41','P42','A娑?,'B娑?,'娲楅拡瀛?','娲楅拡瀛?','娲楅拡瀛?','娲楅拡瀛?','鎺掓瘨瀛?,'搴熸恫瀛?,'娓呮礂瀛?];
  const all = [...mix, ...sample, ...reagent];
  return all.map(code => `<option value="${escapeHtml(code)}" ${code===selected?'selected':''}>${escapeHtml(code)}</option>`).join('');
}
function sampleSlideCards() {
  const rows = [];
  for(let ch=1; ch<=4; ch++) {
    for(let i=1; i<=4; i++) {
      const code = `R${ch}${i}`;
      rows.push(`<div class="barcode-sample-card ${code==='R11'?'active':''}" id="barcodeSampleCard${code}" data-control-id="barcodeSampleCard${code}" data-svg-target="svg-slide-${code.toLowerCase()}" data-barcode-kind="sample"><strong>${code}</strong><span>鏍锋湰 ${ch}-${i}</span><div class="barcode-card-coord"><label>X<input id="barcode${code}XInput" data-control-id="barcode${code}XInput" placeholder="X"></label><label>Y<input id="barcode${code}YInput" data-control-id="barcode${code}YInput" placeholder="Y"></label></div></div>`);
    }
  }
  return rows.join('');
}
function reagentBarcodeCells() {
  const cells = [];
  for(let row=1; row<=8; row++) {
    for(let col=1; col<=5; col++) {
      const code = `S${col}${row}`;
      const locator = row===1 || row===8;
      cells.push(`<div class="reagent-position-cell ${locator?'locator':''}" id="barcodeReagentCell${code}" data-control-id="barcodeReagentCell${code}" data-svg-target="svg-reagent-${code.toLowerCase()}" data-barcode-kind="reagent" data-reagent-code="${code}">${code}</div>`);
    }
  }
  return cells.join('');
}
function renderConfigLiquidClassSection() {
  return `<section class="config-pane-section ${activeConfigSection==='liquid'?'active':''}" id="configSectionLiquidClass" data-control-id="configSectionLiquidClass">
    <div class="liquid-class-table-layout" id="liquidClassTableLayout" data-control-id="liquidClassTableLayout">
      <section class="engineering-card" id="liquidClassBaseCard" data-control-id="liquidClassBaseCard">
        <strong>娑蹭綋绫诲瀷</strong>
        <div class="engineering-mini-grid">
          ${configSelect('liquidClassSelect','褰撳墠绫诲瀷',['Ab','AR','DAB','Wash','PBS','PureWater','Custom'],'Ab')}
          ${configTextInput('liquidClassNewNameInput','鏂板缓鍚嶇О','','鏂板缓娑蹭綋绫诲瀷')}
        </div>
        <div class="engineering-actions">
          ${engineeringButton('鏂板缓娑蹭綋绫诲瀷','閰嶇疆/娑蹭綋绫诲瀷锛氭柊寤烘恫浣撶被鍨?,'primary-lite')}
          ${engineeringButton('璇诲彇娑蹭綋绫诲瀷鍙傛暟','閰嶇疆/娑蹭綋绫诲瀷锛氳鍙栨恫浣撶被鍨嬪弬鏁?)}
          ${engineeringButton('淇濆瓨娑蹭綋绫诲瀷鍙傛暟','閰嶇疆/娑蹭綋绫诲瀷锛氫繚瀛樻恫浣撶被鍨嬪弬鏁?,'ok-lite')}
        </div>
        <div class="engineering-note">Liquid Class 鏄竴缁勭Щ娑插弬鏁伴泦鍚堬紝鐢ㄤ簬閽堝涓嶅悓娑蹭綋绮樺害銆佽〃闈㈠紶鍔涖€佹尌鍙戞€х瓑鐗瑰緛浼樺寲绉绘恫鍔ㄤ綔銆?/div>
      </section>
      <section class="engineering-card" id="liquidAspirationCard" data-control-id="liquidAspirationCard">
        <strong>鍚告恫鐩稿叧鍙傛暟 / Aspiration Parameters</strong>
        <div class="engineering-mini-grid">
          ${configSelect('aspUseLldSelect','鏄惁浣跨敤娑查潰鎺㈡祴',['鏄?,'鍚?],'鏄?)}
          ${configNumberInput('aspLldSensitivityInput','娑查潰鎺㈡祴鐏垫晱搴?,'%')}
          ${configNumberInput('aspLldDownSpeedInput','鎺㈡恫涓嬮檷閫熷害','mm/s')}
          ${configNumberInput('aspFollowDepthInput','娑查潰璺熼殢娣卞害','mm')}
          ${configNumberInput('aspSpeedInput','鍚告恫閫熷害','')}
          ${configNumberInput('aspPreDelayInput','鍚告恫鍓嶅欢鏃?,'ms')}
          ${configNumberInput('aspPostDelayInput','鍚告恫鍚庡欢鏃?,'ms')}
          ${configNumberInput('aspRetractSpeedInput','鍥炴挙閫熷害','mm/s')}
          ${configNumberInput('aspSystemTrailingAirGapInput','System Trailing Air Gap','渭L')}
          ${configNumberInput('aspLeadingAirGapInput','Leading Air Gap','渭L')}
          ${configNumberInput('aspTrailingAirGapInput','Trailing Air Gap','渭L')}
          ${configNumberInput('aspExcessVolumeInput','Excess Volume','渭L')}
          ${configNumberInput('aspConditioningVolumeInput','Conditioning Volume','渭L')}
        </div>
      </section>
      <section class="engineering-card" id="liquidDispenseCard" data-control-id="liquidDispenseCard">
        <strong>鍔犳恫鐩稿叧鍙傛暟 / Dispense Parameters</strong>
        <div class="engineering-mini-grid">
          ${configSelect('dispUseLldSelect','鏄惁浣跨敤娑查潰鎺㈡祴',['鏄?,'鍚?],'鏄?)}
          ${configNumberInput('dispSpeedInput','鍔犳恫閫熷害 / Dispense Speed','')}
          ${configNumberInput('dispBreakoffSpeedInput','鏂恫閫熷害 / Breakoff Speed','')}
          ${configNumberInput('dispBlowoutVolumeInput','鎺掔┖浣撶Н / Blow-out Volume','')}
          ${configNumberInput('dispPreDelayInput','鍔犳恫鍓嶅欢鏃?,'ms')}
          ${configNumberInput('dispPostDelayInput','鍔犳恫鍚庡欢鏃?,'ms')}
          ${configNumberInput('dispRetractSpeedInput','鍥炴挙閫熷害','mm/s')}
          ${configSelect('dispTrailingAfterEachSelect','姣忔鎺掓恫鍚庢坊鍔犲熬闅忕┖姘旈棿闅?,['鏄?,'鍚?],'鏄?)}
        </div>
        <div class="engineering-actions">${engineeringButton('淇濆瓨鍚告恫/鍔犳恫鍙傛暟','閰嶇疆/娑蹭綋绫诲瀷锛氫繚瀛樺惛娑插姞娑插弬鏁?,'ok-lite')}${engineeringButton('鎭㈠榛樿鍙傛暟','閰嶇疆/娑蹭綋绫诲瀷锛氭仮澶嶉粯璁ゅ弬鏁?,'warn-lite')}</div>
      </section>
    </div>
  </section>`;
}
function renderConfigPositionSection() {
  return `<section class="config-pane-section ${activeConfigSection==='position'?'active':''}" id="configSectionPosition" data-control-id="configSectionPosition">
    <div class="engineering-config-module">
      <div class="engineering-status-strip"><span class="engineering-status-dot ok"></span><strong>閫氶亾绉讳綅 / 瀛斾綅鍧愭爣閰嶇疆</strong><span class="subtle">缁濆鍧愭爣绉诲姩銆佸綋鍓嶅潗鏍囪鍙栥€佸瓟浣嶅潗鏍囪鍙栧拰 Z 楂樺害鍙傛暟銆?/span></div>
      <div class="config-position-layout">
        <section class="engineering-card" id="configAbsoluteMoveCard" data-control-id="configAbsoluteMoveCard">
          <strong>缁濆鍧愭爣鍊肩Щ鍔ㄥ拰璇诲彇</strong>
          <div class="engineering-mini-grid three">
            ${configNumberInput('positionAbsoluteXInput','X 鍧愭爣')}
            ${configNumberInput('positionAbsoluteYInput','Y 鍧愭爣')}
            ${configNumberInput('positionAbsoluteZInput','Z 鍧愭爣')}
          </div>
          <div class="engineering-actions">${engineeringButton('绉诲姩','閰嶇疆/閫氶亾绉讳綅锛氱粷瀵瑰潗鏍囩Щ鍔?,'primary-lite')}${engineeringButton('璇诲彇褰撳墠鍧愭爣','閰嶇疆/閫氶亾绉讳綅锛氳鍙栧綋鍓嶅潗鏍?)}</div>
          <div class="engineering-note">璁惧鍧愭爣绯伙細鍙充笂瑙掍负鍘熺偣锛孹 鍚戝乏涓烘锛孻 鍚戜笅涓烘銆?/div>
        </section>
        <section class="engineering-card" id="configHoleMoveCard" data-control-id="configHoleMoveCard">
          <strong>瀛斾綅鍧愭爣绉诲姩鍜岄厤缃?/strong>
          <div class="engineering-mini-grid three">
            <label>瀛斾綅<select id="positionHoleSelect" data-control-id="positionHoleSelect">${configHoleOptions('P11')}</select></label>
            ${configNumberInput('positionHoleXInput','X 鍧愭爣')}
            ${configNumberInput('positionHoleYInput','Y 鍧愭爣')}
          </div>
          <div class="config-hole-z-grid">
            <div class="config-z-radio-list" role="radiogroup" aria-label="Z 鍙傛暟閫夋嫨">
              <label><input type="radio" name="positionZMode" id="positionZTravelRadio" data-control-id="positionZTravelRadio"> Z-Travel锛堝畨鍏ㄧЩ鍔ㄩ珮搴︼級</label>
              <label><input type="radio" name="positionZMode" id="positionZStartRadio" data-control-id="positionZStartRadio" checked> Z-Start锛堟帰娑查珮搴︼級</label>
              <label><input type="radio" name="positionZMode" id="positionZEndRadio" data-control-id="positionZEndRadio"> Z-End锛堢摱搴曟瀬闄愰珮搴︼級</label>
              <label><input type="radio" name="positionZMode" id="positionZDispensRadio" data-control-id="positionZDispensRadio"> Z-Dispens锛堢Щ娑查珮搴︼級</label>
            </div>
            <div class="config-z-value-list">
              <input id="positionZTravelInput" data-control-id="positionZTravelInput" placeholder="璇疯緭鍏ュ唴瀹?>
              <input id="positionZStartInput" data-control-id="positionZStartInput" placeholder="璇疯緭鍏ュ唴瀹?>
              <input id="positionZEndInput" data-control-id="positionZEndInput" placeholder="璇疯緭鍏ュ唴瀹?>
              <input id="positionZDispensInput" data-control-id="positionZDispensInput" placeholder="璇疯緭鍏ュ唴瀹?>
            </div>
          </div>
          <div class="engineering-mini-grid">${configSelect('positionLiquidClassSelect','娑蹭綋绫诲瀷',['Ab','AR','DAB','Wash','PBS','PureWater','Custom'],'Ab')}</div>
          <div class="engineering-actions">${engineeringButton('璇诲彇褰撳墠瀛斾綅淇℃伅','閰嶇疆/閫氶亾绉讳綅锛氳鍙栧綋鍓嶅瓟浣嶄俊鎭?)}${engineeringButton('淇濆瓨褰撳墠瀛斾綅淇℃伅','閰嶇疆/閫氶亾绉讳綅锛氫繚瀛樺綋鍓嶅瓟浣嶄俊鎭?,'ok-lite')}${engineeringButton('绉诲姩','閰嶇疆/閫氶亾绉讳綅锛氱Щ鍔ㄥ埌褰撳墠瀛斾綅','primary-lite')}</div>
        </section>
      </div>
    </div>
  </section>`;
}
function renderConfigPipetteSection() {
  const btn = (id, label, action, cls='') => `<button type="button" id="${id}" data-control-id="${id}" ${cls ? `class="${cls}"` : ''} data-action-log="${escapeHtml(action)}">${escapeHtml(label)}</button>`;
  return `<section class="config-pane-section ${activeConfigSection==='pipette'?'active':''}" id="configSectionPipette" data-control-id="configSectionPipette">
    <div class="engineering-config-module">
      <div class="engineering-status-strip"><span class="engineering-status-dot active"></span><strong>閫氶亾绉绘恫娴嬭瘯</strong><span class="subtle">鍙傛暟銆佸姩浣溿€佸疄鏃舵棩蹇楀垎鍖烘樉绀猴紝閬垮厤鎸夐挳鍜屾棩蹇楁尋鍦ㄤ竴璧枫€?/span></div>
      <div class="config-pipette-workbench" id="configPipetteWorkbench" data-control-id="configPipetteWorkbench">
        <section class="engineering-card" id="configPipetteParamCard" data-control-id="configPipetteParamCard">
          <strong>绉绘恫鍙傛暟</strong>
          <div class="engineering-mini-grid">
            <label>瀛斾綅<select id="pipetteHoleSelect" data-control-id="pipetteHoleSelect">${configHoleOptions('P11')}</select></label>
            ${configNumberInput('pipetteVolumeInput','娑查噺','渭L','100')}
            ${configSelect('pipetteNeedleSelect','鎺у埗閽?,['Z1','Z2','鍙岄拡'],'Z1')}
            ${configSelect('pipetteLiquidClassSelect','娑蹭綋绫诲瀷',['Ab','AR','DAB','Wash','PBS','PureWater','Custom'],'Ab')}
          </div>
          <div class="engineering-note">鍏堥€夋嫨鐩爣瀛斾綅鍜屾恫浣撶被鍨嬶紝鍐嶆墽琛屽惛娑层€佹墦娑叉垨娑查潰鎺㈡祴銆傚瓟浣嶅潗鏍囧拰 Z 鍙傛暟浠嶇敱鈥滈€氶亾绉讳綅鈥濈淮鎶ゃ€?/div>
        </section>
        <section class="engineering-card pipette-action-panel" id="configPipetteActionCard" data-control-id="configPipetteActionCard">
          <strong>鍔ㄤ綔鎺у埗</strong>
          <div class="module-control-grid" id="pipetteActionGrid" data-control-id="pipetteActionGrid">
            ${btn('pipetteAspirateBtn','鍚告恫','閰嶇疆/閫氶亾绉绘恫锛氬惛娑?,'primary-lite')}
            ${btn('pipetteDispenseBtn','鎵撴恫','閰嶇疆/閫氶亾绉绘恫锛氭墦娑?)}
            ${btn('pipetteSystemDispenseBtn','鎵撶郴缁熸恫','閰嶇疆/閫氶亾绉绘恫锛氭墦绯荤粺娑?)}
            ${btn('pipetteLiquidLevelDetectBtn','娑查潰鎺㈡祴','閰嶇疆/閫氶亾绉绘恫锛氭恫闈㈡帰娴?)}
            ${btn('pipetteClearChannelBtn','娓呯┖閫氶亾','閰嶇疆/閫氶亾绉绘恫锛氭竻绌洪€氶亾','warn-lite')}
            ${btn('pipetteStopBtn','鍋滄鍔ㄤ綔','閰嶇疆/閫氶亾绉绘恫锛氬仠姝㈠姩浣?,'warn-lite')}
          </div>
          <div class="engineering-note">鎸夐挳鎸夋墽琛岄鐜囧垎缁勶紝涓ゅ垪甯冨眬渚夸簬 70% 渚ф爮涓嬪崟鎵嬬偣鍑汇€?/div>
        </section>
        <section class="engineering-card pipette-log-panel" id="configPipetteConsoleCard" data-control-id="configPipetteConsoleCard">
          <strong>瀹炴椂鏃ュ織 / Z 杞村弽棣?/strong>
          <div class="test-console config-console-large" id="pipetteRealtimeConsole" data-control-id="pipetteRealtimeConsole">[SIM] 褰撳墠 Z 杞翠綅缃細-- mm\n[SIM] 娑查潰鎺㈡祴缁撴灉锛氭湭鎵ц\n[SIM] 閿欒鏃ュ織锛氭棤\n[SIM] 鍚庣画鍙帴 WebSocket 鎴栦笂浣嶆満鐪熷疄鏁版嵁銆?/div>
        </section>
      </div>
    </div>
  </section>`;
}
function renderConfigScannerSection() {
  return `<section class="config-pane-section ${activeConfigSection==='scanner'?'active':''}" id="configSectionScanner" data-control-id="configSectionScanner">
    <div class="engineering-config-module scanner-layout-module">
      <div class="engineering-status-strip"><span class="engineering-status-dot ok"></span><strong>鎵爜鍣ㄩ厤缃?/strong><span class="subtle">鏍锋湰鎵爜鍣ㄧ粦瀹氭満姊拌噦闅忓姩鐩告満锛涜瘯鍓傛壂鐮佸櫒淇濇寔鐙珛瀵硅薄銆?/span></div>
      <div class="config-scanner-layout">
        <section class="engineering-card" id="scannerComCard" data-control-id="scannerComCard">
          <strong>COM 鍙ｈ缃?/strong>
          <div class="engineering-mini-grid four">
            ${configSelect('scannerTargetSelect','鎵爜鍣?,[['sample','鏍锋湰鎵爜鍣紙鏈烘鑷傞殢鍔ㄧ浉鏈猴級'],['reagent','璇曞墏鎵爜鍣?]],'sample')}
            ${configSelect('scannerComPortSelect','COM鍙?,['COM1','COM2','COM3','COM4'],'COM1')}
            ${configSelect('scannerBaudRateSelect','娉㈢壒鐜?,['115200','57600','38400','9600'],'115200')}
            ${configTextInput('scannerSerialPresetInput','涓插彛鍙傛暟','8 bits / 1 bits / 鏃?,'')}
          </div>
          <div class="engineering-actions">${engineeringButton('璁剧疆COM鍙?,'閰嶇疆/鎵爜鍣細璁剧疆COM鍙?,'primary-lite')}${engineeringButton('璇诲彇COM鍙?,'閰嶇疆/鎵爜鍣細璇诲彇COM鍙?)}</div>
        </section>
        <section class="engineering-card" id="scannerInitCard" data-control-id="scannerInitCard">
          <strong>鍒濆鍖?/strong>
          <div class="engineering-actions compact-actions">
            ${engineeringButton('鍒涘缓鎵爜鍣?,'閰嶇疆/鎵爜鍣細鍒涘缓鎵爜鍣?,'primary-lite')}
            ${engineeringButton('閲嶆柊鍚姩','閰嶇疆/鎵爜鍣細閲嶆柊鍚姩','warn-lite')}
            ${engineeringButton('鎭㈠鍑哄巶','閰嶇疆/鎵爜鍣細鎭㈠鍑哄巶')}
            ${engineeringButton('Raw Model妯″紡','閰嶇疆/鎵爜鍣細Raw Model妯″紡')}
            ${engineeringButton('婵€娲?6杩涘埗涓插彛閫氫俊','閰嶇疆/鎵爜鍣細婵€娲?6杩涘埗涓插彛閫氫俊')}
          </div>
        </section>
        <section class="engineering-card" id="scannerLightCard" data-control-id="scannerLightCard">
          <strong>鏍￠獙鍏?/strong>
          <div class="engineering-actions">${engineeringButton('鎵撳紑鏍￠獙鍏?,'閰嶇疆/鎵爜鍣細鎵撳紑鏍￠獙鍏?,'primary-lite')}${engineeringButton('鍏抽棴鏍￠獙鍏?,'閰嶇疆/鎵爜鍣細鍏抽棴鏍￠獙鍏?)}</div>
        </section>
        <section class="engineering-card" id="scannerTriggerCard" data-control-id="scannerTriggerCard">
          <strong>鎵弿瑙﹀彂</strong>
          <div class="engineering-actions compact-actions">${engineeringButton('婵€娲诲崟娆¤Е鍙?,'閰嶇疆/鎵爜鍣細婵€娲诲崟娆¤Е鍙?,'primary-lite')}${engineeringButton('鍋滄鍗曟瑙﹀彂','閰嶇疆/鎵爜鍣細鍋滄鍗曟瑙﹀彂')}${engineeringButton('婵€娲昏繛缁Е鍙?,'閰嶇疆/鎵爜鍣細婵€娲昏繛缁Е鍙?,'primary-lite')}${engineeringButton('鍋滄杩炵画瑙﹀彂','閰嶇疆/鎵爜鍣細鍋滄杩炵画瑙﹀彂')}</div>
        </section>
        <section class="engineering-card wide" id="scannerRoiCard" data-control-id="scannerRoiCard">
          <strong>ROI 璁剧疆锛堜緷娆′负宸︺€佷笂銆佸銆侀珮锛?/strong>
          <div class="scanner-roi-grid">
            ${configNumberInput('scannerRoiLeftInput','ROI Left')}
            ${configNumberInput('scannerRoiTopInput','ROI Top')}
            ${configNumberInput('scannerRoiWidthInput','ROI Width')}
            ${configNumberInput('scannerRoiHeightInput','ROI Height')}
          </div>
          <div class="engineering-actions">${engineeringButton('璁剧疆ROI','閰嶇疆/鎵爜鍣細璁剧疆ROI','ok-lite')}${engineeringButton('璇诲彇鏉＄爜','閰嶇疆/鎵爜鍣細璇诲彇鏉＄爜','primary-lite')}${engineeringButton('璇诲彇鍥轰欢淇℃伅','閰嶇疆/鎵爜鍣細璇诲彇鍥轰欢淇℃伅')}</div>
          <div class="engineering-mini-grid"><label>鏉＄爜璇诲彇缁撴灉<input id="scannerBarcodeReadInput" data-control-id="scannerBarcodeReadInput" placeholder="璇疯緭鍏ュ唴瀹?></label><label>鍥轰欢淇℃伅<input id="scannerFirmwareInfoInput" data-control-id="scannerFirmwareInfoInput" placeholder="璇疯緭鍏ュ唴瀹?></label></div>
        </section>
      </div>
    </div>
  </section>`;
}
function renderConfigBarcodeSection() {
  return `<section class="config-pane-section ${activeConfigSection==='barcode'?'active':''}" id="configSectionBarcode" data-control-id="configSectionBarcode">
    <div class="engineering-config-module barcode-layout-module">
      <div class="engineering-status-strip"><span class="engineering-status-dot active"></span><strong>鏉＄爜鎵弿閰嶇疆</strong><span class="subtle">鏍锋湰鎵爜浣跨敤鏈烘鑷傞殢鍔ㄧ浉鏈猴紱璇曞墏鎵爜浣跨敤鐙珛璇曞墏鎵爜鍣ㄣ€?/span></div>
      <div class="config-barcode-layout">
        <section class="engineering-card wide barcode-grid-card" id="barcodeTargetCard" data-control-id="barcodeTargetCard">
          <strong>鎵弿瀵硅薄瀹氫綅</strong>
          <div class="barcode-current-target" id="barcodeCurrentTarget" data-control-id="barcodeCurrentTarget">
            <label>褰撳墠瀵硅薄<input id="barcodeSelectedSvgInput" data-control-id="barcodeSelectedSvgInput" value="svg-slide-r11 / R11"></label>
            <label>X 鍧愭爣<input id="barcodeCurrentXInput" data-control-id="barcodeCurrentXInput" value="123"></label>
            <label>Y 鍧愭爣<input id="barcodeCurrentYInput" data-control-id="barcodeCurrentYInput" value="123"></label>
          </div>
          <div class="barcode-zones-wrap">
            <div class="engineering-card barcode-grid-card" id="barcodeSampleZoneCard" data-control-id="barcodeSampleZoneCard"><strong>鏍锋湰鍖?/strong><div class="barcode-sample-grid" id="barcodeSampleGrid" data-control-id="barcodeSampleGrid">${sampleSlideCards()}</div></div>
            <div class="engineering-card barcode-grid-card" id="barcodeReagentZoneCard" data-control-id="barcodeReagentZoneCard"><strong>璇曞墏鍖?/strong><div class="reagent-position-grid" id="barcodeReagentGrid" data-control-id="barcodeReagentGrid">${reagentBarcodeCells()}</div><div class="engineering-note">瀹氫綅鍧愭爣鍙栨瘡鍒楃涓€涓拰鏈€鍚庝竴涓紝鍏朵綑鍙栧钩鍧囧€硷紱鐐瑰嚮浠绘剰鏍间細鑱斿姩宸︿晶瀛敓椤甸潰瀵硅薄銆?/div></div>
          </div>
        </section>
        <section class="engineering-card" id="barcodeRoiCard" data-control-id="barcodeRoiCard">
          <div class="engineering-section-title">ROI 璁剧疆</div>
          <div class="scanner-roi-grid">
            ${configNumberInput('barcodeRoiLeftInput','RoiLeft')}
            ${configNumberInput('barcodeRoiTopInput','RoiTop')}
            ${configNumberInput('barcodeRoiWidthInput','RoiWidth')}
            ${configNumberInput('barcodeRoiHeightInput','RoiHeight')}
          </div>
          <div class="engineering-actions">${engineeringButton('浠庢壂鐮佸櫒璇诲彇Roi鍙傛暟','閰嶇疆/鏉＄爜鎵弿锛氫粠鎵爜鍣ㄨ鍙朢OI')}${engineeringButton('浠庨厤缃枃浠惰鍙朢oi鍙傛暟','閰嶇疆/鏉＄爜鎵弿锛氫粠閰嶇疆鏂囦欢璇诲彇ROI')}${engineeringButton('淇濆瓨Roi鍙傛暟鍒版壂鐮佸櫒鍜岄厤缃枃浠?,'閰嶇疆/鏉＄爜鎵弿锛氫繚瀛楻OI鍙傛暟','ok-lite')}</div>
        </section>
        <section class="engineering-card" id="barcodeCoordinateCard" data-control-id="barcodeCoordinateCard">
          <div class="engineering-section-title">鎵爜鍧愭爣璁剧疆</div>
          <div class="engineering-mini-grid">
            ${configNumberInput('barcodeScanXInput','X杞村潗鏍?,'','123')}
            ${configNumberInput('barcodeScanYInput','Y杞村潗鏍?,'','123')}
          </div>
          <div class="engineering-actions">${engineeringButton('浠庨厤缃枃浠惰鍙栧潗鏍?,'閰嶇疆/鏉＄爜鎵弿锛氫粠閰嶇疆鏂囦欢璇诲彇鍧愭爣')}${engineeringButton('淇濆瓨XY鍧愭爣鍒伴厤缃枃浠?,'閰嶇疆/鏉＄爜鎵弿锛氫繚瀛榅Y鍧愭爣','ok-lite')}</div>
          <div class="module-control-grid">${engineeringButton('寮€濮嬫壂鎻?,'閰嶇疆/鏉＄爜鎵弿锛氬紑濮嬫壂鎻?,'primary-lite')}${engineeringButton('鍋滄鎵弿','閰嶇疆/鏉＄爜鎵弿锛氬仠姝㈡壂鎻?,'warn-lite')}${engineeringButton('娓呯┖鏉＄爜','閰嶇疆/鏉＄爜鎵弿锛氭竻绌烘潯鐮?,'warn-lite')}</div>
        </section>
      </div>
    </div>
  </section>`;
}
function renderConfigMixHeatSection() {
  return `<section class="config-pane-section ${activeConfigSection==='mixheat'?'active':''}" id="configSectionMixHeat" data-control-id="configSectionMixHeat">
    <div class="engineering-config-module">
      <div class="engineering-status-strip"><span class="engineering-status-dot active"></span><strong>娣峰寑鍔犵儹鎺у埗</strong><span class="subtle">浠庘€滄竻娲楁贩鍖€鈥濅腑涓婂崌涓虹嫭绔嬮厤缃瓙椤点€?/span></div>
      <div class="config-mixheat-layout" id="configMixHeatLayout" data-control-id="configMixHeatLayout">
        <section class="engineering-card wide" id="mixHeatMainCard" data-control-id="mixHeatMainCard">
          <strong>鏍锋湰娓╂帶</strong>
          <div class="engineering-mini-grid four">
            ${configSelect('mixHeatChannelSelect','閫氶亾閫夋嫨',['鍏ㄩ€?,'閫氶亾1','閫氶亾2','閫氶亾3','閫氶亾4'],'鍏ㄩ€?)}
            ${configTextInput('mixHeatTargetTempInput','鏍锋湰鐩爣娓╁害','50鈩?)}
            ${configTextInput('mixHeatCurrentTempInput','褰撳墠娓╁害','45鈩?)}
            ${configTextInput('mixHeatStatusInput','鍔犵儹鐘舵€?,'寰呮満')}
          </div>
          <div class="engineering-actions">${engineeringButton('寮€濮嬪姞鐑?,'閰嶇疆/娣峰寑鍔犵儹锛氬紑濮嬪姞鐑?,'primary-lite')}${engineeringButton('鍋滄鍔犵儹','閰嶇疆/娣峰寑鍔犵儹锛氬仠姝㈠姞鐑?,'warn-lite')}${engineeringButton('鏌ヨ娓╁害','閰嶇疆/娣峰寑鍔犵儹锛氭煡璇㈡俯搴?)}</div>
          <div class="engineering-note">杩欓噷浠呬繚鐣欐贩鍖€鍔犵儹鎺у埗銆傝瘯鍓傚埗鍐峰凡杩佺Щ鍒扳€滆瘯鍓傛俯搴︹€濆璞¤鎯咃紝渚涙按妯″潡宸茶縼绉诲埌鏌撹壊鍖轰緵姘?娓呮礂瀛斿璞¤鎯呫€?/div>
        </section>
      </div>
    </div>
  </section>`;
}
function renderConfigThermalSection() {
  return `<section class="config-pane-section ${activeConfigSection==='thermal'?'active':''}" id="configSectionThermal" data-control-id="configSectionThermal">
    <div class="engineering-config-module">
      <div class="engineering-status-strip"><span class="engineering-status-dot ok"></span><strong>娓呮礂娣峰寑</strong><span class="subtle">淇濈暀娣峰寑鍙傛暟銆佹牱鏈竻娲椼€佺數纾侀榾鍜屼粨浣嶇姸鎬侊紱娓╂帶銆佸埗鍐枫€佷緵姘存寜瀵硅薄璇︽儏缁存姢銆?/span></div>
      <div class="config-thermal-layout" id="configThermalLayout" data-control-id="configThermalLayout">
        <section class="engineering-card" id="thermalMixParamCard" data-control-id="thermalMixParamCard">
          <div class="engineering-section-title">娣峰寑鍙傛暟</div>
          <div class="engineering-mini-grid">
            ${configSelect('thermalMixChannelSelect','閫氶亾閫夋嫨',['鍏ㄩ€?,'閫氶亾1','閫氶亾2','閫氶亾3','閫氶亾4'],'鍏ㄩ€?)}
            ${configTextInput('thermalMixOriginInput','娣峰寑鍘熺偣','鍘熺偣')}
            ${configNumberInput('thermalMixStartStrokeInput','娣峰寑濮嬭绋?,'0~200')}
            ${configNumberInput('thermalMixTotalStrokeInput','娣峰寑鎬昏绋?,'100~500')}
            ${configNumberInput('thermalMixTopDwellInput','涓婂仠椤挎椂闂?,'5~255')}
            ${configNumberInput('thermalMixBottomDwellInput','涓嬪仠椤挎椂闂?,'5~255')}
            ${configNumberInput('thermalMixForwardSpeedInput','鐢垫満鍓嶈繘閫熷害','100~1000')}
            ${configNumberInput('thermalMixReverseSpeedInput','鐢垫満鍥為€€閫熷害','100~1000')}
            ${configNumberInput('thermalMixCountInput','娣峰寑娆℃暟','1~255')}
            ${configNumberInput('thermalMixRemainingCountInput','鍓╀綑娣峰寑娆℃暟','','0')}
          </div>
          <div class="engineering-actions">${engineeringButton('寮€濮嬫贩鍖€','閰嶇疆/娓呮礂娣峰寑锛氬紑濮嬫贩鍖€','primary-lite')}${engineeringButton('娣峰寑澶嶄綅','閰嶇疆/娓呮礂娣峰寑锛氭贩鍖€澶嶄綅')}${engineeringButton('鏌ヨ娣峰寑','閰嶇疆/娓呮礂娣峰寑锛氭煡璇㈡贩鍖€')}</div>
        </section>
        <section class="engineering-card" id="thermalSampleWashCard" data-control-id="thermalSampleWashCard">
          <div class="engineering-section-title">鏍锋湰娓呮礂 / 闃€鐘舵€?/div>
          <div class="engineering-mini-grid">
            ${configTextInput('sampleWashTimeInput','鏍锋湰娓呮礂鏃堕棿','2000ms')}
            ${configTextInput('sampleWashTempInput','鏍锋湰娓呮礂娓╁害','45鈩?)}
            <label>鏍锋湰娓呮礂寮€鍏?span class="toggle-sim" id="sampleWashSwitch" data-control-id="sampleWashSwitch"></span></label>
            <label>鐢电闃€寮€鍏?span class="toggle-sim" id="thermalSolenoidSwitch" data-control-id="thermalSolenoidSwitch"></span></label>
            ${configTextInput('thermalSolenoidStatusInput','鐢电闃€鐘舵€?,'鍏抽棴')}
            ${configTextInput('thermalSlotOptocouplerInput','浠撲綅鍏夎€︾姸鎬?,'鏈伄鎸?)}
          </div>
          <div class="engineering-actions">${engineeringButton('鎵撳紑鏍锋湰娓呮礂','閰嶇疆/娓呮礂娣峰寑锛氭墦寮€鏍锋湰娓呮礂','primary-lite')}${engineeringButton('鍏抽棴鏍锋湰娓呮礂','閰嶇疆/娓呮礂娣峰寑锛氬叧闂牱鏈竻娲?)}${engineeringButton('鏌ヨ闃€鐘舵€?,'閰嶇疆/娓呮礂娣峰寑锛氭煡璇㈤榾鐘舵€?)}</div>
        </section>
      </div>
    </div>
  </section>`;
}

function drawerFromChannelSelect(selectId) {
  const v = String(document.getElementById(selectId)?.value || '').trim();
  const m = /閫氶亾\s*([1-4])/.exec(v);
  if(m) return 'ABCD'[Number(m[1]) - 1];
  return '';
}

// 娣峰寑鍔犵儹 drawer 瑙ｆ瀽锛氶€氶亾1-4 -> A/B/C/D锛涘叏閫?-> A,B,C,D锛堟棤鍏ㄦ澘缁熶竴鎺ュ彛锛岄渶閫?drawer 涓嬪彂锛?
function mixHeatDrawers() {
  const sel = String(document.getElementById('mixHeatChannelSelect')?.value || '').trim();
  const m = /閫氶亾\s*([1-4])/.exec(sel);
  if(m) return ['ABCD'[Number(m[1]) - 1]];
  if(/鍏ㄩ€墊鍏ㄩ儴/.test(sel)) return ['A', 'B', 'C', 'D'];
  return ['A'];
}

// 璋冭瘯/閰嶇疆椤?[data-action-log] 宸ョ▼鍔ㄤ綔鍒嗗彂鍣ㄣ€傞€氶亾閫夋嫨 -> drawer A-D锛堜笌 4.2 涓€鑷达級銆?
async function dispatchEngineeringAction(action, btn) {
  if(btn) flashControl(btn);
  const consoleNode = document.querySelector('.test-console');
  try {
    if(action === '閰嶇疆/娓呮礂娣峰寑锛氬紑濮嬫贩鍖€' || action === '閰嶇疆/娓呮礂娣峰寑锛氭贩鍖€澶嶄綅') {
      const drawer = drawerFromChannelSelect('thermalMixChannelSelect');
      if(!drawer) { log('璇峰厛鍦?閫氶亾閫夋嫨"閫夋嫨鍏蜂綋閫氶亾锛?-4锛夊啀鎿嶄綔娣峰寑', 'warn'); return; }
      const op = action.endsWith('娣峰寑澶嶄綅') ? 'stop' : 'start';
      await writeApi(`/api/fluidics/mixers/${encodeURIComponent(drawer)}/${op}`, { method:'POST', body: JSON.stringify({ commandId: makeCommandId('mixer-' + op), reason: `twin ${action}` }) });
      log(`${action}锛堥€氶亾${drawer}锛夊凡涓嬪彂锛岄渶绠＄悊鍛?+ 宸ョ▼浼氳瘽`, 'ok');
      await loadDatabaseSnapshot();
      return;
    }
    if(action === '閰嶇疆/娓呮礂娣峰寑锛氭煡璇㈡贩鍖€') {
      const state = await backendApi('/api/fluidics/state');
      log(`${action}锛歱umps=${(state?.pumps || []).length}, ready=${state?.ready}`, 'ok');
      return;
    }
    if(action === '閰嶇疆/娣峰寑鍔犵儹锛氬紑濮嬪姞鐑? || action === '閰嶇疆/娣峰寑鍔犵儹锛氬仠姝㈠姞鐑?) {
      const isEnabled = action.endsWith('鍋滄鍔犵儹') ? false : true;
      const rawTemp = Number(String(document.getElementById('mixHeatTargetTempInput')?.value || '').replace(/[^\d.\-]/g, ''));
      const targetDeciC = Number.isFinite(rawTemp) ? Math.round(rawTemp * 10) : 500;
      const drawers = mixHeatDrawers();
      for(const d of drawers) {
        await writeApi(`/api/thermal/boards/${encodeURIComponent(d)}`, { method:'POST', body: JSON.stringify({ commandId: makeCommandId('thermal-board'), targetTemperatureDeciC: targetDeciC, isEnabled }) });
      }
      log(`${action}锛歞rawer=${drawers.join(',')} 鐩爣=${rawTemp}鈩?${targetDeciC}) isEnabled=${isEnabled} 宸蹭笅鍙慲, 'ok');
      await loadDatabaseSnapshot();
      return;
    }
    if(action === '閰嶇疆/娣峰寑鍔犵儹锛氭煡璇㈡俯搴?) {
      const state = await backendApi('/api/thermal/state');
      const drawers = mixHeatDrawers();
      const pts = (state?.points || []).filter(p => drawers.includes(p.drawerCode));
      const summary = pts.slice(0, 8).map(p => `${p.drawerCode}${p.slotNo}=${p.currentTemperatureDeciC}`).join(' ') || '--';
      log(`${action}锛歞rawer=${drawers.join(',')} ${pts.length}鐐?${summary}`, 'ok');
      return;
    }
    if(action === '閰嶇疆/鎵爜鍣細鍒涘缓鎵爜鍣?) return await createScannerProfile();
    if(['閰嶇疆/鎵爜鍣細璁剧疆ROI','閰嶇疆/鎵爜鍣細鎵撳紑鏍￠獙鍏?,'閰嶇疆/鎵爜鍣細鍏抽棴鏍￠獙鍏?,'閰嶇疆/鎵爜鍣細閲嶆柊鍚姩'].includes(action)) return await scannerControl(action);
    log(`${action}锛氭殏鏈帴鍏ワ紙鏍锋湰娓呮礂涓夋寜閽寜绾﹀畾淇濈暀鍘熸牱锛屼笉缁戝悗绔級`, 'warn');
  } catch(err) { /* routed by writeApi where used */ }
  if(consoleNode) consoleNode.textContent += `\n[${new Date().toLocaleTimeString()}] ${action}`;
}

function bindEngineeringControlHandlers(root=document) {
  root.querySelectorAll('[data-action-log]').forEach(btn => {
    if(btn.dataset.engineeringBound === 'true') return;
    btn.dataset.engineeringBound = 'true';
    btn.addEventListener('click', () => {
      const action = btn.getAttribute('data-action-log') || btn.textContent.trim();
      dispatchEngineeringAction(action, btn);
    });
  });
  const openLiquidBtn = Array.from(root.querySelectorAll('[data-action-log]')).find(btn => (btn.getAttribute('data-action-log') || '').includes('鎵撳紑娑蹭綋绫诲瀷閰嶇疆'));
  if(openLiquidBtn && !openLiquidBtn.dataset.openLiquidBound) {
    openLiquidBtn.dataset.openLiquidBound = 'true';
    openLiquidBtn.addEventListener('click', () => { showSideTab('config'); renderConfigPane('liquid'); });
  }
}
function collectEngineeringForm(rootId) {
  const root = document.getElementById(rootId); if(!root) return {};
  const data = {};
  root.querySelectorAll('input, select, textarea').forEach(node => { if(node.id) data[node.id] = node.value; });
  return data;
}

function bindConfigNavHandlers(root=document) {
  root.querySelectorAll('[data-config-section]').forEach(btn => {
    if(btn.dataset.configNavBound === 'true') return;
    btn.dataset.configNavBound = 'true';
    btn.addEventListener('click', () => {
      activeConfigSection = normalizeConfigSectionKey(btn.dataset.configSection);
      enterFlowWidePanel();
      scheduleConfigPaneRender(activeConfigSection);
    });
  });
}
function getPhysicalItemByControlId(controlId) {
  if(!controlId) return null;
  return coords.find(item => itemControlId(item) === controlId || item.controlId === controlId) || null;
}
function syncCoordinateInputsForSvgTarget(targetId, source=null) {
  const item = getPhysicalItemByControlId(targetId);
  const selectedInput = document.getElementById('barcodeSelectedSvgInput') || document.getElementById('scannerLinkedSvgInput');
  const xInput = document.getElementById('barcodeCurrentXInput') || document.getElementById('scannerXCoordInput');
  const yInput = document.getElementById('barcodeCurrentYInput') || document.getElementById('scannerYCoordInput');
  if(selectedInput) selectedInput.value = item ? `${targetId} / ${item.name}` : targetId;
  if(item) {
    if(xInput) xInput.value = Number(item.x ?? 0).toFixed(2).replace(/\.00$/,'');
    if(yInput) yInput.value = Number(item.y ?? 0).toFixed(2).replace(/\.00$/,'');
    if(source?.dataset?.barcodeKind === 'reagent') {
      const code = source.dataset.reagentCode || '';
      if(/S\d1$/.test(code)) {
        const firstX = document.getElementById('barcodeReagentFirstXInput');
        const firstY = document.getElementById('barcodeReagentFirstYInput');
        if(firstX) firstX.value = Number(item.x ?? 0).toFixed(2).replace(/\.00$/,'');
        if(firstY) firstY.value = Number(item.y ?? 0).toFixed(2).replace(/\.00$/,'');
      }
      if(/S\d8$/.test(code)) {
        const lastX = document.getElementById('barcodeReagentLastXInput');
        const lastY = document.getElementById('barcodeReagentLastYInput');
        if(lastX) lastX.value = Number(item.x ?? 0).toFixed(2).replace(/\.00$/,'');
        if(lastY) lastY.value = Number(item.y ?? 0).toFixed(2).replace(/\.00$/,'');
      }
    }
  }
}
function bindSvgTargetLinks(root=document) {
  root.querySelectorAll('[data-svg-target]').forEach(node => {
    if(node.dataset.svgTargetBound === 'true') return;
    node.dataset.svgTargetBound = 'true';
    node.addEventListener('click', evt => {
      evt.stopPropagation();
      const targetId = node.getAttribute('data-svg-target');
      if(!targetId) return;
      root.querySelectorAll('[data-svg-target]').forEach(el => el.classList.toggle('active', el === node));
      selectSvgControl(targetId);
      syncCoordinateInputsForSvgTarget(targetId, node);
      const item = getPhysicalItemByControlId(targetId);
      log(`宸茶仈鍔ㄥ乏渚у鐢熼〉闈㈠璞★細${item ? item.name : targetId}`, 'ok');
    });
  });
}

function closeConfigProfileDrawer() {
  const fold = document.getElementById('configProfileFold');
  if(fold && fold.tagName === 'DETAILS') fold.open = false;
}
function bindConfigHandlers() {
  const root = document.getElementById('configPane') || document;
  bindConfigNavHandlers(root);
  ['configProfileSelect','configEditProfileSelect'].forEach(id => { const node = document.getElementById(id); if(node) node.addEventListener('change', () => { selectedConfigId = node.value; selectedConfigStepIndex = 0; renderConfigPane(activeConfigSection); closeConfigProfileDrawer(); }); });
  root.querySelectorAll('[data-select-profile]').forEach(btn => btn.addEventListener('click', () => { selectedConfigId = btn.dataset.selectProfile; selectedConfigStepIndex = 0; renderConfigPane('files'); closeConfigProfileDrawer(); }));
  bindClick('configNewProfileBtn', createNewProfile);
  bindClick('configDuplicateProfileBtn', duplicateProfile);
  bindClick('configDeleteProfileBtn', deleteSelectedProfile);
  bindClick('configExportBtn', () => exportProfile(getSelectedProfile()));
  bindClick('configExportAllBtn', exportAllProfiles);
  bindClick('configImportBtn', () => document.getElementById('configImportInput')?.click());
  const importInput = document.getElementById('configImportInput'); if(importInput) importInput.addEventListener('change', importProfilesFromInput);
  bindClick('configRenameQuickBtn', saveProfileBasics);
  bindClick('configBasicsToggleBtn', () => { configBasicsCollapsed = !configBasicsCollapsed; renderConfigPane('files'); });
  bindClick('configSaveStepBtn', saveSelectedStep);
  bindClick('configMoveStepUpBtn', () => moveSelectedStep(-1));
  bindClick('configMoveStepDownBtn', () => moveSelectedStep(1));
  bindClick('configDeleteStepBtn', deleteSelectedStep);
  bindClick('configSaveRulesBtn', saveProfileRules);
  root.querySelectorAll('.config-op-tile').forEach(tile => {
    tile.addEventListener('click', () => addStepFromOp(tile.dataset.opKey));
    tile.addEventListener('dragstart', evt => { evt.dataTransfer.setData('application/x-op-key', tile.dataset.opKey); evt.dataTransfer.effectAllowed = 'copy'; });
  });
  const timeline = document.getElementById('configEditorTimeline');
  if(timeline) {
    timeline.addEventListener('dragover', evt => { evt.preventDefault(); timeline.classList.add('drag-over'); });
    timeline.addEventListener('dragleave', evt => { if(evt.target === timeline) timeline.classList.remove('drag-over'); });
    timeline.addEventListener('drop', evt => {
      evt.preventDefault(); timeline.classList.remove('drag-over');
      const insertIndex = getTimelineDropIndex(evt);
      const opKey = evt.dataTransfer.getData('application/x-op-key');
      const stepIndex = evt.dataTransfer.getData('application/x-step-index');
      if(opKey) addStepFromOp(opKey, insertIndex);
      if(stepIndex !== '') reorderStep(Number(stepIndex), insertIndex);
    });
  }
  bindConfigStepBrickHandlers(root);
  bindChannelBindingHandlers(root);
  bindEngineeringControlHandlers(root);
  bindSvgTargetLinks(root);
}

function createNewProfile() {
  const base = normalizeProfile(DEFAULT_CONFIG_PROFILES[0]);
  base.id = `profile-${Date.now()}`;
  base.name = `鏂板缓 IHC 閰嶇疆 ${configProfiles.length + 1}`;
  base.description = '鐢遍粯璁?IHC 妯℃澘鏂板缓锛屽彲鎷栨嫿璋冩暣銆?;
  configProfiles.unshift(base); selectedConfigId = base.id; selectedConfigStepIndex = 0; saveConfigProfilesOnly(); renderConfigPane('files'); log(`宸叉柊寤洪厤缃細${base.name}`, 'ok');
}
function duplicateProfile() {
  const src = getSelectedProfile(); if(!src) return;
  const copy = normalizeProfile(src); copy.id = `profile-${Date.now()}`; copy.name = `${src.name} 鍓湰`;
  configProfiles.unshift(copy); selectedConfigId = copy.id; selectedConfigStepIndex = 0; saveConfigProfilesOnly(); renderConfigPane('files'); log(`宸插鍒堕厤缃細${copy.name}`, 'ok');
}
function deleteSelectedProfile() {
  if(configProfiles.length <= 1) { log('鑷冲皯淇濈暀涓€涓厤缃枃浠?, 'warn'); return; }
  const profile = getSelectedProfile(); if(!profile) return;
  configProfiles = configProfiles.filter(p => p.id !== profile.id);
  [1,2,3,4].forEach(id => { if(channelConfigAssignments[id] === profile.id) assignConfigToChannel(id, null, false); });
  selectedConfigId = configProfiles[0]?.id || null; selectedConfigStepIndex = 0; saveConfigProfiles(); renderConfigPane('files'); log(`宸插垹闄ら厤缃細${profile.name}`, 'warn');
}
function saveProfileBasics() {
  const p = getSelectedProfile(); if(!p) return;
  p.name = document.getElementById('configProfileNameInput')?.value.trim() || p.name;
  p.stainType = document.getElementById('configProfileTypeInput')?.value.trim() || p.stainType;
  p.description = document.getElementById('configProfileDescInput')?.value.trim() || '';
  saveConfigProfilesOnly(); renderConfigPane('files'); log(`宸蹭繚瀛橀厤缃熀纭€淇℃伅锛?{p.name}`, 'ok');
}
function getTimelineDropIndex(evt) {
  const timeline = document.getElementById('configEditorTimeline');
  if(!timeline) return getSelectedProfile()?.steps.length || 0;
  const bricks = Array.from(timeline.querySelectorAll('.config-step-brick'));
  for(const brick of bricks) {
    const rect = brick.getBoundingClientRect();
    const idx = Number(brick.dataset.stepIndex || 0);
    if(evt.clientY < rect.top + rect.height / 2) return idx;
  }
  return bricks.length;
}
function addStepFromOp(opKey, insertIndex=null) {
  const p = getSelectedProfile(); const op = OP_DEF_BY_KEY[opKey]; if(!p || !op) return;
  const idx = Number.isFinite(insertIndex) ? Math.max(0, Math.min(insertIndex, p.steps.length)) : p.steps.length;
  p.steps.splice(idx, 0, normalizeConfigStep({ opKey, label:op.label, durationSec:10, toleranceSec:0, immediateAfterPrev:false }, idx));
  selectedConfigStepIndex = idx; saveConfigProfilesOnly(); applyProfileChangesToAssignments(p.id); refreshConfigFlowWorkbenchOnly(); log(`宸插姞鍏ユ搷浣滐細${op.label}锛屼綅缃細绗?${idx+1} 灞俙, 'ok');
}
function saveSelectedStep() {
  const p = getSelectedProfile(); if(!p || !p.steps.length) return;
  const idx = Math.max(0, Math.min(selectedConfigStepIndex, p.steps.length - 1));
  const step = p.steps[idx];
  const opKey = document.getElementById('configStepOpSelect')?.value || step.opKey;
  const op = OP_DEF_BY_KEY[opKey] || OP_DEFS[0];
  step.opKey = op.key;
  step.label = document.getElementById('configStepLabelInput')?.value.trim() || op.label;
  step.durationSec = Math.max(0, Number(document.getElementById('configStepDurationInput')?.value || 0));
  step.toleranceSec = Math.max(0, Number(document.getElementById('configStepToleranceInput')?.value || 0));
  step.targetTempC = document.getElementById('configStepTempInput')?.value === '' ? null : Number(document.getElementById('configStepTempInput')?.value);
  step.reagentRole = document.getElementById('configStepRoleInput')?.value.trim() || '';
  step.immediateAfterPrev = !!document.getElementById('configStepImmediateInput')?.checked;
  step.requiresTemp = !!document.getElementById('configStepTempEnableInput')?.checked;
  step.allowMultiPrimary = !!document.getElementById('configStepMultiPrimaryInput')?.checked;
  step.notes = document.getElementById('configStepNotesInput')?.value.trim() || '';
  saveConfigProfilesOnly(); applyProfileChangesToAssignments(p.id); refreshConfigFlowWorkbenchOnly(); log(`宸蹭繚瀛樼 ${idx+1} 灞傦細${step.label}`, 'ok');
}
function moveSelectedStep(delta) {
  const p = getSelectedProfile(); if(!p) return;
  const from = selectedConfigStepIndex, to = from + delta;
  if(to < 0 || to >= p.steps.length) return;
  const [step] = p.steps.splice(from, 1); p.steps.splice(to, 0, step); selectedConfigStepIndex = to;
  saveConfigProfilesOnly(); applyProfileChangesToAssignments(p.id); refreshConfigFlowWorkbenchOnly();
}
function reorderStep(from, to) {
  const p = getSelectedProfile(); if(!p || from < 0 || from >= p.steps.length) return;
  let insertAt = Math.max(0, Math.min(Number(to), p.steps.length));
  if(from < insertAt) insertAt -= 1;
  if(from === insertAt) return;
  const [step] = p.steps.splice(from, 1); p.steps.splice(insertAt, 0, step); selectedConfigStepIndex = insertAt;
  saveConfigProfilesOnly(); applyProfileChangesToAssignments(p.id); refreshConfigFlowWorkbenchOnly();
}
function deleteSelectedStep() {
  const p = getSelectedProfile(); if(!p || !p.steps.length) return;
  const idx = Math.max(0, Math.min(selectedConfigStepIndex, p.steps.length - 1));
  const [removed] = p.steps.splice(idx, 1); selectedConfigStepIndex = Math.max(0, idx - 1);
  saveConfigProfilesOnly(); applyProfileChangesToAssignments(p.id); refreshConfigFlowWorkbenchOnly(); log(`宸插垹闄ゆ楠わ細${removed.label}`, 'warn');
}
function saveProfileRules() {
  const p = getSelectedProfile(); if(!p) return;
  const temp = document.getElementById('configTargetTempInput')?.value;
  p.targetTempC = temp === '' ? null : Number(temp);
  const tempFrom = document.getElementById('configTempFromInput')?.value;
  p.tempControlFromStep = tempFrom === '' ? null : Number(tempFrom);
  p.allowMultiPrimary = !!document.getElementById('configAllowMultiPrimaryInput')?.checked;
  p.dabRatio = {
    a:Number(document.getElementById('configDabAInput')?.value || 1),
    b:Number(document.getElementById('configDabBInput')?.value || 1),
    pureWater:Number(document.getElementById('configDabWaterInput')?.value || 18),
    preparePolicy:document.getElementById('configDabPolicySelect')?.value || 'per_run'
  };
  p.notes = document.getElementById('configProfileNotesInput')?.value.trim() || '';
  p.steps.forEach((step, idx) => { if(p.tempControlFromStep && idx + 1 >= p.tempControlFromStep) { step.requiresTemp = true; if(!step.targetTempC && p.targetTempC) step.targetTempC = p.targetTempC; } });
  saveConfigProfilesOnly(); applyProfileChangesToAssignments(p.id); renderConfigPane('rules'); log(`宸蹭繚瀛樿鍒欙細${p.name}`, 'ok');
}
function exportProfile(profile) {
  if(!profile) return;
  downloadText(`${profile.name || profile.id}.json`, JSON.stringify(profile, null, 2), 'application/json');
  log(`宸插鍑洪厤缃?JSON锛?{profile.name}`, 'ok');
}
function exportAllProfiles() {
  downloadText('pathology_stainer_config_profiles.json', JSON.stringify({ profiles:configProfiles }, null, 2), 'application/json');
  log('宸插鍑哄叏閮ㄩ厤缃?JSON', 'ok');
}
function downloadText(filename, text, type='text/plain') {
  const blob = new Blob([text], { type });
  const url = URL.createObjectURL(blob);
  const a = document.createElement('a'); a.href = url; a.download = filename.replace(/[\\/:*?"<>|]+/g, '_');
  document.body.appendChild(a); a.click(); a.remove(); URL.revokeObjectURL(url);
}
function importProfilesFromInput(evt) {
  const file = evt.target.files?.[0]; if(!file) return;
  const reader = new FileReader();
  reader.onload = () => {
    try {
      const parsed = JSON.parse(reader.result);
      const incoming = Array.isArray(parsed) ? parsed : Array.isArray(parsed.profiles) ? parsed.profiles : [parsed];
      incoming.map((p, idx) => normalizeProfile(p, `瀵煎叆閰嶇疆 ${idx+1}`)).forEach(profile => {
        const existing = configProfiles.findIndex(p => p.id === profile.id);
        if(existing >= 0) configProfiles[existing] = profile; else configProfiles.unshift(profile);
        selectedConfigId = profile.id;
      });
      saveConfigProfilesOnly(); renderConfigPane('files'); log(`宸插鍏ラ厤缃?JSON锛?{file.name}`, 'ok');
    } catch(err) { log(`閰嶇疆 JSON 瀵煎叆澶辫触锛?{err.message}`, 'err'); }
  };
  reader.readAsText(file, 'utf-8');
  evt.target.value = '';
}

// 鍙緵涓婁綅鏈?鍚庣瀹炴椂鎺ㄩ€佽皟鐢細window.digitalTwin.update({ arm:{x,y,z1,z2,fluid1,fluid2}, cameras:{reagent:'idle|active|complete|error', arm:'idle|active|complete|error'}, items:[{name,state,level}], slideOps:[{name,steps}], slideTemps:[{name,temp}], channels:[{id,state,progress,pulled,configProfileId}], channelConfigs:{1:'ihc-standard-40c'}, configProfiles:[...], liquids:{pure,pbs,waste,toxic}, detail:{title,lines}, sensors:{'svg-reagent-lane-1-position-sensor':'ready|idle|active|error','svg-reagent-lane-1-entry-sensor':'ready|idle|active|error'} })
window.digitalTwin = {
  update(payload={}) {
    if(payload.arm) { arm = {...arm, ...payload.arm}; updateArmVisual(); }
    if(payload.cameras) { Object.assign(cameraStates, payload.cameras); drawAuxPorts(); updateArmVisual(); }
    if(Array.isArray(payload.items)) payload.items.forEach(it => { if(it.name) { if(it.state) itemState.set(it.name, it.state); if(Number.isFinite(it.level)) itemLevels.set(it.name, it.level); } });
    if(Array.isArray(payload.channels)) payload.channels.forEach(ch => { const target = channels[(ch.id||1)-1]; if(target) { Object.assign(target, ch); if(ch.configProfileId !== undefined) channelConfigAssignments[target.id] = ch.configProfileId; } });
    if(payload.channelConfigs) Object.entries(payload.channelConfigs).forEach(([id, profileId]) => assignConfigToChannel(Number(id), profileId, false));
    if(Array.isArray(payload.configProfiles)) { configProfiles = ensureDefaultProfile(payload.configProfiles.map(normalizeProfile)); selectedConfigId = configProfiles[0]?.id || selectedConfigId; saveConfigProfilesOnly(); renderConfigPane(); } 
    if(Array.isArray(payload.slideOps)) payload.slideOps.forEach(op => { if(op.name && Array.isArray(op.steps)) slideOps.set(op.name, op.steps.slice(0, 12)); });
    if(Array.isArray(payload.slideTemps)) payload.slideTemps.forEach(st => { if(st.name && Number.isFinite(st.temp)) slideTemps.set(st.name, st.temp); });
    if(payload.liquids) Object.assign(liquids, payload.liquids);
    if(payload.sensors) Object.entries(payload.sensors).forEach(([id, state]) => itemState.set(id, state));
    if(payload.metrics) updateHeaderMetrics(payload.metrics);
    if(payload.detail && payload.detail.title) setInfoPanel(payload.detail.title, payload.detail.lines || []);
    drawData(); drawSlideOps(); updateVisualStates(); renderChannelCards(); renderLiquids(); updateKpis();
  },
  getState() { return { arm, channels, channelConfigAssignments, configProfiles, appSettings, liquids, metrics:headerMetrics, mode:uiMode, selectedName, currentStepIndex, slideTemps: Object.fromEntries(slideTemps) }; },
  showInfo(title, lines=[]) { setInfoPanel(title, lines); showSideTab('status'); },
  runPrecheck,
  setPrecheckResults(results={}) { window.digitalTwinPrecheckResults = results; },
  reset: resetDemo,
  config: {
    listProfiles() { return deepClone(configProfiles); },
    getProfile(id) { return deepClone(getProfileById(id)); },
    upsertProfile(profile) { const normalized = normalizeProfile(profile); const idx = configProfiles.findIndex(p => p.id === normalized.id); if(idx >= 0) configProfiles[idx] = normalized; else configProfiles.unshift(normalized); selectedConfigId = normalized.id; saveConfigProfilesOnly(); renderConfigPane(); return deepClone(normalized); },
    assignChannel(channelId, profileId) { assignConfigToChannel(Number(channelId), profileId, true); },
    exportChannelAssignments() { return deepClone(channelConfigAssignments); }
  },
  update(payload={}) { Object.entries(payload || {}).forEach(([id, value]) => { const cssId = (window.CSS && CSS.escape) ? CSS.escape(id) : String(id).replace(/([\"'\]\[])/g, '\\$1'); const node = document.getElementById(id) || document.querySelector(`[data-control-id="${cssId}"]`); if(!node) return; if(value && typeof value === 'object') { if('text' in value) node.textContent = value.text; if('html' in value) node.innerHTML = value.html; if('value' in value && 'value' in node) node.value = value.value; if('className' in value) node.className = value.className; if('dataset' in value) Object.entries(value.dataset).forEach(([k,v]) => { node.dataset[k] = v; }); } else if('value' in node) node.value = value; else node.textContent = value; }); },
  engineering: { getDebugForm() { return collectEngineeringForm('debugPane'); }, getConfigForm(sectionId) { return collectEngineeringForm(sectionId || 'configPane'); } },
  auth: {
    getCurrentUser() { return deepClone(CURRENT_USER); },
    listUsers() { return deepClone(backendUsersLoaded ? backendUsers : []); },
    logout: logoutCurrentUser
  },
  getControlIdRegistry
};


function applyServerUser(user) {
  if(!user || !user.username) return false;
  CURRENT_USER = {
    id: String(user.id || user.username),
    username: user.username,
    name: user.displayName || user.name || user.username,
    role: user.activeRole || user.role || 'operator'
  };
  return true;
}
async function restoreBackendSession() {
  try {
    const res = await fetch('/api/current-user', { credentials:'same-origin', cache:'no-store' });
    if(!res.ok) { setLoginVisible(true); return; }
    const user = await res.json();
    if(applyServerUser(user)) { setLoginVisible(false); applyUserRoleUi(); renderUserMenu(); loadDatabaseSnapshot(); }
    else setLoginVisible(true);
  } catch(e) { setLoginVisible(true); }
}
function setLoginError(message='') {
  const node = document.getElementById('loginErrorText');
  if(node) node.textContent = message;
}
function setLoginVisible(visible) {
  const screen = document.getElementById('loginScreen');
  if(screen) screen.classList.toggle('hidden', !visible);
}
function resetStatusDetailToDefault() {
  const title = document.getElementById('statusPanelTitle');
  if(title) title.textContent = '瀵硅薄璇︽儏';
  const detail = document.getElementById('detailBox');
  if(detail) detail.innerHTML = '<strong>鏈€変腑瀵硅薄</strong>鐐瑰嚮浠绘剰璇曞墏銆侀厤娑插瓟銆丄/B娑层€佺幓鐗囥€侀€氶亾鎸夐挳銆侀拡澶淬€佺浉鏈烘垨瀛斾綅锛屽彲鏌ョ湅鐘舵€併€佷綑閲忎笌姝ラ杩涘害銆?;
}
async function loginAsRole(role) {
  const username = (document.getElementById('loginUsername')?.value || '').trim();
  const password = document.getElementById('loginPassword')?.value || '';
  if(!username) { setLoginError('璇疯緭鍏ヨ处鍙枫€?); return; }
  let res;
  try {
    res = await fetch('/api/login', {
      method:'POST',
      headers:{'Content-Type':'application/json'},
      credentials:'same-origin',
      body:JSON.stringify({ username, password, role })
    });
  } catch(err) { setLoginError('鐧诲綍璇锋眰澶辫触锛? + err.message); return; }
  if(!res || !res.ok) { setLoginError('璐﹀彿銆佸瘑鐮佹垨韬唤涓嶅尮閰嶏紝鎴栬鐢ㄦ埛宸插仠鐢ㄣ€?); return; }
  let payload = null;
  try { payload = await res.json(); } catch(e) { payload = null; }
  if(!payload || !payload.ok || !applyServerUser(payload.user || {})) { setLoginError('鐧诲綍澶辫触锛氬悗绔湭杩斿洖鏈夋晥鐢ㄦ埛銆?); return; }
  setLoginError('');
  setLoginVisible(false);
  applyUserRoleUi();
  renderUserMenu();
  flowPanelForcedWide = false;
  normalRightPanelRatio = RIGHT_PANEL_MIN_RATIO;
  persistRightPanelRatio(RIGHT_PANEL_MIN_RATIO);
  setRightPanelRatio(RIGHT_PANEL_MIN_RATIO, false);
  resetStatusDetailToDefault();
  showSideTab('status');
  loadDatabaseSnapshot();
}
async function logoutCurrentUser() {
  try { await fetch('/api/logout', { method:'POST', credentials:'same-origin' }); } catch(e) {}
  CURRENT_USER = { id:'', username:'', name:'鏈櫥褰?, role:'guest' };
  applyUserRoleUi();
  renderUserMenu();
  setLoginVisible(true);
  setInfoPanel('宸查€€鍑?, ['璇烽€夋嫨绠＄悊鍛樻垨瀹為獙鍛樿韩浠介噸鏂扮櫥褰曘€?]);
  showSideTab('status');
}
function applyUserRoleUi() {
  const isAdmin = CURRENT_USER.role === 'admin';
  const debugTab = document.getElementById('debugTab');
  const modeDebugBtn = document.getElementById('modeDebugBtn');
  if(debugTab) { debugTab.classList.toggle('role-hidden', !isAdmin); debugTab.hidden = !isAdmin; debugTab.setAttribute('aria-hidden', isAdmin ? 'false' : 'true'); }
  if(modeDebugBtn) { modeDebugBtn.classList.toggle('role-hidden', !isAdmin); modeDebugBtn.hidden = !isAdmin; modeDebugBtn.setAttribute('aria-hidden', isAdmin ? 'false' : 'true'); }
  const userBtn = document.getElementById('userBtn');
  if(userBtn) {
    userBtn.textContent = CURRENT_USER.name || '鏈櫥褰?;
    userBtn.classList.toggle('role-admin', isAdmin);
    userBtn.classList.toggle('role-operator', CURRENT_USER.role === 'operator');
    userBtn.title = isAdmin ? '绠＄悊鍛? : (CURRENT_USER.role === 'operator' ? '瀹為獙鍛? : '鏈櫥褰?);
  }
  if(!isAdmin && document.getElementById('debugPane')?.classList.contains('active')) showSideTab('status');
  if(!isAdmin && document.getElementById('modeDebugBtn')?.classList.contains('active')) {
    document.querySelectorAll('.mode-btn').forEach(btn => btn.classList.toggle('active', btn.id === 'modeTwinBtn'));
    uiMode = 'twin';
  }
}
function initLoginPage() {
  document.querySelectorAll('[data-login-role]').forEach(btn => {
    if(btn.dataset.loginBound === 'true') return;
    btn.dataset.loginBound = 'true';
    btn.addEventListener('click', () => loginAsRole(btn.getAttribute('data-login-role')));
  });
  ['loginUsername','loginPassword'].forEach(id => {
    const input = document.getElementById(id);
    if(input && input.dataset.enterBound !== 'true') {
      input.dataset.enterBound = 'true';
      input.addEventListener('keydown', evt => { if(evt.key === 'Enter') loginAsRole('operator'); });
    }
  });
  applyUserRoleUi();
  setLoginVisible(!CURRENT_USER.id);
}
function makeCommandId(prefix='ui') {
  return `${prefix}-${Date.now()}-${Math.random().toString(16).slice(2)}`;
}
async function backendApi(path, options={}) {
  const headers = Object.assign({}, options.headers || {});
  const hasBody = Object.prototype.hasOwnProperty.call(options, 'body') && options.body !== undefined;
  if(hasBody && !headers['Content-Type']) headers['Content-Type'] = 'application/json';
  const res = await fetch(path, Object.assign({ credentials:'same-origin', cache:'no-store' }, options, { headers }));
  if(!res.ok) {
    let detail = '';
    let payload = null;
    try {
      payload = await res.json();
      detail = payload.detail || payload.message || payload.code || '';
    } catch(e) {
      try { detail = await res.text(); } catch(_) {}
    }
    const apiErr = new Error(detail || `HTTP ${res.status}`);
    apiErr.status = res.status;
    apiErr.data = payload;
    throw apiErr;
  }
  if(res.status === 204) return null;
  const text = await res.text();
  return text ? JSON.parse(text) : null;
}
function routeWriteError(err) {
  const status = err?.status;
  const detail = err?.message || '';
  if(status === 401) {
    CURRENT_USER = { id:'', username:'', name:'鏈櫥褰?, role:'guest' };
    applyUserRoleUi(); renderUserMenu(); setLoginVisible(true);
    log('鐧诲綍宸插け鏁堬紝璇烽噸鏂扮櫥褰?, 'err');
  } else if(status === 403) {
    log(`鏉冮檺涓嶈冻锛?{detail || '褰撳墠韬唤鏃犳硶鎵ц璇ユ搷浣?}`, 'err');
  } else if(status === 400 || status === 409) {
    log(`璇锋眰鏈€氳繃锛?{detail || '璇锋鏌ヨ緭鍏ュ悗閲嶈瘯'}`, 'err');
  } else {
    log(`鎿嶄綔澶辫触锛?{detail || '璇风◢鍚庨噸璇?}`, 'err');
  }
}
async function writeApi(path, options={}) {
  try {
    return await backendApi(path, options);
  } catch(err) {
    routeWriteError(err);
    throw err;
  }
}
async function apiGetOrNull(path) {
  try {
    return await backendApi(path);
  } catch(err) {
    if(err?.status === 404) return null;
    routeWriteError(err);
    throw err;
  }
}
function normalizeBackendUser(user) {
  const roles = Array.isArray(user?.roles) && user.roles.length ? user.roles : [user?.role || 'operator'];
  const primaryRole = roles.includes('admin') ? 'admin' : (roles[0] || 'operator');
  return {
    id: String(user?.id || user?.username || ''),
    username: String(user?.username || ''),
    name: String(user?.displayName || user?.name || user?.username || ''),
    role: primaryRole,
    roles: roles.map(role => String(role)).filter(Boolean),
    enabled: user?.enabled !== false
  };
}
function normalizeBackendRole(role) {
  return {
    id: String(role?.id || role?.code || ''),
    code: String(role?.code || role?.id || ''),
    name: String(role?.name || role?.code || role?.id || '')
  };
}
function roleDisplayName(role) {
  if(role === 'admin') return '绠＄悊鍛?;
  if(role === 'operator') return '瀹為獙鍛?;
  const match = backendRoles.find(item => item.code === role || item.id === role);
  return match?.name || role;
}
async function refreshBackendUsers(force=false) {
  if(backendUsersLoading) return;
  if(backendUsersLoaded && !force) return;
  backendUsersLoading = true;
  try {
    const [users, roles] = await Promise.all([backendApi('/api/users'), backendApi('/api/roles')]);
    backendUsers = Array.isArray(users) ? users.map(normalizeBackendUser) : [];
    backendRoles = Array.isArray(roles) ? roles.map(normalizeBackendRole).filter(role => role.code) : [];
    backendUsersLoaded = true;
  } finally {
    backendUsersLoading = false;
  }
}
function queueBackendUsersRefresh(force=false) {
  refreshBackendUsers(force).then(() => {
    const pane = document.getElementById('settingsSubpaneUser');
    if(pane) openSettingsPage('鐢ㄦ埛绠＄悊', true);
  }).catch(err => {
    backendUsersLoaded = false;
    log(`鐢ㄦ埛绠＄悊鎺ュ彛璇诲彇澶辫触锛?{err.message}`, 'err');
    const tbody = document.querySelector('#userManagementTable tbody');
    if(tbody) tbody.innerHTML = '<tr><td colspan="5">鐢ㄦ埛绠＄悊鎺ュ彛璇诲彇澶辫触锛岃纭宸蹭娇鐢ㄧ鐞嗗憳璐﹀彿鐧诲綍銆?/td></tr>';
  });
}
function renderUserManagementPane(active=false) {
  if(active && CURRENT_USER.role === 'admin' && !backendUsersLoaded && !backendUsersLoading) queueBackendUsersRefresh(false);
  const users = backendUsersLoaded ? backendUsers : [];
  const rows = backendUsersLoading && !backendUsersLoaded
    ? '<tr><td colspan="5">姝ｅ湪璇诲彇鏁版嵁搴撶敤鎴?..</td></tr>'
    : (users.length ? users.map(user => {
      const roleText = roleDisplayName(user.role);
      const roleClass = user.role === 'admin' ? 'admin' : 'operator';
      const canDelete = user.username !== 'admin' && user.username !== CURRENT_USER.username;
      const canDisable = user.username !== CURRENT_USER.username;
      return `<tr id="userRow-${escapeHtml(user.id)}"><td>${escapeHtml(user.username)}</td><td>${escapeHtml(user.name)}</td><td><span class="user-role-chip ${roleClass}">${escapeHtml(roleText)}</span></td><td><span class="user-status-chip ${user.enabled ? 'enabled' : ''}">${user.enabled ? '鍚敤' : '鍋滅敤'}</span></td><td><div class="user-row-actions"><button type="button" data-user-rename="${escapeHtml(user.id)}">鏀瑰悕</button><button type="button" data-user-roles="${escapeHtml(user.id)}">瑙掕壊</button><button type="button" data-user-toggle="${escapeHtml(user.id)}" ${canDisable ? '' : 'disabled'}>${user.enabled ? '鍋滅敤' : '鍚敤'}</button><button type="button" data-user-reset="${escapeHtml(user.id)}">閲嶇疆瀵嗙爜</button><button type="button" class="danger" data-user-delete="${escapeHtml(user.id)}" ${canDelete ? '' : 'disabled'}>鍒犻櫎</button></div></td></tr>`;
    }).join('') : '<tr><td colspan="5">鏁版嵁搴撴殏鏃犵敤鎴凤紝鎴栧皻鏈畬鎴愯鍙栥€?/td></tr>');
  const roleOptions = (backendRoles.length ? backendRoles : [{code:'operator', name:'瀹為獙鍛?}, {code:'admin', name:'绠＄悊鍛?}])
    .map(role => `<option value="${escapeHtml(role.code)}">${escapeHtml(roleDisplayName(role.code))}</option>`).join('');
  return `<div class="settings-subpane ${active ? 'active' : ''}" id="settingsSubpaneUser" data-settings-subpane="user">
    <div class="settings-section" id="settingsSectionUserManagement"><strong>鐢ㄦ埛绠＄悊</strong>
      <div class="login-permission-note">鏈〉宸叉帴鍏ュ悗绔湡瀹炵敤鎴风鐞?API锛屽彧鏈夌鐞嗗憳浼氳瘽鍙互璇诲彇鍜屼慨鏀规暟鎹簱鐢ㄦ埛銆?/div>
      <div class="user-management-grid">
        <div class="user-table-wrap" id="userManagementTableWrap" data-control-id="userManagementTableWrap"><table class="user-table" id="userManagementTable" data-control-id="userManagementTable"><thead><tr><th>璐﹀彿</th><th>鏄剧ず鍚?/th><th>瑙掕壊</th><th>鐘舵€?/th><th>鎿嶄綔</th></tr></thead><tbody>${rows}</tbody></table></div>
        <section class="engineering-card user-form-card" id="userCreateCard" data-control-id="userCreateCard"><strong>鏂板鐢ㄦ埛</strong><div class="engineering-form-grid"><label>璐﹀彿<input id="userNewUsernameInput" data-control-id="userNewUsernameInput" placeholder="渚嬪 tech01"></label><label>鏄剧ず鍚?input id="userNewNameInput" data-control-id="userNewNameInput" placeholder="濮撳悕鎴栧矖浣?></label><label>瑙掕壊<select id="userNewRoleSelect" data-control-id="userNewRoleSelect">${roleOptions}</select></label><label>鍒濆瀵嗙爜<input id="userNewPasswordInput" data-control-id="userNewPasswordInput" value="123456"></label></div><div class="engineering-actions"><button type="button" id="userAddBtn" data-control-id="userAddBtn" class="ok-lite">鏂板鐢ㄦ埛</button><button type="button" id="userSaveListBtn" data-control-id="userSaveListBtn">鍒锋柊鐢ㄦ埛琛?/button></div><div class="engineering-note">鏂板銆佹敼鍚嶃€佽鑹层€佸惎鍋溿€侀噸缃瘑鐮佸拰鍒犻櫎閮戒細鐩存帴璋冪敤鍚庣 API锛屽苟浠庢暟鎹簱閲嶆柊璇诲彇鍒楄〃銆?/div></section>
      </div>
    </div>
  </div>`;
}
function findBackendUser(id) {
  return backendUsers.find(user => user.id === id);
}
async function reloadUserManagementPane(message, level='ok') {
  backendUsersLoaded = false;
  await refreshBackendUsers(true);
  openSettingsPage('鐢ㄦ埛绠＄悊', true);
  if(message) log(message, level);
}
function bindUserManagementHandlers() {
  const addBtn = document.getElementById('userAddBtn');
  if(addBtn) addBtn.onclick = async () => {
    const username = (document.getElementById('userNewUsernameInput')?.value || '').trim();
    const displayName = (document.getElementById('userNewNameInput')?.value || '').trim() || username;
    const role = document.getElementById('userNewRoleSelect')?.value || 'operator';
    const password = document.getElementById('userNewPasswordInput')?.value || '123456';
    if(!username) { log('鏂板鐢ㄦ埛澶辫触锛氳处鍙蜂笉鑳戒负绌?, 'err'); return; }
    try {
      await backendApi('/api/users', { method:'POST', body: JSON.stringify({ commandId:makeCommandId('user-create'), username, displayName, password, roles:[role] }) });
      await reloadUserManagementPane(`鐢ㄦ埛宸叉柊澧烇細${displayName}`, 'ok');
    } catch(err) { const msg = `鏂板鐢ㄦ埛澶辫触锛?{err.message}`; log(msg, 'err'); alert(msg); }
  };
  const saveBtn = document.getElementById('userSaveListBtn');
  if(saveBtn) saveBtn.onclick = async () => {
    try { await reloadUserManagementPane('鐢ㄦ埛琛ㄥ凡浠庢暟鎹簱鍒锋柊', 'ok'); }
    catch(err) { log(`鍒锋柊鐢ㄦ埛琛ㄥけ璐ワ細${err.message}`, 'err'); }
  };
  document.querySelectorAll('[data-user-rename]').forEach(btn => btn.onclick = async () => {
    const user = findBackendUser(btn.getAttribute('data-user-rename'));
    if(!user) return;
    const displayName = prompt('璇疯緭鍏ユ柊鐨勬樉绀哄悕', user.name) || '';
    if(!displayName.trim()) return;
    try {
      await backendApi(`/api/users/${encodeURIComponent(user.id)}/display-name`, { method:'PUT', body: JSON.stringify({ commandId:makeCommandId('user-rename'), displayName:displayName.trim() }) });
      await reloadUserManagementPane(`鐢ㄦ埛鏄剧ず鍚嶅凡鏇存柊锛?{displayName.trim()}`, 'ok');
    } catch(err) { log(`鏇存柊鏄剧ず鍚嶅け璐ワ細${err.message}`, 'err'); }
  });
  document.querySelectorAll('[data-user-roles]').forEach(btn => btn.onclick = async () => {
    const user = findBackendUser(btn.getAttribute('data-user-roles'));
    if(!user) return;
    const rolesText = prompt('璇疯緭鍏ヨ鑹蹭唬鐮侊紝澶氫釜瑙掕壊鐢ㄩ€楀彿鍒嗛殧', user.roles.join(',')) || '';
    const roles = rolesText.split(',').map(role => role.trim()).filter(Boolean);
    if(!roles.length) return;
    try {
      await backendApi(`/api/users/${encodeURIComponent(user.id)}/roles`, { method:'PUT', body: JSON.stringify({ commandId:makeCommandId('user-roles'), roles }) });
      await reloadUserManagementPane(`鐢ㄦ埛瑙掕壊宸叉洿鏂帮細${user.username}`, 'ok');
    } catch(err) { log(`鏇存柊瑙掕壊澶辫触锛?{err.message}`, 'err'); }
  });
  document.querySelectorAll('[data-user-toggle]').forEach(btn => btn.onclick = async () => {
    const user = findBackendUser(btn.getAttribute('data-user-toggle'));
    if(!user) return;
    if(user.username === CURRENT_USER.username && user.enabled) { log('涓嶈兘鍋滅敤褰撳墠鐧诲綍鐢ㄦ埛', 'err'); return; }
    try {
      await backendApi(`/api/users/${encodeURIComponent(user.id)}/enabled`, { method:'PUT', body: JSON.stringify({ commandId:makeCommandId('user-enabled'), enabled:!user.enabled }) });
      await reloadUserManagementPane(`${user.name}宸?{user.enabled ? '鍋滅敤' : '鍚敤'}`, user.enabled ? 'warn' : 'ok');
    } catch(err) { log(`鏇存柊鐢ㄦ埛鐘舵€佸け璐ワ細${err.message}`, 'err'); }
  });
  document.querySelectorAll('[data-user-reset]').forEach(btn => btn.onclick = async () => {
    const user = findBackendUser(btn.getAttribute('data-user-reset'));
    if(!user) return;
    const newPassword = prompt('璇疯緭鍏ユ柊瀵嗙爜', '123456') || '';
    if(!newPassword) return;
    try {
      await backendApi(`/api/users/${encodeURIComponent(user.id)}/password`, { method:'PUT', body: JSON.stringify({ commandId:makeCommandId('user-password'), newPassword }) });
      await reloadUserManagementPane(`${user.name}瀵嗙爜宸查噸缃甡, 'warn');
    } catch(err) { log(`閲嶇疆瀵嗙爜澶辫触锛?{err.message}`, 'err'); }
  });
  document.querySelectorAll('[data-user-delete]').forEach(btn => btn.onclick = async () => {
    const user = findBackendUser(btn.getAttribute('data-user-delete'));
    if(!user || user.username === 'admin') return;
    if(user.username === CURRENT_USER.username) { log('涓嶈兘鍒犻櫎褰撳墠鐧诲綍鐢ㄦ埛', 'err'); return; }
    if(!confirm(`纭鍒犻櫎鐢ㄦ埛 ${user.username}锛焋)) return;
    try {
      await backendApi(`/api/users/${encodeURIComponent(user.id)}?commandId=${encodeURIComponent(makeCommandId('user-delete'))}`, { method:'DELETE' });
      await reloadUserManagementPane(`鐢ㄦ埛宸插垹闄わ細${user.name}`, 'warn');
    } catch(err) { const msg = `鍒犻櫎鐢ㄦ埛澶辫触锛?{err.message}`; log(msg, 'err'); alert(msg); }
  });
}
function renderUserMenu() {
  const menu = document.getElementById('userMenu');
  const userBtn = document.getElementById('userBtn');
  if(userBtn) userBtn.textContent = CURRENT_USER.name;
  if(!menu) return;
  const buttons = CURRENT_USER.role === 'admin'
    ? ['<button type="button" id="userManageBtn" data-user-action="manage">鐢ㄦ埛绠＄悊</button>', '<button type="button" id="userLogoutBtn" data-user-action="logout">閫€鍑?/button>']
    : ['<button type="button" id="userLogoutBtn" data-user-action="logout">閫€鍑?/button>'];
  menu.innerHTML = buttons.join('');
  menu.querySelectorAll('button').forEach(btn => btn.addEventListener('click', evt => {
    evt.stopPropagation();
    menu.classList.remove('open');
    const action = btn.getAttribute('data-user-action');
    if(action === 'manage') { openSettingsPage('鐢ㄦ埛绠＄悊', true); log('鎵撳紑鐢ㄦ埛绠＄悊锛氱鐞嗗憳鍏ュ彛', 'ok'); }
    if(action === 'logout') { logoutCurrentUser(); log('鐢ㄦ埛宸查€€鍑虹櫥褰?, 'warn'); }
  }));
}
function openSettingsPage(section='绯荤粺璁剧疆', userManage=false) {
  const mode = document.querySelector('.mode-btn.active')?.textContent || '瀛敓妯℃嫙';
  const pane = document.getElementById('settingsPane');
  if(pane) {
    const canShowUsers = CURRENT_USER.role === 'admin';
    const canManageUsers = Boolean(userManage && canShowUsers);
    const userTab = canShowUsers ? `<button type="button" class="settings-tab-btn ${canManageUsers ? 'active' : ''}" data-settings-tab="user" id="settingsTabUser" data-control-id="settingsTabUser">鐢ㄦ埛</button>` : '';
    const userPane = canShowUsers ? renderUserManagementPane(canManageUsers) : '';
    pane.innerHTML = `<div class="settings-module" id="settingsModule" data-control-id="settingsModule">
      <div class="settings-tabs" id="settingsSubTabs" role="tablist" aria-label="璁剧疆瀛愰〉">
        <button type="button" class="settings-tab-btn ${canManageUsers ? '' : 'active'}" data-settings-tab="runtime" id="settingsTabRuntime" data-control-id="settingsTabRuntime">杩愯</button>
        <button type="button" class="settings-tab-btn" data-settings-tab="communication" id="settingsTabCommunication" data-control-id="settingsTabCommunication">閫氳</button>
        <button type="button" class="settings-tab-btn" data-settings-tab="device" id="settingsTabDevice" data-control-id="settingsTabDevice">璁惧</button>
        <button type="button" class="settings-tab-btn" data-settings-tab="safety" id="settingsTabSafety" data-control-id="settingsTabSafety">瀹夊叏</button>
        <button type="button" class="settings-tab-btn" data-settings-tab="ui" id="settingsTabUi" data-control-id="settingsTabUi">鐣岄潰</button>
        ${userTab}
      </div>
      <div class="settings-subpane ${canManageUsers ? '' : 'active'}" id="settingsSubpaneRuntime" data-settings-subpane="runtime">
        <div class="settings-section" id="settingsSectionRuntime"><strong>${escapeHtml(section)}</strong><div class="settings-line" id="settingsCurrentModeLine"><span>褰撳墠妯″紡</span><b id="settingsModeText">${escapeHtml(mode)}</b></div><div class="settings-line" id="settingsPrecheckLine"><span>妫€娴嬬姸鎬?/span><b id="settingsPrecheckText">${precheckPassed ? '宸查€氳繃' : '鏈畬鎴?}</b></div><div class="settings-help">寮€濮嬪墠蹇呴』閫氳繃妫€娴嬶紱寮€濮嬪拰鏆傚仠鎸夐挳鍦ㄦ湭閫氳繃妫€娴嬪墠浼氬紩瀵艰繘鍏ユ娴嬮〉闈€?/div></div>
      </div>
      <div class="settings-subpane" id="settingsSubpaneCommunication" data-settings-subpane="communication">
        <div class="settings-section" id="settingsSectionCommunication"><strong>閫氳涓庢暟鎹?/strong><div class="settings-edit-grid"><label>鏁版嵁鎺ュ彛<select id="settingsDataInterfaceInput" data-control-id="settingsDataInterfaceInput"><option value="WebSocket 棰勭暀" ${appSettings.dataInterface==='WebSocket 棰勭暀'?'selected':''}>WebSocket 棰勭暀</option><option value="涓婁綅鏈虹洿杩? ${appSettings.dataInterface==='涓婁綅鏈虹洿杩??'selected':''}>涓婁綅鏈虹洿杩?/option><option value="绂荤嚎妯℃嫙" ${appSettings.dataInterface==='绂荤嚎妯℃嫙'?'selected':''}>绂荤嚎妯℃嫙</option></select></label><label>涓婁綅鏈?/ WebSocket 鍦板潃<input id="settingsHostAddressInput" data-control-id="settingsHostAddressInput" value="${escapeHtml(appSettings.hostAddress)}"></label><label>蹇冭烦闂撮殧 / 绉?input id="settingsHeartbeatInput" type="number" min="1" step="1" value="${Number(appSettings.heartbeatSec)}"></label><label>鏃ュ織淇濈暀鏉℃暟<input id="settingsLogRetentionInput" type="number" min="50" step="50" value="${Number(appSettings.logRetention)}"></label></div><div class="settings-help">鍚庣画鍙洿鎺ユ帴 WebSocket 鎴栦笂浣嶆満瀹炴椂鏁版嵁銆傛帶浠?ID 宸查鐣欙紝渚夸簬鍚庣缁戝畾銆?/div></div>
      </div>
      <div class="settings-subpane" id="settingsSubpaneDevice" data-settings-subpane="device">
        <div class="settings-section" id="settingsSectionDeviceParameters"><strong>璁惧鍙傛暟</strong><div class="settings-edit-grid"><label>閽堝ご闂磋窛 / mm<input id="settingsNeedleGapInput" type="number" min="0" step="0.1" value="${Number(appSettings.needleGapMm)}"></label><label>璇曞墏鐡堕粯璁ゅ閲?/ ml<input id="settingsReagentCapacityInput" type="number" min="0" step="0.1" value="${Number(appSettings.reagentBottleCapacityMl)}"></label><label>璇曞墏鍖虹洰鏍囨俯搴?/ 鈩?input id="settingsReagentTargetInput" type="number" step="0.1" value="${Number(appSettings.reagentTargetTempC)}"></label><label>鏌撹壊鍖虹洰鏍囨俯搴?/ 鈩?input id="settingsWorkTargetInput" type="number" step="0.1" value="${Number(appSettings.workTargetTempC)}"></label></div><div class="settings-help">璇曞墏鐡跺閲忕敤浜庤瘯鍓傚尯鍥句緥 ml 姹囨€伙紱娓╁害鐢ㄤ簬褰撳墠妯℃嫙鏄剧ず鍜岄厤缃粯璁ゅ€笺€?/div></div>
      </div>
      <div class="settings-subpane" id="settingsSubpaneSafety" data-settings-subpane="safety">
        <div class="settings-section" id="settingsSectionSafetyThresholds"><strong>娑茶矾涓庡畨鍏ㄩ槇鍊?/strong><div class="settings-edit-grid"><label>绾按浣庢恫浣?/ %<input id="settingsPureThresholdInput" type="number" min="0" max="100" step="1" value="${Number(appSettings.pureLowThresholdPct)}"></label><label>PBS浣庢恫浣?/ %<input id="settingsPbsThresholdInput" type="number" min="0" max="100" step="1" value="${Number(appSettings.pbsLowThresholdPct)}"></label><label>搴熸恫婊￠槇鍊?/ %<input id="settingsWasteThresholdInput" type="number" min="0" max="100" step="1" value="${Number(appSettings.wasteFullThresholdPct)}"></label><label>鎺掓瘨妗舵弧闃堝€?/ %<input id="settingsToxicThresholdInput" type="number" min="0" max="100" step="1" value="${Number(appSettings.toxicFullThresholdPct)}"></label></div></div>
      </div>
      <div class="settings-subpane" id="settingsSubpaneUi" data-settings-subpane="ui">
        <div class="settings-section" id="settingsSectionUiPlanning"><strong>鐣岄潰涓庤鍒?/strong><div class="settings-edit-grid"><label>鎵撳紑椤甸潰鍚庤嚜鍔ㄦ娴?select id="settingsAutoPrecheckInput" data-control-id="settingsAutoPrecheckInput"><option value="false" ${!appSettings.autoRunPrecheck?'selected':''}>鍚?/option><option value="true" ${appSettings.autoRunPrecheck?'selected':''}>鏄?/option></select></label><label>鍙充晶鏍忓揩鎹锋搷浣?span class="settings-readonly-field">搴曢儴宸ュ叿琛屾寜閽彲鍦?30% / 70% 涔嬮棿蹇€熷垏鎹?/span></label></div><div class="settings-help">鐜荤墖姝ラ鐐圭敱椤甸潰鐗╃悊甯冨眬鍐冲畾锛岄厤缃枃浠跺彲淇濈暀瀹屾暣姝ラ锛岀晫闈笉鍐嶆彁渚涙樉绀轰笂闄愰檺鍒堕」銆?/div></div>
      </div>
      ${userPane}
      <div class="settings-actions" id="settingsActions"><button type="button" id="settingsSaveBtn" data-control-id="settingsSaveBtn">淇濆瓨璁剧疆</button><button type="button" id="settingsResetBtn" data-control-id="settingsResetBtn">鎭㈠榛樿</button></div>
    </div>`;
    const toggleSettingsActions = (key) => {
      const actions = document.getElementById('settingsActions');
      if(actions) actions.style.display = (key === 'user') ? 'none' : '';
    };
    document.querySelectorAll('[data-settings-tab]').forEach(btn => {
      btn.addEventListener('click', () => {
        const key = btn.getAttribute('data-settings-tab');
        document.querySelectorAll('[data-settings-tab]').forEach(b => b.classList.toggle('active', b === btn));
        document.querySelectorAll('[data-settings-subpane]').forEach(pane => pane.classList.toggle('active', pane.getAttribute('data-settings-subpane') === key));
        toggleSettingsActions(key);
        if(key === 'user' && CURRENT_USER.role === 'admin' && !backendUsersLoaded && !backendUsersLoading) queueBackendUsersRefresh(false);
      });
    });
    const initialSettingsTab = document.querySelector('[data-settings-tab].active');
    toggleSettingsActions(initialSettingsTab ? initialSettingsTab.getAttribute('data-settings-tab') : 'runtime');
    bindClick('settingsSaveBtn', saveSettingsFromPane);
    bindClick('settingsResetBtn', resetAppSettings);
    if(canShowUsers) bindUserManagementHandlers();
  }
  showSideTab('settings');
  log(`鎵撳紑${section}锛氬彸渚ф爮杩涘叆璁剧疆椤甸潰`, 'ok');
}


function selectSvgControl(nodeOrId) {
  const node = typeof nodeOrId === 'string' ? document.getElementById(nodeOrId) : nodeOrId;
  if(!node) return;
  const id = node.id || node.getAttribute('data-control-id');
  if(!id) return;
  selectedSvgControlId = id;
  document.querySelectorAll('.svg-control-selected').forEach(el => el.classList.remove('svg-control-selected'));
  node.classList.add('svg-control-selected');
  flashSvgControl(node);
}
function flashControl(btn) {
  if(!btn) return;
  btn.classList.remove('control-flash');
  void btn.offsetWidth;
  btn.classList.add('control-flash');
  setTimeout(() => btn.classList.remove('control-flash'), 620);
}
function flashSvgControl(node) {
  if(!node) return;
  node.classList.remove('svg-control-flash');
  try { node.getBBox(); } catch(e) {}
  node.classList.add('svg-control-flash');
  setTimeout(() => node.classList.remove('svg-control-flash'), 620);
}
document.querySelectorAll('.top-action, .mode-btn, .tab-btn, .top-icon-action, .user-btn').forEach(btn => {
  btn.addEventListener('click', () => flashControl(btn), { capture:true });
});
function bindClick(id, handler) { const node = document.getElementById(id); if(node) node.addEventListener('click', handler); }

const RIGHT_PANEL_MIN_RATIO = 0.30;
const RIGHT_PANEL_MAX_RATIO = 0.70;
const RIGHT_PANEL_DEFAULT_RATIO = 0.30;
let flowPanelForcedWide = false;
let normalRightPanelRatio = RIGHT_PANEL_DEFAULT_RATIO;
let flowPanelResizeFrame = null;
function clampRightPanelRatio(ratio) {
  const n = Number(ratio);
  return Math.max(RIGHT_PANEL_MIN_RATIO, Math.min(RIGHT_PANEL_MAX_RATIO, Number.isFinite(n) ? n : RIGHT_PANEL_DEFAULT_RATIO));
}
function getResizableMainWidth() {
  const mainEl = document.querySelector('main');
  if(!mainEl) return window.innerWidth || 1200;
  const rect = mainEl.getBoundingClientRect();
  return Math.max(1, rect.width - 8);
}
function getCurrentRightPanelRatio() {
  const span = getResizableMainWidth();
  const raw = getComputedStyle(document.documentElement).getPropertyValue('--right-panel-width').trim().replace('px','');
  const px = Number(raw);
  if(!Number.isFinite(px) || px <= 0) return normalRightPanelRatio || RIGHT_PANEL_DEFAULT_RATIO;
  return clampRightPanelRatio(px / span);
}
function persistRightPanelRatio(ratio) {
  const clamped = clampRightPanelRatio(ratio);
  normalRightPanelRatio = clamped;
  try {
    localStorage.setItem('digitalTwinRightPanelNormalRatio', String(clamped));
    localStorage.setItem('digitalTwinRightPanelRatio', String(clamped));
  } catch(e) {}
}
function updateRightPanelQuickToggle() {
  const quick = document.getElementById('rightPanelQuickToggle');
  const text = document.getElementById('rightPanelQuickToggleText');
  const icon = document.getElementById('rightPanelQuickToggleIcon');
  const collapseBtn = document.getElementById('rightPanelCollapseBtn');
  const expandBtn = document.getElementById('rightPanelExpandBtn');
  const ratio = getCurrentRightPanelRatio();
  const expanded = ratio >= 0.50;
  if(quick) {
    quick.dataset.state = expanded ? 'expanded' : 'compact';
    quick.title = expanded ? '鏀剁缉鍙充晶杈规爮鍒?30%' : '灞曞紑鍙充晶杈规爮鍒?70%';
    quick.setAttribute('aria-label', quick.title);
    if(icon) icon.textContent = expanded ? '猡? : '猡?;
    if(text) text.textContent = expanded ? '鏀剁缉' : '灞曞紑';
  }
  if(collapseBtn) {
    collapseBtn.classList.toggle('active', !expanded);
    collapseBtn.disabled = ratio <= RIGHT_PANEL_MIN_RATIO + 0.01;
    collapseBtn.title = '鏀剁缉鍙充晶杈规爮鍒?30%';
    collapseBtn.setAttribute('aria-label', collapseBtn.title);
  }
  if(expandBtn) {
    expandBtn.classList.toggle('active', expanded);
    expandBtn.disabled = ratio >= RIGHT_PANEL_MAX_RATIO - 0.01;
    expandBtn.title = '灞曞紑鍙充晶杈规爮鍒?70%';
    expandBtn.setAttribute('aria-label', expandBtn.title);
  }
}
function applyRightPanelWidth(value, persist=true) {
  const span = getResizableMainWidth();
  const minW = Math.max(420, span * RIGHT_PANEL_MIN_RATIO);
  const maxW = Math.max(minW, span * RIGHT_PANEL_MAX_RATIO);
  const next = Math.max(minW, Math.min(maxW, Math.round(value)));
  document.documentElement.style.setProperty('--right-panel-width', `${next}px`);
  const ratio = clampRightPanelRatio(next / span);
  try {
    if(persist) {
      localStorage.setItem('digitalTwinRightPanelWidth', String(next));
      localStorage.setItem('digitalTwinRightPanelRatio', String(ratio));
      if(!flowPanelForcedWide) persistRightPanelRatio(ratio);
    }
  } catch(e) {}
  updateRightPanelQuickToggle();
  return next;
}
function setRightPanelRatio(ratio, persist=true) {
  const span = getResizableMainWidth();
  const clamped = clampRightPanelRatio(ratio);
  return applyRightPanelWidth(span * clamped, persist);
}
function enterFlowWidePanel() {
  if(!flowPanelForcedWide) {
    normalRightPanelRatio = getCurrentRightPanelRatio() || normalRightPanelRatio || RIGHT_PANEL_DEFAULT_RATIO;
    persistRightPanelRatio(normalRightPanelRatio);
  }
  flowPanelForcedWide = true;
  if(flowPanelResizeFrame) cancelAnimationFrame(flowPanelResizeFrame);
  flowPanelResizeFrame = requestAnimationFrame(() => {
    flowPanelResizeFrame = null;
    if(flowPanelForcedWide) setRightPanelRatio(RIGHT_PANEL_MAX_RATIO, false);
  });
}
function leaveFlowWidePanel() {
  if(flowPanelResizeFrame) {
    cancelAnimationFrame(flowPanelResizeFrame);
    flowPanelResizeFrame = null;
  }
  if(!flowPanelForcedWide) return;
  flowPanelForcedWide = false;
  let restore = normalRightPanelRatio || RIGHT_PANEL_DEFAULT_RATIO;
  try {
    restore = Number(localStorage.getItem('digitalTwinRightPanelNormalRatio')) || restore;
  } catch(e) {}
  setRightPanelRatio(clampRightPanelRatio(restore), false);
}
function toggleRightPanelWidth() {
  const current = getCurrentRightPanelRatio();
  const next = current >= 0.50 ? RIGHT_PANEL_MIN_RATIO : RIGHT_PANEL_MAX_RATIO;
  setRightPanelRatio(next, !flowPanelForcedWide);
  if(!flowPanelForcedWide) persistRightPanelRatio(next);
}
function initLayoutResize() {
  const mainEl = document.querySelector('main');
  const handle = document.getElementById('layoutResizeHandle');
  const quick = document.getElementById('rightPanelQuickToggle');
  const collapseBtn = document.getElementById('rightPanelCollapseBtn');
  const expandBtn = document.getElementById('rightPanelExpandBtn');
  if(quick && !quick.dataset.bound) {
    quick.dataset.bound = 'true';
    quick.addEventListener('click', evt => { evt.stopPropagation(); toggleRightPanelWidth(); });
  }
  if(collapseBtn && !collapseBtn.dataset.bound) {
    collapseBtn.dataset.bound = 'true';
    collapseBtn.addEventListener('click', evt => { evt.stopPropagation(); setRightPanelRatio(RIGHT_PANEL_MIN_RATIO, !flowPanelForcedWide); if(!flowPanelForcedWide) persistRightPanelRatio(RIGHT_PANEL_MIN_RATIO); });
  }
  if(expandBtn && !expandBtn.dataset.bound) {
    expandBtn.dataset.bound = 'true';
    expandBtn.addEventListener('click', evt => { evt.stopPropagation(); setRightPanelRatio(RIGHT_PANEL_MAX_RATIO, !flowPanelForcedWide); if(!flowPanelForcedWide) persistRightPanelRatio(RIGHT_PANEL_MAX_RATIO); });
  }
  if(!mainEl || !handle) { updateRightPanelQuickToggle(); return; }
  const applyWidth = value => applyRightPanelWidth(value, !flowPanelForcedWide);
  try {
    const savedNormal = Number(localStorage.getItem('digitalTwinRightPanelNormalRatio'));
    const savedRatio = Number(localStorage.getItem('digitalTwinRightPanelRatio'));
    normalRightPanelRatio = clampRightPanelRatio(savedNormal || savedRatio || RIGHT_PANEL_DEFAULT_RATIO);
    setRightPanelRatio(normalRightPanelRatio, false);
  } catch(e) { setRightPanelRatio(RIGHT_PANEL_DEFAULT_RATIO, false); }
  let dragging = false;
  handle.addEventListener('pointerdown', evt => {
    if(window.matchMedia('(max-width: 1180px)').matches) return;
    dragging = true;
    handle.classList.add('dragging');
    handle.setPointerCapture?.(evt.pointerId);
    evt.preventDefault();
  });
  handle.addEventListener('pointermove', evt => {
    if(!dragging) return;
    const rect = mainEl.getBoundingClientRect();
    applyWidth(rect.right - evt.clientX - 6);
  });
  const stop = evt => {
    if(!dragging) return;
    dragging = false;
    handle.classList.remove('dragging');
    if(!flowPanelForcedWide) persistRightPanelRatio(getCurrentRightPanelRatio());
    try { handle.releasePointerCapture?.(evt.pointerId); } catch(e) {}
  };
  handle.addEventListener('pointerup', stop);
  handle.addEventListener('pointercancel', stop);
  window.addEventListener('resize', () => {
    if(flowPanelForcedWide) setRightPanelRatio(RIGHT_PANEL_MAX_RATIO, false);
    else setRightPanelRatio(normalRightPanelRatio || RIGHT_PANEL_DEFAULT_RATIO, false);
  });
}


svg.addEventListener('click', () => { selectedName = null; selectedSvgControlId = null; document.querySelectorAll('.svg-control-selected').forEach(el => el.classList.remove('svg-control-selected')); renderDetail(null); showSideTab('status'); updateVisualStates(); });
initLayoutResize();
updateSideHeaderStatus();
bindClick('startBtn', () => { showSideTab('status'); startRealRun(); });
bindClick('pauseBtn', () => { pauseOrResumeRun(); });
bindClick('resetBtn', () => { setInfoPanel('绯荤粺澶嶄綅', ['鏈烘鑷傝繑鍥炴礂鍐呴拡鍘熺偣锛屾祦绋嬬姸鎬佸拰缂撳瓨鏁版嵁宸叉竻绌恒€?]); resetDemo(); });
bindClick('scanSamplesBtn', scanSamples);
bindClick('scanReagentsBtn', scanReagents);
bindClick('lowReagentBtn', simulateLowReagent);
bindClick('pullChannelBtn', togglePullChannel);
bindClick('alarmBtn', simulateAlarm);
bindClick('userBtn', (evt) => { evt.stopPropagation(); const menu = document.getElementById('userMenu'); if(menu) menu.classList.toggle('open'); });
document.addEventListener('click', () => { const menu = document.getElementById('userMenu'); if(menu) menu.classList.remove('open'); });
document.querySelectorAll('.mode-btn').forEach(btn => {
  btn.addEventListener('click', () => {
    document.querySelectorAll('.mode-btn').forEach(b => b.classList.toggle('active', b === btn));
    uiMode = btn.dataset.mode || 'twin';
    const settingsModeText = document.getElementById('settingsModeText'); if(settingsModeText) settingsModeText.textContent = btn.textContent;
    const labels = { twin:'瀛敓妯℃嫙', debug:'璋冭瘯', production:'鐢熶骇' };
    setInfoPanel('杩愯妯″紡鍒囨崲', [`褰撳墠妯″紡锛?{labels[uiMode] || uiMode}`, '妯″紡鍒囨崲缁撴灉缁熶竴鏄剧ず鍦ㄧ姸鎬佸崱鐗囦腑銆?]);
    showSideTab('status');
    log(`杩愯妯″紡鍒囨崲涓猴細${labels[uiMode] || uiMode}`, 'ok');
  });
});
document.querySelectorAll('[data-side-tab]').forEach(btn => {
  btn.addEventListener('click', () => {
    const target = btn.getAttribute('data-side-tab');
    if(target === 'precheck') {
      showSideTab('precheck');
      renderPrecheckList();
      if(appSettings.autoRunPrecheck && !precheckRunning && !precheckPassed) runPrecheck();
      return;
    }
    if(target === 'debug') {
      if(CURRENT_USER.role !== 'admin') { log('瀹為獙鍛樻潈闄愰殣钘忚皟璇曢〉闈紝璇蜂娇鐢ㄧ鐞嗗憳璐﹀彿鐧诲綍銆?, 'warn'); showSideTab('status'); return; }
      showSideTab('debug');
      return;
    }
    if(target === 'config') {
      showSideTab('config');
      return;
    }
    if(target === 'settings') {
      openSettingsPage('绯荤粺璁剧疆');
      return;
    }
    showSideTab(target);
  });
});
const csvInput = document.getElementById('csvInput');
if(csvInput) csvInput.addEventListener('change', e => { if(e.target.files && e.target.files[0]) loadCsvFile(e.target.files[0]); });

async function loadDatabaseSnapshot() {
  try {
    const response = await fetch('/api/twin/snapshot', { cache:'no-store', credentials:'same-origin' });
    if(!response.ok) throw new Error(`HTTP ${response.status}`);
    const snapshot = await response.json();
    window.digitalTwinDbSnapshot = snapshot;
    if(snapshot.precheckResults) window.digitalTwinPrecheckResults = snapshot.precheckResults;
    applyDatabaseSnapshot(snapshot);
    log('鏁版嵁搴撳揩鐓у凡鍔犺浇锛氬墠绔暟鍊煎凡鎸?DB/null 绛栫暐鍒锋柊', 'ok');
  } catch(err) {
    console.warn('鏁版嵁搴撳揩鐓у姞杞藉け璐?, err);
    log('鏁版嵁搴撳揩鐓у姞杞藉け璐ワ細' + err.message, 'err');
  }
}
function applyNullableMapEntries(map, entries, keyField, valueField) {
  if(!Array.isArray(entries)) return;
  entries.forEach(row => {
    if(!row || !row[keyField]) return;
    map.set(row[keyField], row[valueField] === undefined ? null : row[valueField]);
  });
}
function applyDatabaseSnapshot(snapshot) {
  if(!snapshot) return;
  const payload = snapshot.digitalTwinPayload || {};
  if(Array.isArray(payload.items)) {
    payload.items.forEach(it => {
      if(!it?.name) return;
      itemLevels.set(it.name, it.level === undefined ? null : it.level);
      if(it.state === null || it.state === undefined) itemState.delete(it.name); else itemState.set(it.name, it.state);
    });
  }
  if(Array.isArray(payload.slideTemps)) payload.slideTemps.forEach(st => { if(st?.name) slideTemps.set(st.name, st.temp === undefined ? null : st.temp); });
  if(Array.isArray(payload.slideOps)) payload.slideOps.forEach(op => { if(op?.name && Array.isArray(op.steps)) slideOps.set(op.name, op.steps.slice(0, 12)); });
  if(Array.isArray(payload.channels)) payload.channels.forEach(ch => {
    const target = channels[(Number(ch.id) || 1) - 1];
    if(!target) return;
    ['state','progress','pulled','configProfileId'].forEach(k => { if(Object.prototype.hasOwnProperty.call(ch, k)) target[k] = ch[k]; });
  });
  if(payload.configProfiles) {
    const dbProfiles = payload.configProfiles.map(normalizeProfile).filter(p => p.steps.length);
    configProfiles = dbProfiles.length ? dbProfiles : ensureDefaultProfile(dbProfiles);
    selectedConfigId = configProfiles[0]?.id || selectedConfigId;
    saveConfigProfilesOnly();
  }
  if(payload.liquids) Object.assign(liquids, payload.liquids);
  if(payload.metrics) updateHeaderMetrics(payload.metrics);
  if(payload.cameras) Object.assign(cameraStates, payload.cameras);
  if(payload.arm) arm = {...arm, ...payload.arm};
  const values = snapshot.control_values || {};
  try {
    Object.entries(values).forEach(([id, value]) => {
      const node = document.getElementById(id) || document.querySelector(`[data-control-id="${String(id).replace(/"/g, '\\"')}"]`);
      if(!node) return;
      if(node.closest && node.closest('#loginScreen')) return;
      const normalized = value === null || value === undefined ? '' : value;
      if('value' in node) {
        node.value = normalized;
      } else if(node.namespaceURI === 'http://www.w3.org/2000/svg' && node.tagName.toLowerCase() === 'g') {
        return;
      } else {
        node.textContent = normalized === '' ? '鈥? : String(normalized);
      }
    });
  } catch(e) {
    console.warn('control_values 搴旂敤鍑洪敊', e);
  }
  // 鍒锋柊 appSettings锛欴B 鎻愪緵鐨勬暟鍊艰鐩栧埌 appSettings 骞舵寔涔呭寲锛圖B 鎸佷箙瑕嗙洊绛栫暐锛?
  try {
    const _get = id => document.getElementById(id);
    const _numOrNull = id => {
      const node = _get(id);
      if (!node) return null;
      const v = String(node.value ?? '').trim();
      if (v === '') return null;
      const n = Number(v);
      return Number.isFinite(n) ? n : null;
    };
    appSettings = {
      ...appSettings,
      reagentBottleCapacityMl: _numOrNull('settingsReagentCapacityInput'),
      reagentTargetTempC:      _numOrNull('settingsReagentTargetInput'),
      workTargetTempC:         _numOrNull('settingsWorkTargetInput'),
      needleGapMm:             _numOrNull('settingsNeedleGapInput'),
      pureLowThresholdPct:     _numOrNull('settingsPureThresholdInput'),
      pbsLowThresholdPct:      _numOrNull('settingsPbsThresholdInput'),
      wasteFullThresholdPct:   _numOrNull('settingsWasteThresholdInput'),
      toxicFullThresholdPct:   _numOrNull('settingsToxicThresholdInput'),
    };
    saveAppSettings();
  } catch(e) {
    console.warn('appSettings 浠?DB 蹇収鍒锋柊澶辫触', e);
  }
  drawData(); drawSlideOps(); updateVisualStates(); renderChannelCards(); renderLiquids(); updateKpis(); renderDetail(selectedName ? byName.get(selectedName) : null);
}
window.digitalTwinLoadDatabaseSnapshot = loadDatabaseSnapshot;

setInterval(() => { updateSideHeaderStatus(); updateKpis(); updateHeaderMetrics(); }, 1000);

resetDemo();
restoreBackendSession();
[1,2,3,4].forEach(id => { if(channelConfigAssignments[id] && !getProfileById(channelConfigAssignments[id])) channelConfigAssignments[id] = null; assignConfigToChannel(id, channelConfigAssignments[id], false); });
  if(window.digitalTwin?.engineering) {
    window.digitalTwin.engineering.getConfigSectionForm = function(sectionKey) {
      const key = normalizeConfigSectionKey(sectionKey || activeConfigSection);
      const section = document.querySelector(`[data-rendered-section="${key}"]`) || document.getElementById('configActiveSection') || document.getElementById('configPane');
      const data = {};
      section?.querySelectorAll('input, select, textarea').forEach(node => { if(node.id) data[node.id] = node.type === 'checkbox' ? node.checked : node.value; });
      return data;
    };
  }

renderConfigPane();
renderDebugPane();
renderPrecheckList();
renderUserMenu();
initLoginPage();
bindLogFilter();
bindWarnFilter();
bindProductionSubTabs();
log('椤甸潰鍔犺浇瀹屾垚锛氫娇鐢ㄥ唴宓屽竷灞€缁樺埗 2D 鏁板瓧瀛敓', 'ok');
log('浜や簰鎻愮ず锛氶《閮ㄦ娴嬮€氳繃鍚庡惎鐢ㄥ紑濮嬶紱鐐瑰嚮鍥惧厓鍦ㄧ姸鎬侀〉鏌ョ湅璇︽儏', '');

