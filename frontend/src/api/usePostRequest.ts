// import useSWR from "swr";
//
// const baseUrl = "http://localhost:5000";
//
// export const usePostRequest = (path: string, body: any) => {
//   if (!path) throw new Error('Path is required');
//
//   const url = `${baseUrl}/${path}`;
//
//   const { data, error, isLoading } = useSWR(
//     // Ключ содержит URL и данные тела для уникальности
//     [url, body],
//     // Фетчер возьмёт глобальную функцию fetcher и добавит параметры POST
//     ([url, body]) =>
//       fetch(url, {
//         method: "POST",
//         headers: {
//           "Content-Type": "application/json",
//         },
//         body: JSON.stringify(body),
//       }),
//     {
//       revalidateOnFocus: false, // Настройки SWR (например, отключить повторное выполнение)
//     }
//   );
//
//   return { data, error, isLoading };
// };
export async function sendRequest(url:string, {arg}: {arg: any}){
  return fetch(url, {
    method: "POST",
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(arg)
  })
}
