import axios from 'axios';
axios.defaults.baseURL = process.env.REACT_APP_API_KEY ;

axios.interceptors.request.use(
  (config) =>
    config,
  (error) => {
    console.log(error);
    return Promise.reject(error);
  }
);
axios.interceptors.response.use(
  (response) =>
    response,
  (error) => {
    console.log(error);
    return Promise.reject(error);
  }
);

export default {
  
  getTasks: async () => {
    const result = await axios.get(`items`)
    return result.data;
  },

  addTask: async (name) => {
    await axios.post(`${name}`)
    return {};
  },

  setCompleted: async (id, isComplete) => {
    await axios.put(`${id}/${isComplete}`)
    return {};
  },

  deleteTask: async (id) => {
    await axios.delete(`${id}`)
    return {};
  }
};
