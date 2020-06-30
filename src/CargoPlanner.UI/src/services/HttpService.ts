const defaultHeaders = new Headers({
  Accept: 'application/json',
  'Content-Type': 'application/json',
});

async function http<T>(request: RequestInfo): Promise<Response> {
  const response: Response = await fetch(request);

  if (!response.ok) {
    console.log('ERROR');
    console.log(response);
  }

  return response;
}

export async function httpGet<T>(
  url: string,
  args: RequestInit = {
    method: 'get',
    headers: defaultHeaders,
  },
): Promise<Response> {
  console.log('GET @ ' + url);
  return await http<T>(new Request(url, args));
}

export async function httpDelete<T>(
  url: string,
  args: RequestInit = {
    method: 'delete',
    headers: defaultHeaders,
  },
): Promise<Response> {
  console.log('DELETE @ ' + url);
  return await http<T>(new Request(url, args));
}

export async function httpPost<T>(
  url: string,
  body: any,
  args: RequestInit = {
    method: 'post',
    headers: new Headers({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': 'http://localhost:3000',
    }),
    body: body,
  },
): Promise<Response> {
  console.log('POST @ ' + url);
  return await http<T>(new Request(url, args));
}

export async function httpPut<T>(
  url: string,
  body: any,
  args: RequestInit = {
    method: 'put',
    headers: defaultHeaders,
    body: body,
  },
): Promise<Response> {
  console.log('PUT @ ' + url);
  return await http<T>(new Request(url, args));
}
