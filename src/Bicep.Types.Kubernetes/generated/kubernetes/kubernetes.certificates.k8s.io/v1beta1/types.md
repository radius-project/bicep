# kubernetes.certificates.k8s.io @ v1beta1

## Resource kubernetes.certificates.k8s.io/CertificateSigningRequest@v1beta1
* **Valid Scope(s)**: Unknown
### Properties
* **apiVersion**: 'certificates.k8s.io/v1beta1' (ReadOnly, DeployTimeConstant): The api version.
* **kind**: 'CertificateSigningRequest' (ReadOnly, DeployTimeConstant): The resource kind.
* **metadata**: [metadata](#metadata) (Required): The resource metadata.
* **spec**: [IoK8SApiCertificatesV1Beta1CertificateSigningRequestSpec](#iok8sapicertificatesv1beta1certificatesigningrequestspec): This information is immutable after the request is created. Only the Request and Usages fields can be set on creation, other fields are derived by Kubernetes and cannot be modified by users.
* **status**: [IoK8SApiCertificatesV1Beta1CertificateSigningRequestStatus](#iok8sapicertificatesv1beta1certificatesigningrequeststatus)

## metadata
### Properties
* **annotations**: [annotations](#annotations): The annotations for the resource.
* **labels**: [labels](#labels): The labels for the resource.
* **name**: string (Required, DeployTimeConstant): The name of the resource.

## annotations
### Properties
### Additional Properties
* **Additional Properties Type**: string

## labels
### Properties
### Additional Properties
* **Additional Properties Type**: string

## IoK8SApiCertificatesV1Beta1CertificateSigningRequestSpec
### Properties
* **extra**: [IoK8SApiCertificatesV1Beta1CertificateSigningRequestSpecExtra](#iok8sapicertificatesv1beta1certificatesigningrequestspecextra): Extra information about the requesting user. See user.Info interface for details.
* **groups**: string[]: Group information about the requesting user. See user.Info interface for details.
* **request**: any (Required): Base64-encoded PKCS#10 CSR data
* **signerName**: string: Requested signer for the request. It is a qualified name in the form: `scope-hostname.io/name`. If empty, it will be defaulted:
 1. If it's a kubelet client certificate, it is assigned
    "kubernetes.io/kube-apiserver-client-kubelet".
 2. If it's a kubelet serving certificate, it is assigned
    "kubernetes.io/kubelet-serving".
 3. Otherwise, it is assigned "kubernetes.io/legacy-unknown".
Distribution of trust for signers happens out of band. You can select on this field using `spec.signerName`.
* **uid**: string: UID information about the requesting user. See user.Info interface for details.
* **usages**: string[]: allowedUsages specifies a set of usage contexts the key will be valid for. See: https://tools.ietf.org/html/rfc5280#section-4.2.1.3
     https://tools.ietf.org/html/rfc5280#section-4.2.1.12
Valid values are:
 "signing",
 "digital signature",
 "content commitment",
 "key encipherment",
 "key agreement",
 "data encipherment",
 "cert sign",
 "crl sign",
 "encipher only",
 "decipher only",
 "any",
 "server auth",
 "client auth",
 "code signing",
 "email protection",
 "s/mime",
 "ipsec end system",
 "ipsec tunnel",
 "ipsec user",
 "timestamping",
 "ocsp signing",
 "microsoft sgc",
 "netscape sgc"
* **username**: string: Information about the requesting user. See user.Info interface for details.

## IoK8SApiCertificatesV1Beta1CertificateSigningRequestSpecExtra
### Properties
### Additional Properties
* **Additional Properties Type**: string[]

## IoK8SApiCertificatesV1Beta1CertificateSigningRequestStatus
### Properties
* **certificate**: any: If request was approved, the controller will place the issued certificate here.
* **conditions**: [IoK8SApiCertificatesV1Beta1CertificateSigningRequestCondition](#iok8sapicertificatesv1beta1certificatesigningrequestcondition)[]: Conditions applied to the request, such as approval or denial.

## IoK8SApiCertificatesV1Beta1CertificateSigningRequestCondition
### Properties
* **lastTransitionTime**: string: Time is a wrapper around time.Time which supports correct marshaling to YAML and JSON.  Wrappers are provided for many of the factory methods that the time package offers.
* **lastUpdateTime**: string: Time is a wrapper around time.Time which supports correct marshaling to YAML and JSON.  Wrappers are provided for many of the factory methods that the time package offers.
* **message**: string: human readable message with details about the request state
* **reason**: string: brief reason for the request state
* **status**: string: Status of the condition, one of True, False, Unknown. Approved, Denied, and Failed conditions may not be "False" or "Unknown". Defaults to "True". If unset, should be treated as "True".
* **type**: string (Required): type of the condition. Known conditions include "Approved", "Denied", and "Failed".

