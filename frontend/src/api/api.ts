const baseUrl = "http://138.124.20.138";

export async function getRequest(path: string) {
  const url =  baseUrl + path;
  let res =  await fetch(url, {
    method: 'GET',
    credentials: 'include',
    headers: { 'Content-Type': 'application/json' }
  })

  if (!res.ok) {
    const error = new Error('An error occurred while fetching the data.')
    // Добавление дополнительной информации в объект ошибки.
    throw error
  }
  
  return res.json()
}

export async function sendRequest(path:string, {arg}: {arg: any}){
  const url =  baseUrl + path;
  const res = await fetch(url, {
    method: "POST",
    credentials: 'include',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(arg)
  })

  if (!res.ok) {
    const error = new Error('An error occurred while fetching the data.')
    throw error
  }

  return res.json()
}