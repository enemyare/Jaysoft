const baseUrl = "http://localhost:5000";


export async function getRequest(path: string) {
  const url =  baseUrl + path;
  return await fetch(url, {
    method: 'GET',
    credentials: 'include',
    headers: { 'Content-Type': 'application/json' }
  })
}

export async function sendRequest(path:string, {arg}: {arg: any}){
  const url =  baseUrl + path;
  return await fetch(url, {
    method: "POST",
    credentials: 'include',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(arg)
  })
}


