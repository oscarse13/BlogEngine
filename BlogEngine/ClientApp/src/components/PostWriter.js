import React, { Component } from 'react';
import { AppConfiguration } from "read-appsettings-json";
import axios from 'axios';

export class PostWriter extends Component {
    static displayName = PostWriter.name;


    constructor(props) {
        super(props);
        this.state = { posts: [], loading: true, id: 0, notFound: true, enableErrorMessage: false, showForm: false, title: '', content: '', status: '' };
        this.url = AppConfiguration.Setting().Apiendpoint;
        this.newPost = this.newPost.bind(this);
        this.editPost = this.editPost.bind(this);
        this.savePost = this.savePost.bind(this);
        this.cancel = this.cancel.bind(this);
        this.handleChange = this.handleChange.bind(this);
        this.submitPost = this.submitPost.bind(this);
        this.handleCallApi = this.handleCallApi.bind(this);
        this.handleSavePost = this.handleSavePost.bind(this);
        this.userId = document.getElementById('userId').value;
    }

    componentDidMount() {
        this.populateData();
    }

    newPost() {

        this.setState({ showForm: true, title: '', content: '', id: 0, status: 'Created' });
    }

    cancel() {

        this.setState({ showForm: false, title: '', content: '', id: 0 });
    }

    editPost(e, post) {

        this.setState({ showForm: true, title: post.title, content: post.content, id: post.id, status: post.status });
    }

    submitPost(event, post) {


        const postId = post ? post.id : this.state.id;

        this.handleCallApi(this.url + 'post/UpdateStatePost/' + postId + '/Pending');
    }

    handleSavePost(event) {
        event.target = '_blank';
        this.savePost(event);
    }

    savePost(event) {
        event.preventDefault();
        let status = this.state.status;
        if (event.target === '_blank') {
            status = 'Pending';
        }
        const data = { id: this.state.id, title: this.state.title, content: this.state.content, writerId: this.userId, status: status };
        this.handleCallApi(this.url + 'post/CreateUpdatePost', data);
    }

    handleChange(event) {
        const { target: { name, value } } = event;
        this.setState({ [name]: value });
    }

    handleCallApi(url, data) {
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'UserId': this.userId
            },
            body: data ? JSON.stringify(data) : {}
        }).then(this.fetchStatusHandler)
            .then(data => {
                this.setState({ loading: true, showForm: false, title: '', content: '', id: 0 });
                this.populateData();
            })
            .catch(error => {
                console.log(error);
            });
    }


    renderTable(posts) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Action</th>
                        <th>Id</th>
                        <th>Title</th>
                        <th>Status</th>
                        <th>Created Date</th>
                        <th>Approval date</th>
                    </tr>
                </thead>
                <tbody>
                    {posts.map(post =>
                        <tr key={post.id}>
                            <td>
                                <button className="btn btn-primary" hidden={post.status === 'Created' || post.status === 'Rejected' ? '' : 'hidden'} onClick={(e) => this.editPost(e, post)}>Edit</button>
                                <button className="btn btn-success" hidden={post.status === 'Created' || post.status === 'Rejected' ? '' : 'hidden'} onClick={(e) => this.submitPost(e, post)} title="Submit for publish approval" >Submit</button>
                            </td>
                            <td>{post.id}</td>
                            <td>{post.title}</td>
                            <td>{post.status}</td>
                            <td>{post.createdDate}</td>
                            <td>{post.approvalDate}</td>

                        </tr>
                    )}
                </tbody>
            </table >
        );
    }


    render() {
        let contents = this.state.enableErrorMessage
            ? <p><em>Something went wrong...</em></p>
            : this.state.notFound
                ? <p><em>No records...</em></p>
                : this.state.loading
                    ? <p><em>Loading...</em></p>
                    : this.renderTable(this.state.posts);

        return (
            <div>
            <h1><span className="badge badge-secondary">Writer</span></h1>
                <div hidden={this.state.enablePermisionMessage ? 'hidden' : ''}>
                    <div hidden={this.state.showForm ? 'hidden' : ''}>
                        <button className="btn btn-primary" onClick={this.newPost}>New Post</button>
                        <p></p>
                        {contents}
                        <p></p>
                    </div>
                    <div hidden={this.state.showForm ? '' : 'hidden'}>
                        <h4>Post</h4>
                        <div className="row">
                            <div className="col-10">
                                <form id="post-form" method="post" onSubmit={this.savePost}>
                                    <div className="form-group">
                                        <label htmlFor="title">Title</label>
                                        <br />
                                        <input type="text" id="title" name="title" value={this.state.title} placeholder="Title" onChange={this.handleChange} required />
                                    </div>
                                    <div className="form-group">
                                        <label htmlFor="content">Content</label>
                                        <br />
                                        <textarea id="content" name="content" rows="15" cols="155" value={this.state.content} onChange={this.handleChange} required >
                                        </textarea>
                                    </div>
                                    <div className="row">
                                        <button id="save" className="btn btn-primary" >Save</button>
                                        <input type="button" className="btn btn-success" onClick={this.handleSavePost} value="Submit" />
                                        <input type="button" className="btn btn-secondary" onClick={this.cancel} value="Cancel" />
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
                <div hidden={this.state.enablePermisionMessage ? '' : 'hidden'}>
                    You have not permission for this view!
                </div>

            </div>
        );
    }

    async populateData() {
        await this.getPosts();
    }



    async getPosts() {
        axios.get('Identity/Account/Manage').then(rolesResponse => {
            let htmLogin = rolesResponse.data;
            let begin = htmLogin.search('<roles>');
            let end = htmLogin.search('</roles>');
            let roles = htmLogin.substr(begin, end - begin).replace('<roles>', '').replace('</roles>', '');
            roles = JSON.parse(roles);
            if (roles.writer === 'False') {
                this.setState({ enablePermisionMessage: true });
            }
        }
        );

        fetch(this.url + 'post/GetPostsByWriter/' + this.userId)
            .then(this.fetchStatusHandler)
            .then(data => {
                this.setState({ posts: data ? data : [], loading: false, notFound: data ? data.length === 0 : true, enableErrorMessage: data === null });
            })
            .catch(error => {
                this.handleError(error);
            });
    }

    fetchStatusHandler(response) {
        if (response.status === 200) {
            return response.json();
        } else if (response.status === 404) {
            return [];
        }
        else {
            console.log(response);
            return null;
        }
    }

    handleError(error) {
        this.setState({ enableErrorMessage: true });
        console.log(error);
    }
}
