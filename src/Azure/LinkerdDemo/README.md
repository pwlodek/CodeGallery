# LinkerdDemo
Quick sample to showcase some cool things in Linkerd mesh:

1. Proxy auto injection
2. Service profiles
3. Traffic split for doing canary releases

The app is composed of a simple 4 service:

1. Gateway which accepts traffic
2. Traffic is passed to ServiceA, ServiceA fails with 500 from time to time on route GET /api/values
3. There is also a Bot service which generates artificial traffic by sending Http GET /api/values/1
4. Gateway is exposed on the ingress
5. You can fix the error by applying service profile to backend-svc, this will make sure that Gateway retries error on ServiceA, the client will always see the success
6. You can apply traffic split and do canary release, by sending portion of traffic from ServiceA to ServiceB
