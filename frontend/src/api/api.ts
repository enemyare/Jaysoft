const baseUrl = "http://localhost:5000";


export async function getRequest(path: string) {
  const url =  baseUrl + path;
  return fetch(url, {
    method: 'GET',
    headers: { 'Content-Type': 'application/json' }
  }).then(res => res.json())
}

export async function sendRequest(path:string, {arg}: {arg: any}){
  const url =  baseUrl + path;
  return fetch(url, {
    method: "POST",
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(arg)
  })
}


