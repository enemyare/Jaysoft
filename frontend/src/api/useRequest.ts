import useSWR from "swr"


const baseUrl = "http://localhost:5000";

export const useRequest = (path: string) => {
  if (!path) throw new Error('Path is required');

  const url = `${baseUrl}/${path}`;
  const {data, error, isLoading} = useSWR(url)
  return {data, error, isLoading}
}